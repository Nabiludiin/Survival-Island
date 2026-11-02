using System;
using UnityEngine;

[Serializable]
public class PipeColliderConnection
{
	public int[] bitmaskValues;

	public GameObject colliderPrefab;

	public bool ContainsBitmaskValue(int bitmaskValue)
	{
		for (int i = 0; i < bitmaskValues.Length; i++)
		{
			if (bitmaskValues[i] == bitmaskValue)
			{
				return true;
			}
		}
		return false;
	}
}
