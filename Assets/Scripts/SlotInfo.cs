using UnityEngine;
using UnityEngine.UI;
using static Enums;

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