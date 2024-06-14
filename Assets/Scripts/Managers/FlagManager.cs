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
using UnityEngine.Events;

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
    [HideInInspector] public UnityEvent<FlagColour> FlagReachedEvent = new(); // Event to be invoked when a flag is reached

    [SerializeField] private PlayerController player;
    [SerializeField] private List<FlagHeight> flagHeights;
    [SerializeField] private float heightOffset = 0.3f; // use this to adjust the height at which the flag is considered reached

    private List<FlagColour> flagsWon = new();  // Tracks the flags the player has reached in a run

    // calling HighestFlag will return the highest flag the player has reached or null if no flags have been reached
    public FlagColour? HighestFlag => flagsWon.Count > 0 ? flagsWon[flagsWon.Count - 1] : null;

    /// <summary>
    /// Helper struct to associate a flag height with a flag colour.
    /// </summary>
    [Serializable]
    private struct FlagHeight
    {
        [field: SerializeField] public float Height { get; private set; }
        [field: SerializeField] public FlagColour Colour { get; private set; }
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
            if (flagsWon.Contains(flag.Colour)) { continue; }
            // If the player has reached the flag, add it to the list of flags won
            if (player.AdjustedPlayerPosY >= flag.Height - heightOffset)
            {
                FlagReachedEvent.Invoke(flag.Colour);
                flagsWon.Add(flag.Colour);
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