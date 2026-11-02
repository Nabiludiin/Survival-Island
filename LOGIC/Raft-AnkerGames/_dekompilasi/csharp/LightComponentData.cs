using System;
using UnityEngine;

[Serializable]
public class LightComponentData : QuestInteractable_ComponentData
{
	public bool lightEnabled;

	public LightComponentSubData[] lightSettings;

	public override void OnFinishedInteraction(Network_Player player, bool atStart, bool successfull)
	{
		base.OnFinishedInteraction(player, atStart, successfull);
		LightComponentSubData[] array = lightSettings;
		foreach (LightComponentSubData lightComponentSubData in array)
		{
			Light[] lights = lightComponentSubData.lights;
			foreach (Light light in lights)
			{
				if (light != null)
				{
					light.enabled = lightEnabled;
				}
			}
			Renderer[] renderers = lightComponentSubData.renderers;
			foreach (Renderer renderer in renderers)
			{
				if (renderer != null)
				{
					renderer.sharedMaterial = (lightEnabled ? lightComponentSubData.emissionMaterial : lightComponentSubData.normalMaterial);
				}
			}
		}
	}
}
