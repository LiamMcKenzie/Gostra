/// <remarks>
/// Author: Johnathan
/// Date Created: 7/6/2024
/// Contributors: Based on code found at https://www.youtube.com/watch?v=gzPjH7-gHLE, Assisted by Github Copilot
/// </remarks>
// <summary>
/// This is an alternative class to the SplineAnimate component that moves an object along a spline.
/// </summary>

using UnityEngine;
using UnityEngine.Splines;

/// <summary>
/// This class moves an object along a spline.
/// Should be attached to the object that needs to move along the spline.
/// </summary>
public class MoveAlongSpline : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private SplineContainer spline;  // The spline to move along
    [SerializeField] private float rotationSpeed = 5f;  // The speed at which to rotate the object

    private float currentDistance = 0f; // The current distance along the spline
    private float startSpeed;   // The starting speed of the object
    private float splineLength; // The length of the spline

    void Start()
    {
        splineLength = spline.CalculateLength();
        startSpeed = player.Speed;
        ResetPosition();
    }

    void Update()
    {
        if (player.IsIdle || player.IsFalling) { return; }
        
        UpdatePosition();
        UpdateRotation();

        // If the end of the spline is reached
        if (currentDistance >= 1f)
        {
            player.Fall();
        }
        else
        {
            // Increment the distance travelled along the spline
            currentDistance += CalculateMovement();
        }
    }

    /// <summary>
    /// Updates the rotation of the object to face the direction of the spline.
    /// </summary>
    private void UpdateRotation()
    {
        // Calculate the target rotation on the spline
        Vector3 targetDirection = spline.EvaluateTangent(currentDistance);

        // Rotate the character towards the target rotation on the spline
        if (targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Updates the position of the object along the spline.
    /// </summary>
    private void UpdatePosition()
    {
        // Calculate the target position on the spline
        Vector3 targetPosition = spline.EvaluatePosition(currentDistance);

        // Move the character towards the target position on the spline
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, player.Speed * Time.deltaTime);
    }

    /// <summary>
    /// Calculates the movement of the object along the spline.
    /// </summary>
    private float CalculateMovement() => player.Speed * Time.deltaTime / spline.CalculateLength();
    

    /// <summary>
    /// Resets the position of the object to the start of the spline.
    /// </summary>
    [ContextMenu("Reset Position")]
    public void ResetPosition()
    {
        currentDistance = 0f;
        transform.position = spline.EvaluatePosition(currentDistance);
    }
}