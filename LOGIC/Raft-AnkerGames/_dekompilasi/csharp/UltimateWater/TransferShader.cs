using UltimateWater.Internal;
using UnityEngine;

namespace UltimateWater;

public static class TransferShader
{
	public enum KernelType
	{
		Horizontal,
		Vertical,
		Unknown
	}

	private static readonly string[] _KernelName = new string[2] { "VerticalTransfer", "HorizontalTransfer" };

	private static readonly int[] _ThreadGroupX = new int[2];

	private static readonly int[] _ThreadGroupY = new int[2];

	private static readonly int[] _ThreadGroupZ = new int[2];

	private const string _SourceAName = "SourceA";

	private const string _SourceBName = "SourceB";

	private const string _DestinationAName = "DestinationA";

	private const string _DestinationBName = "DestinationB";

	private const string _InvWidthName = "InvWidth";

	private const string _InvHeightName = "InvHeight";

	private const string _FromName = "From";

	private const string _ToName = "To";

	private static readonly int[] _Kernel = new int[2] { -1, -1 };

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

	public static RenderTexture HorizontalSourceA
	{
		set
		{
			Shader.SetTexture(HorizontalKernel, "SourceA", value);
		}
	}

	public static RenderTexture HorizontalSourceB
	{
		set
		{
			Shader.SetTexture(HorizontalKernel, "SourceB", value);
		}
	}

	public static RenderTexture HorizontalDestinationA
	{
		set
		{
			Shader.SetTexture(HorizontalKernel, "DestinationA", value);
		}
	}

	public static RenderTexture HorizontalDestinationB
	{
		set
		{
			Shader.SetTexture(HorizontalKernel, "DestinationB", value);
		}
	}

	public static RenderTexture VerticalSourceA
	{
		set
		{
			Shader.SetTexture(VerticalKernel, "SourceA", value);
		}
	}

	public static RenderTexture VerticalSourceB
	{
		set
		{
			Shader.SetTexture(VerticalKernel, "SourceB", value);
		}
	}

	public static RenderTexture VerticalDestinationA
	{
		set
		{
			Shader.SetTexture(VerticalKernel, "DestinationA", value);
		}
	}

	public static RenderTexture VerticalDestinationB
	{
		set
		{
			Shader.SetTexture(VerticalKernel, "DestinationB", value);
		}
	}

	public static int From
	{
		set
		{
			Shader.SetInt("From", value);
		}
	}

	public static int To
	{
		set
		{
			Shader.SetInt("To", value);
		}
	}

	public static float Width
	{
		set
		{
			Shader.SetFloat("InvWidth", 1f / value);
		}
	}

	public static float Height
	{
		set
		{
			Shader.SetFloat("InvHeight", 1f / value);
		}
	}

	public static ComputeShader Shader => ShaderUtility.Instance.Get(ComputeShaderList.Transfer);

	public static void Dispatch(KernelType type, int width, int height)
	{
		Shader.Dispatch(_Kernel[(int)type], width / _ThreadGroupX[(int)type], height / _ThreadGroupY[(int)type], 1 / _ThreadGroupZ[(int)type]);
	}

	public static void DispatchVertical(int height)
	{
		Dispatch(KernelType.Vertical, height, 1);
	}

	public static void DistpachHorizontal(int width)
	{
		Dispatch(KernelType.Horizontal, 1, width);
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
