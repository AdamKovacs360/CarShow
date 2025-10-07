using TMPro;
using UnityEngine;

[System.Serializable]
public class CarModel
{
    public string ModelName;
    public GameObject carPrefab;
}

public class CarSelector : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Assign the UI TextMeshPro element that displays the name")]
    [SerializeField] private TextMeshProUGUI modelNameText;
    [SerializeField] private CarModel[] carModels;
    [SerializeField] private Camera mainCamera;

    void Start()
    {
        CarSelection(0);
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

    void CarSelection(int CarIndex)
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
                carModels[i].carPrefab.GetComponent<CarUpgradeManager>().enabled = true;
                mainCamera.transform.position = carModels[i].carPrefab.transform.position + new Vector3(-1f, 1f, -10f);
                modelNameText.text = carModels[i].ModelName;
                Debug.Log("Selected car: " + carModels[i].ModelName);
            }
            else if (carModels[i].carPrefab != null)
            {
                carModels[i].carPrefab.GetComponent<CarUpgradeManager>().enabled = false;
            }
        }
    }
}
