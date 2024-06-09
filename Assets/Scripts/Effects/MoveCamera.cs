using UnityEngine;
using System.Collections;
using System;
using Unity.VisualScripting;

public class MoveCamera : MonoBehaviour
{
    private Transform startTransform;

    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private float time = 1f;
    [SerializeField] private float proximityCheck = 0.02f;
    private bool isMoving = false;  

    private void Start()
    {
        startTransform = transform;
    }

    // coroutine to move camera to desired position
    public void MoveToGamePosition(Action afterCameraMovement = null)
    {
        StartCoroutine(MoveToPositionCoroutine(target, afterCameraMovement));
    }

    // coroutine to move camera to start position
    public void MoveToStartPosition(Action afterCameraMovement = null)
    {
        StartCoroutine(MoveToPositionCoroutine(startTransform, afterCameraMovement));
    }

    private IEnumerator MoveToPositionCoroutine(Transform targetTransform, Action afterCameraMovementCallback = null)
    {
        if (isMoving)
        {
            Debug.LogWarning("Camera is already moving");
            yield break;
        }
        isMoving = true;
        float elapsedTime = 0;
        Vector3 velocity = Vector3.zero;
        Quaternion startRotation = transform.rotation;

        while (Vector3.Distance(transform.position, targetTransform.position) > proximityCheck)
        {
            // Move
            transform.position = Vector3.SmoothDamp(transform.position, targetTransform.position, ref velocity, time);

            // Rotate
            float rotationSpeed = Quaternion.Angle(transform.rotation, targetTransform.rotation) / time;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetTransform.rotation, rotationSpeed * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // This code makes sure the object is exactly at the target position and rotation at the end,
        // But I commented this out because it caused a noticeable bump at the end of the movement, and the camera gets close enough without it
        // Alternatively you could reduce proximityCheck until the correction is not noticeable
        // transform.position = targetTransform.position;
        // transform.rotation = targetTransform.rotation;

        afterCameraMovementCallback?.Invoke();
        isMoving = false;
    }

}