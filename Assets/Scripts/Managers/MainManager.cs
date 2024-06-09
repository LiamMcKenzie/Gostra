using System.Collections;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor.PackageManager.Requests;

public class MainManager : MonoBehaviour
{
    [SerializeField] private MoveCamera moveCamera;
    [SerializeField] private UIManager uIManager;
    [SerializeField] private PlayerController player;
    [SerializeField] private MeterManager meterManager;
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
        // consume ReachedPoleEvent from PlayerController
        //player.ReachedPoleEvent.AddListener(() => balanceBeam?.StartBalanceBeam());
        player.PlayerFellEvent.AddListener(EndGame);
        meterManager.ActivateGameObjects(false);
    }

    private void EndGame()
    {
        meterManager.StopMeters();
        StartCoroutine(WaitThen(Reset));
    }

    private IEnumerator WaitThen(Action callback)
    {
        yield return new WaitForSeconds(timeBeforeReset);
        callback?.Invoke();
    }

    private IEnumerator WaitThen(Action[] callbacks)
    {
        yield return new WaitForSeconds(timeBeforeReset);

        foreach (var callback in callbacks)
        {
            callback?.Invoke();
        }
    }

    // called when any key is pressed
    public void OnAnyKeyPressed(InputAction.CallbackContext context)
    {
        if (GameStarted == false)
        {
            uIManager.ShowTitleScreen(false);
            moveCamera.MoveToGamePosition(StartFromTitleScreen);
        }
    }
    public void StartFromTitleScreen()
    {
        GameStarted = true;
        meterManager.ActivateGameObjects(true);
        meterManager.StartRunningSlider();
    }

    private void Reset()
    {
        player.Reset();
        meterManager.Reset();
        meterManager.StartRunningSlider();
    }
}