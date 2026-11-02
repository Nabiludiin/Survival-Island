using FMOD.Studio;
using FMODUnity;
using RG.Parsecs.Common;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class MusicManager : MonoBehaviour
{
	[Tooltip("The music event set in this SoundSlot will be played back on start by this script.")]
	[SerializeField]
	private SoundSlot _music;

	[Tooltip("The ambient event set in this SoundSlot will be played back on start by this script.")]
	[SerializeField]
	private SoundSlot _ambient;

	[SerializeField]
	private string _musicBus = "bus:/";

	[SerializeField]
	private string _animationBus = "bus:/";

	[SerializeField]
	private string _sfxBus = "bus:/";

	[SerializeField]
	private float _startMusicParameter;

	private EventInstance _musicEvent;

	private void Start()
	{
		if (_music != null && !string.IsNullOrEmpty(_music.SoundEventName))
		{
			_musicEvent = RuntimeManager.CreateInstance(_music.SoundEventName);
			_musicEvent.setParameterByName("MusicPar", _startMusicParameter);
			_musicEvent.start();
		}
		else
		{
			Debug.LogWarning("Music SoundSlot not set or invalid in MusicManager.");
		}
		if (_ambient != null && !string.IsNullOrEmpty(_ambient.SoundEventName))
		{
			RuntimeManager.CreateInstance(_ambient.SoundEventName).start();
		}
		else
		{
			Debug.LogWarning("Ambient SoundSlot not set or invalid in MusicManager.");
		}
	}

	public void StopMusic()
	{
		if (!string.IsNullOrEmpty(_musicBus))
		{
			RuntimeManager.GetBus(_musicBus).stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
		else
		{
			Debug.LogWarning("MusicBus not properly set in MusicManager");
		}
		if (!string.IsNullOrEmpty(_animationBus))
		{
			RuntimeManager.GetBus(_animationBus).stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
		else
		{
			Debug.LogWarning("AnimationBus not properly set in MusicManager");
		}
		if (!string.IsNullOrEmpty(_sfxBus))
		{
			RuntimeManager.GetBus(_sfxBus).stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
		else
		{
			Debug.LogWarning("SfxBus not properly set in MusicManager");
		}
	}

	public void SetMusicParameter(float parameter)
	{
		RuntimeManager.GetBus(_musicBus).stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		_musicEvent.setParameterByName("MusicPar", parameter);
		_musicEvent.start();
	}
}
