using System.Collections.Generic;
using UltimateWater;
using UnityEngine;
using UnityEngine.AzureSky;
using UnityEngine.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GraphicsSettingsBox : OptionsMenuBox
{
	public static int OverrideScreenWidth = -1;

	public static int OverrideScreenHeight = -1;

	public static int OverrideFullScreen = -1;

	[Header("Components")]
	[SerializeField]
	private Dropdown resolutionDropdown;

	[SerializeField]
	private Dropdown waterQualityDropdown;

	[SerializeField]
	private Dropdown qualitySettingsDropdown;

	[SerializeField]
	private Dropdown textureQualityDropdown;

	[SerializeField]
	private Dropdown shadowTypeDropdown;

	[SerializeField]
	private Dropdown shadowCascadeDropdown;

	[SerializeField]
	private Dropdown shadowResolutionDropdown;

	[SerializeField]
	private Dropdown reflectionsDropdown;

	[SerializeField]
	private Dropdown fpsCapDropdown;

	[SerializeField]
	private Dropdown fullscreenModeDropdown;

	[SerializeField]
	private Toggle vsyncToggle;

	[SerializeField]
	private Toggle aoToggle;

	[SerializeField]
	private Toggle aaToggle;

	[SerializeField]
	private Slider shadowDistanceSlider;

	public Slider FOVSlider;

	public PostProcessingProfile postEffects;

	public PostProcessingProfile postEffectsMenu;

	public bool disableEffectsUnderwater = true;

	public bool aberationEnabled;

	public bool vignetteEnabled;

	private ChromaticAberrationModel.Settings aberationSettings;

	private VignetteModel.Settings vignetteSettings;

	private ColorGradingModel.Settings colorGradingSettings;

	private AzureSkyFogScattering skyFog;

	private Water water;

	private Network_Player localPlayer;

	private RGD_Settings lastLoadedSetting;

	private VolumetricLightRenderer volumetricLightRenderer;

	public Water PlaywayWater
	{
		get
		{
			if (water == null)
			{
				water = Object.FindObjectOfType<Water>();
			}
			return water;
		}
	}

	public override BoxVisibility GetBoxVisibility()
	{
		return BoxVisibility.Pc;
	}

	public override void SetFocus(bool gamepad)
	{
		fullscreenModeDropdown.Select();
	}

	public override void SettingStart()
	{
		Initialize();
		SavePostEffectSettings();
		SceneManager.sceneLoaded += SceneManager_sceneLoaded;
	}

	public override void SettingsUpdate()
	{
		base.Update();
		if (Raft_Network.InMenuScene || !disableEffectsUnderwater)
		{
			return;
		}
		if (localPlayer == null)
		{
			localPlayer = ComponentManager<Network_Player>.Value;
			return;
		}
		SubmersionState submersionState = localPlayer.PersonController.SubmersionState;
		if (skyFog == null)
		{
			skyFog = localPlayer.Camera.GetComponent<AzureSkyFogScattering>();
		}
		else if (submersionState == SubmersionState.Full && skyFog.enabled)
		{
			skyFog.enabled = false;
		}
		else if (submersionState == SubmersionState.None && !skyFog.enabled)
		{
			skyFog.enabled = true;
		}
	}

	private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.name == Raft_Network.MenuSceneName)
		{
			LoadPostEffectSettings();
		}
	}

	private new void OnDestroy()
	{
		SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
	}

	public override void Save(RGD_Settings settings)
	{
		base.Save(settings);
		settings.graphics.overallQuality = QualitySettings.GetQualityLevel();
		settings.graphics.waterQuality = WaterQualitySettings.Instance.GetQualityLevel();
		settings.graphics.resolutionIndex = resolutionDropdown.value;
		settings.graphics.resolutionWidth = Screen.currentResolution.width;
		settings.graphics.resolutionHeight = Screen.currentResolution.height;
		settings.graphics.resolutionRefreshRate = Screen.currentResolution.refreshRate;
		settings.graphics.vsync = QualitySettings.vSyncCount;
		settings.graphics.ambientOcclusion = postEffects.ambientOcclusion.enabled;
		settings.graphics.antialiasing = postEffects.antialiasing.enabled;
		settings.graphics.textureQuality = QualitySettings.masterTextureLimit;
		settings.graphics.shadowType = QualitySettings.shadows;
		settings.graphics.shadowCascades = QualitySettings.shadowCascades;
		settings.graphics.shadowDistance = QualitySettings.shadowDistance;
		settings.graphics.shadowResolution = QualitySettings.shadowResolution;
		settings.graphics.fpsIndex = fpsCapDropdown.value;
		settings.graphics.FOVValue = (int)FOVSlider.value;
		settings.graphics.reflectionDropDownValue = (int)SkyManager.ReflectionMode;
		settings.graphics.fullscreenDropdownIndex = fullscreenModeDropdown.value;
	}

	public override void Load(RGD_Settings settings)
	{
		base.Load(settings);
		lastLoadedSetting = settings;
		Initialize();
		qualitySettingsDropdown.value = settings.graphics.overallQuality;
		qualitySettingsDropdown.RefreshShownValue();
		if (QualitySettings.GetQualityLevel() != settings.graphics.overallQuality)
		{
			QualitySettings.SetQualityLevel(settings.graphics.overallQuality);
		}
		waterQualityDropdown.value = settings.graphics.waterQuality;
		waterQualityDropdown.RefreshShownValue();
		if (PlaywayWater != null && WaterQualitySettings.Instance.GetQualityLevel() != waterQualityDropdown.value)
		{
			WaterQualitySettings.Instance.SetQualityLevel(waterQualityDropdown.value);
		}
		if (Screen.resolutions.Length == 0)
		{
			Invoke("LoadResolutionAndFullscreenModeLate", 0.3f);
		}
		else
		{
			LoadResolutionAndFullscreenMode(settings);
		}
		if (QualitySettings.vSyncCount != settings.graphics.vsync)
		{
			QualitySettings.vSyncCount = settings.graphics.vsync;
		}
		vsyncToggle.isOn = settings.graphics.vsync != 0;
		if (postEffects.ambientOcclusion.enabled != settings.graphics.ambientOcclusion)
		{
			postEffects.ambientOcclusion.enabled = settings.graphics.ambientOcclusion;
			postEffectsMenu.ambientOcclusion.enabled = settings.graphics.ambientOcclusion;
		}
		aoToggle.isOn = settings.graphics.ambientOcclusion;
		if (postEffects.antialiasing.enabled != settings.graphics.antialiasing)
		{
			postEffects.antialiasing.enabled = settings.graphics.antialiasing;
			postEffectsMenu.antialiasing.enabled = settings.graphics.antialiasing;
		}
		aaToggle.isOn = settings.graphics.antialiasing;
		textureQualityDropdown.value = Mathf.Abs(settings.graphics.textureQuality - 3);
		textureQualityDropdown.RefreshShownValue();
		shadowTypeDropdown.value = (int)settings.graphics.shadowType;
		shadowTypeDropdown.RefreshShownValue();
		shadowResolutionDropdown.value = (int)settings.graphics.shadowResolution;
		shadowResolutionDropdown.RefreshShownValue();
		shadowCascadeDropdown.value = ((settings.graphics.shadowCascades > 0) ? (settings.graphics.shadowCascades / 2) : 0);
		shadowCascadeDropdown.RefreshShownValue();
		shadowDistanceSlider.value = settings.graphics.shadowDistance;
		fpsCapDropdown.value = settings.graphics.fpsIndex;
		fpsCapDropdown.RefreshShownValue();
		OnFPSCapChange();
		reflectionsDropdown.value = settings.graphics.reflectionDropDownValue;
		reflectionsDropdown.RefreshShownValue();
		OnReflectionsChange();
		FOVSlider.value = settings.graphics.FOVValue;
	}

	public void OnQualityChange()
	{
		QualitySettings.SetQualityLevel(qualitySettingsDropdown.value);
		ApplyCustomProperties();
		ApplyVolumetricLightingProperties(qualitySettingsDropdown.value);
		Resources.UnloadUnusedAssets();
	}

	public void OnResolutionChange()
	{
		ApplyCurrentResolutionAndFullscreenMode();
	}

	public void OnFullscreenModeChange()
	{
		ApplyCurrentResolutionAndFullscreenMode();
	}

	public void OnVsyncChange()
	{
		QualitySettings.vSyncCount = (vsyncToggle.isOn ? 1 : 0);
	}

	public void OnAOChange()
	{
		postEffects.ambientOcclusion.enabled = aoToggle.isOn;
		postEffectsMenu.ambientOcclusion.enabled = aoToggle.isOn;
	}

	public void OnAAChange()
	{
		postEffects.antialiasing.enabled = aaToggle.isOn;
		postEffectsMenu.antialiasing.enabled = aaToggle.isOn;
	}

	public void OnWaterQualityChange()
	{
		if (PlaywayWater != null)
		{
			WaterQualitySettings.Instance.SetQualityLevel(waterQualityDropdown.value);
		}
	}

	public void OnTextureQualityChange()
	{
		QualitySettings.masterTextureLimit = 3 - textureQualityDropdown.value;
	}

	public void OnShadowTypeChange()
	{
		QualitySettings.shadows = (ShadowQuality)shadowTypeDropdown.value;
	}

	public void OnShadowCascadeChange()
	{
		switch (shadowCascadeDropdown.value)
		{
		case 0:
			QualitySettings.shadowCascades = 0;
			break;
		case 1:
			QualitySettings.shadowCascades = 2;
			break;
		case 2:
			QualitySettings.shadowCascades = 4;
			break;
		}
	}

	public void OnShadowDistanceChange()
	{
		QualitySettings.shadowDistance = shadowDistanceSlider.value;
	}

	public void OnShadowResolutionChange()
	{
		QualitySettings.shadowResolution = (ShadowResolution)shadowResolutionDropdown.value;
	}

	public void OnReflectionsChange()
	{
		SkyManager.ReflectionMode = (ReflectionType)reflectionsDropdown.value;
	}

	public void OnFPSCapChange()
	{
		int num = 60;
		switch (fpsCapDropdown.value)
		{
		case 0:
			num = 30;
			break;
		case 1:
			num = 60;
			break;
		case 2:
			num = 90;
			break;
		case 3:
			num = 120;
			break;
		case 4:
			num = 144;
			break;
		case 5:
			num = -1;
			break;
		}
		if (Application.targetFrameRate != num)
		{
			Application.targetFrameRate = num;
		}
		GameManager.targetFrameRate = num;
	}

	public void OnFOVSliderChange()
	{
		if (localPlayer == null)
		{
			localPlayer = ComponentManager<Network_Player>.Value;
		}
		if (localPlayer != null)
		{
			localPlayer.Camera.fieldOfView = FOVSlider.value;
		}
	}

	private void Initialize()
	{
		InitializeResolutions();
		InitializeDropdowns();
	}

	private void InitializeDropdowns()
	{
		if (qualitySettingsDropdown.options.Count == 0)
		{
			qualitySettingsDropdown.GetComponent<LocalizeDropdownSemih>().UpdateLocalization();
		}
		if (waterQualityDropdown.options.Count == 0)
		{
			waterQualityDropdown.GetComponent<LocalizeDropdownSemih>().UpdateLocalization();
		}
		if (shadowCascadeDropdown.options.Count == 0)
		{
			shadowCascadeDropdown.GetComponent<LocalizeDropdownSemih>().UpdateLocalization();
		}
		if (shadowResolutionDropdown.options.Count == 0)
		{
			shadowResolutionDropdown.GetComponent<LocalizeDropdownSemih>().UpdateLocalization();
		}
		if (shadowTypeDropdown.options.Count == 0)
		{
			shadowTypeDropdown.GetComponent<LocalizeDropdownSemih>().UpdateLocalization();
		}
		if (textureQualityDropdown.options.Count == 0)
		{
			textureQualityDropdown.GetComponent<LocalizeDropdownSemih>().UpdateLocalization();
		}
		if (fpsCapDropdown.options.Count == 0)
		{
			fpsCapDropdown.GetComponent<LocalizeDropdownSemih>().UpdateLocalization();
		}
		if (reflectionsDropdown.options.Count == 0)
		{
			reflectionsDropdown.GetComponent<LocalizeDropdownSemih>().UpdateLocalization();
		}
		if (fullscreenModeDropdown.options.Count == 0)
		{
			fullscreenModeDropdown.GetComponent<LocalizeDropdownSemih>().UpdateLocalization();
		}
	}

	private void InitializeResolutions()
	{
		if (resolutionDropdown.options.Count == 0)
		{
			List<string> list = new List<string>();
			for (int i = 0; i < Screen.resolutions.Length; i++)
			{
				Resolution resolution = Screen.resolutions[i];
				string item = resolution.ToString();
				list.Add(item);
			}
			resolutionDropdown.AddOptions(list);
		}
	}

	private void LoadResolutionAndFullscreenModeLate()
	{
		LoadResolutionAndFullscreenMode(lastLoadedSetting);
	}

	private void LoadResolutionAndFullscreenMode(RGD_Settings settings)
	{
		if (settings.graphics.resolutionIndex >= 0 && settings.graphics.resolutionIndex < resolutionDropdown.options.Count)
		{
			resolutionDropdown.value = settings.graphics.resolutionIndex;
		}
		else
		{
			resolutionDropdown.value = resolutionDropdown.options.Count - 1;
		}
		resolutionDropdown.RefreshShownValue();
		if (settings.graphics.fullscreenDropdownIndex >= 0 && settings.graphics.fullscreenDropdownIndex < fullscreenModeDropdown.options.Count)
		{
			fullscreenModeDropdown.value = settings.graphics.fullscreenDropdownIndex;
		}
		else
		{
			fullscreenModeDropdown.value = 0;
		}
		ApplyCurrentResolutionAndFullscreenMode();
	}

	private bool ResolutionExists(int width, int height, int refreshRate, ref int resolutionIndex)
	{
		for (int i = 0; i < Screen.resolutions.Length; i++)
		{
			Resolution resolution = Screen.resolutions[i];
			if (resolution.width == width && resolution.height == height && resolution.refreshRate == refreshRate)
			{
				resolutionIndex = i;
				return true;
			}
		}
		return false;
	}

	private void SavePostEffectSettings()
	{
		aberationEnabled = postEffects.chromaticAberration.enabled;
		vignetteEnabled = postEffects.chromaticAberration.enabled;
		aberationSettings = postEffects.chromaticAberration.settings;
		vignetteSettings = postEffects.vignette.settings;
		colorGradingSettings = postEffects.colorGrading.settings;
	}

	private void LoadPostEffectSettings()
	{
		postEffects.chromaticAberration.enabled = aberationEnabled;
		postEffectsMenu.chromaticAberration.enabled = aberationEnabled;
		postEffects.vignette.enabled = vignetteEnabled;
		postEffectsMenu.vignette.enabled = vignetteEnabled;
		postEffects.chromaticAberration.settings = aberationSettings;
		postEffectsMenu.chromaticAberration.settings = aberationSettings;
		postEffects.vignette.settings = vignetteSettings;
		postEffectsMenu.vignette.settings = vignetteSettings;
		postEffects.colorGrading.settings = colorGradingSettings;
		postEffectsMenu.colorGrading.settings = colorGradingSettings;
	}

	private void ApplyCustomProperties()
	{
		QualitySettings.masterTextureLimit = 3 - textureQualityDropdown.value;
		QualitySettings.shadows = (ShadowQuality)shadowTypeDropdown.value;
		QualitySettings.shadowCascades = DropDownToCascade(shadowCascadeDropdown.value);
		QualitySettings.shadowDistance = shadowDistanceSlider.value;
		QualitySettings.shadowResolution = (ShadowResolution)shadowResolutionDropdown.value;
	}

	private void ApplyVolumetricLightingProperties(int qualitySettingsValue)
	{
		if (volumetricLightRenderer == null)
		{
			volumetricLightRenderer = GetComponentOnCamera<VolumetricLightRenderer>();
		}
		if (volumetricLightRenderer != null)
		{
			bool flag = VolumetricLightingEnabled(qualitySettingsValue);
			VolumetricLightRenderer.VolumtericResolution volumtericResolution = VolumetricLightRenderer.VolumtericResolution.Half;
			switch (qualitySettingsValue)
			{
			case 3:
				volumtericResolution = VolumetricLightRenderer.VolumtericResolution.Full;
				break;
			case 4:
				volumtericResolution = VolumetricLightRenderer.VolumtericResolution.Full;
				break;
			default:
				flag = true;
				break;
			}
			if (volumetricLightRenderer.enabled != flag)
			{
				volumetricLightRenderer.enabled = flag;
			}
			if (volumetricLightRenderer.Resolution != volumtericResolution)
			{
				volumetricLightRenderer.Resolution = volumtericResolution;
			}
		}
	}

	private bool VolumetricLightingEnabled(int qualitySettingsValue)
	{
		return qualitySettingsValue >= 2;
	}

	private int DropDownToCascade(int dropDownValue)
	{
		return dropDownValue switch
		{
			0 => 0, 
			1 => 2, 
			_ => 4, 
		};
	}

	public void ApplyCurrentResolutionAndFullscreenMode()
	{
		Resolution resolution = GetResolutionFromCurrentSettings();
		if (OverrideScreenWidth > 0 && OverrideScreenHeight > 0)
		{
			resolution = new Resolution
			{
				width = OverrideScreenWidth,
				height = OverrideScreenHeight
			};
		}
		FullScreenMode fullscreenMode = GetFullscreenModeFromCurrentSettings();
		if (OverrideFullScreen >= 0 && OverrideFullScreen <= 3)
		{
			fullscreenMode = (FullScreenMode)OverrideFullScreen;
		}
		Screen.SetResolution(resolution.width, resolution.height, fullscreenMode, resolution.refreshRate);
	}

	private Resolution GetResolutionFromCurrentSettings()
	{
		Resolution result = Screen.resolutions[Screen.resolutions.Length - 1];
		if (resolutionDropdown.value >= 0 || resolutionDropdown.value < Screen.resolutions.Length)
		{
			result = Screen.resolutions[resolutionDropdown.value];
		}
		return result;
	}

	private FullScreenMode GetFullscreenModeFromCurrentSettings()
	{
		return fullscreenModeDropdown.value switch
		{
			0 => FullScreenMode.ExclusiveFullScreen, 
			1 => FullScreenMode.FullScreenWindow, 
			2 => FullScreenMode.Windowed, 
			_ => FullScreenMode.FullScreenWindow, 
		};
	}

	private T GetComponentOnCamera<T>()
	{
		Network_Player value = ComponentManager<Network_Player>.Value;
		if (value == null || value.Camera == null)
		{
			return default;
		}
		return value.Camera.GetComponent<T>();
	}
}
