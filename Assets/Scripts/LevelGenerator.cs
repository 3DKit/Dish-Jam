using UnityEngine;
using TMPro;

public class LevelGenerator : MonoBehaviour
{
    [Header("References")]
    public LevelData levelData;
    public SlotGenerator slotGenerator;
    public ElevatorGenerator elevatorGenerator;
    public PlateGridGenerator plateGridGenerator;
    public WaiterGenerator waiterGenerator;
    public PlateButtonManager plateButtonManager;

    [Header("UI References")]
    public Transform currentColorsPanel;
    public Transform nextColorsPanel;
    public GameObject colorImagePrefab;
    public GameObject slotPanel;
    public GameObject slotPrefab;
    public TextMeshProUGUI levelNumberText;
    public TextMeshProUGUI timeLimitText;

    [Header("Plate Grid References")]
    public GameObject platesPanel;
    public GameObject panelPrefab;
    public GameObject imagePrefab;

    [Header("Waiter References")]
    public Transform waiterPanel;
    public GameObject waiterPrefab;

    private void Awake()
    {
        if (slotGenerator != null)
        {
            slotGenerator.levelData = levelData;
            slotGenerator.slotPanel = slotPanel;
            slotGenerator.slotPrefab = slotPrefab;
            slotGenerator.levelNumberText = levelNumberText;
            slotGenerator.timeLimitText = timeLimitText;
        }
        if (elevatorGenerator != null)
        {
            elevatorGenerator.levelData = levelData;
            elevatorGenerator.currentColorsPanel = currentColorsPanel;
            elevatorGenerator.nextColorsPanel = nextColorsPanel;
            elevatorGenerator.colorImagePrefab = colorImagePrefab;
        }
        if (plateGridGenerator != null)
        {
            plateGridGenerator.dishData = levelData.dishData;
            plateGridGenerator.platesPanel = platesPanel;
            plateGridGenerator.panelPrefab = panelPrefab;
            plateGridGenerator.imagePrefab = imagePrefab;
        }
        if (waiterGenerator != null)
        {
            waiterGenerator.levelData = levelData;
            waiterGenerator.waiterPanel = waiterPanel;
            waiterGenerator.waiterPrefab = waiterPrefab;
        }
    }

    private void Start()
    {
        StartLevel();
    }

    private void Update()
    {
        UpdateUITexts();
    }

    public void StartLevel()
    {
        if (levelData == null) { Debug.LogError("LevelData yok!"); return; }
        if (slotGenerator != null) slotGenerator.GenerateLevel();
        if (elevatorGenerator != null) elevatorGenerator.GenerateElevatorColors();
        if (plateGridGenerator != null) plateGridGenerator.GeneratePlateGrid();
        if (waiterGenerator != null) waiterGenerator.GenerateWaiters();
        if (plateButtonManager != null) plateButtonManager.GetAllPlatePanels();
    }

    public void ClearLevel()
    {
        if (slotGenerator != null) slotGenerator.ClearExistingLevel();
        if (elevatorGenerator != null) elevatorGenerator.ClearPanel(elevatorGenerator.currentColorsPanel);
        if (elevatorGenerator != null) elevatorGenerator.ClearPanel(elevatorGenerator.nextColorsPanel);
        if (plateGridGenerator != null) plateGridGenerator.ClearExistingGrid();
        if (waiterGenerator != null) waiterGenerator.ClearWaiters();
    }

    public void UpdateElevator()
    {
        if (elevatorGenerator != null) elevatorGenerator.GenerateElevatorColors();
    }

    public void UpdateDishes()
    {
        if (slotGenerator != null) slotGenerator.GenerateLevel();
    }

    public void UpdatePlateButtons()
    {
        if (plateButtonManager != null) plateButtonManager.GetAllPlatePanels();
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
}