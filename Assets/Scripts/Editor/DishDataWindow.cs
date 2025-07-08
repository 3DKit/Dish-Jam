using UnityEngine;
using UnityEditor;
using static Enums;

public class DishDataWindow : EditorWindow
{
    private DishData dishData;
    private GameColors[,] gridColors;
    private Vector2 scrollPosition;
    private float cellSize = 50f;
    private float spacing = 5f;
    private int selectedGridX = -1;
    private int selectedGridY = -1;

    [MenuItem("Window/DishJam/Dish Grid Editor")]
    public static void ShowWindow()
    {
        DishDataWindow window = GetWindow<DishDataWindow>("Dish Grid Editor");
        window.minSize = new Vector2(600, 800);
    }

    private void OnEnable()
    {
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        if (dishData == null) return;
        
        if (dishData.verticalSlots <= 0) dishData.verticalSlots = 1;
        if (dishData.horizontalSlots <= 0) dishData.horizontalSlots = 1;
        
        gridColors = new GameColors[dishData.horizontalSlots, dishData.verticalSlots];
        
        // Tüm grid'i varsayılan olarak Red yap
        for (int x = 0; x < dishData.horizontalSlots; x++)
        {
            for (int y = 0; y < dishData.verticalSlots; y++)
            {
                gridColors[x, y] = GameColors.Red;
            }
        }
        
        // Mevcut dish bilgilerini grid'e yükle
        if (dishData.dishes != null)
        {
            foreach (var dish in dishData.dishes)
            {
                if (dish.horizontalSlot >= 0 && dish.horizontalSlot < dishData.horizontalSlots &&
                    dish.verticalSlot >= 0 && dish.verticalSlot < dishData.verticalSlots)
                {
                    gridColors[dish.horizontalSlot, dish.verticalSlot] = dish.dishColor;
                }
            }
        }
    }

    private void OnGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Dish Grid Editor", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        // DishData seçici
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("DishData Asset:", GUILayout.Width(100));
        DishData newDishData = (DishData)EditorGUILayout.ObjectField(dishData, typeof(DishData), false);
        if (newDishData != dishData)
        {
            dishData = newDishData;
            InitializeGrid();
        }
        EditorGUILayout.EndHorizontal();

        if (dishData == null)
        {
            EditorGUILayout.HelpBox("Please select a DishData asset to edit.", MessageType.Info);
            return;
        }

        EditorGUILayout.Space();

        // Slot sayılarını ayarlama - Horizontal sol üstte
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Horizontal Slots:", GUILayout.Width(120));
        
        if (GUILayout.Button("+", GUILayout.Width(30)))
        {
            dishData.horizontalSlots++;
            InitializeGrid();
        }
        
        EditorGUILayout.LabelField(dishData.horizontalSlots.ToString(), GUILayout.Width(30));
        
        if (GUILayout.Button("-", GUILayout.Width(30)) && dishData.horizontalSlots > 1)
        {
            dishData.horizontalSlots--;
            InitializeGrid();
        }
        
        // Boşluk ekle
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        // Grid çizimi - Ortalanmış
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        DrawGrid();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        // Vertical sağ altta - Grid'in altında
        EditorGUILayout.BeginHorizontal();
        // Boşluk ekle
        GUILayout.FlexibleSpace();
        
        EditorGUILayout.LabelField("Vertical Slots:", GUILayout.Width(120));
        
        if (GUILayout.Button("+", GUILayout.Width(30)))
        {
            dishData.verticalSlots++;
            InitializeGrid();
        }
        
        EditorGUILayout.LabelField(dishData.verticalSlots.ToString(), GUILayout.Width(30));
        
