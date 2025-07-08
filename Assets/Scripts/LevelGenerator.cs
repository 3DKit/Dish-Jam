using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Enums;

public class LevelGenerator : MonoBehaviour
{
    [Header("References")]
    public LevelData levelData;
    public GameObject slotPanel;
    public GameObject slotPrefab;
    public PlateGridGenerator plateGridGenerator;
    
    [Header("UI References")]
    public TextMeshProUGUI levelNumberText;
    public TextMeshProUGUI timeLimitText;
    
    [Header("Generated Objects")]
    public GameObject[] generatedSlots;
    
    private void Start()
    {
        if (levelData != null)
        {
            GenerateLevel();
        }
    }
    
    [ContextMenu("Generate Level")]
    public void GenerateLevel()
    {
        if (levelData == null)
        {
            Debug.LogError("LevelData is null!");
            return;
        }
        
        // Mevcut level'i temizle
        ClearExistingLevel();
        
        // UI text'lerini güncelle
        UpdateUITexts();
        
        // Slot'ları oluştur
        GenerateSlots();
        
        // Plate grid'i oluştur
        GeneratePlateGrid();
        
        Debug.Log($"Level {levelData.levelNumber} generated with {levelData.availableSlots} slots");
    }
    
    private void UpdateUITexts()
    {
        if (levelNumberText != null)
        {
            levelNumberText.text = $"Level {levelData.levelNumber}";
        }
        
        if (timeLimitText != null)
        {
            int minutes = Mathf.FloorToInt(levelData.timeLimit / 60f);
            int seconds = Mathf.FloorToInt(levelData.timeLimit % 60f);
            timeLimitText.text = $"Time: {minutes:00}:{seconds:00}";
        }
    }
    
    private void GenerateSlots()
    {
        if (slotPanel == null)
        {
            Debug.LogError("SlotPanel is null!");
            return;
        }
        
        int slotCount = levelData.availableSlots;
        generatedSlots = new GameObject[slotCount];
        
        for (int i = 0; i < slotCount; i++)
        {
            GameObject slot = CreateSlot(i);
            generatedSlots[i] = slot;
        }
    }
    
    private GameObject CreateSlot(int slotIndex)
    {
        GameObject slot;
        
        if (slotPrefab != null)
        {
            slot = Instantiate(slotPrefab, slotPanel.transform);
        }
        else
        {
            // Varsayılan slot oluştur
            slot = new GameObject($"Slot_{slotIndex}");
            slot.transform.SetParent(slotPanel.transform, false);
            
            // Image component ekle
            Image slotImage = slot.AddComponent<Image>();
            slotImage.color = new Color(0.8f, 0.8f, 0.8f, 0.3f);
            
            // Layout Element ekle
            LayoutElement layoutElement = slot.AddComponent<LayoutElement>();
            layoutElement.preferredHeight = 80f;
            layoutElement.preferredWidth = 80f;
            layoutElement.minHeight = 60f;
            layoutElement.minWidth = 60f;
            layoutElement.flexibleHeight = 0;
            layoutElement.flexibleWidth = 0;
        }
        
        // Slot'a index bilgisi ekle
        SlotInfo slotInfo = slot.GetComponent<SlotInfo>();
        if (slotInfo == null)
        {
            slotInfo = slot.AddComponent<SlotInfo>();
        }
        slotInfo.slotIndex = slotIndex;
        slotInfo.isOccupied = false;
        
        return slot;
    }
    
    [ContextMenu("Clear Level")]
    public void ClearExistingLevel()
    {
        if (generatedSlots != null)
        {
            foreach (var slot in generatedSlots)
            {
                if (slot != null)
                {
                    DestroyImmediate(slot);
                }
            }
        }
        
        generatedSlots = null;
    }
    
    private void GeneratePlateGrid()
    {
        if (plateGridGenerator != null && levelData.dishData != null)
        {
            // PlateGridGenerator'a DishData'yı ata
            plateGridGenerator.dishData = levelData.dishData;
            
            // Plate grid'i oluştur
            plateGridGenerator.GeneratePlateGrid();
        }
        else
        {
            Debug.LogWarning("PlateGridGenerator or DishData is null!");
        }
    }
    
    [ContextMenu("Update UI Texts")]
    public void UpdateUITextsOnly()
    {
        if (levelData != null)
        {
            UpdateUITexts();
        }
    }
}

// Slot bilgilerini tutmak için yardımcı sınıf
public class SlotInfo : MonoBehaviour
{
    public int slotIndex;
    public bool isOccupied;
    public GameColors occupiedColor;
    
    public void SetOccupied(GameColors color)
    {
        isOccupied = true;
        occupiedColor = color;
        
        // Slot rengini güncelle
        Image slotImage = GetComponent<Image>();
        if (slotImage != null)
        {
            slotImage.color = GetColorFromEnum(color);
        }
    }
    
    public void SetEmpty()
    {
        isOccupied = false;
        
        // Slot rengini varsayılana döndür
        Image slotImage = GetComponent<Image>();
        if (slotImage != null)
        {
            slotImage.color = new Color(0.8f, 0.8f, 0.8f, 0.3f);
        }
    }
    
    private Color GetColorFromEnum(GameColors gameColor)
    {
        switch (gameColor)
        {
            case GameColors.Red: return new Color(1f, 0.3f, 0.3f, 0.8f);
            case GameColors.Green: return new Color(0.3f, 1f, 0.3f, 0.8f);
            case GameColors.Blue: return new Color(0.3f, 0.3f, 1f, 0.8f);
            case GameColors.Yellow: return new Color(1f, 1f, 0.3f, 0.8f);
            case GameColors.Purple: return new Color(0.7f, 0.3f, 0.7f, 0.8f);
            case GameColors.Orange: return new Color(1f, 0.6f, 0.3f, 0.8f);
            case GameColors.Pink: return new Color(1f, 0.5f, 0.8f, 0.8f);
            case GameColors.Brown: return new Color(0.7f, 0.5f, 0.3f, 0.8f);
            case GameColors.Gray: return new Color(0.6f, 0.6f, 0.6f, 0.8f);
            default: return new Color(1f, 1f, 1f, 0.8f);
        }
    }
} 