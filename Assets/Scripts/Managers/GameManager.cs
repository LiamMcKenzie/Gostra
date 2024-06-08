/// <remarks>
/// Author: Johnathan
/// Date Created: 7/6/2024
/// Contributors: Assisted by Github Copilot
/// </remarks>
// <summary>
/// This class manages the UI elements in the game.
/// </summary>

using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject titleScreenPanel;
    public bool isGameActive { get; private set; } = false;

    # region Singleton
    public static GameManager Instance { get; private set; }

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

}
