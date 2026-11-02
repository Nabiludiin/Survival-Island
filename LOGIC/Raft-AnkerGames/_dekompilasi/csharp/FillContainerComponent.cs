using System;
using System.Collections;
using UnityEngine;

public class FillContainerComponent : MonoBehaviour
{
	public Action OnFilledCompletely;

	[SerializeField]
	private Axis axisToMove = Axis.Y;

	[SerializeField]
	private Transform transformToMove;

	[SerializeField]
	private Interval_Float localAxisValueInterval;

	[SerializeField]
	private float moveTowardsSpeed = 5f;

	[SerializeField]
	private bool disableObjectAtZero = true;

	[SerializeField]
	[Range(0f, 1f)]
	private float startValue = 1f;

	[SerializeField]
	private float currentNormalizedValue;

	public float CurrentNormalizedValue
	{
		get
		{
			return currentNormalizedValue;
		}
		set
		{
			if (currentNormalizedValue != 1f && value == 1f)
			{
				currentNormalizedValue = value;
				OnFilledCompletely?.Invoke();
			}
			currentNormalizedValue = value;
		}
	}

	private void OnEnable()
	{
		SetYPosition(CurrentNormalizedValue);
	}

	private void OnValidate()
	{
		if (axisToMove == Axis.NZ)
		{
			axisToMove = Axis.Z;
		}
		if (axisToMove == Axis.NY)
		{
			axisToMove = Axis.Y;
		}
		if (axisToMove == Axis.NX)
		{
			axisToMove = Axis.X;
		}
		if (axisToMove == Axis.All || axisToMove == Axis.None)
		{
			axisToMove = Axis.Y;
		}
		if (CurrentNormalizedValue != startValue)
		{
			SetNormalizedValue(startValue, instant: true);
		}
	}

	public void EnableFillGameObject()
	{
		transformToMove.gameObject.SetActiveSafe(value: true);
	}

	public void DisableFillGameObject()
	{
		transformToMove.gameObject.SetActiveSafe(value: false);
	}

	public void SetNormalizedValue(float normalizedValue, bool instant)
	{
		if (!gameObject.activeInHierarchy | instant)
		{
			CurrentNormalizedValue = normalizedValue;
			SetYPosition(CurrentNormalizedValue);
		}
		else
		{
			StopAllCoroutines();
			StartCoroutine(SetYPositionCoroutine(normalizedValue));
		}
	}

	private IEnumerator SetYPositionCoroutine(float normalizedValue)
	{
		while (CurrentNormalizedValue != normalizedValue)
		{
			CurrentNormalizedValue = Mathf.MoveTowards(CurrentNormalizedValue, normalizedValue, Time.deltaTime * moveTowardsSpeed);
			SetYPosition(CurrentNormalizedValue);
			yield return null;
		}
		CurrentNormalizedValue = normalizedValue;
		SetYPosition(CurrentNormalizedValue);
	}

	private void SetYPosition(float normalizedValue)
	{
		if (transformToMove == null)
		{
			return;
		}
		if (disableObjectAtZero)
		{
			if (normalizedValue > 0f)
			{
				EnableFillGameObject();
			}
			else
			{
				DisableFillGameObject();
			}
		}
		Vector3 localPosition = transformToMove.localPosition;
		switch (axisToMove)
		{
		case Axis.X:
			localPosition.x = localAxisValueInterval.GetValueFromNormalized(normalizedValue);
			break;
		case Axis.Y:
			localPosition.y = localAxisValueInterval.GetValueFromNormalized(normalizedValue);
			break;
		case Axis.Z:
			localPosition.z = localAxisValueInterval.GetValueFromNormalized(normalizedValue);
			break;
		}
		transformToMove.localPosition = localPosition;
	}
}
