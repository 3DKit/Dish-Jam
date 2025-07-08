using UnityEngine;
using System.Collections.Generic;

public class PlateButtonManager : MonoBehaviour
{
    private LevelGenerator levelGenerator;
    private GameObject[] platePanels;
    private List<PlateButton> plateButtons = new List<PlateButton>();

    private void Awake()
    {
        Debug.Log("PlateButtonManager started");
        levelGenerator = this.GetComponent<LevelGenerator>();
    }

    public void GetAllPlatePanels()
    {
        if (levelGenerator != null && levelGenerator.plateGridGenerator != null)
        {
            platePanels = levelGenerator.plateGridGenerator.generatedPanels;
        }
        else
        {
            platePanels = new GameObject[0];
        }
        GetAllPlateButtons();
    }

    public void GetAllPlateButtons()
    {
        foreach (var panel in platePanels)
        {
            var buttons = panel.GetComponentsInChildren<PlateButton>();
            foreach (var button in buttons)
            {
                RegisterPlateButton(button);
            }
        }
        UpdateAllButtons();
    }

    public void RegisterPlateButton(PlateButton button)
    {
        if (!plateButtons.Contains(button))
            plateButtons.Add(button);
        UpdateAllButtons();
    }

    public void UnregisterPlateButton(PlateButton button)
    {
        if (plateButtons.Contains(button))
            plateButtons.Remove(button);
        UpdateAllButtons();
    }

    public void UpdateAllButtons()
    {
        foreach (var button in plateButtons)
        {
            button.UpdateInteractableState(IsButtonTop(button));
        }
    }

    public bool IsButtonTop(PlateButton button)
    {
        if (button.transform.parent == null) return false;
        int myIndex = button.transform.GetSiblingIndex();
        int childCount = button.transform.parent.childCount;
        return myIndex == 0;
    }

    public void SetOnlyTopPlateButtonsInteractable()
    {
        if (platePanels == null) return;
        foreach (var panel in platePanels)
        {
            if (panel == null) continue;
            int childCount = panel.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                var child = panel.transform.GetChild(i);
                var plateButton = child.GetComponent<PlateButton>();
                if (plateButton != null)
                {
                    bool isTop = (i == 0);
                    plateButton.UpdateInteractableState(isTop);
                }
            }
        }
    }

    public void OnPlateButtonClicked(PlateButton button)
    {
        if (!levelGenerator.slotGenerator.IsSlotsEmpty())
        {
            Debug.Log("Slots are not empty");
            return;
        }
        GameObject parentPanel = button.transform.parent.gameObject;
        if (levelGenerator != null && levelGenerator.slotGenerator != null)
        {
            levelGenerator.slotGenerator.PlacePlateToNextSlot(button.gameObject);
        }
        MoveAllPlateButtonsToTop(parentPanel);
        UnregisterPlateButton(button);
        SetOnlyTopPlateButtonsInteractable();
    }

    public void MoveAllPlateButtonsToTop(GameObject panel)
    {
        Debug.Log(panel.name);
        float spacing = levelGenerator.levelData.dishData.imageSpacing;
        Debug.Log(spacing);
        int childCount = panel.transform.childCount;
        Debug.Log(childCount);
        for (int i = 0; i < childCount; i++)
        {
            var child = panel.transform.GetChild(i);
            var rect = child.GetComponent<RectTransform>();
            if (rect != null)
            {
                //rectransform.y add spacing
                rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y + spacing);
            }
        }
    }
}