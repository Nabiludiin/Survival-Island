using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class AnimatorPropertySetting_Legit
{
	public AnimatorPropertyType propertyType;

	public string parameterName;

	public int intValue;

	public float floatValue;

	public bool lerp;

	public float lerpSpeed;

	public bool boolValue;

	public Coroutine lerpRoutine;

	public bool IsFloatType()
	{
		return propertyType == AnimatorPropertyType.Float;
	}

	public bool IsBoolType()
	{
		return propertyType == AnimatorPropertyType.Bool;
	}

	public bool IsIntType()
	{
		return propertyType == AnimatorPropertyType.Int;
	}

	public IEnumerator LerpFloat(Animator anim, string parameterName, float newValue, float lerpSpeed)
	{
		float currentValue = anim.GetFloat(parameterName);
		float diff = 1f;
		while (diff > 0.05f)
		{
			currentValue = Mathf.Lerp(currentValue, newValue, Time.deltaTime * lerpSpeed);
			anim.SetFloat(parameterName, currentValue);
			diff = Mathf.Abs(currentValue - newValue);
			yield return null;
		}
		anim.SetFloat(parameterName, newValue);
	}

	public void ApplyDataToAnimator(Animator anim, Coroutine lerpRoutineRef)
	{
		switch (propertyType)
		{
		case AnimatorPropertyType.Float:
			if (lerp)
			{
				if (lerpRoutineRef != null)
				{
					CoroutineManager.Singleton.StopCoroutine(lerpRoutineRef);
					lerpRoutineRef = null;
				}
				lerpRoutineRef = CoroutineManager.Singleton.AddCoroutine(LerpFloat(anim, parameterName, floatValue, lerpSpeed));
			}
			else
			{
				anim.SetFloat(parameterName, floatValue);
			}
			break;
		case AnimatorPropertyType.Bool:
			anim.SetBool(parameterName, boolValue);
			break;
		case AnimatorPropertyType.Trigger:
			anim.SetTrigger(parameterName);
			break;
		case AnimatorPropertyType.Int:
			anim.SetInteger(parameterName, intValue);
			break;
		}
	}
}
