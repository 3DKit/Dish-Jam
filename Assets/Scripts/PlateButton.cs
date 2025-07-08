using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlateButton : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector] public SlotGenerator slotGenerator;
    private PlateButtonManager manager;

    private void Start()
    {
        slotGenerator = FindAnyObjectByType<SlotGenerator>();
        manager = FindAnyObjectByType<PlateButtonManager>();
    }

    public void UpdateInteractableState(bool isTop)
    {
        this.GetComponent<Image>().color = new Color(this.GetComponent<Image>().color.r, this.GetComponent<Image>().color.g, this.GetComponent<Image>().color.b, isTop ? 1f : 0.5f);
        enabled = isTop;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!enabled) return;
        if (manager != null)
        {
            manager.OnPlateButtonClicked(this);
        }
    }
}