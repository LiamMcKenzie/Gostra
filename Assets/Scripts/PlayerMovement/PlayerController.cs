/// <remarks>
/// Author: Johnathan & Liam
/// Date Created: 7/6/2024
/// Contributors: Assisted by Github Copilot
/// </remarks>
// <summary>
/// This class controls the player's movement.
/// </summary>

using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class controls the player's movement and rotation.
/// It should be attached to the player object.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [HideInInspector] public UnityEvent ReachedPoleEvent;
    [HideInInspector] public UnityEvent PlayerFellEvent;

    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private MoveAlongSpline moveAlongSpline;

    [Header("Speed")]
    [SerializeField] private float speedReductionFactor = 0.5f; // The factor to reduce speed by
    [field: SerializeField] public float Speed { get; set; } = 1f;  // The speed at which to move the object

    [Header("Rotation")]
    [SerializeField, Range(-1, 1)] public float playerRotation; //-1 and 1 are the limits for player rotation, if rotation is over 1 or less than -1 that means the player has fallen
    [SerializeField] private float maxRotation = 45; //how much the player will rotate when the playerRotation is 1 or -1 (eg 45 degrees)

    [Header("Falling thresholds")]
    [SerializeField, Range(-1, 1)] private float fallThreshold = 0.9f; //the amount the player has to rotate before falling 
    [SerializeField, Range(0, 3)] private float speedThreshold = 0.3f; //the amount the player has to slow down to before falling

    private bool isFalling = false;
    private Transform playerPosition;  // The player's position
    private float playerStartPosY;  // The player's starting Y position

    public bool IsIdle  { get; private set; } = true; 
    public float AdjustedPlayerPosY => Mathf.Max(0, PlayerPosY - playerStartPosY); // The player's adjusted Y position where the starting Y position is effectively 0

    private float PlayerPosY => playerPosition.position.y;  // The player's current Y position
    private bool reachedPole;

    void Start()
    {
        playerPosition = transform;
        playerStartPosY = PlayerPosY;
        animator.Play("Idle");
    }

    void Update()
    {
        if (isFalling || IsIdle) { return; }

        AdjustSpeedByHeight();
        CheckThresholds();   
    }

    /// <summary>
    /// Adjusts the player's rotation based on the input value
    /// </summary>
    /// <param name="playerRotation">The input value to adjust the player's rotation</param>
    public void RotatePlayer(float playerRotation)
    {
        this.playerRotation = playerRotation;
        playerRotation = Mathf.Clamp(playerRotation, -maxRotation, maxRotation); //this makes sure the player rotation is between -1 and 1
        Quaternion rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, playerRotation * -maxRotation);
        transform.localRotation = rotation; //this rotates the player based on the playerRotation value
    }

    /// <summary>
    /// Adjusts the speed of the player based on their height (Y position)
    /// </summary>
    private void AdjustSpeedByHeight()
    {
        if (AdjustedPlayerPosY <= 0) { return; } // If the player is at the starting Y position, don't adjust the speed 
        if (reachedPole == false)
        {
            reachedPole = true;
            ReachedPoleEvent.Invoke();
        }
        float newSpeed = Speed - (AdjustedPlayerPosY * speedReductionFactor);
        Speed = Mathf.Max(newSpeed, 0); // Ensure speed doesn't go below 0
    }

    /// <summary>
    /// Makes the player fall and disables the animator component
    /// </summary>
    public void Fall()
    {
        Speed = 0f;
        isFalling = true;
        animator.enabled = false; //disable the animator component. which causes the player to become a ragdoll
        PlayerFellEvent.Invoke();
    }

    /// <summary>
    /// Makes the player run
    /// </summary>
    public void Run()
    {
        IsIdle = false;
        animator.Play("Sprint");
    }

    /// <summary>
    /// Checks if the player has lost balance or speed
    /// </summary>
    private void CheckThresholds()
    {
        bool lostBalance = playerRotation > fallThreshold || playerRotation < -fallThreshold;
        bool lostSpeed = Speed <= speedThreshold;
        
        if (lostBalance || lostSpeed)
        {
            Fall();
        }
    }

    /// <summary>
    /// Resets the player's position and rotation
    /// </summary>
    public void Reset()
    {
        IsIdle = true;
        isFalling = false;
        reachedPole = false;
        animator.Play("Idle");
        Speed = 0f;
        playerRotation = 0;
        transform.rotation = Quaternion.identity;
        animator.enabled = true;
        moveAlongSpline.ResetPosition();
    }
}
