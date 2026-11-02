using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoundationCounter : MonoBehaviour, IBlockPlaced
{
	[SerializeField]
	private Transform[] wheels;

	[SerializeField]
	private float turnTime = 1f;

	private static RaftBounds bounds;

	private bool hasBeenPlaced;

	private List<Coroutine> coroutines = new List<Coroutine>();

	public void OnBlockPlacedInterface()
	{
		hasBeenPlaced = true;
		if (bounds == null)
		{
			bounds = ComponentManager<RaftBounds>.Value;
		}
		RaftBounds raftBounds = bounds;
		raftBounds.OnFoundationCountChange = (Action<int>)Delegate.Combine(raftBounds.OnFoundationCountChange, new Action<int>(RefreshCount));
		RefreshCount(bounds.FoundationCount);
	}

	private void OnDestroy()
	{
		if (!GameManager.IsQuittingGame && hasBeenPlaced)
		{
			if (bounds != null)
			{
				RaftBounds raftBounds = bounds;
				raftBounds.OnFoundationCountChange = (Action<int>)Delegate.Remove(raftBounds.OnFoundationCountChange, new Action<int>(RefreshCount));
			}
			ClearRoutines();
		}
	}

	private void RefreshCount(int newCount)
	{
		ClearRoutines();
		string text = newCount.ToString();
		for (int i = 0; i < wheels.Length; i++)
		{
			int num = text.Length - 1 - i;
			if (num >= 0 && num < text.Length)
			{
				SetWheel(i, (int)char.GetNumericValue(text[num]));
			}
			else
			{
				SetWheel(i, 0);
			}
		}
	}

	private void SetWheel(int wheelIndex, int number)
	{
		if (wheelIndex >= 0 && wheelIndex < wheels.Length)
		{
			Transform transform = wheels[wheelIndex];
			if (transform != null)
			{
				float targetAngle = 36f * (float)number;
				coroutines.Add(CoroutineManager.Singleton.AddCoroutine(TurnWheelToAngle(transform, targetAngle)));
			}
		}
	}

	private IEnumerator TurnWheelToAngle(Transform wheel, float targetAngle)
	{
		Quaternion startRotation = wheel.localRotation;
		float endTime = Time.time + turnTime;
		while (!(wheel == null))
		{
			float num = 1f - Mathf.Clamp01((endTime - Time.time) / turnTime);
			wheel.localRotation = Quaternion.Lerp(startRotation, Quaternion.AngleAxis(targetAngle, Vector3.right), num);
			if (!(num >= 1f))
			{
				yield return null;
				continue;
			}
			break;
		}
	}

	private void ClearRoutines()
	{
		if (coroutines == null)
		{
			return;
		}
		for (int i = 0; i < coroutines.Count; i++)
		{
			Coroutine coroutine = coroutines[i];
			if (coroutine != null)
			{
				CoroutineManager.Singleton.StopCoroutine(coroutine);
			}
		}
		coroutines.Clear();
	}
}
