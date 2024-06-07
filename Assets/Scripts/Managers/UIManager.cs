/// <remarks>
/// Author: Johnathan
/// Date Created: 7/6/2024
/// Contributors: Assisted by Github Copilot
/// </remarks>
// <summary>
/// This class manages the UI elements in the game.
/// </summary>

using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject titleScreenPanel;

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

}
