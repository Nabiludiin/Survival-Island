using UltimateWater.Internal;
using UnityEngine;

namespace UltimateWater;

public static class SetupShader
{
	public const string KernelName = "Setup";

	public const string MultiName = "MultiSetup";

	public const string PrimaryName = "Previous";

	public const string SecondaryName = "Current";

	public const string PositionName = "Position";

	public const string ForceName = "Force";

	public const string ScaleName = "_Scale";

	private static int _Kernel = -1;

	private static int _Multi = -1;

	public static int Kernel
	{
		get
		{
			if (_Kernel == -1)
			{
				_Kernel = Shader.FindKernel("Setup");
			}
			return _Kernel;
		}
	}

	public static int Multi
	{
		get
		{
			if (_Multi == -1)
			{
				_Multi = Shader.FindKernel("MultiSetup");
			}
			return _Multi;
		}
	}

	public static RenderTexture Primary
	{
		set
		{
			Shader.SetTexture(Kernel, "Previous", value);
		}
	}

	public static RenderTexture Secondary
	{
		set
		{
			Shader.SetTexture(Kernel, "Current", value);
		}
	}

	public static Vector2 Position
	{
		set
		{
			Shader.SetVector("Position", value);
		}
	}

	public static float Force
	{
		set
		{
			Shader.SetFloat("Force", value);
		}
	}

	public static int Scale
	{
		set
		{
			Shader.SetInt("_Scale", value);
		}
	}

	public static ComputeShader Shader => ShaderUtility.Instance.Get(ComputeShaderList.Setup);

	public static void Dispatch()
	{
		Shader.Dispatch(Kernel, 1, 1, 1);
	}
}
