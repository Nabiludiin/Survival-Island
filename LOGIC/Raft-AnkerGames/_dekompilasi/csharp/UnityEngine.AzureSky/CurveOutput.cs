using System;

namespace UnityEngine.AzureSky;

[Serializable]
public class CurveOutput
{
	public int curveIndex;

	public AnimationCurve[] curveOutput = new AnimationCurve[3]
	{
		AnimationCurve.Linear(0f, 0.5f, 24f, 0.5f),
		AnimationCurve.Linear(-1f, 0.5f, 1f, 0.5f),
		AnimationCurve.Linear(-1f, 0.5f, 1f, 0.5f)
	};
}
