using UnityEngine;
using static Enums;

[CreateAssetMenu(fileName = "LevelData", menuName = "DishJam/Level Data")]
public class LevelData : ScriptableObject
{
    [System.Serializable]
    public class LevelFloor
    {
        public GameColors[] floorColors;
    }

    [Header("Level Settings")]
    public int levelNumber;
    public int availableSlots = 6;
    public float timeLimit = 60f;
    
    [Header("Floor Configuration")]
    public LevelFloor[] floors;
    
    [Header("References")]
    public DishData dishData;
} 