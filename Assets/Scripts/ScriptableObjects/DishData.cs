using UnityEngine;
using static Enums;

[CreateAssetMenu(fileName = "DishData", menuName = "DishJam/Dish Data")]
public class DishData : ScriptableObject
{
    [Header("Dish Settings")]
    public int verticalSlots;
    public int horizontalSlots;
    public float imageSpacing = 84f;
    public float imageSize = 64f;

    [System.Serializable]
    public class DishInfo
    {
        public GameColors dishColor;
        public int horizontalSlot;
        public int verticalSlot;
    }

    public DishInfo[] dishes;
} 