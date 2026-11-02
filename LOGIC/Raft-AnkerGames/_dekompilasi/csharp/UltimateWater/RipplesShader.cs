using UltimateWater.Internal;
using UnityEngine;

namespace UltimateWater;

public static class RipplesShader
{
	private static int _ThreadGroupX;

	private static int _ThreadGroupY;

	private static int _ThreadGroupZ;

	private const string _KernelName = "Simulation";

	private const string _DepthName = "Depth";

	private const string _PreviousDepthName = "DepthT1";

	private const string _StaticDepthName = "StaticDepth";

	private const string _SizeInvName = "SizeInv";

	private const string _SizeName = "Size";

	private const string _PrimaryName = "MatrixT1";

	private const string _SecondaryName = "MatrixT2";

	private const string _PropagationName = "Propagation";

	private const string _DampingName = "Damping";

	private const string _GainName = "Gain";

	private const string _HeightGainName = "HeightGain";

	private const string _HeightOffsetName = "HeightOffset";

	private static int _Kernel = -1;

	public static int Kernel
	{
		get
		{
			if (_Kernel == -1)
			{
				_Kernel = Shader.FindKernel("Simulation");
				Shader.GetKernelThreadGroupSizes(Kernel, out var x, out var y, out var z);
				_ThreadGroupX = (int)x;
				_ThreadGroupY = (int)y;
				_ThreadGroupZ = (int)z;
			}
			return _Kernel;
		}
	}

	public static Vector2 Size
	{
		set
		{
			Shader.SetVector("SizeInv", new Vector2(1f / value.x, 1f / value.y));
			Shader.SetInts("Size", (int)value.x, (int)value.y);
		}
	}

	public static ComputeShader Shader => ShaderUtility.Instance.Get(ComputeShaderList.Simulation);

	public static void SetDepth(Texture value, Material material)
	{
		SetTextureShaderVariable("Depth", value, material);
	}

	public static void SetPreviousDepth(Texture value, Material material)
	{
		SetTextureShaderVariable("DepthT1", value, material);
	}

	public static void SetStaticDepth(Texture value, Material material)
	{
		SetTextureShaderVariable("StaticDepth", value, material);
	}

	public static void SetPrimary(Texture value, Material material)
	{
		SetTextureShaderVariable("MatrixT1", value, material);
	}

	public static void SetSecondary(Texture value, Material material)
	{
		SetTextureShaderVariable("MatrixT2", value, material);
	}

	public static void SetDamping(float value, Material material)
	{
		SetFloatShaderVariable("Damping", value, material);
	}

	public static void SetPropagation(float value, Material material)
	{
		SetFloatShaderVariable("Propagation", value, material);
	}

	public static void SetGain(float value, Material material)
	{
		SetFloatShaderVariable("Gain", value, material);
	}

	public static void SetHeightGain(float value, Material material)
	{
		SetFloatShaderVariable("HeightGain", value, material);
	}

	public static void SetHeightOffset(float value, Material material)
	{
		SetFloatShaderVariable("HeightOffset", value, material);
	}

	public static void Dispatch(int width, int height)
	{
		Shader.Dispatch(Kernel, width / _ThreadGroupX, height / _ThreadGroupY, 1 / _ThreadGroupZ);
	}

	private static void SetFloatShaderVariable(string name, float value, Material material)
	{
		switch (WaterQualitySettings.Instance.Ripples.ShaderMode)
		{
		case WaterRipplesData.ShaderModes.ComputeShader:
			Shader.SetFloat(name, value);
			break;
		case WaterRipplesData.ShaderModes.PixelShader:
			material.SetFloat(name, value);
			break;
		}
	}

	private static void SetTextureShaderVariable(string name, Texture value, Material material)
	{
		switch (WaterQualitySettings.Instance.Ripples.ShaderMode)
		{
		case WaterRipplesData.ShaderModes.ComputeShader:
			Shader.SetTexture(Kernel, name, value);
			break;
		case WaterRipplesData.ShaderModes.PixelShader:
			material.SetTexture(name, value);
			break;
		}
	}
}
