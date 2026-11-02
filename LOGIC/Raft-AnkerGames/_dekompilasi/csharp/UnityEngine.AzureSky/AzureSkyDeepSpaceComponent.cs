using System;

namespace UnityEngine.AzureSky;

[Serializable]
public class AzureSkyDeepSpaceComponent
{
	public int moonColorGradientIndex;

	public Gradient[] moonColorGradientColor = new Gradient[3]
	{
		new Gradient(),
		new Gradient(),
		new Gradient()
	};

	public int moonBrightColorGradientIndex;

	public Gradient[] moonBrightColorGradientColor = new Gradient[3]
	{
		new Gradient(),
		new Gradient(),
		new Gradient()
	};

	public int moonBrightRangeCurveIndex;

	public AnimationCurve[] moonBrightRangeCurve = new AnimationCurve[3]
	{
		AnimationCurve.Linear(0f, 50f, 24f, 50f),
		AnimationCurve.Linear(-1f, 50f, 1f, 50f),
		AnimationCurve.Linear(-1f, 50f, 1f, 50f)
	};

	public int moonEmissionCurveIndex;

	public AnimationCurve[] moonEmissionCurve = new AnimationCurve[3]
	{
		AnimationCurve.Linear(0f, 10f, 24f, 10f),
		AnimationCurve.Linear(-1f, 10f, 1f, 10f),
		AnimationCurve.Linear(-1f, 10f, 1f, 10f)
	};

	public float starsScintillation = 5f;

	public int starfieldIntensityCurveIndex;

	public AnimationCurve[] starfieldIntensityCurve = new AnimationCurve[3]
	{
		AnimationCurve.Linear(0f, 0f, 24f, 0f),
		AnimationCurve.Linear(-1f, 0f, 1f, 0f),
		AnimationCurve.Linear(-1f, 0f, 1f, 0f)
	};

	public int milkyWayIntensityCurveIndex;

	public AnimationCurve[] milkyWayIntensityCurve = new AnimationCurve[3]
	{
		AnimationCurve.Linear(0f, 0f, 24f, 0f),
		AnimationCurve.Linear(-1f, 0f, 1f, 0f),
		AnimationCurve.Linear(-1f, 0f, 1f, 0f)
	};

	public int fastIndexes;

	public void FastIndexesChange()
	{
		moonColorGradientIndex = fastIndexes;
		moonBrightColorGradientIndex = fastIndexes;
		moonBrightRangeCurveIndex = fastIndexes;
		moonEmissionCurveIndex = fastIndexes;
		starfieldIntensityCurveIndex = fastIndexes;
		milkyWayIntensityCurveIndex = fastIndexes;
	}
}
