using UnityEngine;

namespace UltimateWater.Internal;

public sealed class Dx11FFT : GpuFFT
{
	private readonly ComputeShader _Shader;

	private readonly int _KernelIndex;

	public Dx11FFT(ComputeShader shader, int resolution, bool highPrecision, bool twoChannels)
		: base(resolution, highPrecision, twoChannels, usesUAV: true)
	{
		_Shader = shader;
		_KernelIndex = _NumButterflies - 5 << 1;
		if (twoChannels)
		{
			_KernelIndex += 10;
		}
	}

	public override void SetupMaterials()
	{
	}

	public override void ComputeFFT(Texture tex, RenderTexture target)
	{
		TemporaryRenderTexture temporary = _RenderTexturesSet.GetTemporary();
		if (!target.IsCreated())
		{
			target.enableRandomWrite = true;
			target.Create();
		}
		_Shader.SetTexture(_KernelIndex, "_ButterflyTex", Butterfly);
		_Shader.SetTexture(_KernelIndex, "_SourceTex", tex);
		_Shader.SetTexture(_KernelIndex, "_TargetTex", (RenderTexture)temporary);
		_Shader.Dispatch(_KernelIndex, 1, tex.height, 1);
		_Shader.SetTexture(_KernelIndex + 1, "_ButterflyTex", Butterfly);
		_Shader.SetTexture(_KernelIndex + 1, "_SourceTex", (RenderTexture)temporary);
		_Shader.SetTexture(_KernelIndex + 1, "_TargetTex", target);
		_Shader.Dispatch(_KernelIndex + 1, 1, tex.height, 1);
		temporary.Dispose();
	}

	protected override void FillButterflyTexture(Texture2D butterfly, int[][] indices, Vector2[][] weights)
	{
		Color color = default;
		for (int i = 0; i < _NumButterflies; i++)
		{
			for (int j = 0; j < 2; j++)
			{
				int num = ((j != 0) ? _Resolution : 0);
				for (int k = 0; k < _Resolution; k++)
				{
					int num2 = _NumButterflies - i - 1;
					int num3 = k << 1;
					color.r = indices[num2][num3] + num;
					color.g = indices[num2][num3 + 1] + num;
					color.b = weights[i][k].x;
					color.a = weights[i][k].y;
					butterfly.SetPixel(num + k, i, color);
				}
			}
		}
	}
}
