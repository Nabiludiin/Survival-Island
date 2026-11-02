using FMODUnity;
using UnityEngine;

public class GabFmodSoundTesting : MonoBehaviour
{
	public bool triggerViaSustainEmitter;

	public StudioEventEmitter emitter;

	public StudioEventEmitterSustain emitterSustain;

	private void Update()
	{
		emitter.EventInstance.getPlaybackState(out var state);
		Debug.Log(state);
	}

	private void PlaySound()
	{
		if (triggerViaSustainEmitter)
		{
			emitterSustain.Play();
		}
		else
		{
			emitter.Play();
		}
	}

	private void keyOffue()
	{
		if (triggerViaSustainEmitter)
		{
			emitter.EventInstance.keyOff();
		}
		else
		{
			emitterSustain.eventEmitter.EventInstance.keyOff();
		}
	}

	private void Stop()
	{
		if (triggerViaSustainEmitter)
		{
			emitterSustain.Stop();
		}
		else
		{
			emitter.Stop();
		}
	}

	private void StopHARD()
	{
		if (triggerViaSustainEmitter)
		{
			emitterSustain.StopHard();
		}
		else
		{
			emitter.Stop();
		}
	}
}
