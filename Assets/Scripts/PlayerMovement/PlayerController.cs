/// <remarks>
/// Author: Johnathan & Liam
/// Date Created: 7/6/2024
/// Contributors: Assisted by Github Copilot
/// </remarks>
// <summary>
/// This class controls the player's movement.
/// The PlayerAnimator script was refactored into this script.
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
    [HideInInspector] public UnityEvent<float> PlayerFellEvent;

    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private MoveAlongSpline moveAlongSpline;
    [SerializeField] private GameObject meshes;

    [Header("Speed")]
    [SerializeField] private float speedReductionFactor = 0.5f; // The factor to reduce speed by

    [Header("Rotation")]
    [SerializeField, Range(-1, 1)] public float playerRotation; //-1 and 1 are the limits for player rotation, if rotation is over 1 or less than -1 that means the player has fallen
    [SerializeField] private float maxRotation = 45; //how much the player will rotate when the playerRotation is 1 or -1 (eg 45 degrees)

    [Header("Falling thresholds")]
    [SerializeField, Range(0, 3)] private float speedThreshold = 0.3f; //the amount the player has to slow down to before falling
    [SerializeField] bool godMode = false; //if true the player will not fall unless they are at the top of the pole

    private Transform playerPosition;  // The player's position
    private float playerStartPosY;  // The player's starting Y position
    private float speed; // The player's speed

    public bool IsIdle { get; private set; } = true;
    public bool ReachedPole { get; private set; } = false;
    public bool IsFalling { get; private set; } = false;

    public float AdjustedPlayerPosY => Mathf.Max(0, PlayerPosY - playerStartPosY); // The player's adjusted Y position where the starting Y position is effectively 0
    private float PlayerPosY => playerPosition.position.y;  // The player's current Y position

    public float Speed
    {
        get { return speed; }
        set { speed = value < 0 ? 0 : value; }
    }

    void Start()
    {
        playerPosition = transform;
        playerStartPosY = PlayerPosY;
        animator.Play("Idle");
    }

    void Update()
    {
        if (IsFalling || IsIdle) { return; }

        AdjustSpeedByHeight();
        CheckSpeedThreshold();
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
        if (ReachedPole == false)
        {
            ReachedPole = true;
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
        if (IsFalling || (godMode && AdjustedPlayerPosY < 3.75f)) { return; }
        Speed = 0f;
        IsFalling = true;
        animator.enabled = false; //disable the animator component. which causes the player to become a ragdoll
        PlayerFellEvent.Invoke(AdjustedPlayerPosY);
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
    /// Checks if the player has lost speed
    /// </summary>
    private void CheckSpeedThreshold()
    {
        if (Speed <= speedThreshold)
        {
            Fall();
        }
    }

    /// <summary>
    /// Resets the player's position and rotation
    /// </summary>
    public void Reset()
    {
        meshes.SetActive(true);
        IsIdle = true;
        IsFalling = false;
        ReachedPole = false;
        animator.Play("Idle");
        Speed = 0f;
        playerRotation = 0;
        transform.rotation = Quaternion.identity;
        animator.enabled = true;
        moveAlongSpline.ResetPosition();
    }

    /// <summary>
    /// Hides the player's meshes
    /// </summary>
    public void HidePlayer()
    {
        meshes.SetActive(false);
    }
}
