using System;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

[Serializable]
public class FloatOptionalRange
{
	public FloatType FloatType;

	public float Value;

	public bool UseRange;

	public Range RangeValue = new Range();

	public bool UseCurve;

	public AnimationCurve Curve;

	public float CurveStepEachUse;

	public float CurveStartTime;

	private float _t;

	private bool _first = true;

	public bool IsDelay()
	{
		switch (FloatType)
		{
		case FloatType.Value:
			if (Value > 0f)
			{
				return true;
			}
			break;
		case FloatType.Range:
			return true;
		case FloatType.Curve:
			return true;
		}
		return false;
	}

	public static implicit operator float(FloatOptionalRange sceneField)
	{
		if (sceneField == null)
		{
			return 0f;
		}
		if (sceneField.UseRange)
		{
			return sceneField.RangeValue.RandomValue;
		}
		if (sceneField.UseCurve)
		{
			if (sceneField._first)
			{
				sceneField._t = sceneField.CurveStartTime;
				sceneField._first = false;
			}
			float result = sceneField.Curve.Evaluate(sceneField._t);
			sceneField._t += sceneField.CurveStepEachUse;
			Keyframe[] keys = sceneField.Curve.keys;
			sceneField._t = Mathf.Clamp(sceneField._t, 0f, keys[keys.Length - 1].time);
			return result;
		}
		return sceneField.Value;
	}

	public static implicit operator FloatOptionalRange(float val)
	{
		return new FloatOptionalRange
		{
			Value = val,
			UseRange = false
		};
	}
}
