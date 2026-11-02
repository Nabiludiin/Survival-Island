using UnityEngine;

public class QuestInteractableComponent_MaterialShaderProperty : QuestInteractableComponentBase<MaterialShaderPropertyData>
{
	protected override void Awake()
	{
		base.Awake();
		foreach (MaterialShaderPropertyData dataComponent in dataComponents)
		{
			if (dataComponent is MaterialShaderPropertyData materialShaderPropertyData)
			{
				materialShaderPropertyData.materialPropertyBlock = new MaterialPropertyBlock();
			}
		}
	}
}
