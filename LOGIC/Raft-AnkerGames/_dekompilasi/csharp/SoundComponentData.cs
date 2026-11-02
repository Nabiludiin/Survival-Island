using System;
using FMODUnity;
using UnityEngine;

[Serializable]
public class SoundComponentData : QuestInteractable_ComponentData
{
	public bool useOneShotSound = true;

	[EventRef]
	public string eventRef_sound;

	public EventReference er_sound;

	public Transform soundPlayLocationTransform;

	public StudioEventEmitter[] emitter;

	public int hapticEvent;
}
