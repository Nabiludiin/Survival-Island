using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MergeData
{
	public BoxCollider mergeCollider;

	public int averageLenght;

	public List<LineData> mergedLines;

	private Vector2 dimensions = Vector2.zero;

	public MergeData(int p_averageLength)
	{
		mergedLines = new List<LineData>();
		averageLenght = p_averageLength;
	}

	public Vector2 GetDimensions()
	{
		return dimensions;
	}
}
