using System;
using FMODUnity;
using UnityEngine;

[Serializable]
public class StateSoundPlaySetting
{
	public AIStateEvent stateEvent;

	public SoundSetting soundSetting;

	public StudioEventEmitter eventEmitter;

	[SerializeField]
	[EventRef]
	public string eventRef;

	public float soundCooldown;

	public float latestSoundPlayTimeStamp;
}
