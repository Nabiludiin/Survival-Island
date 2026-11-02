using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsBox : OptionsMenuBox
{
	[Header("Components")]
	[Space(20f)]
	[SerializeField]
	private UISlider masterVolumeSlider;

	[SerializeField]
	private UISlider musicVolumeSlider;

	[SerializeField]
	private UISlider ambienceVolumeSlider;

	[SerializeField]
	private UISlider sfxVolumeSlider;

	[SerializeField]
	private UISlider uiVolumeSlider;

	[SerializeField]
	private UISlider notebookVolumeSlider;

	[SerializeField]
	private Dropdown musicRateDropdown;

	private VCA VCA_Master;

	private VCA VCA_Music;

	private VCA VCA_Ambience;

	private VCA VCA_SFX;

	private VCA VCA_UI;

	private VCA VCA_Notebook;

	private bool firstTime = true;

	private bool isMuted = true;

	private bool voiceChatDisabled = true;

	public float MasterVolume
	{
		get
		{
			return masterVolumeSlider.NormalizedValue;
		}
		set
		{
			float value2 = masterVolumeSlider.MaxValue * value;
			masterVolumeSlider.SetValue(value2);
		}
	}

	public float MusicVolume
	{
		get
		{
			return musicVolumeSlider.NormalizedValue;
		}
		set
		{
			float value2 = musicVolumeSlider.MaxValue * value;
			musicVolumeSlider.SetValue(value2);
		}
	}

	public float AmbienceVolume
	{
		get
		{
			return ambienceVolumeSlider.NormalizedValue;
		}
		set
		{
			float value2 = ambienceVolumeSlider.MaxValue * value;
			ambienceVolumeSlider.SetValue(value2);
		}
	}

	public float SFXVolume
	{
		get
		{
			return sfxVolumeSlider.NormalizedValue;
		}
		set
		{
			float value2 = sfxVolumeSlider.MaxValue * value;
			sfxVolumeSlider.SetValue(value2);
		}
	}

	public float UIVolume
	{
		get
		{
			return uiVolumeSlider.NormalizedValue;
		}
		set
		{
			float value2 = uiVolumeSlider.MaxValue * value;
			uiVolumeSlider.SetValue(value2);
		}
	}

	public float NotebookVolume
	{
		get
		{
			return notebookVolumeSlider.NormalizedValue;
		}
		set
		{
			float value2 = notebookVolumeSlider.MaxValue * value;
			notebookVolumeSlider.SetValue(value2);
		}
	}

	public bool IsMuted
	{
		get
		{
			return isMuted;
		}
		set
		{
			isMuted = value;
		}
	}

	public bool VoicechatDisabled
	{
		get
		{
			return voiceChatDisabled;
		}
		set
		{
			voiceChatDisabled = value;
		}
	}

	public override void SetFocus(bool gamepad)
	{
		masterVolumeSlider.Slider.Select();
	}

	public override void SettingStart()
	{
		base.SettingStart();
		InitializeDropdowns();
		VCA_Master = RuntimeManager.GetVCA("vca:/master");
		VCA_Music = RuntimeManager.GetVCA("vca:/music");
		VCA_Ambience = RuntimeManager.GetVCA("vca:/ambience");
		VCA_SFX = RuntimeManager.GetVCA("vca:/sfx");
		VCA_UI = RuntimeManager.GetVCA("vca:/ui");
		VCA_Notebook = RuntimeManager.GetVCA("vca:/logs");
		masterVolumeSlider.SliderEvent.AddListener(delegate
		{
			OnSliderValueChanged();
		});
		musicVolumeSlider.SliderEvent.AddListener(delegate
		{
			OnSliderValueChanged();
		});
		ambienceVolumeSlider.SliderEvent.AddListener(delegate
		{
			OnSliderValueChanged();
		});
		sfxVolumeSlider.SliderEvent.AddListener(delegate
		{
			OnSliderValueChanged();
		});
		uiVolumeSlider.SliderEvent.AddListener(delegate
		{
			OnSliderValueChanged();
		});
		notebookVolumeSlider.SliderEvent.AddListener(delegate
		{
			OnSliderValueChanged();
		});
	}

	public override void Save(RGD_Settings settings)
	{
		base.Save(settings);
		settings.audio.masterVolume = MasterVolume;
		settings.audio.musicVolume = MusicVolume;
		settings.audio.ambienceVolume = AmbienceVolume;
		settings.audio.sfxVolume = SFXVolume;
		settings.audio.uiVolume = UIVolume;
		settings.audio.notebookVolume = NotebookVolume;
		settings.audio.musicRateDropdownIndex = musicRateDropdown.value;
		settings.audio.isMuted = IsMuted;
		settings.audio.voiceChatDisabled = VoicechatDisabled;
	}

	public override void Load(RGD_Settings settings)
	{
		base.Load(settings);
		InitializeDropdowns();
		MasterVolume = settings.audio.masterVolume * masterVolumeSlider.MaxValue;
		MusicVolume = settings.audio.musicVolume * masterVolumeSlider.MaxValue;
		AmbienceVolume = settings.audio.ambienceVolume * masterVolumeSlider.MaxValue;
		SFXVolume = settings.audio.sfxVolume * masterVolumeSlider.MaxValue;
		UIVolume = settings.audio.uiVolume * masterVolumeSlider.MaxValue;
		NotebookVolume = settings.audio.notebookVolume * masterVolumeSlider.MaxValue;
		musicRateDropdown.value = settings.audio.musicRateDropdownIndex;
		musicRateDropdown.RefreshShownValue();
		if (firstTime)
		{
			firstTime = false;
			Invoke("LateLoad", 0.5f);
		}
		else
		{
			ApplyVolumeToMixer();
		}
		IsMuted = settings.audio.isMuted;
		VoicechatDisabled = settings.audio.voiceChatDisabled;
	}

	private void LateLoad()
	{
		ApplyVolumeToMixer();
	}

	private void InitializeDropdowns()
	{
		if (musicRateDropdown.options.Count == 0)
		{
			musicRateDropdown.GetComponent<LocalizeDropdownSemih>().UpdateLocalization();
		}
	}

	private void ApplyVolumeToMixer()
	{
		VCA_Master.setVolume(MasterVolume);
		VCA_Music.setVolume(MusicVolume);
		VCA_Ambience.setVolume(AmbienceVolume);
		VCA_SFX.setVolume(SFXVolume);
		VCA_UI.setVolume(UIVolume);
		VCA_Notebook.setVolume(NotebookVolume);
	}

	private void OnSliderValueChanged()
	{
		ApplyVolumeToMixer();
	}

	public void OnMusicRateDropdownChanged()
	{
		SoundManager value = ComponentManager<SoundManager>.Value;
		if (value != null)
		{
			float value2 = 0f;
			switch (musicRateDropdown.value)
			{
			case 0:
				value2 = 0f;
				break;
			case 1:
				value2 = 0.5f;
				break;
			case 2:
				value2 = 4f;
				break;
			case 3:
				value2 = 16f;
				break;
			case 4:
				value2 = 32f;
				break;
			}
			value.eventEmitter_Music.SetParameter("playbackrate", value2);
		}
		else
		{
			Debug.Log("Can't apply musicrate from audiosettingsbox because soundmanager was null");
		}
	}
}
