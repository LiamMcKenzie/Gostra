/// <remarks>
/// Author: Johnathan
/// Date Created: 7/6/2024
/// Contributors: Assisted by Github Copilot
/// </remarks>
// <summary>
/// This class controls the game state
/// </summary>

using System.Collections;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This class controls the game state
/// </summary>
public class MainManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MoveCamera moveCamera;
    [SerializeField] private UIManager uIManager;
    [SerializeField] private MeterManager meterManager;
    [SerializeField] private PlayerController player;

    [Header("Settings")]
    [SerializeField] private float timeBeforeReset = 2f;
    public bool GameStarted { get; private set; } = false;

    # region Singleton
    public static MainManager Instance { get; private set; }

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

    void Start()
    {
        player.PlayerFellEvent.AddListener(EndGame);
        uIManager.ShowMeters(false);
    }

    /// <summary>
    /// Ends the game
    /// </summary>
    private void EndGame()
    {
        meterManager.StopMeters();
        StartCoroutine(WaitThen(Reset));
    }

    /// <summary>
    /// Waits for a certain amount of time before calling a callback
    /// </summary>
    /// <param name="callback">The callback to call after the time has passed</param>
    private IEnumerator WaitThen(Action callback)
    {
        yield return new WaitForSeconds(timeBeforeReset);
        callback?.Invoke();
    }

    /// <summary>
    /// Starts the game
    /// </summary>
    /// <param name="context">The context of the input action</param>
    public void OnAnyKeyPressed(InputAction.CallbackContext context)
    {
        if (GameStarted == false)
        {
            uIManager.ShowTitleScreen(false);
            moveCamera.MoveToGamePosition(StartFromTitleScreen);
        }
    }

    /// <summary>
    /// Starts the game from the title screen
    /// </summary>
    public void StartFromTitleScreen()
    {
        GameStarted = true;
        uIManager.ShowMeters(true);
        meterManager.StartRunningSlider();
    }

    /// <summary>
    /// Resets the game
    /// </summary>
    private void Reset()
    {
        player.Reset();
        meterManager.Reset();
        meterManager.StartRunningSlider();
    }
}