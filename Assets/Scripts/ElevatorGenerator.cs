using UnityEngine;
using UnityEngine.UI;
using static Enums;

public class ElevatorGenerator : MonoBehaviour
{
    [HideInInspector] public LevelData levelData;
    [HideInInspector] public Transform currentColorsPanel;
    [HideInInspector] public Transform nextColorsPanel;
    [HideInInspector] public GameObject colorImagePrefab; // Basit bir Image objesi prefabı
    public int currentFloorIndex = 0;

    public void GenerateElevatorColors()
    {
        if (levelData == null || levelData.floors == null || levelData.floors.Length == 0)
        {
            Debug.LogError("LevelData veya floors eksik!");
            return;
        }
        // Mevcut renk objelerini temizle
        ClearPanel(currentColorsPanel);
        ClearPanel(nextColorsPanel);
        // Current Colors
        if (currentFloorIndex < levelData.floors.Length)
        {
            CreateColorImages(currentColorsPanel, levelData.floors[currentFloorIndex].floorColors);
        }
        // Next Colors
        if (currentFloorIndex + 1 < levelData.floors.Length)
        {
            CreateColorImages(nextColorsPanel, levelData.floors[currentFloorIndex + 1].floorColors);
        }
    }

    private void CreateColorImages(Transform panel, GameColors[] colors)
    {
        foreach (var color in colors)
        {
            GameObject imgObj = colorImagePrefab != null ? Instantiate(colorImagePrefab, panel) : new GameObject("ColorImage", typeof(Image));
            if (colorImagePrefab == null) imgObj.transform.SetParent(panel, false);
            Image img = imgObj.GetComponent<Image>();
            if (img == null) img = imgObj.AddComponent<Image>();
            img.color = GetColorFromEnum(color);
        }
    }

    public void ClearPanel(Transform panel)
    {
        foreach (Transform child in panel)
        {
            DestroyImmediate(child.gameObject);
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