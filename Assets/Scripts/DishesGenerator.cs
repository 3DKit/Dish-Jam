using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Enums;

public class SlotGenerator : MonoBehaviour
{
    [HideInInspector] public LevelData levelData;
    [HideInInspector] public GameObject slotPanel;
    [HideInInspector] public GameObject slotPrefab;

    [HideInInspector] public TextMeshProUGUI levelNumberText;
    [HideInInspector] public TextMeshProUGUI timeLimitText;

    [Header("Generated Objects")]
    public GameObject[] generatedSlots;

    [ContextMenu("Generate Level")]
    public void GenerateLevel()
    {
        if (levelData == null)
        {
            Debug.LogError("LevelData is null!");
            return;
        }
        ClearExistingLevel();
        GenerateSlots();
        Debug.Log($"Level {levelData.levelNumber} generated with {levelData.availableSlots} slots");
    }

    private void GenerateSlots()
    {
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
        slot = Instantiate(slotPrefab, slotPanel.transform);
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

    //Have empy slots
    public bool IsSlotsEmpty()
    {
        if (generatedSlots == null) return false;
        foreach (var slot in generatedSlots)
        {
            SlotInfo slotInfo = slot.GetComponent<SlotInfo>();
            if (slotInfo != null && !slotInfo.isOccupied)
            {
                return true;
            }
        }
        return false;
    }

    public void PlacePlateToNextSlot(GameObject plate)
    {
        if (generatedSlots == null) return;
        for (int i = 0; i < generatedSlots.Length; i++)
        {
            Debug.Log("PlacePlateToNextSlot called");
            SlotInfo slotInfo = generatedSlots[i].GetComponent<SlotInfo>();
            if (slotInfo != null && !slotInfo.isOccupied)
            {
                plate.transform.SetParent(generatedSlots[i].transform, false);
                plate.transform.SetAsLastSibling();
                slotInfo.isOccupied = true;
                // RectTransform offset düzelt
                RectTransform plateRect = plate.GetComponent<RectTransform>();
                if (plateRect != null)
                {
                    plateRect.anchoredPosition = Vector2.zero;
                    plateRect.localPosition = Vector3.zero;
                }
                break;
            }
        }
    }
}