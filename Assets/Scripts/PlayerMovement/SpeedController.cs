/// <remarks>
/// Author: Johnathan
/// Date Created: 7/6/2024
/// Contributors: Assisted by Github Copilot
/// </remarks>
// <summary>
/// This adjusts the speed of the player based on their height.
/// </summary>

using UnityEngine;

/// <summary>
/// This class adjusts the speed of the player based on their height.
/// Should be attached to the object that needs to move along the spline.
/// </summary>
public class SpeedController : MonoBehaviour
{
    [SerializeField] private MoveAlongSpline moveAlongSpline;   // The MoveAlongSpline component to adjust speed of
    [SerializeField] private float speedReductionFactor = 0.5f; // The factor to reduce speed by
    [SerializeField] private Transform playerPosition;  // The player's position

    private float playerStartPosY;  // The player's starting Y position

    private float PlayerPosY => playerPosition.position.y;  // The player's current Y position

    void Start()
    {
        playerStartPosY = PlayerPosY;
    }

    void Update()
    {
        AdjustSpeedByHeight();
    }

    /// <summary>
    /// Adjusts the speed of the player based on their height (Y position)
    /// </summary>
    private void AdjustSpeedByHeight()
    {
        float adjustedPlayerPosY = PlayerPosY - playerStartPosY;
        float newSpeed = moveAlongSpline.Speed - (adjustedPlayerPosY * speedReductionFactor);
        moveAlongSpline.Speed = Mathf.Max(newSpeed, 0); // Ensure speed doesn't go below 0
    }
}
