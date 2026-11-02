using System;
using System.Collections.Generic;
using UnityEngine;

namespace UltimateWater.Internal;

public class SpectrumResolver : SpectrumResolverCPU
{
	public enum SpectrumType
	{
		Height,
		Normal,
		Displacement,
		RawDirectional,
		RawOmnidirectional
	}

	private Texture2D _TileSizeLookup;

	private Texture _OmnidirectionalSpectrum;

	private RenderTexture _TotalOmnidirectionalSpectrum;

	private RenderTexture _DirectionalSpectrum;

	private RenderTexture _HeightSpectrum;

	private RenderTexture _NormalSpectrum;

	private RenderTexture _DisplacementSpectrum;

	private RenderBuffer[] _RenderTargetsx2;

	private RenderBuffer[] _RenderTargetsx3;

	private bool _TileSizesLookupDirty = true;

	private bool _DirectionalSpectrumDirty = true;

	private Vector4 _TileSizes;

	private Mesh _SpectrumDownsamplingMesh;

	private readonly Material _AnimationMaterial;

	private readonly Water _Water;

	private readonly WindWaves _WindWaves;

	public Texture TileSizeLookup
	{
		get
		{
			ValidateTileSizeLookup();
			return _TileSizeLookup;
		}
	}

	public float RenderTime { get; private set; }

	public SpectrumResolver(Water water, WindWaves windWaves, Shader spectrumShader)
		: base(water, windWaves, 4)
	{
		_Water = water;
		_WindWaves = windWaves;
		_AnimationMaterial = new Material(spectrumShader)
		{
			hideFlags = HideFlags.DontSave
		};
		_AnimationMaterial.SetFloat(ShaderVariables.RenderTime, Time.time);
		if (windWaves.LoopDuration != 0f)
		{
			_AnimationMaterial.EnableKeyword("_LOOPING");
			_AnimationMaterial.SetFloat("_LoopDuration", windWaves.LoopDuration);
		}
	}

	public Texture RenderHeightSpectrumAt(float time)
	{
		CheckResources();
		RenderTexture rawDirectionalSpectrum = GetRawDirectionalSpectrum();
		RenderTime = time;
		_AnimationMaterial.SetFloat(ShaderVariables.RenderTime, time);
		Graphics.Blit(rawDirectionalSpectrum, _HeightSpectrum, _AnimationMaterial, 0);
		return _HeightSpectrum;
	}

	public Texture RenderNormalsSpectrumAt(float time)
	{
		CheckResources();
		RenderTexture rawDirectionalSpectrum = GetRawDirectionalSpectrum();
		RenderTime = time;
		_AnimationMaterial.SetFloat(ShaderVariables.RenderTime, time);
		Graphics.Blit(rawDirectionalSpectrum, _NormalSpectrum, _AnimationMaterial, 1);
		return _NormalSpectrum;
	}

	public void RenderDisplacementsSpectraAt(float time, out Texture height, out Texture displacement)
	{
		CheckResources();
		height = _HeightSpectrum;
		displacement = _DisplacementSpectrum;
		_RenderTargetsx2[0] = _HeightSpectrum.colorBuffer;
		_RenderTargetsx2[1] = _DisplacementSpectrum.colorBuffer;
		RenderTexture rawDirectionalSpectrum = GetRawDirectionalSpectrum();
		RenderTime = time;
		_AnimationMaterial.SetFloat(ShaderVariables.RenderTime, time);
		Graphics.SetRenderTarget(_RenderTargetsx2, _HeightSpectrum.depthBuffer);
		Graphics.Blit(rawDirectionalSpectrum, _AnimationMaterial, 5);
		Graphics.SetRenderTarget(null);
	}

	public void RenderCompleteSpectraAt(float time, out Texture height, out Texture normal, out Texture displacement)
	{
		CheckResources();
		height = _HeightSpectrum;
		normal = _NormalSpectrum;
		displacement = _DisplacementSpectrum;
		_RenderTargetsx3[0] = _HeightSpectrum.colorBuffer;
		_RenderTargetsx3[1] = _NormalSpectrum.colorBuffer;
		_RenderTargetsx3[2] = _DisplacementSpectrum.colorBuffer;
		RenderTexture rawDirectionalSpectrum = GetRawDirectionalSpectrum();
		RenderTime = time;
		_AnimationMaterial.SetFloat(ShaderVariables.RenderTime, time);
		Graphics.SetRenderTarget(_RenderTargetsx3, _HeightSpectrum.depthBuffer);
		Graphics.Blit(rawDirectionalSpectrum, _AnimationMaterial, 2);
		Graphics.SetRenderTarget(null);
	}

