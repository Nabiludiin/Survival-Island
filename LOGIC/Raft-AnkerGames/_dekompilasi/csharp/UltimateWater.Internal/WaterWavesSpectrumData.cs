using UnityEngine;

namespace UltimateWater.Internal;

public sealed class WaterWavesSpectrumData : WaterWavesSpectrumDataBase
{
	private readonly WaterWavesSpectrum _Spectrum;

	public WaterWavesSpectrumData(Water water, WindWaves windWaves, WaterWavesSpectrum spectrum)
		: base(water, windWaves, spectrum.TileSize, spectrum.Gravity)
	{
		_Spectrum = spectrum;
	}

	protected override void GenerateContents(Vector3[][] spectrumValues)
	{
		int finalResolution = _WindWaves.FinalResolution;
		Vector4 tileSizeScales = _WindWaves.TileSizeScales;
		int num = ((_Water.Seed != 0) ? _Water.Seed : Random.Range(0, 1000000));
		int maxSpectrumResolution = WaterQualitySettings.Instance.GetQualityLevelsDirect()[^1].MaxSpectrumResolution;
		if (finalResolution > maxSpectrumResolution)
		{
			Debug.LogWarningFormat("In water quality settings spectrum resolution of {0} is max, but at runtime a spectrum with resolution of {1} is generated. That may generate some unexpected behaviour. Make sure that the last water quality level has the highest resolution and don't alter it at runtime.", maxSpectrumResolution, finalResolution);
		}
		for (byte b = 0; b < 4; b++)
		{
			Random.State state = Random.state;
			Random.InitState(num + b);
			_Spectrum.ComputeSpectrum(spectrumValues[b], tileSizeScales[b], maxSpectrumResolution, null);
			Random.state = state;
		}
	}
}
