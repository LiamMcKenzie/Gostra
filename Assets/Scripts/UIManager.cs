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
