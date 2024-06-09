using UnityEngine;

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
    public void ShowTitleScreen(bool active = true)
    {
        titleScreen.SetActive(active);
    }

    public void ShowMeters(bool activate)
    {
        runningDisplay.gameObject.SetActive(activate);
        balanceDisplay.gameObject.SetActive(activate);
    }
}