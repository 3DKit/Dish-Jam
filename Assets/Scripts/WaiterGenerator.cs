using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using static Enums;

public class WaiterGenerator : MonoBehaviour
{
    [HideInInspector] public LevelData levelData;
    [HideInInspector] public Transform waiterPanel;
    [HideInInspector] public GameObject waiterPrefab;
    public int currentFloorIndex = 0;

    public void GenerateWaiters()
    {
        if (levelData == null || waiterPanel == null || waiterPrefab == null)
        {
            Debug.LogError("LevelData, WaiterPanel veya WaiterPrefab eksik!");
            return;
        }
        ClearWaiters();
        if (levelData.floors == null || levelData.floors.Length == 0) return;
        if (currentFloorIndex >= levelData.floors.Length) return;
        var colors = levelData.floors[currentFloorIndex].floorColors;

        // HorizontalLayoutGroup'u aktif et
        var layout = waiterPanel.GetComponent<HorizontalLayoutGroup>();
        if (layout != null) layout.enabled = true;

        foreach (var color in colors)
        {
            GameObject waiter = Instantiate(waiterPrefab, waiterPanel);
            Image img = waiter.GetComponent<Image>();
            if (img == null) img = waiter.AddComponent<Image>();
            img.color = GetColorFromEnum(color);
        }

        // 1 kare bekle ve devre dışı bırak
        StartCoroutine(DisableLayoutAfterDelay(layout));
    }

    private IEnumerator DisableLayoutAfterDelay(HorizontalLayoutGroup layout)
    {
        yield return new WaitForSeconds(1f);
        layout.enabled = false;
        StopCoroutine(DisableLayoutAfterDelay(layout));
    }

    public void ClearWaiters()
    {
        foreach (Transform child in waiterPanel)
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