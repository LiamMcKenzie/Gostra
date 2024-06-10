/// <remarks>
/// Author: Johnathan
/// Date Created: 9/6/2024
/// Contributors: Assisted by Github Copilot
/// </remarks>
// <summary>
/// This class controls the debug panel.
/// </summary>

using UnityEngine;
using TMPro;

/// <summary>
/// This class controls the debug panel.
/// </summary>
public class DebugPanel : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private TMP_Text playerSpeedText;


    void Update()
    {
        playerSpeedText.text = $"Player Speed: {player.Speed}";
    }
}