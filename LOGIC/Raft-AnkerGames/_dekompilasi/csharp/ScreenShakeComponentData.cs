using System;
using UnityEngine;

[Serializable]
public class ScreenShakeComponentData : QuestInteractable_ComponentData
{
	public SO_CameraShaker cameraShakeSettings;

	[Tooltip("How close does the player need to be to experience the shake")]
	public float shakeRadius = 10f;
}
