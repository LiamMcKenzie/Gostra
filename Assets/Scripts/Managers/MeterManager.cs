using UnityEngine;

public class MeterManager : MonoBehaviour
{
    [SerializeField] private BalanceBeam balanceBeam;
    [SerializeField] private RunningSlider runningSlider;
    [SerializeField] private PlayerController player;

        [SerializeField] private GameObject balanceDisplay;
        [SerializeField] private GameObject runningDisplay;

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

    public void ActivateGameObjects(bool activate)
    {
        runningDisplay.gameObject.SetActive(activate);
        balanceDisplay.gameObject.SetActive(activate);
    }
}