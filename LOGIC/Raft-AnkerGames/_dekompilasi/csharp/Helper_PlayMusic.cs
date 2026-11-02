using FMODUnity;
using UnityEngine;

public class Helper_PlayMusic : MonoBehaviour
{
	[SerializeField]
	private string parameterName = "fightintensity";

	[SerializeField]
	private float parameterValue = 1f;

	[SerializeField]
	private float parameterStoppedValue;

	[SerializeField]
	private bool stopOnRevert;

	protected SoundManager soundManager;

	public void Start()
	{
		soundManager = ComponentManager<SoundManager>.Value;
	}

	public virtual StudioEventEmitter GetEmitterFromSoundManager()
	{
		if (soundManager != null)
		{
			return soundManager.eventEmitter_Music;
		}
		return null;
	}

	public void ApplyMusicSettings()
	{
		StudioEventEmitter emitterFromSoundManager = GetEmitterFromSoundManager();
		if (emitterFromSoundManager != null)
		{
			if (emitterFromSoundManager.IsPlaying())
			{
				emitterFromSoundManager.Stop();
			}
			emitterFromSoundManager.EventInstance.setTimelinePosition(0);
			emitterFromSoundManager.Play();
			if (parameterName != string.Empty)
			{
				emitterFromSoundManager.SetParameter(parameterName, parameterValue);
			}
		}
	}

	public void RevertMusicSettings()
	{
		StudioEventEmitter emitterFromSoundManager = GetEmitterFromSoundManager();
		if (emitterFromSoundManager != null)
		{
			if (parameterName != string.Empty)
			{
				emitterFromSoundManager.SetParameter(parameterName, parameterStoppedValue);
			}
			if (stopOnRevert)
			{
				emitterFromSoundManager.Stop();
			}
		}
	}
}
