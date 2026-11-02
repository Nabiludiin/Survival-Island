using System;
using UnityEngine;

[Serializable]
public class MaterialShaderPropertyData : QuestInteractable_ComponentData
{
	public Renderer renderer;

	public string shaderPropertyName = "Undefined";

	public float materialPropertyValue;

	[HideInInspector]
	public MaterialPropertyBlock materialPropertyBlock;

	public override void OnFinishedInteraction(Network_Player player, bool atStart, bool successfull)
	{
		base.OnFinishedInteraction(player, atStart, successfull);
		if (shaderPropertyName != "Undefined" && renderer != null)
		{
			renderer.GetPropertyBlock(materialPropertyBlock);
			materialPropertyBlock.SetFloat(shaderPropertyName, materialPropertyValue);
			renderer.SetPropertyBlock(materialPropertyBlock);
		}
	}
}
