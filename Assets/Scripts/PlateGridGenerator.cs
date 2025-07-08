using UnityEngine;
using UnityEngine.UI;
using static Enums;

public class PlateGridGenerator : MonoBehaviour
{
    [Header("References")]
    [HideInInspector] public DishData dishData;
    [HideInInspector] public GameObject platesPanel;
    [HideInInspector] public GameObject panelPrefab;
    [HideInInspector] public GameObject imagePrefab;
    
    [Header("Grid Settings")]
    public float imageSpacing = 128f;
    
    [Header("Generated Objects")]
    public GameObject[] generatedPanels;
    public Image[][] generatedImages;
    
    [ContextMenu("Generate Plate Grid")]
    public void GeneratePlateGrid()
    {
        if (dishData == null || platesPanel == null)
        {
            Debug.LogError("DishData or PlatesPanel is null!");
            return;
        }
        ClearExistingGrid();
        int horizontalCount = dishData.horizontalSlots;
        int verticalCount = dishData.verticalSlots;
        
        generatedPanels = new GameObject[horizontalCount];
        generatedImages = new Image[horizontalCount][];
        
        // Horizontal panelleri oluştur
        for (int x = 0; x < horizontalCount; x++)
        {
            // Panel oluştur
            GameObject panel = CreatePanel(x);
            generatedPanels[x] = panel;
            generatedImages[x] = new Image[verticalCount];
            
            // Vertical image'ları oluştur
                    for (int y = 0; y < verticalCount; y++)
        {
            Image image = CreateImage(panel, x, y, dishData.imageSize);
            generatedImages[x][y] = image;
            SetImageColor(image, x, y);
        }
        }
        
        Debug.Log($"Plate grid generated: {horizontalCount}x{verticalCount} with {dishData.imageSize}x{dishData.imageSize} images");
    }
    
    private GameObject CreatePanel(int horizontalIndex)
    {
        GameObject panel;
        
        if (panelPrefab != null)
        {
            panel = Instantiate(panelPrefab, platesPanel.transform);
        }
        else
        {
            // Varsayılan panel oluştur
            panel = new GameObject($"Panel_{horizontalIndex}");
            panel.transform.SetParent(platesPanel.transform, false);
            
            // Image component ekle
            Image panelImage = panel.AddComponent<Image>();
            panelImage.color = new Color(0.2f, 0.2f, 0.2f, 0.5f);
            
            // Content Size Fitter ekle (panel boyutunu içeriğe göre ayarlar)
            ContentSizeFitter csf = panel.AddComponent<ContentSizeFitter>();
            csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            csf.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        }
        
        return panel;
    }
    
    private Image CreateImage(GameObject parent, int x, int y, float size)
    {
        GameObject imageObj;
        
        if (imagePrefab != null)
        {
            imageObj = Instantiate(imagePrefab, parent.transform);
        }
        else
        {
            // Varsayılan image oluştur
            imageObj = new GameObject($"Image_{x}_{y}");
            imageObj.transform.SetParent(parent.transform, false);
            
            // Image component ekle
            Image image = imageObj.AddComponent<Image>();
            image.color = Color.white;
            
                    // RectTransform ayarları - Top Center
        RectTransform rectTransform = imageObj.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 1f);
        rectTransform.anchorMax = new Vector2(0.5f, 1f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.sizeDelta = new Vector2(dishData.imageSize, dishData.imageSize);
        rectTransform.anchoredPosition = new Vector2(0, -y * dishData.imageSpacing);
            
            return image;
        }
        
        Image img = imageObj.GetComponent<Image>();
        if (img == null)
        {
            img = imageObj.AddComponent<Image>();
        }
        
        // RectTransform ayarları (prefab için) - Top Center'dan başlasın
        RectTransform prefabRectTransform = imageObj.GetComponent<RectTransform>();
        prefabRectTransform.anchorMin = new Vector2(0.5f, 1f);
        prefabRectTransform.anchorMax = new Vector2(0.5f, 1f);
        prefabRectTransform.pivot = new Vector2(0.5f, 0.5f);
        prefabRectTransform.sizeDelta = new Vector2(dishData.imageSize, dishData.imageSize);
        float yOffset = -y * dishData.imageSpacing - dishData.imageSize / 2f;
        prefabRectTransform.anchoredPosition = new Vector2(0, yOffset);
        
        return img;
    }
    
    private void SetImageColor(Image image, int x, int y)
    {
        if (dishData.dishes == null || dishData.dishes.Length == 0)
        {
            image.color = Color.clear;
            return;
        }
        
        // DishData'dan renk bul
        bool found = false;
        GameColors color = GameColors.Red; // Varsayılan
        
        foreach (var dish in dishData.dishes)
        {
            if (dish.horizontalSlot == x && dish.verticalSlot == y)
            {
                color = dish.dishColor;
                found = true;
                break;
            }
        }
        
        // Renk ayarla
        if (found)
            image.color = GetColorFromEnum(color);
        else
            image.color = Color.clear;
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
            default: return new Color(1f, 1f, 1f, 1f);
        }
    }
    
    [ContextMenu("Clear Grid")]
    public void ClearExistingGrid()
    {
        if (generatedPanels != null)
        {
            foreach (var panel in generatedPanels)
            {
                if (panel != null)
                {
                    DestroyImmediate(panel);
                }
            }
        }
        
        generatedPanels = null;
        generatedImages = null;
    }
    
    [ContextMenu("Update Colors")]
    public void UpdateColors()
    {
        if (generatedImages == null) return;
        
        for (int x = 0; x < generatedImages.Length; x++)
        {
            if (generatedImages[x] != null)
            {
                for (int y = 0; y < generatedImages[x].Length; y++)
                {
                    if (generatedImages[x][y] != null)
                    {
                        SetImageColor(generatedImages[x][y], x, y);
                    }
                }
            }
        }
    }
} 