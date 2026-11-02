using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using I2.Loc;
using RG.Core.GameVersion;
using Rewired;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Menu;

public class OptionsManager : MonoBehaviour
{
	public SettingsSO settings;

	public Slider masterSlider;

	public Slider musicSlider;

	public Slider soundSlider;

	public Slider gammaSlider;

	public TextMeshProUGUI currentLang;

	public TextMeshProUGUI currentResolutionLabels;

	public TextMeshProUGUI currentQualityLabel;

	public Toggle fullscreenToggle;

	public GameObject toggleOn;

	public GameObject toggleOff;

	private List<ResolutionWithoutRefreshRate> _resolutions = new List<ResolutionWithoutRefreshRate>();

	private int currentRes;

	private int originalRes;

	[SerializeField]
	private LocalizedString[] qualityLocales;

	[SerializeField]
	private LanguagesTranslations langLocales;

	[SerializeField]
	private GameVersion _gameVersionData;

	[SerializeField]
	private TextMeshProUGUI _versionLabel;

	private Bus _master;

	private Bus _music;

	private Bus _sfx;

	private Bus _anim;

	private Bus _uiMenu;

	[SerializeField]
	private string MASTER_BUS = "bus:/Master";

	[SerializeField]
	private string MUSIC_BUS = "bus:/Master/Music";

	[SerializeField]
	private string SFX_BUS = "bus:/Master/SFX";

	[SerializeField]
	private string ANIM_BUS = "bus:/Master/Animation";

	[SerializeField]
	private string UIMENU_BUS = "bus:/Master/UIMenu";

	private void Awake()
	{
		ShowGameVersion();
		InitializeFmodBus();
		Resolution[] resolutions = Screen.resolutions;
		foreach (Resolution resolution in resolutions)
		{
			bool flag = false;
			foreach (ResolutionWithoutRefreshRate resolution2 in _resolutions)
			{
				if (resolution2.Equals(resolution))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				_resolutions.Add(new ResolutionWithoutRefreshRate(resolution));
			}
		}
	}

	private void ShowGameVersion()
	{
		if (_gameVersionData != null)
		{
			_versionLabel.text = _gameVersionData.ToString();
		}
	}

	public void SetAllSettings()
	{
		InitializeFmodBus();
		RefreshLanguage();
		SetVolumes();
		SetRewiredMap(settings.runtimeData.keyboardMap);
		if (ReInput.userDataStore != null)
		{
			ReInput.userDataStore.Load();
		}
	}

	public void InitializeFmodBus()
	{
		_master = RuntimeManager.GetBus(MASTER_BUS);
		_music = RuntimeManager.GetBus(MUSIC_BUS);
		_sfx = RuntimeManager.GetBus(SFX_BUS);
		_uiMenu = RuntimeManager.GetBus(UIMENU_BUS);
		_anim = RuntimeManager.GetBus(ANIM_BUS);
	}

	private void OnEnable()
	{
		RefreshVolSliders();
		RefreshGammaSliders();
		RefreshLanguage();
		RefreshCurrentRes();
		RefreshFullScreenToggle();
		RefreshQuality();
	}

	private void RefreshVolSliders()
	{
		if (musicSlider != null)
		{
			musicSlider.value = settings.runtimeData.musicVolume;
		}
		if (soundSlider != null)
		{
			soundSlider.value = settings.runtimeData.soundVolume;
		}
		if (masterSlider != null)
		{
			masterSlider.value = settings.runtimeData.masterVolume;
		}
	}

	private void RefreshGammaSliders()
	{
		if (!(gammaSlider == null))
		{
			gammaSlider.value = settings.runtimeData.gammaVal;
		}
	}

	private void RefreshLanguage()
	{
		if (SteamManager.Initialized)
		{
			string text = SteamApps.GetCurrentGameLanguage();
			Debug.Log("Steam language :" + text);
			if (text.ToLower().Equals("schinese"))
			{
				text = SystemLanguage.ChineseSimplified.ToString();
			}
			else if (text.ToLower().Equals("tchinese"))
			{
				text = SystemLanguage.ChineseTraditional.ToString();
			}
			else if (text.ToLower().Equals("brazilian"))
			{
				text = SystemLanguage.Portuguese.ToString();
			}
			if (text != settings.runtimeData.previousSteamLanguage || string.IsNullOrEmpty(settings.runtimeData.previousSteamLanguage))
			{
				Debug.Log("Steam language changed to " + text);
				settings.runtimeData.previousSteamLanguage = text;
				settings.runtimeData.currentLanguage = text;
			}
			if (!string.IsNullOrEmpty(settings.runtimeData.currentLanguage))
			{
				LocalizationManager.CurrentLanguage = settings.runtimeData.currentLanguage;
			}
			if (!(currentLang == null) && langLocales != null)
			{
				string currentLanguageCode = LocalizationManager.CurrentLanguageCode;
				string langTranslation = langLocales.GetLangTranslation(currentLanguageCode);
				currentLang.text = langTranslation;
			}
		}
	}

