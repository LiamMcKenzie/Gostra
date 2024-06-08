using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private BalanceBeam balanceBeam;
    [SerializeField] private PlayerController player;

    void Start()
    {
        // consume ReachedPoleEvent from PlayerController
        player.ReachedPoleEvent.AddListener(() => balanceBeam.StartBalanceBeam());
    }
}