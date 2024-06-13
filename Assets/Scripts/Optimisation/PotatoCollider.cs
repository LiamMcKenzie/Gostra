/// <remarks>
/// Author: Johnathan
/// Date Created: 10/6/2024
/// Contributors: Assisted by Github Copilot
/// </remarks>
/// <summary>
/// This class controls collider attached to the ground in potato mode.
/// </summary>

using UnityEngine;

/// <summary>
/// This class controls collider attached to the ground in potato mode.
/// </summary>
public class PotatoCollider : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    void OnCollisionEnter(Collision collision)
    {
        // If the player has reached the pole and is falling, hide the player
        if (collision.gameObject.CompareTag("Player") && player.ReachedPole && player.IsFalling)
        {          
            player.HidePlayer();
        }
    }
}