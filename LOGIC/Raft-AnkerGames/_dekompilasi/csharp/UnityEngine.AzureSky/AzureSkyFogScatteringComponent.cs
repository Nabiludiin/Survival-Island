using System;

namespace UnityEngine.AzureSky;

[Serializable]
public class AzureSkyFogScatteringComponent
{
	public float fogScale = 0.9f;

	public int fogBlendCurveIndex;

	public AnimationCurve[] fogBlendCurve = new AnimationCurve[3]
	{
		AnimationCurve.Linear(0f, 0.25f, 24f, 0.25f),
		AnimationCurve.Linear(-1f, 0.25f, 1f, 0.25f),
		AnimationCurve.Linear(-1f, 0.25f, 1f, 0.25f)
	};

	public int fogDensityCurveIndex;

	public AnimationCurve[] fogDensityCurve = new AnimationCurve[3]
	{
		AnimationCurve.Linear(0f, 1f, 24f, 1f),
		AnimationCurve.Linear(-1f, 1f, 1f, 1f),
		AnimationCurve.Linear(-1f, 1f, 1f, 1f)
	};

	public int fogDistanceCurveIndex;

	public AnimationCurve[] fogDistanceCurve = new AnimationCurve[3]
	{
		AnimationCurve.Linear(0f, 5000f, 24f, 5000f),
		AnimationCurve.Linear(-1f, 5000f, 1f, 5000f),
		AnimationCurve.Linear(-1f, 5000f, 1f, 5000f)
	};

	public int heightFogBlendCurveIndex;

	public AnimationCurve[] heightFogBlendCurve = new AnimationCurve[3]
	{
		AnimationCurve.Linear(0f, 1f, 24f, 1f),
		AnimationCurve.Linear(-1f, 1f, 1f, 1f),
		AnimationCurve.Linear(-1f, 1f, 1f, 1f)
	};

	public int heightFogDensityCurveIndex;

	public AnimationCurve[] heightFogDensityCurve = new AnimationCurve[3]
	{
		AnimationCurve.Linear(0f, 0.5f, 24f, 0.5f),
		AnimationCurve.Linear(-1f, 0.5f, 1f, 0.5f),
		AnimationCurve.Linear(-1f, 0.5f, 1f, 0.5f)
	};

	public int heightFogDistanceCurveIndex;

	public AnimationCurve[] heightFogDistanceCurve = new AnimationCurve[3]
	{
		AnimationCurve.Linear(0f, 500f, 24f, 500f),
		AnimationCurve.Linear(-1f, 500f, 1f, 500f),
		AnimationCurve.Linear(-1f, 500f, 1f, 500f)
	};

	public int heightFogStartCurveIndex;

	public AnimationCurve[] heightFogStartCurve = new AnimationCurve[3]
	{
		AnimationCurve.Linear(0f, 0f, 24f, 0f),
		AnimationCurve.Linear(-1f, 0f, 1f, 0f),
		AnimationCurve.Linear(-1f, 0f, 1f, 0f)
	};

	public int heightFogEndCurveIndex;

	public AnimationCurve[] heightFogEndCurve = new AnimationCurve[3]
	{
		AnimationCurve.Linear(0f, 100f, 24f, 100f),
		AnimationCurve.Linear(-1f, 100f, 1f, 100f),
		AnimationCurve.Linear(-1f, 100f, 1f, 100f)
	};

	public int fastIndexes;

	public void FastIndexesChange()
	{
		fogBlendCurveIndex = fastIndexes;
		fogDensityCurveIndex = fastIndexes;
		fogDistanceCurveIndex = fastIndexes;
		heightFogBlendCurveIndex = fastIndexes;
		heightFogDensityCurveIndex = fastIndexes;
		heightFogDistanceCurveIndex = fastIndexes;
		heightFogStartCurveIndex = fastIndexes;
		heightFogEndCurveIndex = fastIndexes;
	}
}
