using UltimateWater.Internal;
using UnityEngine;

namespace UltimateWater;

public static class GaussianShader
{
	public enum KernelType
	{
		Horizontal,
		Vertical
	}

	private static readonly string[] _KernelName = new string[2] { "Gaussian_Horizontal", "Gaussian_Vertical" };

	private static readonly int[] _Kernel = new int[2] { -1, -1 };

	private static readonly int[] _ThreadGroupX = new int[2];

	private static readonly int[] _ThreadGroupY = new int[2];

	private static readonly int[] _ThreadGroupZ = new int[2];

	private static readonly string _InputName = "Input";

	private static readonly string _OutputName = "Output";

	private static readonly string _K0Name = "k0";

	private static readonly string _K1Name = "k1";

	private static readonly string _K2Name = "k2";

	public static int VerticalKernel
	{
		get
		{
			Assign(KernelType.Vertical);
			return _Kernel[1];
		}
	}

	public static int HorizontalKernel
	{
		get
		{
			Assign(KernelType.Horizontal);
			return _Kernel[0];
		}
	}

	public static float Term0
	{
		set
		{
			Shader.SetFloat(_K0Name, value);
		}
	}

	public static float Term1
	{
		set
		{
			Shader.SetFloat(_K1Name, value);
		}
	}

	public static float Term2
	{
		set
		{
			Shader.SetFloat(_K2Name, value);
		}
	}

	public static RenderTexture VerticalInput
	{
		set
		{
			Shader.SetTexture(VerticalKernel, _InputName, value);
		}
	}

	public static RenderTexture HorizontalInput
	{
		set
		{
			Shader.SetTexture(HorizontalKernel, _InputName, value);
		}
	}

	public static RenderTexture VerticalOutput
	{
		set
		{
			Shader.SetTexture(VerticalKernel, _OutputName, value);
		}
	}

	public static RenderTexture HorizontalOutput
	{
		set
		{
			Shader.SetTexture(HorizontalKernel, _OutputName, value);
		}
	}

	public static ComputeShader Shader => ShaderUtility.Instance.Get(ComputeShaderList.Gauss);

	public static void Dispatch(KernelType type, int width, int height)
	{
		Shader.Dispatch(_Kernel[(int)type], width / _ThreadGroupX[(int)type], height / _ThreadGroupY[(int)type], 1 / _ThreadGroupZ[(int)type]);
	}

	private static void Assign(KernelType type)
	{
		if (_Kernel[(int)type] == -1)
		{
			_Kernel[(int)type] = Shader.FindKernel(_KernelName[(int)type]);
			Shader.GetKernelThreadGroupSizes(_Kernel[(int)type], out var x, out var y, out var z);
			_ThreadGroupX[(int)type] = (int)x;
			_ThreadGroupY[(int)type] = (int)y;
			_ThreadGroupZ[(int)type] = (int)z;
		}
	}
}