	public Texture GetSpectrum(SpectrumType type)
	{
		return type switch
		{
			SpectrumType.Height => _HeightSpectrum, 
			SpectrumType.Normal => _NormalSpectrum, 
			SpectrumType.Displacement => _DisplacementSpectrum, 
			SpectrumType.RawDirectional => _DirectionalSpectrum, 
			SpectrumType.RawOmnidirectional => _OmnidirectionalSpectrum, 
			_ => throw new InvalidOperationException(), 
		};
	}

	public override void AddSpectrum(WaterWavesSpectrumDataBase spectrum)
	{
		base.AddSpectrum(spectrum);
		_DirectionalSpectrumDirty = true;
	}

	public override void RemoveSpectrum(WaterWavesSpectrumDataBase spectrum)
	{
		base.RemoveSpectrum(spectrum);
		_DirectionalSpectrumDirty = true;
	}

	public override void SetDirectionalSpectrumDirty()
	{
		base.SetDirectionalSpectrumDirty();
		_DirectionalSpectrumDirty = true;
	}

	internal override void OnProfilesChanged()
	{
		base.OnProfilesChanged();
		if (_TileSizes != _WindWaves.TileSizes)
		{
			_TileSizesLookupDirty = true;
			_TileSizes = _WindWaves.TileSizes;
		}
		RenderTotalOmnidirectionalSpectrum();
	}

	private void RenderTotalOmnidirectionalSpectrum()
	{
		_AnimationMaterial.SetFloat("_Gravity", _Water.Gravity);
		_AnimationMaterial.SetVector("_TargetResolution", new Vector4(_WindWaves.FinalResolution, _WindWaves.FinalResolution, 0f, 0f));
		List<Water.WeightedProfile> profiles = _Water.ProfilesManager.Profiles;
		if (profiles.Count > 1)
		{
			RenderTexture totalOmnidirectionalSpectrum = GetTotalOmnidirectionalSpectrum();
			totalOmnidirectionalSpectrum.Clear(Color.black);
			for (int i = 0; i < profiles.Count; i++)
			{
				Water.WeightedProfile weightedProfile = profiles[i];
				if (!(weightedProfile.Weight <= 0.0001f))
				{
					WaterWavesSpectrum spectrum = weightedProfile.Profile.Spectrum;
					if (!_SpectraDataCache.TryGetValue(spectrum, out var value))
					{
						value = GetSpectrumData(spectrum);
					}
					_AnimationMaterial.SetFloat("_Weight", value.Weight);
					Graphics.Blit(value.Texture, totalOmnidirectionalSpectrum, _AnimationMaterial, 4);
				}
			}
			_OmnidirectionalSpectrum = totalOmnidirectionalSpectrum;
		}
		else if (profiles.Count != 0)
		{
			WaterWavesSpectrum spectrum2 = profiles[0].Profile.Spectrum;
			if (!_SpectraDataCache.TryGetValue(spectrum2, out var value2))
			{
				value2 = GetSpectrumData(spectrum2);
			}
			value2.Weight = 1f;
			_OmnidirectionalSpectrum = value2.Texture;
		}
		_Water.Renderer.PropertyBlock.SetFloat("_MaxDisplacement", MaxHorizontalDisplacement);
	}

	private void RenderDirectionalSpectrum()
	{
		if (_OmnidirectionalSpectrum == null)
		{
			RenderTotalOmnidirectionalSpectrum();
		}
		ValidateTileSizeLookup();
		_AnimationMaterial.SetFloat("_Directionality", 1f - _WindWaves.SpectrumDirectionality);
		_AnimationMaterial.SetVector("_WindDirection", WindDirection);
		_AnimationMaterial.SetTexture("_TileSizeLookup", _TileSizeLookup);
		Graphics.Blit(_OmnidirectionalSpectrum, _DirectionalSpectrum, _AnimationMaterial, 3);
		AddOverlayToDirectionalSpectrum();
		_DirectionalSpectrumDirty = false;
	}

