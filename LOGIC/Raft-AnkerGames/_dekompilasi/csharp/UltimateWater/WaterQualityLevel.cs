using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace UltimateWater;

[Serializable]
public struct WaterQualityLevel
{
	[FormerlySerializedAs("name")]
	public string Name;

	[Tooltip("All simulations will be performed at most with this resolution")]
	[FormerlySerializedAs("maxSpectrumResolution")]
	public int MaxSpectrumResolution;

	[FormerlySerializedAs("allowHighPrecisionTextures")]
	public bool AllowHighPrecisionTextures;

	[FormerlySerializedAs("allowHighQualityNormalMaps")]
	public bool AllowHighQualityNormalMaps;

	[Range(0f, 1f)]
	[FormerlySerializedAs("tileSizeScale")]
	public float TileSizeScale;

	[FormerlySerializedAs("wavesMode")]
	public WaterWavesMode WavesMode;

	[FormerlySerializedAs("allowSpray")]
	public bool AllowSpray;

	[FormerlySerializedAs("foamQuality")]
	[Range(0f, 1f)]
	public float FoamQuality;

	[FormerlySerializedAs("maxTesselationFactor")]
	[Range(0f, 1f)]
	public float MaxTesselationFactor;

	[FormerlySerializedAs("maxVertexCount")]
	public int MaxVertexCount;

	[FormerlySerializedAs("maxTesselatedVertexCount")]
	public int MaxTesselatedVertexCount;

	[FormerlySerializedAs("allowAlphaBlending")]
	public bool AllowAlphaBlending;

	public void ResetToDefaults()
	{
		Name = "";
		MaxSpectrumResolution = 256;
		AllowHighPrecisionTextures = true;
		TileSizeScale = 1f;
		WavesMode = WaterWavesMode.AllowAll;
		AllowSpray = true;
		FoamQuality = 1f;
		MaxTesselationFactor = 1f;
		MaxVertexCount = 500000;
		MaxTesselatedVertexCount = 120000;
		AllowAlphaBlending = true;
	}
}
