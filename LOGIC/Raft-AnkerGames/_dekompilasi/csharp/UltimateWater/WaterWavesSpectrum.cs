using System;
using UnityEngine;

namespace UltimateWater;

public abstract class WaterWavesSpectrum
{
	protected float _TileSize;

	protected float _Gravity;

	protected float _WindSpeed;

	protected float _Amplitude;

	public float TileSize => _TileSize * WaterQualitySettings.Instance.TileSizeScale;

	public float Gravity => _Gravity;

	public abstract void ComputeSpectrum(Vector3[] spectrum, float tileSizeMultiplier, int maxResolution, System.Random random);

	protected WaterWavesSpectrum(float tileSize, float gravity, float windSpeed, float amplitude)
	{
		_TileSize = tileSize;
		_Gravity = gravity;
		_WindSpeed = windSpeed;
		_Amplitude = amplitude;
	}
}
