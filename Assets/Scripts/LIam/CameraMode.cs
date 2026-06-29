using System.Collections;
using UnityEngine;

public class CameraMode : MonoBehaviour
{
    [Header("Keys")]
    public KeyCode toggleKey = KeyCode.C;
    public KeyCode nextKey = KeyCode.RightArrow;
    public KeyCode prevKey = KeyCode.LeftArrow;

    [Header("Camera Points")]
    public CameraPoint[] cameraPoints;

    [Header("Camera Controller")]
    public MonoBehaviour cameraController; // drag your mouse look script here

    [Header("Transition")]
    public float moveDuration = 0.8f;
    public AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    bool inCameraMode = false;
    bool isMoving = false;
    int currentIndex = 0;

    Camera mainCam;
    Vector3 originalPosition;
    Quaternion originalRotation;

    void Start()
    {
        mainCam = Camera.main;
        originalPosition = mainCam.transform.position;
        originalRotation = mainCam.transform.rotation;
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            if (inCameraMode)
                ExitCameraMode();
            else
                EnterCameraMode();
        }

        if (inCameraMode)
            Input.ResetInputAxes();

        if (!inCameraMode || isMoving) return;

        if (Input.GetKeyDown(nextKey))
            StartCoroutine(MoveToPoint((currentIndex + 1) % cameraPoints.Length));

        if (Input.GetKeyDown(prevKey))
            StartCoroutine(MoveToPoint((currentIndex - 1 + cameraPoints.Length) % cameraPoints.Length));
    }

    void EnterCameraMode()
    {
        inCameraMode = true;
        currentIndex = 0;

        if (cameraController != null)
            cameraController.enabled = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        StartCoroutine(MoveToPoint(currentIndex));
    }

    void ExitCameraMode()
    {
        inCameraMode = false;

        if (cameraController != null)
            cameraController.enabled = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        StartCoroutine(MoveToTransform(originalPosition, originalRotation));
    }

    IEnumerator MoveToPoint(int index)
    {
        currentIndex = index;
        CameraPoint point = cameraPoints[index];

        Vector3 targetPos = point.anchor.position;
        Quaternion targetRot;

        if (point.lookTarget != null)
        {
            Vector3 direction = (point.lookTarget.position - point.anchor.position).normalized;
            targetRot = Quaternion.LookRotation(direction, Vector3.up);
        }
        else
        {
            targetRot = point.anchor.rotation;
        }

        yield return StartCoroutine(MoveToTransform(targetPos, targetRot));
    }

    IEnumerator MoveToTransform(Vector3 targetPos, Quaternion targetRot)
    {
        isMoving = true;

        Vector3 startPos = mainCam.transform.position;
        Quaternion startRot = mainCam.transform.rotation;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = moveCurve.Evaluate(elapsed / moveDuration);
            mainCam.transform.position = Vector3.Lerp(startPos, targetPos, t);
            mainCam.transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }

        mainCam.transform.position = targetPos;
        mainCam.transform.rotation = targetRot;

        isMoving = false;
    }

    void OnDrawGizmos()
    {
        if (cameraPoints == null) return;

        foreach (CameraPoint point in cameraPoints)
        {
            if (point.anchor == null) continue;

            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(point.anchor.position, 0.15f);

            if (point.lookTarget != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(point.anchor.position, point.lookTarget.position);
                Gizmos.DrawSphere(point.lookTarget.position, 0.1f);
            }
            else
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawRay(point.anchor.position, point.anchor.forward * 1.5f);
            }
        }
    }
}

[System.Serializable]
public class CameraPoint
{
    public Transform anchor;
    public Transform lookTarget;
}