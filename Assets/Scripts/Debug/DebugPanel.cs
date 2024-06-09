using UnityEngine;
using TMPro;

public class DebugPanel : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private TMP_Text playerSpeedText;


    void Update()
    {
        playerSpeedText.text = $"Player Speed: {player.Speed}";
    }
}