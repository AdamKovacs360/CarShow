using TMPro;
using UnityEngine;
using Unity.Cinemachine;

[System.Serializable]
public class CarModel
{
    public string ModelName;
    public GameObject carPrefab;
    public CinemachineCamera carCamera;
}

public class CarSelector : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [Header("UI References")]
    [Tooltip("Assign the UI TextMeshPro element that displays the name")]
    [SerializeField] private TextMeshProUGUI modelNameText;
    [SerializeField] private CarModel[] carModels;

    [SerializeField] private TextMeshProUGUI[] partNametext;


    void Start()
    {
        DisableAllCarSelection();
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.isPlayerActive)
        {
            // Car selection
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                CarSelection(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                CarSelection(1);
            }
        }
    }

    public void CarSelection(int CarIndex)
    {
        if (CarIndex < 0 || CarIndex >= carModels.Length)
        {
            Debug.LogError("Invalid car index: " + CarIndex);
            return;
        }

        for (int i = 0; i < carModels.Length; i++)
        {
            //Enable selected car and focus its camera
            if (carModels[i].carPrefab != null && i == CarIndex)
            {
                carModels[i].carCamera.Priority = 10;
                carModels[i].carPrefab.GetComponent<CarUpgradeManager>().enabled = true;
                modelNameText.text = carModels[i].ModelName;
                Debug.Log("Selected car: " + carModels[i].ModelName);

                //Restet and update part names Text in the UI
                ResetPartNametext();
                for (int j = 0; j < carModels[i].carPrefab.GetComponent<CarUpgradeManager>().carPartGroups.Length; j++)
                {
                    if (carModels[i].carPrefab.GetComponent<CarUpgradeManager>().carPartGroups[j] != null)
                    {
                        partNametext[j].text = carModels[i].carPrefab.GetComponent<CarUpgradeManager>().carPartGroups[j].GetPartName();
                        HighlightCurrentGroup(carModels[i].carPrefab.GetComponent<CarUpgradeManager>().GetCurrentGooupIndex());
                    }
                }
                carModels[i].carPrefab.GetComponent<CarUpgradeManager>().carPartGroups[1].GetPartName();
            }
            //Disable other cars and their cameras
            else if (carModels[i].carPrefab != null)
            {
                carModels[i].carCamera.Priority = 0;
                carModels[i].carPrefab.GetComponent<CarUpgradeManager>().enabled = false;
            }
        }
    }

   public void DisableAllCarSelection()
    {
        for (int i = 0; i < carModels.Length; i++)
        {
            if (carModels[i].carPrefab != null)
            {
                carModels[i].carPrefab.GetComponent<CarUpgradeManager>().enabled = false;
            }
        }
    }

    void ResetPartNametext()
    {
        for (int i = 0; i < partNametext.Length; i++)
        {
            partNametext[i].text = "";
        }
    }
    public void HighlightCurrentGroup(int num)
    {
        for (int i = 0; i < carModels.Length; i++)
        {
            for (int j = 0; j < carModels[i].carPrefab.GetComponent<CarUpgradeManager>().carPartGroups.Length; j++)
            {
                if (carModels[i].carPrefab.GetComponent<CarUpgradeManager>().carPartGroups[j] != null)
                {
                    partNametext[j].color = (j == num) ? Color.yellow : Color.white;
                }
            }
        }
    }
}
