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
    [Header("UI References")]
    [Tooltip("Assign the UI TextMeshPro element that displays the name")]
    [SerializeField] private TextMeshProUGUI modelNameText;
    [SerializeField] private CarModel[] carModels;

    void Start()
    {
        DisableAllCarSelection();
    }

    // Update is called once per frame
    void Update()
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

    public void CarSelection(int CarIndex)
    {
        if (CarIndex < 0 || CarIndex >= carModels.Length)
        {
            Debug.LogError("Invalid car index: " + CarIndex);
            return;
        }

        for (int i = 0; i < carModels.Length; i++)
        {
            if (carModels[i].carPrefab != null && i == CarIndex)
            {
                carModels[i].carCamera.Priority = 10;
                carModels[i].carPrefab.GetComponent<CarUpgradeManager>().enabled = true;
                modelNameText.text = carModels[i].ModelName;
                Debug.Log("Selected car: " + carModels[i].ModelName);
            }
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
}
