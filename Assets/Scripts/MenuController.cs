using TMPro;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private PlayerController player;
    [SerializeField] private Button playButton;
    [SerializeField] private Button quiteButton;
    [SerializeField] private CinemachineCamera menuCamera;
    [SerializeField] private CinemachineCamera playerCamera;

    public void OnPlayButtonClicked()
    {
        mainMenuCanvas.SetActive(false);
        playerCamera.Priority = 10;
        menuCamera.Priority = 0;
        player.isPlayerActive = true;
    }

    public void OnExitButtonClicked()
    {
        Debug.Log("Quit Application");
        Application.Quit();
    }
}
