/// <remarks>
/// Author: Johnathan
/// Date Created: 14/6/2024
/// Contributors: Assisted by Github Copilot
/// </remarks>
// <summary>
/// This class controls the panel that displays the win message. 
/// </summary>

using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

/// <summary>
/// This class controls the panel that displays the win message.
/// It chooses the correct message to display based on the flag colour.
/// </summary>
public class WinPanel : MonoBehaviour
{
    /// <summary>
    /// Helper struct to associate a flag colour with a win heading.
    /// </summary>
    [Serializable]
    private struct WinPanelData
    {   
        [field: SerializeField] public FlagColour FlagColour { get; private set; }
        [field: SerializeField] public string WinHeading { get; private set; }
    }

    [SerializeField] private List<WinPanelData> winPanelData = new();
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TMP_Text winHeading;
    [SerializeField] private TMP_Text winMessage;

    /// <summary>
    /// Shows the win panel with the correct message based on the flag colour.
    /// </summary>
    /// <param name="heightReached">The height reached by the player</param>
    /// <param name="flagColour">The colour of the flag reached by the player</param>
    public void ShowWinPanel(float heightReached, FlagColour? flagColour)
    {
        winPanel.SetActive(true);
        WinPanelData data = winPanelData.Find(x => x.FlagColour == flagColour);
        winHeading.text = data.WinHeading;
        winMessage.text = WriteWinMessage(heightReached, flagColour);
    }

    /// <summary>
    /// Hides the win panel.
    /// </summary>
    public void HideWinPanel()
    {
        winPanel.SetActive(false);
    }

    /// <summary>
    /// Writes the win message based on the height reached and the flag colour.
    /// </summary>
    private string WriteWinMessage(float heightReached, FlagColour? flagColour)
    {
        string height = (flagColour == FlagColour.Black) ? "all the way" : $"{heightReached:F2}m";
        return $"You made it {height} up the pole and got the {flagColour} flag!";
    }
}