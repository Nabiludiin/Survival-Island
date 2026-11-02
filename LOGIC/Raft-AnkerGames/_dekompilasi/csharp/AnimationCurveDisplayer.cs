using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCurveDisplayer : MonoBehaviour
{
	public List<float> values;

	public int maxSavedValuesLocally = 5;

	public Action<float> OnValueAdded;

	private void CheckListCount()
	{
		for (int num = values.Count - 1; num >= 0; num--)
		{
			if (values.Count > maxSavedValuesLocally - 1)
			{
				values.RemoveAt(0);
			}
		}
	}

	public void AddValueToList(float value)
	{
		values.Add(value);
		OnValueAdded?.Invoke(value);
		CheckListCount();
	}

	public void ClearList()
	{
		values.Clear();
	}
}
