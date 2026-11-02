using System;
using UnityEngine;

[Serializable]
public class YieldManipulator
{
	public float baseYield;

	[Tooltip("How hungry/thirsty are you?, 0 = Could eat a horse, 1 = I'm full")]
	[Range(0f, 1f)]
	public float cravePercent;

	[Space(20f)]
	public float finalYield;

	[SerializeField]
	private AnimationCurve curve;

	public float GetValue(float craveNormal)
	{
		craveNormal = Mathf.Clamp(craveNormal, 0f, 1f);
		return Mathf.Clamp(curve.Evaluate(craveNormal), 0f, 1f);
	}
}
