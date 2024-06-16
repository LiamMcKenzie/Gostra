/// <remarks>
/// Author: Johnathan
/// Date Created: 13/6/2024
/// Contributors: Assisted by Github Copilot
/// </remarks>
/// <summary>
/// This class controls a dropdown settings menu;
/// </summary>


using UnityEngine;
using System.Collections;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] private float dropDownSpeed = 0.2f;
    [SerializeField] private GameObject settingsPanel;

    private bool settingsOpen = false;
    private RectTransform settingsPanelRectTransform;

    private float SettingsPanelHeight => settingsPanelRectTransform.rect.height;
    private float AnchoredX => settingsPanelRectTransform.anchoredPosition3D.x;
    private Vector3 OffscreenPos => new(AnchoredX, SettingsPanelHeight / 2);
    private Vector3 OnscreenPos => new(AnchoredX, -SettingsPanelHeight / 2);

    void Start()
    {
        settingsPanelRectTransform = settingsPanel.GetComponent<RectTransform>();
        // Start the settings panel offscreen
        settingsPanelRectTransform.anchoredPosition3D = OffscreenPos;
    }

    /// <summary>
    /// Toggles the settings panel on and off
    /// </summary>
    public void ToggleSettings()
    {
        Vector3 targetPos = settingsOpen ? OffscreenPos : OnscreenPos;
        StartCoroutine(MoveSettingsPanel(targetPos));
    }

    /// <summary>
    /// Moves the settings panel to the target position
    /// </summary>
    /// <param name="targetPos">The target position to move the settings panel to</param>
    private IEnumerator MoveSettingsPanel(Vector3 targetPos)
    {
        float time = 0;

        Vector3 startPos = settingsPanelRectTransform.anchoredPosition3D;

        while (time < dropDownSpeed)
        {
            time += Time.deltaTime;
            settingsPanelRectTransform.anchoredPosition3D = Vector3.Lerp(startPos, targetPos, time / dropDownSpeed);
            yield return null;
        }

        settingsPanelRectTransform.anchoredPosition3D = targetPos;
        settingsOpen = !settingsOpen;
    }
}