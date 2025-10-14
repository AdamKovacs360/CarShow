using UnityEngine;
using Unity.Cinemachine;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform cameraTransform;
    private Transform playerTransform;
    [SerializeField] private CinemachineCamera carSelectorCamera;
    [SerializeField] private CinemachineCamera PlayerCamera;
    [SerializeField] private GameObject carSelectionCanvas;
    [SerializeField] private CarSelector _carSelector; // Changed type from GameObject to CarSelector
    [SerializeField] private GameObject interactionCanvas;
    private float moveSpeed = 5f;
    CharacterController controller;
    private bool isPlayerActive;
    private bool isPlayerInTrigger;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerTransform = player.transform;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerCamera.Priority = 10;
        carSelectorCamera.Priority = 0;
        isPlayerActive = true;
        isPlayerInTrigger = false;
        carSelectionCanvas.SetActive(false);
        interactionCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
            //Exit car selection
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ExitFromCarSelection();
            }

        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.E))
            {
                EnterToCarSelection();
            }

        // Player movement
        if (isPlayerActive)
        {
            Vector3 inputDirection = Vector3.zero;

            if (Input.GetKey(KeyCode.W))
            {
                inputDirection += Vector3.forward;
            }
            if (Input.GetKey(KeyCode.S))
            {
                inputDirection += Vector3.back;
            }
            if (Input.GetKey(KeyCode.A))
            {
                inputDirection += Vector3.left;
            }
            if (Input.GetKey(KeyCode.D))
            {
                inputDirection += Vector3.right;
            }

            if(inputDirection.sqrMagnitude > 0.01f)
            {
                // Convert input direction to world space relative to camera
                Vector3 moveDirection = cameraTransform.TransformDirection(inputDirection.normalized);
                moveDirection.y = 0; // Keep movement horizontal
                moveDirection.Normalize();

                // Move the player
                controller.Move(moveDirection * moveSpeed * Time.deltaTime);

                // Rotate player to face movement direction
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }

    }
    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            isPlayerInTrigger = true;
            Debug.Log("Player is inside trigger area");

            if (isPlayerActive)
            {
                interactionCanvas.SetActive(true);
            }
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            isPlayerInTrigger = false;
            interactionCanvas.SetActive(false);
            Debug.Log("Player exited trigger area");
        }
    }

    void ExitFromCarSelection()
    {
        carSelectionCanvas.SetActive(false);
        PlayerCamera.Priority = 10;
        carSelectorCamera.Priority = 0;
        _carSelector.enabled = false;
        isPlayerActive = true;
        _carSelector.DisableAllCarSelection();

        Debug.Log("Exited car selection");
    }

    void EnterToCarSelection()
    {
        Debug.Log("Entered car selection");
        interactionCanvas.SetActive(false);
        _carSelector.enabled = true;
        isPlayerActive = false;
        carSelectionCanvas.SetActive(true);
        carSelectorCamera.Priority = 10;
        PlayerCamera.Priority = 0;
        _carSelector.CarSelection(0);
    }
}