	private void AddOverlayToDirectionalSpectrum()
	{
		if (_SpectrumDownsamplingMesh == null)
		{
			_SpectrumDownsamplingMesh = CreateDownsamplingMesh();
		}
		for (int num = _OverlayedSpectra.Count - 1; num >= 0; num--)
		{
			WaterWavesSpectrumDataBase waterWavesSpectrumDataBase = _OverlayedSpectra[num];
			Texture2D texture = waterWavesSpectrumDataBase.Texture;
			_AnimationMaterial.SetFloat("_Weight", waterWavesSpectrumDataBase.Weight);
			_AnimationMaterial.SetVector("_WindDirection", waterWavesSpectrumDataBase.WindDirection);
			float weatherSystemRadius = waterWavesSpectrumDataBase.WeatherSystemRadius;
			_AnimationMaterial.SetVector("_WeatherSystemRadius", new Vector4(2f * weatherSystemRadius, weatherSystemRadius * weatherSystemRadius, 0f, 0f));
			Vector2 weatherSystemOffset = waterWavesSpectrumDataBase.WeatherSystemOffset;
			_AnimationMaterial.SetVector("_WeatherSystemOffset", new Vector4(weatherSystemOffset.x, weatherSystemOffset.y, weatherSystemOffset.magnitude, 0f));
			Graphics.Blit(texture, _DirectionalSpectrum, _AnimationMaterial, 6);
		}
	}

	internal RenderTexture GetRawDirectionalSpectrum()
	{
		if ((_DirectionalSpectrumDirty || !_DirectionalSpectrum.IsCreated()) && Application.isPlaying)
		{
			CheckResources();
			RenderDirectionalSpectrum();
		}
		return _DirectionalSpectrum;
	}

	private RenderTexture GetTotalOmnidirectionalSpectrum()
	{
		if (_TotalOmnidirectionalSpectrum == null)
		{
			int num = _WindWaves.FinalResolution << 1;
			_TotalOmnidirectionalSpectrum = new RenderTexture(num, num, 0, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear)
			{
				name = "[UWS] SpectrumResolver - Total Omnidirectional Spectrum",
				hideFlags = HideFlags.DontSave,
				filterMode = FilterMode.Point,
				wrapMode = TextureWrapMode.Repeat
			};
		}
		return _TotalOmnidirectionalSpectrum;
	}

	private void CheckResources()
	{
		if (_HeightSpectrum == null)
		{
			int num = _WindWaves.FinalResolution << 1;
			bool finalHighPrecision = _WindWaves.FinalHighPrecision;
			_HeightSpectrum = new RenderTexture(num, num, 0, finalHighPrecision ? RenderTextureFormat.RGFloat : RenderTextureFormat.RGHalf, RenderTextureReadWrite.Linear)
			{
				name = "[UWS] SpectrumResolver - Water Height Spectrum",
				hideFlags = HideFlags.DontSave,
				filterMode = FilterMode.Point
			};
			_NormalSpectrum = new RenderTexture(num, num, 0, finalHighPrecision ? RenderTextureFormat.ARGBFloat : RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear)
			{
				name = "[UWS] SpectrumResolver - Water Normals Spectrum",
				hideFlags = HideFlags.DontSave,
				filterMode = FilterMode.Point
			};
			_DisplacementSpectrum = new RenderTexture(num, num, 0, finalHighPrecision ? RenderTextureFormat.ARGBFloat : RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear)
			{
				name = "[UWS] SpectrumResolver - Water Displacement Spectrum",
				hideFlags = HideFlags.DontSave,
				filterMode = FilterMode.Point
			};
			_DirectionalSpectrum = new RenderTexture(num, num, 0, finalHighPrecision ? RenderTextureFormat.RGFloat : RenderTextureFormat.RGHalf, RenderTextureReadWrite.Linear)
			{
				hideFlags = HideFlags.DontSave,
				filterMode = FilterMode.Point,
				wrapMode = TextureWrapMode.Clamp
			};
			_RenderTargetsx2 = new RenderBuffer[2] { _HeightSpectrum.colorBuffer, _DisplacementSpectrum.colorBuffer };
			_RenderTargetsx3 = new RenderBuffer[3] { _HeightSpectrum.colorBuffer, _NormalSpectrum.colorBuffer, _DisplacementSpectrum.colorBuffer };
		}
	}

	internal override void OnMapsFormatChanged(bool resolution)
	{
		base.OnMapsFormatChanged(resolution);
		if (_TotalOmnidirectionalSpectrum != null)
		{
			UnityEngine.Object.Destroy(_TotalOmnidirectionalSpectrum);
			_TotalOmnidirectionalSpectrum = null;
		}
		if (_HeightSpectrum != null)
		{
			UnityEngine.Object.Destroy(_HeightSpectrum);
			_HeightSpectrum = null;
		}
		if (_NormalSpectrum != null)
		{
			UnityEngine.Object.Destroy(_NormalSpectrum);
			_NormalSpectrum = null;
		}
		if (_DisplacementSpectrum != null)
		{
			UnityEngine.Object.Destroy(_DisplacementSpectrum);
			_DisplacementSpectrum = null;
		}
		if (_DirectionalSpectrum != null)
		{
			UnityEngine.Object.Destroy(_DirectionalSpectrum);
			_DirectionalSpectrum = null;
		}
		if (_TileSizeLookup != null)
		{
			UnityEngine.Object.Destroy(_TileSizeLookup);
			_TileSizeLookup = null;
			_TileSizesLookupDirty = true;
		}
		_OmnidirectionalSpectrum = null;
		_RenderTargetsx2 = null;
		_RenderTargetsx3 = null;
	}

