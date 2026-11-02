using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

[Serializable]
public class SoundManagerSnapshot
{
	[EventRef]
	[SerializeField]
	private string eventRef = string.Empty;

	public EventInstance snapShot;

	public void Initialize()
	{
		snapShot = RuntimeManager.CreateInstance(eventRef);
	}

	public void Destroy()
	{
		snapShot.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		snapShot.release();
		snapShot.clearHandle();
	}
}
