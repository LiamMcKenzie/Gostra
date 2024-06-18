/// <remarks>
/// Author: Johnathan
/// Date Created: 13/6/2024
/// Contributors: Assisted by Github Copilot
/// </remarks>
/// <summary>
/// This class controls the in game UI panel.
/// </summary>

using UnityEngine;
using UnityEngine.UI;

public class UIPanel : MonoBehaviour
{
    [SerializeField] private Button toggleSoundButton;
    [SerializeField] private Sprite soundOnIcon;
    [SerializeField] private Sprite soundOffIcon;

    private bool soundOn = true;

    void Start()
    {
        // Load soundOn from player prefs
        soundOn = PlayerPrefs.GetInt("SoundOn", 1) == 1;
        toggleSoundButton.image.sprite = soundOn ? soundOnIcon : soundOffIcon;
        /* call the audio manager here */
    }

    /// <summary>
    /// Toggles the sound on and off
    /// </summary>
    /// <param name="soundOn">Whether the sound is on or off</param>
    public void ToggleSound()
    {
        soundOn = !soundOn;
        toggleSoundButton.image.sprite = soundOn ? soundOnIcon : soundOffIcon;
        /* call the audio manager here */
        // save soundOn to player prefs
        PlayerPrefs.SetInt("SoundOn", soundOn ? 1 : 0);
    }

    /// <summary>
    /// Quits the game
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }
}