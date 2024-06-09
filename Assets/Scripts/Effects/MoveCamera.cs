/// <remarks>
/// Author: Johnathan
/// Date Created: 9/6/2024
/// Contributors: Assisted by Github Copilot
/// </remarks>
// <summary>
/// This class moves the camera to and fro a target position.
/// </summary>

using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// This class moves the camera to and fro a target position.
/// </summary>
public class MoveCamera : MonoBehaviour
{
    [SerializeField] private Transform target; // The target position to move to
    [SerializeField] private float time = 1f; // Time in which the movement takes place
    [SerializeField] private float proximityCheck = 0.02f; // How close the camera needs to be to the target position to stop moving

    private Vector3 startPosition; // The starting position of the camera
    private Quaternion startRotation; // The starting rotation of the camera
    private bool isMoving = false;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    /// <summary>
    /// Moves the camera to the target position
    /// </summary>
    /// <param name="afterCameraMovement">The action to perform after the camera has moved</param>
    public void MoveToGamePosition(Action afterCameraMovement = null)
    {
        StartCoroutine(MoveToPositionCoroutine(target.position, target.rotation, afterCameraMovement));
    }

    /// <summary>
    /// Moves the camera to the starting position
    /// </summary>
    /// <param name="afterCameraMovement">The action to perform after the camera has moved</param>
    public void MoveToStartPosition(Action afterCameraMovement = null)
    {
        StartCoroutine(MoveToPositionCoroutine(startPosition, startRotation, afterCameraMovement));
    }

    /// <summary>
    /// Moves the camera to the target position
    /// </summary>
    /// <param name="targetPosition">The target position to move to</param>
    /// <param name="targetRotation">The target rotation to move to</param>
    /// <param name="afterCameraMovementCallback">The action to perform after the camera has moved</param>
    private IEnumerator MoveToPositionCoroutine(Vector3 targetPosition, Quaternion targetRotation, Action afterCameraMovementCallback = null)
    {
        if (isMoving)
        {
            Debug.LogWarning("Camera is already moving");
            yield break;
        }
        isMoving = true;
        float elapsedTime = 0;
        Vector3 velocity = Vector3.zero;

        while (Vector3.Distance(transform.position, targetPosition) > proximityCheck)
        {
            // Move
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, time);

            // Rotate
            float rotationSpeed = Quaternion.Angle(transform.rotation, targetRotation) / time;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        /*
         * This code makes sure the camera is exactly at the target position and rotation at the end,
         * But I commented this out because it caused a noticeable bump at the end of the movement, and the camera gets close enough without it
         * Alternatively you could reduce proximityCheck until the correction is not noticeable
         */
        // transform.position = targetTransform.position;
        // transform.rotation = targetTransform.rotation;

        afterCameraMovementCallback?.Invoke();
        isMoving = false;
    }
}
