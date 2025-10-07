using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    [SerializeField] private Transform target;   // Object to orbit around
    [SerializeField] private float distance = 5f; // Default distance from target
    [SerializeField] private float rotationSpeed = 3f; // Mouse sensitivity
    [SerializeField] private float zoomSpeed = 2f;     // Scroll wheel zoom
    [SerializeField] private float minDistance = 2f;   // Clamp zoom
    [SerializeField] private float maxDistance = 15f;

    private float yaw = 0f;
    private float pitch = 0f;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("CameraOrbit: No target assigned!");
            enabled = false;
            return;
        }

        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Rotate only when holding MMB
        if (Input.GetMouseButton(2))
        {
            yaw += Input.GetAxis("Mouse X") * rotationSpeed;
            pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;

            // Clamp pitch so camera doesn’t flip
            pitch = Mathf.Clamp(pitch, -40f, 80f);
        }

        // Zoom with scroll wheel
        distance -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        // Update camera position & rotation
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 position = target.position - (rotation * Vector3.forward * distance);

        transform.rotation = rotation;
        transform.position = position;
    }
}
