using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace UltimateWater.Internal;

[Serializable]
public sealed class BlurSSS : Blur
{
	[SerializeField]
	[FormerlySerializedAs("initializedDefaults")]
	private bool _InitializedDefaults;

	public override void Validate(string shaderName, string computeShaderName = null, int kernelIndex = 0)
	{
		base.Validate(shaderName, computeShaderName, kernelIndex);
		if (!_InitializedDefaults)
		{
			Iterations = 5;
			_InitializedDefaults = true;
		}
	}

	public void Apply(RenderTexture source, RenderTexture target, Color absorptionColor, float worldSpaceSize, float lightFractionToIgnore)
	{
		ApplyPixelShader(source, target, absorptionColor, worldSpaceSize, lightFractionToIgnore);
	}

	private void ApplyPixelShader(RenderTexture source, RenderTexture target, Color absorptionColor, float worldSpaceSize, float lightFractionToIgnore)
	{
		Color value = absorptionColor * (-2.5f * worldSpaceSize);
		float num = ((value.r > value.g) ? value.r : value.g);
		if (value.b > num)
		{
			num = value.b;
		}
		float value2 = Mathf.Log(lightFractionToIgnore) / num;
		FilterMode filterMode = source.filterMode;
		source.filterMode = FilterMode.Bilinear;
		Material blurMaterial = BlurMaterial;
		blurMaterial.SetColor(ShaderVariables.AbsorptionColorPerPixel, value);
		blurMaterial.SetFloat(ShaderVariables.MaxDistance, value2);
		Graphics.Blit(source, target, blurMaterial, 0);
		source.filterMode = filterMode;
	}
}
