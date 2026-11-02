using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace UltimateWater.Internal;

[Serializable]
public class Blur
{
	[HideInInspector]
	[SerializeField]
	[FormerlySerializedAs("blurComputeShader")]
	protected ComputeShader _BlurComputeShader;

	[HideInInspector]
	[SerializeField]
	[FormerlySerializedAs("blurShader")]
	private Shader _BlurShader;

	[HideInInspector]
	[SerializeField]
	[FormerlySerializedAs("computeShaderKernelIndex")]
	private int _ComputeShaderKernelIndex;

	[FormerlySerializedAs("iterations")]
	[Range(0f, 10f)]
	[SerializeField]
	private int _Iterations = 1;

	[FormerlySerializedAs("size")]
	[SerializeField]
	private float _Size = 0.005f;

	private Material _BlurMaterial;

	private int _PassIndex;

	protected ComputeBuffer _ShaderWeights;

	public int Iterations
	{
		get
		{
			return _Iterations;
		}
		set
		{
			float totalSize = TotalSize;
			_Iterations = value;
			TotalSize = totalSize;
		}
	}

	public float Size
	{
		get
		{
			return _Size;
		}
		set
		{
			_Size = value;
		}
	}

	public float TotalSize
	{
		get
		{
			return _Size * (float)_Iterations;
		}
		set
		{
			_Size = value / (float)_Iterations;
		}
	}

	public Material BlurMaterial
	{
		get
		{
			if (_BlurMaterial == null)
			{
				if (_BlurShader == null)
				{
					Validate();
				}
				_BlurMaterial = new Material(_BlurShader)
				{
					hideFlags = HideFlags.DontSave
				};
			}
			return _BlurMaterial;
		}
		set
		{
			_BlurMaterial = value;
		}
	}

	public int PassIndex
	{
		get
		{
			return _PassIndex;
		}
		set
		{
			_PassIndex = value;
		}
	}

	public virtual void Validate(string shaderName, string computeShaderName = null, int kernelIndex = 0)
	{
		_BlurShader = Shader.Find(shaderName);
		if (computeShaderName != null)
		{
			_BlurComputeShader = Resources.Load<ComputeShader>(computeShaderName);
			_ComputeShaderKernelIndex = kernelIndex;
		}
		else
		{
			_BlurComputeShader = null;
		}
	}

	public void Dispose()
	{
		if (_BlurMaterial != null)
		{
			UnityEngine.Object.DestroyImmediate(_BlurMaterial);
		}
		if (_ShaderWeights != null)
		{
			_ShaderWeights.Release();
			_ShaderWeights = null;
		}
	}

	public void Validate()
	{
		Validate("UltimateWater/Utilities/Blur", "Shaders/Blurs");
	}

	public void Apply(RenderTexture target)
	{
		if (_Iterations != 0)
		{
			ApplyPixelShader(target);
		}
	}

	protected void ApplyComputeShader(RenderTexture target)
	{
		if (_ShaderWeights == null)
		{
			_ShaderWeights = new ComputeBuffer(4, 16);
			_ShaderWeights.SetData(new Color[4]
			{
				new Color(0.324f, 0.324f, 0.324f, 1f),
				new Color(0.232f, 0.232f, 0.232f, 1f),
				new Color(0.0855f, 0.0855f, 0.0855f, 1f),
				new Color(0.0205f, 0.0205f, 0.0205f, 1f)
			});
		}
		int num = target.width switch
		{
			256 => _ComputeShaderKernelIndex + 2, 
			128 => _ComputeShaderKernelIndex, 
			_ => _ComputeShaderKernelIndex + 4, 
		};
		TemporaryRenderTexture temporary = RenderTexturesCache.GetTemporary(target.width, target.height, 0, target.format, linear: true, uav: true);
		_BlurComputeShader.SetBuffer(num, "weights", _ShaderWeights);
		_BlurComputeShader.SetTexture(num, "_MainTex", target);
		_BlurComputeShader.SetTexture(num, "_Output", (RenderTexture)temporary);
		_BlurComputeShader.Dispatch(num, 1, target.height, 1);
		num++;
		_BlurComputeShader.SetBuffer(num, "weights", _ShaderWeights);
		_BlurComputeShader.SetTexture(num, "_MainTex", (RenderTexture)temporary);
		_BlurComputeShader.SetTexture(num, "_Output", target);
		_BlurComputeShader.Dispatch(num, target.width, 1, 1);
		temporary.Dispose();
	}

	protected void ApplyPixelShader(RenderTexture target)
	{
		Material blurMaterial = BlurMaterial;
		FilterMode filterMode = target.filterMode;
		target.filterMode = FilterMode.Bilinear;
		RenderTexture temporary = RenderTexture.GetTemporary(target.width, target.height, 0, target.format);
		temporary.filterMode = FilterMode.Bilinear;
		temporary.name = "[UWS] Blur - ApplyPixelShader temporary";
		for (int i = 0; i < _Iterations; i++)
		{
			float num = _Size * (1f + (float)i * 0.5f);
			blurMaterial.SetVector(ShaderVariables.Offset, new Vector4(num, 0f, 0f, 0f));
			Graphics.Blit(target, temporary, blurMaterial, _PassIndex);
			blurMaterial.SetVector(ShaderVariables.Offset, new Vector4(0f, num, 0f, 0f));
			Graphics.Blit(temporary, target, blurMaterial, _PassIndex);
		}
		target.filterMode = filterMode;
		RenderTexture.ReleaseTemporary(temporary);
	}
}
