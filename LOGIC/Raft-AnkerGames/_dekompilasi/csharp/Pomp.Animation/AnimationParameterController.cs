using System;
using UnityEngine;

namespace Pomp.Animation;

public class AnimationParameterController : MonoBehaviour
{
	[Serializable]
	protected class MovementAnimationParam : AnimationParamData
	{
		public override string DefaultParamName => PARAMETER_MOVEMENTMULTIPLIER;

		public MovementAnimationParam()
		{
			id = ID.MOVEMENT;
		}
	}

	public class AnimationParamData
	{
		public enum ID
		{
			NONE,
			MOVEMENT
		}

		public float minValue;

		public float maxValue;

		public float minAnimationSpeed;

		public float maxAnimationSpeed;

		[SerializeField]
		protected ID id;

		public virtual string DefaultParamName => "";

		public float NormalizeValueToParameter(float value)
		{
			return Helper.MapValue(minValue, maxValue, value, minAnimationSpeed, maxAnimationSpeed);
		}
	}

	public static string PARAMETER_MOVEMENTMULTIPLIER = "AnimationSpeedMultiplier";

	public AnimatorController animatorController;

	[SerializeField]
	private MovementAnimationParam movementAnimationParam;

	public void OnValidate()
	{
		if (animatorController == null)
		{
			animatorController = GetComponentInChildren<AnimatorController>();
		}
	}

	public void ApplyParameter(AnimationParamData.ID id, string parameter, float value)
	{
		if (id == AnimationParamData.ID.MOVEMENT)
		{
			movementAnimationParam.NormalizeValueToParameter(value);
			animatorController.SetFloat(parameter, value);
		}
	}

	public void ApplyParameter(AnimationParamData.ID id, float value)
	{
		if (id == AnimationParamData.ID.MOVEMENT)
		{
			float value2 = movementAnimationParam.NormalizeValueToParameter(value);
			animatorController.SetFloat(movementAnimationParam.DefaultParamName, value2);
		}
	}
}
