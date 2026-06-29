using UnityEngine;
using TMPro;

[System.Serializable]
public class CarPartGroup
{
    [Header("Name of the parts")]
    [Tooltip("E.g., Tyres, Hood, Spoiler")]
    public string PartName;

    [Header("Part Variations")]
    [Tooltip("If this part has multiple objects, add them as a group")]
    public GameObject[] Options;

    [HideInInspector] public int currentOptionIndex = 0;
    [HideInInspector] public int GroupSize => Options.Length;

    public string GetPartName()
    {
        return PartName;
    }
}

public class CarUpgradeManager : MonoBehaviour
{
    [SerializeField] private CarSelector CarSelection;

    [Header("UI References")]
    [Tooltip("Assign the UI TextMeshPro element that displays the array")]
    [SerializeField] private TextMeshProUGUI arreySizeText;
    public CarPartGroup[] carPartGroups;
    
    [Header("Car Object Reference")]
    [Tooltip("The camera will rotate around this object")]
    [SerializeField] private GameObject carModel;

    private int currentGroupIndex = 0;
    private int maxGroups;

    void Start()
    {
        InitializeCarParts();
        UpdateArrayText();
        CarSelection.HighlightCurrentGroup(GetCurrentGooupIndex());
    }

    void Update()
    {
        // Switch part group (e.g., tyres ? hood ? spoiler)
        if (Input.GetKeyDown(KeyCode.W))
        {
            CycleCarPartGroups(-1);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            CycleCarPartGroups(1);
        }

        // Cycle options within the selected group
        if (Input.GetKeyDown(KeyCode.D))
        {
            CycleOption(1);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            CycleOption(-1);
        }
    }
    public void InitializeCarParts()
    {
        maxGroups = carPartGroups.Length;

        for (int i = 0; i < carPartGroups.Length; i++)
        {
            if (carPartGroups[i].Options == null || carPartGroups[i].Options.Length == 0)
            {
                Debug.LogError($"Part group '{carPartGroups[i].PartName}' has no options assigned.");
                continue;
            }
            currentGroupIndex = i;
            ApplyPart(i, 0);
        }
    }

    void CycleOption(int direction)
    {
        CarPartGroup group = carPartGroups[currentGroupIndex];
        if (group.Options.Length == 0) return;

        group.currentOptionIndex = (group.currentOptionIndex + direction + group.Options.Length) % group.Options.Length;
        ApplyPart(currentGroupIndex, group.currentOptionIndex);
        UpdateArrayText();
        Debug.Log($"Swapped {group.PartName} to option {group.currentOptionIndex}");
    }

    void CycleCarPartGroups(int direction)
    {
        currentGroupIndex = (currentGroupIndex + direction + maxGroups) % maxGroups;
        CarSelection.HighlightCurrentGroup(currentGroupIndex);
        UpdateArrayText();
        Debug.Log("Selected part group: " + carPartGroups[currentGroupIndex].PartName);
    }

    void ApplyPart(int groupIndex, int optionIndex)
    {
        for (int i = 0; i < carPartGroups[groupIndex].Options.Length; i++)
        {
            if (carPartGroups[groupIndex].Options[i] != null)
                carPartGroups[groupIndex].Options[i].SetActive(i == optionIndex);
        }
    }

    void UpdateArrayText()
    {
        CarPartGroup groups = carPartGroups[currentGroupIndex];

        if (groups.GroupSize > 0)
        {
            int currentOption = groups.currentOptionIndex + 1;
            arreySizeText.text = $"{currentOption}/{groups.GroupSize}";
        }
        else
        {
            arreySizeText.text = "0/0";
        }
    }

    public int GetCurrentGooupIndex()
    {
        return currentGroupIndex;
    }
}