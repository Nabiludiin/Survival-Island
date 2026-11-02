using System;

namespace UnityEngine.AzureSky;

[Serializable]
public class AzureSkyScatteringComponent
{
	public Vector3 lambda = new Vector3(680f, 550f, 440f);

	public float N = 2.545E+25f;

	public float kr = 8.4f;

	public float km = 1.25f;

	public int rayleighCurveIndex;

	public AnimationCurve[] rayleighCurve = new AnimationCurve[3]
	{
		AnimationCurve.Linear(0f, 1f, 24f, 1f),
		AnimationCurve.Linear(-1f, 1f, 1f, 1f),
		AnimationCurve.Linear(-1f, 1f, 1f, 1f)
	};

	public int mieCurveIndex;

	public AnimationCurve[] mieCurve = new AnimationCurve[3]
	{
		AnimationCurve.Linear(0f, 1f, 24f, 1f),
		AnimationCurve.Linear(-1f, 1f, 1f, 1f),
		AnimationCurve.Linear(-1f, 1f, 1f, 1f)
	};

	public int nightIntensityCurveIndex;

	public AnimationCurve[] nightIntensityCurve = new AnimationCurve[3]
	{
		AnimationCurve.Linear(0f, 0.25f, 24f, 0.25f),
		AnimationCurve.Linear(-1f, 0.25f, 1f, 0.25f),
		AnimationCurve.Linear(-1f, 0.25f, 1f, 0.25f)
	};

	public int rayleighGradientIndex;

	public Gradient[] rayleighGradientColor = new Gradient[3]
	{
		new Gradient(),
		new Gradient(),
		new Gradient()
	};

	public int mieGradientIndex;

	public Gradient[] mieGradientColor = new Gradient[3]
	{
		new Gradient(),
		new Gradient(),
		new Gradient()
	};

	public int sunDiskGradientIndex;

	public Gradient[] sunDiskGradientColor = new Gradient[3]
	{
		new Gradient(),
		new Gradient(),
		new Gradient()
	};

	public int scatteringCurveIndex;

	public AnimationCurve[] scatteringCurve = new AnimationCurve[3]
	{
		AnimationCurve.Linear(0f, 0.5f, 24f, 0.5f),
		AnimationCurve.Linear(-1f, 0.5f, 1f, 0.5f),
		AnimationCurve.Linear(-1f, 0.5f, 1f, 0.5f)
	};

	public int sunDiskIntensityCurveIndex;

	public AnimationCurve[] sunDiskIntensityCurve = new AnimationCurve[3]
	{
		AnimationCurve.Linear(0f, 3f, 24f, 3f),
		AnimationCurve.Linear(-1f, 3f, 1f, 3f),
		AnimationCurve.Linear(-1f, 3f, 1f, 3f)
	};

	public int exposureCurveIndex;

	public AnimationCurve[] exposureCurve = new AnimationCurve[3]
	{
		AnimationCurve.Linear(0f, 1.5f, 24f, 1.5f),
		AnimationCurve.Linear(-1f, 1.5f, 1f, 1.5f),
		AnimationCurve.Linear(-1f, 1.5f, 1f, 1.5f)
	};

	public int fastIndexes;

	public void FastIndexesChange()
	{
		rayleighCurveIndex = fastIndexes;
		mieCurveIndex = fastIndexes;
		nightIntensityCurveIndex = fastIndexes;
		rayleighGradientIndex = fastIndexes;
		mieGradientIndex = fastIndexes;
		sunDiskGradientIndex = fastIndexes;
		scatteringCurveIndex = fastIndexes;
		sunDiskIntensityCurveIndex = fastIndexes;
		exposureCurveIndex = fastIndexes;
	}
}
