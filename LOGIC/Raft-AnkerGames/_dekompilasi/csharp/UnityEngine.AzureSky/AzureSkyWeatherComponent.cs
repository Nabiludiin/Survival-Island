using System;

namespace UnityEngine.AzureSky;

[Serializable]
public class AzureSkyWeatherComponent
{
	public int weatherRainGradientColorIndex;

	public Gradient[] weatherRainGradientColor = new Gradient[3]
	{
		new Gradient(),
		new Gradient(),
		new Gradient()
	};

	public int weatherSnowGradientColorIndex;

	public Gradient[] weatherSnowGradientColor = new Gradient[3]
	{
		new Gradient(),
		new Gradient(),
		new Gradient()
	};

	public int weatherRainIntensityCurveIndex;

	public AnimationCurve[] weatherRainIntensityCurve = new AnimationCurve[3]
	{
		AnimationCurve.Linear(0f, 0f, 24f, 0f),
		AnimationCurve.Linear(-1f, 0f, 1f, 0f),
		AnimationCurve.Linear(-1f, 0f, 1f, 0f)
	};

	public int weatherSnowIntensityCurveIndex;

	public AnimationCurve[] weatherSnowIntensityCurve = new AnimationCurve[3]
	{
		AnimationCurve.Linear(0f, 0f, 24f, 0f),
		AnimationCurve.Linear(-1f, 0f, 1f, 0f),
		AnimationCurve.Linear(-1f, 0f, 1f, 0f)
	};

	public int weatherWindSpeedCurveIndex;

	public AnimationCurve[] weatherWindSpeedCurve = new AnimationCurve[3]
	{
		AnimationCurve.Linear(0f, 0f, 24f, 0f),
		AnimationCurve.Linear(-1f, 0f, 1f, 0f),
		AnimationCurve.Linear(-1f, 0f, 1f, 0f)
	};

	public int weatherWindDirectionCurveIndex;

	public AnimationCurve[] weatherWindDirectionCurve = new AnimationCurve[3]
	{
		AnimationCurve.Linear(0f, 0f, 24f, 0f),
		AnimationCurve.Linear(-1f, 0f, 1f, 0f),
		AnimationCurve.Linear(-1f, 0f, 1f, 0f)
	};

	public int weatherThunderLightIntensityCurveIndex;

	public AnimationCurve[] weatherThunderLightIntensityCurve = new AnimationCurve[3]
	{
		AnimationCurve.Linear(0f, 0.25f, 24f, 0.25f),
		AnimationCurve.Linear(-1f, 0.25f, 1f, 0.25f),
		AnimationCurve.Linear(-1f, 0.25f, 1f, 0.25f)
	};

	public int weatherThunderCloudIntensityCurveIndex;

	public AnimationCurve[] weatherThunderCloudIntensityCurve = new AnimationCurve[3]
	{
		AnimationCurve.Linear(0f, 0.5f, 24f, 0.5f),
		AnimationCurve.Linear(-1f, 0.5f, 1f, 0.5f),
		AnimationCurve.Linear(-1f, 0.5f, 1f, 0.5f)
	};

	public float weatherRainLightVolume;

	public float weatherRainMediumVolume;

	public float weatherRainHeavyVolume;

	public float weatherWindLightVolume;

	public float weatherWindMediumVolume;

	public float weatherWindHeavyVolume;

	public int weatherWetnessCurveIndex;

	public AnimationCurve[] weatherWetnessCurve = new AnimationCurve[3]
	{
		AnimationCurve.Linear(0f, 0f, 24f, 0f),
		AnimationCurve.Linear(-1f, 0f, 1f, 0f),
		AnimationCurve.Linear(-1f, 0f, 1f, 0f)
	};

	public int weatherSnowAmountCurveIndex;

	public AnimationCurve[] weatherSnowAmountCurve = new AnimationCurve[3]
	{
		AnimationCurve.Linear(0f, 0f, 24f, 0f),
		AnimationCurve.Linear(-1f, 0f, 1f, 0f),
		AnimationCurve.Linear(-1f, 0f, 1f, 0f)
	};

	public int weatherCovarageCurveIndex;

	public AnimationCurve[] weatherCovarageCurve = new AnimationCurve[3]
	{
		AnimationCurve.Linear(0f, 0f, 24f, 0f),
		AnimationCurve.Linear(-1f, 0f, 1f, 0f),
		AnimationCurve.Linear(-1f, 0f, 1f, 0f)
	};

	public int weatherOutputColor1Index;

	public Gradient[] weatherOutputColor1GradientColor = new Gradient[3]
	{
		new Gradient(),
		new Gradient(),
		new Gradient()
	};

	public int weatherOutputColor2Index;

	public Gradient[] weatherOutputColor2GradientColor = new Gradient[3]
	{
		new Gradient(),
		new Gradient(),
		new Gradient()
	};

	public int weatherOutputColor3Index;

	public Gradient[] weatherOutputColor3GradientColor = new Gradient[3]
	{
		new Gradient(),
		new Gradient(),
		new Gradient()
	};

	public int fastIndexes;

	public void FastIndexesChange()
	{
		weatherRainGradientColorIndex = fastIndexes;
		weatherSnowGradientColorIndex = fastIndexes;
		weatherRainIntensityCurveIndex = fastIndexes;
		weatherSnowIntensityCurveIndex = fastIndexes;
		weatherWindSpeedCurveIndex = fastIndexes;
		weatherWindDirectionCurveIndex = fastIndexes;
		weatherThunderLightIntensityCurveIndex = fastIndexes;
		weatherThunderCloudIntensityCurveIndex = fastIndexes;
		weatherWetnessCurveIndex = fastIndexes;
		weatherSnowAmountCurveIndex = fastIndexes;
		weatherCovarageCurveIndex = fastIndexes;
		weatherOutputColor1Index = fastIndexes;
		weatherOutputColor2Index = fastIndexes;
		weatherOutputColor3Index = fastIndexes;
	}
}
