using System;
using UnityEngine;

namespace UltimateWater;

public sealed class WavesRendererGerstner
{
	[Serializable]
	public class Data
	{
		[Range(0f, 20f)]
		public int NumGerstners = 20;
	}

	private readonly Water _Water;

	private readonly WindWaves _WindWaves;

	private readonly Data _Data;

	private Gerstner4[] _GerstnerFours;

	private int _LastUpdateFrame;

	private bool _Enabled;

	public bool Enabled => _Enabled;

	public WavesRendererGerstner(Water water, WindWaves windWaves, Data data)
	{
		_Water = water;
		_WindWaves = windWaves;
		_Data = data;
	}

	public void OnWaterRender(Camera camera)
	{
		if (Application.isPlaying && _Enabled)
		{
			UpdateWaves();
		}
	}

	public void OnWaterPostRender(Camera camera)
	{
	}

	public void BuildShaderVariant(ShaderVariant variant, Water water, WindWaves windWaves, WaterQualityLevel qualityLevel)
	{
		variant.SetUnityKeyword("_WAVES_GERSTNER", _Enabled);
	}

	private void UpdateWaves()
	{
		int frameCount = Time.frameCount;
		if (_LastUpdateFrame != frameCount)
		{
			_LastUpdateFrame = frameCount;
			MaterialPropertyBlock propertyBlock = _Water.Renderer.PropertyBlock;
			float time = Time.time;
			Vector4 value = default;
			for (int i = 0; i < _GerstnerFours.Length; i++)
			{
				Gerstner4 gerstner = _GerstnerFours[i];
				value.x = gerstner.Wave0.Offset + gerstner.Wave0.Speed * time;
				value.y = gerstner.Wave1.Offset + gerstner.Wave1.Speed * time;
				value.z = gerstner.Wave2.Offset + gerstner.Wave2.Speed * time;
				value.w = gerstner.Wave3.Offset + gerstner.Wave3.Speed * time;
				propertyBlock.SetVector("_GrOff" + i, value);
			}
		}
	}

	internal void Enable()
	{
		if (!_Enabled)
		{
			_Enabled = true;
			if (Application.isPlaying)
			{
				_Water.ProfilesManager.Changed.AddListener(OnProfilesChanged);
				FindMostMeaningfulWaves();
			}
		}
	}

	internal void Disable()
	{
		if (_Enabled)
		{
			_Enabled = false;
		}
	}

	internal void OnValidate(WindWaves windWaves)
	{
		if (_Enabled)
		{
			FindMostMeaningfulWaves();
		}
	}

	private void FindMostMeaningfulWaves()
	{
		_WindWaves.SpectrumResolver.SetDirectWaveEvaluationMode(_Data.NumGerstners);
		WaterWave[] directWaves = _WindWaves.SpectrumResolver.DirectWaves;
		int num = 0;
		int num2 = directWaves.Length >> 2;
		_GerstnerFours = new Gerstner4[num2];
		Vector2[] array = new Vector2[4];
		for (int i = 0; i < 4; i++)
		{
			float num3 = _WindWaves.TileSizes[i];
			array[i].x = num3 + 0.5f / (float)_WindWaves.FinalResolution * num3;
			array[i].y = 0f - num3 + 0.5f / (float)_WindWaves.FinalResolution * num3;
		}
		for (int j = 0; j < num2; j++)
		{
			GerstnerWave wave = ((num < directWaves.Length) ? new GerstnerWave(directWaves[num++], array) : new GerstnerWave());
			GerstnerWave wave2 = ((num < directWaves.Length) ? new GerstnerWave(directWaves[num++], array) : new GerstnerWave());
			GerstnerWave wave3 = ((num < directWaves.Length) ? new GerstnerWave(directWaves[num++], array) : new GerstnerWave());
			GerstnerWave wave4 = ((num < directWaves.Length) ? new GerstnerWave(directWaves[num++], array) : new GerstnerWave());
			_GerstnerFours[j] = new Gerstner4(wave, wave2, wave3, wave4);
		}
		UpdateMaterial();
	}

	private void UpdateMaterial()
	{
		MaterialPropertyBlock propertyBlock = _Water.Renderer.PropertyBlock;
		Vector4 value = default;
		Vector4 value2 = default;
		Vector4 value3 = default;
		Vector4 value4 = default;
		for (int i = 0; i < _GerstnerFours.Length; i++)
		{
			Gerstner4 gerstner = _GerstnerFours[i];
			value.x = gerstner.Wave0.Amplitude;
			value2.x = gerstner.Wave0.Frequency;
			value3.x = gerstner.Wave0.Direction.x;
			value3.y = gerstner.Wave0.Direction.y;
			value.y = gerstner.Wave1.Amplitude;
			value2.y = gerstner.Wave1.Frequency;
			value3.z = gerstner.Wave1.Direction.x;
			value3.w = gerstner.Wave1.Direction.y;
			value.z = gerstner.Wave2.Amplitude;
			value2.z = gerstner.Wave2.Frequency;
			value4.x = gerstner.Wave2.Direction.x;
			value4.y = gerstner.Wave2.Direction.y;
			value.w = gerstner.Wave3.Amplitude;
			value2.w = gerstner.Wave3.Frequency;
			value4.z = gerstner.Wave3.Direction.x;
			value4.w = gerstner.Wave3.Direction.y;
			propertyBlock.SetVector("_GrAB" + i, value3);
			propertyBlock.SetVector("_GrCD" + i, value4);
			propertyBlock.SetVector("_GrAmp" + i, value);
			propertyBlock.SetVector("_GrFrq" + i, value2);
		}
		for (int j = _GerstnerFours.Length; j < 5; j++)
		{
			propertyBlock.SetVector("_GrAmp" + j, Vector4.zero);
		}
	}

	private void OnProfilesChanged(Water water)
	{
		FindMostMeaningfulWaves();
	}
}