	private void RefreshFullScreenToggle()
	{
		if (!(fullscreenToggle == null))
		{
			fullscreenToggle.isOn = Screen.fullScreen;
			toggleOn.SetActive(fullscreenToggle.isOn);
			toggleOff.SetActive(!fullscreenToggle.isOn);
		}
	}

	private void RefreshQuality()
	{
		if (!(currentQualityLabel == null))
		{
			int qualityLevel = QualitySettings.GetQualityLevel();
			Debug.Log("current quality " + QualitySettings.names[qualityLevel]);
			currentQualityLabel.text = qualityLocales[qualityLevel];
		}
	}

	public void UpdateMusicVolume(Slider slider)
	{
		settings.runtimeData.musicVolume = slider.value;
		SetVolumes();
	}

	public void UpdateSoundVolume(Slider slider)
	{
		settings.runtimeData.soundVolume = slider.value;
		SetVolumes();
	}

	public void UpdateMasterVolume(Slider slider)
	{
		settings.runtimeData.masterVolume = slider.value;
		SetVolumes();
	}

	public void UpdateGamma(Slider slider)
	{
		settings.runtimeData.gammaVal = slider.value;
	}

	public void NextLanguage()
	{
		string currentLanguage = LocalizationManager.CurrentLanguage;
		List<string> allLanguages = LocalizationManager.GetAllLanguages();
		int num = allLanguages.IndexOf(currentLanguage);
		num++;
		if (num >= allLanguages.Count)
		{
			num = 0;
		}
		LocalizationManager.CurrentLanguage = allLanguages[num];
		settings.runtimeData.currentLanguage = LocalizationManager.CurrentLanguage;
		RefreshLanguage();
		RefreshQuality();
	}

	private void RefreshCurrentRes()
	{
		if (currentResolutionLabels == null)
		{
			return;
		}
		for (int i = 0; i < _resolutions.Count; i++)
		{
			if (_resolutions[i].width == Screen.width && _resolutions[i].height == Screen.height)
			{
				currentRes = i;
				break;
			}
		}
		ChangeResolution(0);
	}

	public void ChangeResolution(int dir)
	{
		currentRes += dir;
		if (currentRes >= _resolutions.Count)
		{
			currentRes = 0;
		}
		if (currentRes < 0)
		{
			currentRes = _resolutions.Count - 1;
		}
		currentResolutionLabels.text = _resolutions[currentRes].ToString();
		if (dir != 0)
		{
			ApplyRes(currentRes);
		}
	}

	public void ApplyRes(int id)
	{
		Resolution resolution = _resolutions[id].ToResolution();
		Debug.Log("setting resolution " + resolution.ToString());
		Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
	}

	public void UpdateToggleFullscreen()
	{
		if (!(fullscreenToggle == null))
		{
			if (Screen.fullScreen != fullscreenToggle.isOn)
			{
				Screen.fullScreen = fullscreenToggle.isOn;
			}
			toggleOn.SetActive(fullscreenToggle.isOn);
			toggleOff.SetActive(!fullscreenToggle.isOn);
		}
	}

	public void SetControlsScheme(KeyboardSchema schema)
	{
		if (schema.toggle.isOn)
		{
			Debug.Log("Current schema is " + schema.map);
			settings.runtimeData.keyboardMap = schema.map;
			SetRewiredMap(settings.runtimeData.keyboardMap);
		}
	}

	private void SetRewiredMap(string map)
	{
		Player.ControllerHelper.MapHelper maps = ReInput.players.GetPlayer(0).controllers.maps;
		foreach (ControllerMap item in maps.GetAllMapsInCategory(3))
		{
			item.enabled = false;
		}
		maps.SetMapsEnabled(state: true, "movement", map);
	}

	public void UpdateNextQuality()
	{
		int num = QualitySettings.names.Length;
		int num2 = QualitySettings.GetQualityLevel() + 1;
		if (num2 >= num)
		{
			num2 = 0;
		}
		QualitySettings.SetQualityLevel(num2, applyExpensiveChanges: true);
		RefreshQuality();
	}

	public void SetVolumes()
	{
		_music.setVolume(settings.runtimeData.musicVolume);
		_sfx.setVolume(settings.runtimeData.soundVolume);
		_uiMenu.setVolume(settings.runtimeData.soundVolume);
		_master.setVolume(settings.runtimeData.masterVolume);
		_anim.setVolume(settings.runtimeData.soundVolume);
	}

	public void PlayAudio(string MusicSoundPath)
	{
		if (!string.IsNullOrEmpty(MusicSoundPath))
		{
			RuntimeManager.PlayOneShot(MusicSoundPath);
		}
	}
}
