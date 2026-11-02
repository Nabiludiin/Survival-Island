using System;

namespace UnityEngine.AzureSky;

[Serializable]
public class AzureSkyCloudsComponent
{
	public float dynamicCloudLayer1Altitude = 10f;

	public float dynamicCloudLayer1Direction = 1f;

	public float dynamicCloudLayer1Speed = 0.1f;

	public int dynamicCloudLayer1GradientColor1Index;

	public Gradient[] dynamicCloudLayer1GradientColor1 = new Gradient[3]
	{
		new Gradient(),
		new Gradient(),
		new Gradient()
	};

	public int dynamicCloudLayer1GradientColor2Index;

	public Gradient[] dynamicCloudLayer1GradientColor2 = new Gradient[3]
	{
		new Gradient(),
		new Gradient(),
		new Gradient()
	};

	public int dynamicCloudLayer1DensityCurveIndex;

	public AnimationCurve[] dynamicCloudLayer1DensityCurve = new AnimationCurve[3]
	{
		AnimationCurve.Linear(0f, 0.7f, 24f, 0.7f),
		AnimationCurve.Linear(-1f, 0.7f, 1f, 0.7f),
		AnimationCurve.Linear(-1f, 0.7f, 1f, 0.7f)
	};

	public int fastIndexes;

	public Texture2D staticCloudTexture;

	public int staticCloudColorIndex;

	public Gradient[] staticCloudColor = new Gradient[3]
	{
		new Gradient(),
		new Gradient(),
		new Gradient()
	};

	public int staticCloudScatteringCurveIndex;

	public AnimationCurve[] staticCloudScatteringCurve = new AnimationCurve[3]
	{
		AnimationCurve.Linear(0f, 1f, 24f, 1f),
		AnimationCurve.Linear(-1f, 1f, 1f, 1f),
		AnimationCurve.Linear(-1f, 1f, 1f, 1f)
	};

	public int staticCloudExtinctionCurveIndex;

	public AnimationCurve[] staticCloudExtinctionCurve = new AnimationCurve[3]
	{
		AnimationCurve.Linear(0f, 0.25f, 24f, 0.25f),
		AnimationCurve.Linear(-1f, 0.25f, 1f, 0.25f),
		AnimationCurve.Linear(-1f, 0.25f, 1f, 0.25f)
	};

	public int staticCloudPowerCurveIndex;

	public AnimationCurve[] staticCloudPowerCurve = new AnimationCurve[3]
	{
		AnimationCurve.Linear(0f, 2.2f, 24f, 2.2f),
		AnimationCurve.Linear(-1f, 2.2f, 1f, 2.2f),
		AnimationCurve.Linear(-1f, 2.2f, 1f, 2.2f)
	};

	public int staticCloudIntensityCurveIndex;

	public AnimationCurve[] staticCloudIntensityCurve = new AnimationCurve[3]
	{
		AnimationCurve.Linear(0f, 1f, 24f, 1f),
		AnimationCurve.Linear(-1f, 1f, 1f, 1f),
		AnimationCurve.Linear(-1f, 1f, 1f, 1f)
	};

	public float staticCloudRotationSpeed;

	public void FastIndexesChange()
	{
		dynamicCloudLayer1GradientColor1Index = fastIndexes;
		dynamicCloudLayer1GradientColor2Index = fastIndexes;
		dynamicCloudLayer1DensityCurveIndex = fastIndexes;
		staticCloudColorIndex = fastIndexes;
		staticCloudScatteringCurveIndex = fastIndexes;
		staticCloudExtinctionCurveIndex = fastIndexes;
		staticCloudPowerCurveIndex = fastIndexes;
		staticCloudIntensityCurveIndex = fastIndexes;
	}
}