	private void ValidateTileSizeLookup()
	{
		if (_TileSizesLookupDirty)
		{
			if (_TileSizeLookup == null)
			{
				_TileSizeLookup = new Texture2D(2, 2, SystemInfo.SupportsTextureFormat(TextureFormat.RGBAFloat) ? TextureFormat.RGBAFloat : TextureFormat.RGBAHalf, mipChain: false, linear: true)
				{
					hideFlags = HideFlags.DontSave,
					wrapMode = TextureWrapMode.Clamp,
					filterMode = FilterMode.Point
				};
			}
			_TileSizeLookup.SetPixel(0, 0, new Color(0.5f, 0.5f, 1f / _TileSizes.x, 0f));
			_TileSizeLookup.SetPixel(1, 0, new Color(1.5f, 0.5f, 1f / _TileSizes.y, 0f));
			_TileSizeLookup.SetPixel(0, 1, new Color(0.5f, 1.5f, 1f / _TileSizes.z, 0f));
			_TileSizeLookup.SetPixel(1, 1, new Color(1.5f, 1.5f, 1f / _TileSizes.w, 0f));
			_TileSizeLookup.Apply(updateMipmaps: false, makeNoLongerReadable: false);
			_TileSizesLookupDirty = false;
		}
	}

	private static void AddQuad(Vector3[] vertices, Vector3[] origins, Vector2[] uvs, int index, float xOffset, float yOffset, int originIndex)
	{
		originIndex += index;
		float num = xOffset * 2f - 1f;
		float num2 = yOffset * 2f - 1f;
		uvs[index] = new Vector2(xOffset, yOffset);
		vertices[index++] = new Vector3(num, num2, 0.1f);
		uvs[index] = new Vector2(xOffset, yOffset + 0.25f);
		vertices[index++] = new Vector3(num, num2 + 0.5f, 0.1f);
		uvs[index] = new Vector2(xOffset + 0.25f, yOffset + 0.25f);
		vertices[index++] = new Vector3(num + 0.5f, num2 + 0.5f, 0.1f);
		uvs[index] = new Vector2(xOffset + 0.25f, yOffset);
		vertices[index] = new Vector3(num + 0.5f, num2, 0.1f);
		origins[index--] = vertices[originIndex];
		origins[index--] = vertices[originIndex];
		origins[index--] = vertices[originIndex];
		origins[index] = vertices[originIndex];
	}

	private static void AddQuads(Vector3[] vertices, Vector3[] origins, Vector2[] uvs, int index, float xOffset, float yOffset)
	{
		AddQuad(vertices, origins, uvs, index, xOffset, yOffset, 0);
		AddQuad(vertices, origins, uvs, index + 4, xOffset + 0.25f, yOffset, 3);
		AddQuad(vertices, origins, uvs, index + 8, xOffset, yOffset + 0.25f, 1);
		AddQuad(vertices, origins, uvs, index + 12, xOffset + 0.25f, yOffset + 0.25f, 2);
	}

	private static Mesh CreateDownsamplingMesh()
	{
		Mesh mesh = new Mesh
		{
			name = "[PW Water] Spectrum Downsampling Mesh"
		};
		Vector3[] vertices = new Vector3[64];
		Vector3[] array = new Vector3[64];
		Vector2[] array2 = new Vector2[64];
		int[] array3 = new int[64];
		for (int i = 0; i < array3.Length; i++)
		{
			array3[i] = i;
		}
		AddQuads(vertices, array, array2, 0, 0f, 0f);
		AddQuads(vertices, array, array2, 16, 0.5f, 0f);
		AddQuads(vertices, array, array2, 32, 0f, 0.5f);
		AddQuads(vertices, array, array2, 48, 0.5f, 0.5f);
		mesh.vertices = vertices;
		mesh.normals = array;
		mesh.uv = array2;
		mesh.SetIndices(array3, MeshTopology.Quads, 0);
		return mesh;
	}
}
