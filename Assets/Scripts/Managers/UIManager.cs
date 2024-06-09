using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject titleScreen;

    public void ShowTitleScreen(bool active = true)
    {
        titleScreen.SetActive(active);
    }

}