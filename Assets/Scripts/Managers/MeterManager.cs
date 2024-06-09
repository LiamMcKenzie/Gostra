using UnityEngine;

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
        // consume ReachedPoleEvent from PlayerController
        player.ReachedPoleEvent.AddListener(() => balanceBeam?.StartBalanceBeam());
    }

    public void Reset()
    {
        runningSlider.Reset();
        balanceBeam.Reset();
    }

    public void StartRunningSlider()
    {
        runningSlider.StartRunningSlider();
    }

    public void StopMeters()
    {
        runningSlider.StopRunningSlider();
        balanceBeam.StopBalanceBeam();
    }
}