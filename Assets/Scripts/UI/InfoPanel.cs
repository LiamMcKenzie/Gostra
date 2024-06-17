/// <remarks>
/// Author: Johnathan
/// Date Created: 14/6/2024
/// Contributors: Assisted by Github Copilot
/// </remarks>
// <summary>
/// This class controls the info panel.
/// </summary>

using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

/// <summary>
/// This class controls the info panel.
/// This includes updating the speed and height text, and updating the flags attained display.
/// </summary>
public class InfoPanel : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private FlagManager flagManager;
    [SerializeField] private InfoText speedText;
    [SerializeField] private InfoText heightText;
    [SerializeField] private List<UIFlag> uIFlags;

    [Header("Settings")]
    [SerializeField, Range(0, 1)] private float unreachedFlagAlpha = 0.4f;  // How transparent to make the UI flags when unreached
  
    /// <summary>
    /// Helper class to store the integer and decimal parts of a float.
    /// </summary>
    [Serializable]
    private class InfoText
    {
        [SerializeField] private TMP_Text integerText;
        [SerializeField] private TMP_Text decimalText;

        /// <summary>
        /// Formats and updates the text fields of the InfoText.
        /// </summary>
        public void UpdateText(float value)
        {
            integerText.text = ((int)value).ToString();
            decimalText.text = (value - (int)value).ToString("F2").Substring(1);
        }
        
        /// <summary>
        /// Resets the text fields of the InfoText.
        /// </summary>
        public void Reset()
        {
            integerText.text = "0";
            decimalText.text = ".00";
        }
    }

    /// <summary>
    /// Helper struct to associate a flag image with a flag colour.
    /// </summary>
    [Serializable]
    private struct UIFlag
    {
        [field: SerializeField] public Image FlagImage { get; private set; }
        [field: SerializeField] public FlagColour FlagColour { get; private set; }
    }

    void Start()
    {
        flagManager.FlagReachedEvent.AddListener(ActivateFlag);
        ChangeAllFlagAlphas(unreachedFlagAlpha);
    }

    void Update()
    {
        // dont update performance info if the player is falling or idle
        if (playerController.IsFalling || playerController.IsIdle) { return; }
        UpdatePerformanceInfo(playerController.Speed, playerController.AdjustedPlayerPosY);
    }

    /// <summary>
    /// Updates the speed and height text fields.
    /// </summary>
    private void UpdatePerformanceInfo(float speed, float height)
    {
        speedText.UpdateText(speed);
        heightText.UpdateText(height);
    }

    /// <summary>
    /// Activates the flag with the given colour.
    /// </summary>
    /// <param name="flagColour">The colour enum of the flag to activate.</param>
    private void ActivateFlag(FlagColour flagColour)
    {
        var flagToActivate = uIFlags.Find(flag => flag.FlagColour == flagColour);
        flagToActivate.FlagImage.color = new Color(flagToActivate.FlagImage.color.r, flagToActivate.FlagImage.color.g, flagToActivate.FlagImage.color.b, 1);
    }

    /// <summary>
    /// Changes the alpha of a flag image.
    /// </summary>
    private void ChangeFlagAlpha(UIFlag flag, float alpha) => flag.FlagImage.color = new Color(flag.FlagImage.color.r, flag.FlagImage.color.g, flag.FlagImage.color.b, alpha);

    /// <summary>
    /// Changes the alpha of all flag images.
    /// </summary>
    private void ChangeAllFlagAlphas(float alpha) => uIFlags.ForEach(flag => ChangeFlagAlpha(flag, alpha));

    /// <summary>
    /// Resets the info panel.
    /// </summary>
    public void Reset()
    {
        ChangeAllFlagAlphas(unreachedFlagAlpha);
        speedText.Reset();
        heightText.Reset();
    }
}