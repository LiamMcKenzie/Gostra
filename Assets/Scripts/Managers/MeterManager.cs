/// <remarks>
/// Author: Johnathan
/// Date Created: 7/6/2024
/// Contributors: Assisted by Github Copilot
/// </remarks>
// <summary>
/// High-level manager for the meters in the game.
/// Exists to interface with the meters from a single class.
/// </summary>

using UnityEngine;

/// <summary>
/// Manages the balance and running meters in the game.
/// </summary>
public class MeterManager : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private BalanceBeam balanceBeam;
    [SerializeField] private RunningSlider runningSlider;

    # region Singleton
    public static MeterManager Instance { get; private set; }

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
        player.ReachedPoleEvent.AddListener(() => balanceBeam.StartBalanceBeam());
    }

    /// <summary>
    /// Resets the meters to their starting positions
    /// </summary>
    public void Reset()
    {
        runningSlider.Reset();
        balanceBeam.Reset();
    }

    /// <summary>
    /// Starts the running slider
    /// </summary>
    public void StartRunningSlider()
    {
        runningSlider.StartRunningSlider();
    }

    /// <summary>
    /// Stops the meters
    /// </summary>
    public void StopMeters()
    {
        runningSlider.StopRunningSlider();
        balanceBeam.StopBalanceBeam();
    }
}