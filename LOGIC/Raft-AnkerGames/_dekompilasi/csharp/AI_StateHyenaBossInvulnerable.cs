using UnityEngine;

public class AI_StateHyenaBossInvulnerable : AI_Component_ApplyInvurnerability
{
	[SerializeField]
	private Renderer renderer;

	[SerializeField]
	private float shaderIntensity = 1.5f;

	private MaterialPropertyBlock materialPropertyBlock;

	public override void OnEnterState()
	{
		base.OnEnterState();
		if (materialPropertyBlock == null)
		{
			materialPropertyBlock = new MaterialPropertyBlock();
		}
		renderer.GetPropertyBlock(materialPropertyBlock);
		materialPropertyBlock.SetFloat("_Invulnarable_Intensity", shaderIntensity);
		renderer.SetPropertyBlock(materialPropertyBlock);
	}

	public override void OnExitState()
	{
		base.OnExitState();
		if (materialPropertyBlock == null)
		{
			materialPropertyBlock = new MaterialPropertyBlock();
		}
		renderer.GetPropertyBlock(materialPropertyBlock);
		materialPropertyBlock.SetFloat("_Invulnarable_Intensity", 0f);
		renderer.SetPropertyBlock(materialPropertyBlock);
	}
}
