/// <remarks>
/// Author: Johnathan
/// Date Created: 10/6/2024
/// Contributors: Assisted by Github Copilot
/// </remarks>
/// <summary>
/// This class controls the optimisation settings
/// </summary>

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class controls the optimisation settings
/// </summary>
public class OptimisationManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MainManager mainManager;

    [Header("Optimisation Elements")]
    [SerializeField] private GameObject prerenderedBG;
    [SerializeField] private GameObject groundQuad;
    [SerializeField] private GameObject terrain;
    [SerializeField] private Terrain terrainComponent;
    [SerializeField] private Light directionalLight;
    [SerializeField] private MeshRenderer poleRenderer;

    [Header("Camera")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform cameraGamePosition;
    [SerializeField] private Transform cameraStartPosition;

    [Header("UI Elements")]
    [SerializeField] private Slider foliageDensitySlider;
    [SerializeField] private Toggle shadowsToggle;
    [SerializeField] private Toggle potatoToggle;

    /// <summary>
    /// Helper struct to store player settings
    /// </summary>
    private struct PlayerSettingsPrefs
    {
        public bool ShadowsOn { get; }
        public bool PotatoModeActive { get; }
        public float FoliageDensity { get; }

        public PlayerSettingsPrefs(bool shadowsOn, bool potatoModeActive, float foliageDensity)
        {
            ShadowsOn = shadowsOn;
            PotatoModeActive = potatoModeActive;
            FoliageDensity = foliageDensity;
        }
    }

    #region Singleton
    public static OptimisationManager Instance { get; private set; }

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
        // Apply the player's settings from the player prefs
        ApplySettingsPrefs();
    }

    void OnApplicationQuit()
    {
        // Reset the terrain settings
        terrainComponent.detailObjectDensity = 1;
        terrainComponent.drawTreesAndFoliage = true;
    }

    /// <summary>
    /// Activates potato mode, useful for low-end devices or web builds
    /// </summary>
    [ContextMenu("Activate Potato Mode")]
    public void ActivatePotatoMode()
    {
        // switch to shorter reset time
        mainManager.SwitchResetTime(potatoActive: true);

        // adjust UI elements
        ToggleUIInteractable(false);
        //shadowsToggle.isOn = false;

        // Disable terrain component
        terrain.SetActive(false);
        // Disable shadows on directional light
        directionalLight.shadows = LightShadows.None;
        // Disable pole renderer
        poleRenderer.enabled = false;

        // Enable ground quad
        groundQuad.SetActive(true);
        // Enable prerendered background
        prerenderedBG.SetActive(true);

        // Move camera into place (skip animation)
        MoveAndRotateCamera(cameraGamePosition);

        // save to player prefs
        PlayerPrefs.SetInt("PotatoMode", 1);
    }

    /// <summary>
    /// Deactivates potato mode
    /// </summary>
    [ContextMenu("Deactivate Potato Mode")]
    public void DeactivatePotatoMode()
    {
        // switch to normal reset time
        mainManager.SwitchResetTime(potatoActive: false);

        // adjust UI elements
        ToggleUIInteractable(true);

        // Enable terrain component
        terrain.SetActive(true);
        // draw foliage
        terrainComponent.drawTreesAndFoliage = true;
        // Enable pole renderer
        poleRenderer.enabled = true;

        // Disable ground quad
        groundQuad.SetActive(false);
        // Disable prerendered background
        prerenderedBG.SetActive(false);

        // Reenable camera movement
        if (mainManager.GameStarted)
        {
            MoveAndRotateCamera(cameraGamePosition);
        }
        else
        {
            MoveAndRotateCamera(cameraStartPosition);
        }

        // save to player prefs
        PlayerPrefs.SetInt("PotatoMode", 0);

        ApplySettingsPrefs();
    }

    /// <summary>
    /// Activates potato mode based on the provided bool
    /// </summary>
    /// <param name="activate">Whether to activate or deactivate potato mode</param>
    public void PotatoMode(bool activate)
    {
        if (activate)
        {
            ActivatePotatoMode();
        }
        else
        {
            DeactivatePotatoMode();
        }
    }

    /// <summary>
    /// Toggles shadows on the directional light
    /// </summary>
    /// <param name="shadowsOn">Whether to enable or disable shadows</param>
    public void SetShadows(bool shadowsOn)
    {
        directionalLight.shadows = shadowsOn ? LightShadows.Soft : LightShadows.None;

        PlayerPrefs.SetInt("ShadowsOn", shadowsOn ? 1 : 0);
    }

    /// <summary>
    /// Sets the foliage density on the terrain
    /// </summary>
    /// <param name="newDensity">The new density value</param>
    public void SetFoliageDensity(float newDensity)
    {
        // set detail density scale
        terrainComponent.detailObjectDensity = newDensity;

        // save to player prefs
        PlayerPrefs.SetFloat("FoliageDensity", newDensity);
    }

    /// <summary>
    /// Toggles the interactivity of the UI elements based on the provided bool
    /// </summary>
    /// <param name="interactable">Whether the UI elements should be interactable</param>
    private void ToggleUIInteractable(bool interactable = true)
    {
        shadowsToggle.interactable = interactable;
        foliageDensitySlider.interactable = interactable;
    }

    /// <summary>
    /// Moves and rotates the camera to the target transform
    /// </summary>
    /// <param name="targetTransform">The target transform to move the camera to</param>
    private void MoveAndRotateCamera(Transform targetTransform)
    {
        mainCamera.transform.position = targetTransform.position;
        mainCamera.transform.rotation = targetTransform.rotation;
    }

    /// <summary>
    /// Gets the player settings from the player prefs
    /// </summary>
    /// <returns>A player settings struct with the current settings</returns>
    private PlayerSettingsPrefs GetPlayerSettings()
    {
        bool shadowsOn = PlayerPrefs.GetInt("ShadowsOn", 1) == 1;
        bool potatoModeActive = PlayerPrefs.GetInt("PotatoMode", 0) == 1;
        float foliageDensity = PlayerPrefs.GetFloat("FoliageDensity", 1);
        return new PlayerSettingsPrefs(shadowsOn, potatoModeActive, foliageDensity);
    }

    /// <summary>
    /// Applies the player settings from the player prefs
    /// </summary>
    /// <param name="skipPotato">Whether to skip the potato mode settings</param>
    /// <remarks>
    /// The logic in this method reflects that while the UI elements need to programmitically set to the correct state,
    /// the OnValueChanged listeners attached to the UI elements need to be accounted for.
    /// </remarks>
    private void ApplySettingsPrefs()
    {
        var playerSettings = GetPlayerSettings();

        // changing the slider value will trigger the OnValueChanged listener which will call SetFoliageDensity
        foliageDensitySlider.value = playerSettings.FoliageDensity;

        if (shadowsToggle.isOn == playerSettings.ShadowsOn) // toggle state matches player prefs, so call SetShadows directly
        {
            SetShadows(playerSettings.ShadowsOn);
        }
        else    // toggle state doesn't match player prefs, so set the toggle state which in turn calls SetShadows
        {
            shadowsToggle.isOn = playerSettings.ShadowsOn;
        }

        if (playerSettings.PotatoModeActive)
        {
            // Change the potato toggle state to trigger the OnValueChanged listener and call PotatoMode
            potatoToggle.isOn = playerSettings.PotatoModeActive;
        }
    }
}
