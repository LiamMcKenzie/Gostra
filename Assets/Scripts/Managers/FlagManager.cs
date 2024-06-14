/// <remarks>
/// Author: Johnathan
/// Date Created: 14/6/2024
/// Contributors: Assisted by Github Copilot
/// </remarks>
// <summary>
/// This class tracks the flags the player has reached.
/// </summary>

using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The colour of the flag.
/// </summary>
public enum FlagColour
{
    Blue,
    Green,
    Black
}

/// <summary>
/// This class tracks the flags the player has reached.
/// </summary>
public class FlagManager : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private List<FlagHeight> flagHeights;
    [SerializeField] private float heightOffset = 0.3f; // use this to adjust the height at which the flag is considered reached

    // calling HighestFlag will return the highest flag the player has reached or null if no flags have been reached
    public FlagColour? HighestFlag => flagsWon.Count > 0 ? flagsWon[flagsWon.Count - 1] : null;

    private List<FlagColour> flagsWon = new();  // Tracks the flags the player has reached in a run

    /// <summary>
    /// Helper struct to associate a flag height with a flag colour.
    /// </summary>
    [Serializable]
    private struct FlagHeight
    {
        public float height;
        public FlagColour colour;
    }

    void Update()
    {
        CheckFlags();
    }

    /// <summary>
    /// Checks if the player has reached any flags.
    /// </summary>
    private void CheckFlags()
    {
        foreach (FlagHeight flag in flagHeights)
        {
            // If the player has already reached this flag, skip it
            if (flagsWon.Contains(flag.colour)) { continue; }
            // If the player has reached the flag, add it to the list of flags won
            if (player.AdjustedPlayerPosY >= flag.height - heightOffset)
            {
                flagsWon.Add(flag.colour);
            }
        }
    }

    /// <summary>
    /// Resets the flagsWon list.
    /// </summary>
    public void Reset()
    {
        flagsWon.Clear();
    }
}