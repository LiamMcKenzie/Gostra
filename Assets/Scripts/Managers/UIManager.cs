/// <remarks>
/// Author: Johnathan
/// Date Created: 8/6/2024
/// Contributors: Assisted by Github Copilot
/// </remarks>
// <summary>
/// Manages the UI elements in the game.
/// </summary>

using UnityEngine;

/// <summary>
/// Manages the UI elements in the game.
/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject titleScreen;
    [SerializeField] private GameObject balanceDisplay;
    [SerializeField] private GameObject runningDisplay;

    # region Singleton
    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    /// <summary>
    /// Shows or hides the title screen
    /// </summary>
    /// <param name="active">Whether to show or hide the title screen</param>
    public void ShowTitleScreen(bool active = true)
    {
        titleScreen.SetActive(active);
    }

    /// <summary>
    /// Shows or hides the running and balance meters
    /// </summary>
    /// <param name="activate">Whether to show or hide the meters</param>
    public void ShowMeters(bool activate)
    {
        runningDisplay.gameObject.SetActive(activate);
        balanceDisplay.gameObject.SetActive(activate);
    }
}