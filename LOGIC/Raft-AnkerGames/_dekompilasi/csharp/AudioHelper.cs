using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public static class AudioHelper
{
	public static void PlayOneShotWithParameters(string fmodEvent, Vector3 position, params (string name, float value)[] parameters)
	{
		EventInstance eventInstance = RuntimeManager.CreateInstance(fmodEvent);
		for (int i = 0; i < parameters.Length; i++)
		{
			var (name, value) = parameters[i];
			eventInstance.setParameterByName(name, value);
		}
		eventInstance.set3DAttributes(position.To3DAttributes());
		eventInstance.start();
		eventInstance.release();
	}

	public static void PlayOneShotWithParameters(EventReference fmodEvent, Vector3 position, params (string name, float value)[] parameters)
	{
		if (!fmodEvent.IsNull)
		{
			EventInstance eventInstance = RuntimeManager.CreateInstance(fmodEvent);
			for (int i = 0; i < parameters.Length; i++)
			{
				var (name, value) = parameters[i];
				eventInstance.setParameterByName(name, value);
			}
			eventInstance.set3DAttributes(position.To3DAttributes());
			eventInstance.start();
			eventInstance.release();
		}
	}
}
