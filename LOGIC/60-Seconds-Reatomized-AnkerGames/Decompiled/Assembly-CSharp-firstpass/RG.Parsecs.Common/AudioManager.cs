using System;
using System.Collections;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace RG.Parsecs.Common;

public class AudioManager : MonoBehaviour
{
	private static AudioManager _instance;

	[SerializeField]
	private string[] _musicBusNames;

	[SerializeField]
	private string[] _sfxBusNames;

	[SerializeField]
	private string[] _uiBusNames;

	private bool _initialized;

	private Bus[] _musicBuses;

	private Bus[] _sfxBuses;

	private Bus[] _uiBuses;

	private EventInstance _musicEventInstance;

	private readonly WaitForSeconds WAIT_FOR_TENTH_OF_SECOND = new WaitForSeconds(0.1f);

	private const string PITCH_PARAM_NAME = "Pitch";

	private const string PAN_PARAM_NAME = "Pan";

	public static AudioManager Instance => _instance;

	private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
		}
		else if (_instance != this)
		{
			UnityEngine.Object.Destroy(gameObject);
			return;
		}
		Initialize();
	}

	public void Initialize()
	{
		if (!_initialized)
		{
			InitializeBuses();
			_initialized = true;
		}
	}

	private void InitializeBuses()
	{
		InitializeFmodBuses(_musicBusNames, ref _musicBuses);
		InitializeFmodBuses(_sfxBusNames, ref _sfxBuses);
		InitializeFmodBuses(_uiBusNames, ref _uiBuses);
	}

	private void InitializeFmodBuses(string[] names, ref Bus[] buses)
	{
		if (buses == null)
		{
			buses = new Bus[names.Length];
		}
		for (int i = 0; i < names.Length; i++)
		{
			buses[i] = GetBus(names[i]);
		}
	}

	private Bus GetBus(string busName)
	{
		try
		{
			return RuntimeManager.GetBus(busName);
		}
		catch (Exception ex)
		{
			Debug.LogWarning(ex.Message);
		}
		return default;
	}

	public float GetVolume(string busName)
	{
		Bus bus = GetBus(busName);
		return GetVolume(bus);
	}

	private float GetVolume(Bus bus)
	{
		float finalvolume = 0f;
		float volume = 0f;
		if (bus.isValid())
		{
			bus.getVolume(out volume, out finalvolume);
		}
		return volume;
	}

	private float GetFirstValidBusVolume(Bus[] buses)
	{
		float result = 0f;
		for (int i = 0; i < buses.Length; i++)
		{
			if (buses[i].isValid())
			{
				result = GetVolume(buses[i]);
				break;
			}
		}
		return result;
	}

	public void SetVolume(string busName, float volume)
	{
		Bus bus = GetBus(busName);
		SetVolume(bus, volume);
	}

	private void SetVolume(Bus bus, float volume)
	{
		if (bus.isValid())
		{
			bus.setVolume(volume);
		}
	}

	private void SetVolume(Bus[] buses, float volume)
	{
		for (int i = 0; i < buses.Length; i++)
		{
			SetVolume(buses[i], volume);
		}
	}

	public void SetMuted(string busName, bool muted)
	{
		Bus bus = GetBus(busName);
		SetMuted(bus, muted);
	}

	private void SetMuted(Bus bus, bool muted)
	{
		if (bus.isValid())
		{
			bus.setMute(muted);
		}
	}

	private void SetMuted(Bus[] buses, bool muted)
	{
		for (int i = 0; i < buses.Length; i++)
		{
			SetMuted(buses[i], muted);
		}
	}

	public bool IsMuted(string busName)
	{
		Bus bus = GetBus(busName);
		return IsMuted(bus);
	}

	private bool IsMuted(Bus bus)
	{
		bool mute = true;
		if (bus.isValid())
		{
			bus.getMute(out mute);
		}
		return mute;
	}

	private bool IsMuted(Bus[] buses)
	{
		bool flag = true;
		for (int i = 0; i < buses.Length; i++)
		{
			flag &= IsMuted(buses[i]);
		}
		return flag;
	}

	public void SetPausedAll(bool paused)
	{
		if (RuntimeManager.IsInitialized)
		{
			RuntimeManager.PauseAllEvents(paused);
		}
	}

	public void SetPaused(string busName, bool paused)
	{
		Bus bus = GetBus(busName);
		SetPaused(bus, paused);
	}

	private void SetPaused(Bus bus, bool paused)
	{
		if (bus.isValid())
		{
			bus.setPaused(paused);
		}
	}

	private void SetPaused(Bus[] buses, bool paused)
	{
		for (int i = 0; i < buses.Length; i++)
		{
			SetPaused(buses[i], paused);
		}
	}

	public bool IsPaused(string busName)
	{
		Bus bus = GetBus(busName);
		return IsPaused(bus);
	}

	private bool IsPaused(Bus bus)
	{
		bool paused = false;
		if (bus.isValid())
		{
			bus.getPaused(out paused);
		}
		return paused;
	}

	private bool IsPaused(Bus[] buses)
	{
		bool flag = true;
		for (int i = 0; i < buses.Length; i++)
		{
			flag &= IsPaused(buses[i]);
		}
		return flag;
	}

	public void StopPlayingImmediately(string busName)
	{
		Bus bus = GetBus(busName);
		StopPlayingImmediately(bus);
	}

	private void StopPlayingImmediately(Bus bus)
	{
		bus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
	}

	private void StopPlayingImmediately(Bus[] buses)
	{
		for (int i = 0; i < buses.Length; i++)
		{
			StopPlayingImmediately(buses[i]);
		}
	}

	public void StopPlayingFadeOut(string busName)
	{
		Bus bus = GetBus(busName);
		StopPlayingFadeOut(bus);
	}

	private void StopPlayingFadeOut(Bus bus)
	{
		bus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
	}

	private void StopPlayingFadeOut(Bus[] buses)
	{
		for (int i = 0; i < buses.Length; i++)
		{
			StopPlayingFadeOut(buses[i]);
		}
	}

	public static void PlaySoundAtPoint(string eventRef, Transform transform, float volume = 1f, float pitch = 1f, float pan = 0f)
	{
		StudioEventEmitter studioEventEmitter = SpawnNewEmitter(eventRef, transform.position);
		if (!(studioEventEmitter == null))
		{
			studioEventEmitter.Event = eventRef;
			studioEventEmitter.SetParameter("Pan", pan);
			studioEventEmitter.SetParameter("Pitch", pitch);
			studioEventEmitter.EventInstance.setVolume(volume);
			studioEventEmitter.Play();
			if (Instance != null && Instance.gameObject.activeSelf)
			{
				Instance.StartCoroutine(DestroyEmitterWhenFinished(studioEventEmitter));
			}
		}
	}

	private static IEnumerator DestroyEmitterWhenFinished(StudioEventEmitter emitter)
	{
		if (emitter == null)
		{
			yield return null;
		}
		while (emitter.IsPlaying() && !(emitter == null))
		{
			yield return Instance.WAIT_FOR_TENTH_OF_SECOND;
		}
		if (emitter != null && emitter.gameObject != null)
		{
			UnityEngine.Object.Destroy(emitter.gameObject);
		}
	}

	private static StudioEventEmitter SpawnNewEmitter(string name, Vector3 position)
	{
		GameObject gameObject = new GameObject(name);
		gameObject.transform.SetPositionAndRotation(position, Quaternion.identity);
		return gameObject.AddComponent<StudioEventEmitter>();
	}

	public static void PlaySoundWithParameter(string eventRef, string paramName, float paramValue, float volume = 1f, float pitch = 1f, float pan = 0f)
	{
		EventInstance eventInstance = CreateEventInstance(eventRef, pitch, pan, volume);
		eventInstance.setParameterByName(paramName, paramValue);
		eventInstance.start();
		eventInstance.release();
	}

	public static void PlaySound(string eventRef, float volume = 1f, float pitch = 1f, float pan = 0f)
	{
		PlaySoundAndReturnInstance(eventRef, volume, pitch, pan);
	}

	public static EventInstance PlaySoundAndReturnInstance(string eventRef, float volume = 1f, float pitch = 1f, float pan = 0f)
	{
		EventInstance result = CreateEventInstance(eventRef, pitch, pan, volume);
		if (result.isValid())
		{
			result.start();
			result.release();
		}
		return result;
	}

	public static void PlaySoundDelayed(string eventRef, float delayTime, float volume = 1f, float pitch = 1f, float pan = 0f)
	{
		EventInstance eventInstance = CreateEventInstance(eventRef, pitch, pan, volume);
		if (_instance != null && eventInstance.isValid())
		{
			_instance.StartCoroutine(_instance.PlaySoundDelayedCoroutine(eventInstance, delayTime));
		}
	}

	private IEnumerator PlaySoundDelayedCoroutine(EventInstance eventInstance, float delayTime)
	{
		yield return new WaitForSeconds(delayTime);
		if (eventInstance.isValid())
		{
			eventInstance.start();
			eventInstance.release();
		}
	}

	private static EventInstance CreateEventInstance(string eventRef, float volume, float pitch, float pan)
	{
		if (!RuntimeManager.IsInitialized)
		{
			return default;
		}
		EventInstance result = RuntimeManager.CreateInstance(eventRef);
		result.setParameterByName("Pitch", pitch);
		result.setParameterByName("Pan", pan);
		result.setVolume(volume);
		return result;
	}

	public static void StopSound(EventInstance eventInstance, FMOD.Studio.STOP_MODE stopMode)
	{
		if (eventInstance.isValid())
		{
			eventInstance.stop(stopMode);
		}
	}

	public void PlayMusicImmediately(string eventRef, float volume = 1f, float pitch = 1f, float pan = 0.5f)
	{
		PlayMusic(eventRef, volume, pitch, pan, FMOD.Studio.STOP_MODE.IMMEDIATE);
	}

	public void PlayMusicFadeOut(string eventRef, float volume = 1f, float pitch = 1f, float pan = 0.5f)
	{
		PlayMusic(eventRef, volume, pitch, pan, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
	}

	private void PlayMusic(string eventRef, float volume, float pitch, float pan, FMOD.Studio.STOP_MODE stopMode)
	{
		if (!_musicEventInstance.isValid())
		{
			_musicEventInstance = CreateEventInstance(eventRef, volume, pitch, pan);
		}
		else
		{
			_musicEventInstance.getPlaybackState(out var state);
			if (state != PLAYBACK_STATE.STOPPED)
			{
				_musicEventInstance.stop(stopMode);
			}
			_musicEventInstance.release();
			_musicEventInstance = CreateEventInstance(eventRef, volume, pitch, pan);
		}
		_musicEventInstance.start();
	}

	public void SetMusicEventParameter(string paramName, float paramValue)
	{
		if (_musicEventInstance.isValid())
		{
			_musicEventInstance.setParameterByName(paramName, paramValue);
		}
	}

	public float GetMusicEventParameter(string paramName)
	{
		if (!_musicEventInstance.isValid())
		{
			return 0f;
		}
		float value = 0f;
		_musicEventInstance.getParameterByName(paramName, out value);
		return value;
	}

	public float GetMusicVolume()
	{
		if (!_initialized)
		{
			return 0f;
		}
		return GetFirstValidBusVolume(_musicBuses);
	}

	public void SetMusicVolume(float volume)
	{
		if (_initialized)
		{
			SetVolume(_musicBuses, volume);
		}
	}

	public void SetMusicMuted(bool muted)
	{
		if (_initialized)
		{
			SetMuted(_musicBuses, muted);
		}
	}

	public bool IsMusicMuted()
	{
		if (!_initialized)
		{
			return false;
		}
		return IsMuted(_musicBuses);
	}

	public bool IsSfxMuted()
	{
		if (!_initialized)
		{
			return false;
		}
		return IsMuted(_sfxBuses);
	}

	public void SetMusicPaused(bool paused)
	{
		if (_initialized)
		{
			SetPaused(_musicBuses, paused);
		}
	}

	public bool IsMusicPaused()
	{
		if (!_initialized)
		{
			return false;
		}
		return IsPaused(_musicBuses);
	}

	public void StopPlayingMusicImmediately()
	{
		if (_initialized)
		{
			StopPlayingImmediately(_musicBuses);
		}
	}

	public void StopPlayingMusicFadeOut()
	{
		if (_initialized)
		{
			StopPlayingFadeOut(_musicBuses);
		}
	}

	public float GetSfxVolume()
	{
		if (!_initialized)
		{
			return 0f;
		}
		return GetFirstValidBusVolume(_sfxBuses);
	}

	public void SetSfxVolume(float volume)
	{
		if (_initialized)
		{
			SetVolume(_sfxBuses, volume);
		}
	}

	public void SetSfxMuted(bool muted)
	{
		if (_initialized)
		{
			SetMuted(_sfxBuses, muted);
		}
	}

	public void SetSfxPaused(bool paused)
	{
		if (_initialized)
		{
			SetPaused(_sfxBuses, paused);
		}
	}

	public bool IsSfxPaused()
	{
		if (!_initialized)
		{
			return false;
		}
		return IsPaused(_sfxBuses);
	}

	public void StopPlayingSfxImmediately()
	{
		if (_initialized)
		{
			StopPlayingImmediately(_sfxBuses);
		}
	}

	public void StopPlayingSfxFadeOut()
	{
		if (_initialized)
		{
			StopPlayingFadeOut(_sfxBuses);
		}
	}

	public float GetUiVolume()
	{
		if (!_initialized)
		{
			return 0f;
		}
		return GetFirstValidBusVolume(_uiBuses);
	}

	public void SetUiVolume(float volume)
	{
		if (_initialized)
		{
			SetVolume(_uiBuses, volume);
		}
	}

	public void SetUiMuted(bool muted)
	{
		if (_initialized)
		{
			SetMuted(_uiBuses, muted);
		}
	}

	public void SetUiPaused(bool paused)
	{
		if (_initialized)
		{
			SetPaused(_uiBuses, paused);
		}
	}

	public bool IsUiPaused()
	{
		if (!_initialized)
		{
			return false;
		}
		return IsPaused(_uiBuses);
	}

	public void StopPlayingUiImmediately()
	{
		if (_initialized)
		{
			StopPlayingImmediately(_uiBuses);
		}
	}

	public void StopPlayingUiFadeOut()
	{
		if (_initialized)
		{
			StopPlayingFadeOut(_uiBuses);
		}
	}
}
