using System;

namespace UnityEngine.AzureSky;

[Serializable]
public class AzureSkyLightingComponent
{
	public int directionalLightIntensityCurveIndex;

	public AnimationCurve[] directionalLightIntensityCurve = new AnimationCurve[3]
	{
		AnimationCurve.Linear(0f, 1f, 24f, 1f),
		AnimationCurve.Linear(-1f, 1f, 1f, 1f),
		AnimationCurve.Linear(-1f, 1f, 1f, 1f)
	};

	public int directionalLightGradientColorIndex;

	public Gradient[] directionalLightGradientColor = new Gradient[3]
	{
		new Gradient(),
		new Gradient(),
		new Gradient()
	};

	public int ambientIntensityCurveIndex;

	public AnimationCurve[] ambientIntensityCurve = new AnimationCurve[3]
	{
		AnimationCurve.Linear(0f, 1f, 24f, 1f),
		AnimationCurve.Linear(-1f, 1f, 1f, 1f),
		AnimationCurve.Linear(-1f, 1f, 1f, 1f)
	};

	public int ambientSkyGradientColorIndex;

	public Gradient[] ambientSkyGradientColor = new Gradient[3]
	{
		new Gradient(),
		new Gradient(),
		new Gradient()
	};

	public int equatorGradientColorIndex;

	public Gradient[] equatorGradientColor = new Gradient[3]
	{
		new Gradient(),
		new Gradient(),
		new Gradient()
	};

	public int groundGradientColorIndex;

	public Gradient[] groundGradientColor = new Gradient[3]
	{
		new Gradient(),
		new Gradient(),
		new Gradient()
	};

	public int reflectionIntensityCurveIndex;

	public AnimationCurve[] reflectionIntensityCurve = new AnimationCurve[3]
	{
		AnimationCurve.Linear(0f, 1f, 24f, 1f),
		AnimationCurve.Linear(-1f, 1f, 1f, 1f),
		AnimationCurve.Linear(-1f, 1f, 1f, 1f)
	};

	public int fastIndexes;

	public void FastIndexesChange()
	{
		directionalLightIntensityCurveIndex = fastIndexes;
		directionalLightGradientColorIndex = fastIndexes;
		ambientIntensityCurveIndex = fastIndexes;
		ambientSkyGradientColorIndex = fastIndexes;
		equatorGradientColorIndex = fastIndexes;
		groundGradientColorIndex = fastIndexes;
		reflectionIntensityCurveIndex = fastIndexes;
	}
}
