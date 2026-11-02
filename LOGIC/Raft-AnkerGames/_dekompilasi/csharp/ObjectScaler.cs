using System.Collections;
using UnityEngine;

public class ObjectScaler : MonoBehaviour
{
	[SerializeField]
	private Axis axisToScale = Axis.Y;

	[SerializeField]
	private Transform transformToScale;

	[SerializeField]
	private Interval_Float localAxisValueInterval;

	[SerializeField]
	private float lerpSpeed = 5f;

	[SerializeField]
	[Range(0f, 1f)]
	private float startValue = 1f;

	[SerializeField]
	private float currentNormalizedValue;

	private void OnEnable()
	{
		SetYScale(currentNormalizedValue);
	}

	private void OnValidate()
	{
		if (axisToScale == Axis.NZ)
		{
			axisToScale = Axis.Z;
		}
		if (axisToScale == Axis.NY)
		{
			axisToScale = Axis.Y;
		}
		if (axisToScale == Axis.NX)
		{
			axisToScale = Axis.X;
		}
		if (axisToScale == Axis.All || axisToScale == Axis.None)
		{
			axisToScale = Axis.Y;
		}
		if (currentNormalizedValue != startValue)
		{
			SetNormalizedValue(startValue, instant: true);
		}
	}

	public float GetCurrentNormalizedValue()
	{
		return currentNormalizedValue;
	}

	public void EnableFillGameObject()
	{
		transformToScale.gameObject.SetActiveSafe(value: true);
	}

	public void DisableFillGameObject()
	{
		transformToScale.gameObject.SetActiveSafe(value: false);
	}

	public void SetNormalizedValue(float normalizedValue, bool instant)
	{
		if (!(gameObject == null))
		{
			if (!gameObject.activeInHierarchy | instant)
			{
				currentNormalizedValue = normalizedValue;
				SetYScale(currentNormalizedValue);
			}
			else
			{
				StopAllCoroutines();
				StartCoroutine(SetYPositionCoroutine(normalizedValue));
			}
		}
	}

	private IEnumerator SetYPositionCoroutine(float normalizedValue)
	{
		while (currentNormalizedValue != normalizedValue)
		{
			currentNormalizedValue = Mathf.Lerp(currentNormalizedValue, normalizedValue, Time.deltaTime * lerpSpeed);
			if (Mathf.Abs(currentNormalizedValue - normalizedValue) < 0.001f)
			{
				currentNormalizedValue = normalizedValue;
			}
			SetYScale(currentNormalizedValue);
			yield return null;
		}
		currentNormalizedValue = normalizedValue;
		SetYScale(currentNormalizedValue);
	}

	private void SetYScale(float normalizedValue)
	{
		if (!(transformToScale == null))
		{
			if (normalizedValue > 0f)
			{
				EnableFillGameObject();
			}
			else
			{
				DisableFillGameObject();
			}
			Vector3 localScale = transformToScale.localScale;
			switch (axisToScale)
			{
			case Axis.X:
				localScale.x = localAxisValueInterval.GetValueFromNormalized(normalizedValue);
				break;
			case Axis.Y:
				localScale.y = localAxisValueInterval.GetValueFromNormalized(normalizedValue);
				break;
			case Axis.Z:
				localScale.z = localAxisValueInterval.GetValueFromNormalized(normalizedValue);
				break;
			}
			transformToScale.localScale = localScale;
		}
	}
}