        if (GUILayout.Button("-", GUILayout.Width(30)) && dishData.verticalSlots > 1)
        {
            dishData.verticalSlots--;
            InitializeGrid();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        // Renk seçici - Başlık ayrıştırılmış
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Color Selection", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical(GUILayout.Width(250));
        GameColors selectedColor = (GameColors)EditorGUILayout.EnumPopup("Selected Color:", GetSelectedColor());
        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        
        if (GUI.changed)
        {
            SetSelectedColor(selectedColor);
        }

        EditorGUILayout.Space();

        // Kaydet butonu - Ortalanmış
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Save Grid to DishData", GUILayout.Width(200)))
        {
            SaveGridToDishData();
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        // Mevcut dish listesi - Başlık ayrıştırılmış
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Current Dishes", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        if (dishData.dishes != null)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical(GUILayout.Width(300));
            
            foreach (var dish in dishData.dishes)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"H:{dish.horizontalSlot} V:{dish.verticalSlot}", GUILayout.Width(80));
                EditorGUILayout.LabelField(dish.dishColor.ToString());
                EditorGUILayout.EndHorizontal();
            }
            
            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }
    }

    private void DrawGrid()
    {
        // Grid yüksekliğini hesapla - maksimum 400px, minimum 200px
        float gridHeight = Mathf.Min(400f, Mathf.Max(200f, dishData.verticalSlots * (cellSize + spacing) + 20f));
        
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(gridHeight));
        
        float totalWidth = dishData.horizontalSlots * (cellSize + spacing) - spacing;
        float totalHeight = dishData.verticalSlots * (cellSize + spacing) - spacing;
        
        Rect gridRect = GUILayoutUtility.GetRect(totalWidth, totalHeight);
        
        // Grid çizimi
        for (int x = 0; x < dishData.horizontalSlots; x++)
        {
            for (int y = 0; y < dishData.verticalSlots; y++)
            {
                float xPos = gridRect.x + x * (cellSize + spacing);
                float yPos = gridRect.y + y * (cellSize + spacing);
                
                Rect cellRect = new Rect(xPos, yPos, cellSize, cellSize);
                
                // Hücre rengi
                Color cellColor = GetColorFromEnum(gridColors[x, y]);
                GUI.color = cellColor;
                GUI.Box(cellRect, "");
                
                // Hücre tıklama
                if (Event.current.type == EventType.MouseDown && cellRect.Contains(Event.current.mousePosition))
                {
                    selectedGridX = x;
                    selectedGridY = y;
                    GUI.changed = true;
                    Repaint();
                }
                
                // Seçili hücre vurgusu
                if (selectedGridX == x && selectedGridY == y)
                {
                    GUI.color = Color.white;
                    GUI.Box(cellRect, "");
                    // Seçili hücre için ek vurgu
                    GUI.color = Color.yellow;
                    GUI.Box(new Rect(cellRect.x - 2, cellRect.y - 2, cellRect.width + 4, cellRect.height + 4), "");
                }
                
                // Koordinat etiketi
                GUI.color = Color.black;
                GUI.Label(cellRect, $"{x},{y}", EditorStyles.centeredGreyMiniLabel);
            }
        }
        
        GUI.color = Color.white;
        EditorGUILayout.EndScrollView();
    }

    private GameColors GetSelectedColor()
    {
        if (selectedGridX >= 0 && selectedGridY >= 0 && 
            selectedGridX < dishData.horizontalSlots && selectedGridY < dishData.verticalSlots)
        {
            return gridColors[selectedGridX, selectedGridY];
        }
        return GameColors.Red;
    }

    private void SetSelectedColor(GameColors color)
    {
        if (selectedGridX >= 0 && selectedGridY >= 0 && 
            selectedGridX < dishData.horizontalSlots && selectedGridY < dishData.verticalSlots)
        {
            gridColors[selectedGridX, selectedGridY] = color;
        }
    }

    private Color GetColorFromEnum(GameColors gameColor)
    {
        switch (gameColor)
        {
            case GameColors.Red: return new Color(1f, 0.3f, 0.3f, 1f);
            case GameColors.Green: return new Color(0.3f, 1f, 0.3f, 1f);
            case GameColors.Blue: return new Color(0.3f, 0.3f, 1f, 1f);
            case GameColors.Yellow: return new Color(1f, 1f, 0.3f, 1f);
            case GameColors.Purple: return new Color(0.7f, 0.3f, 0.7f, 1f);
            case GameColors.Orange: return new Color(1f, 0.6f, 0.3f, 1f);
            case GameColors.Pink: return new Color(1f, 0.5f, 0.8f, 1f);
            case GameColors.Brown: return new Color(0.7f, 0.5f, 0.3f, 1f);
            case GameColors.Gray: return new Color(0.6f, 0.6f, 0.6f, 1f);
            default: return new Color(1f, 1f, 1f, 0.8f);
        }
    }

    private void SaveGridToDishData()
    {
        // Tüm grid hücrelerini kaydet (Red dahil)
        int totalCells = dishData.horizontalSlots * dishData.verticalSlots;
        dishData.dishes = new DishData.DishInfo[totalCells];
        int index = 0;

        for (int x = 0; x < dishData.horizontalSlots; x++)
        {
            for (int y = 0; y < dishData.verticalSlots; y++)
            {
                dishData.dishes[index] = new DishData.DishInfo
                {
                    horizontalSlot = x,
                    verticalSlot = y,
                    dishColor = gridColors[x, y]
                };
                index++;
            }
        }

        EditorUtility.SetDirty(dishData);
        AssetDatabase.SaveAssets();
        Debug.Log($"Grid saved! {totalCells} dishes created (including Red ones).");
    }
} 