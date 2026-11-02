using System;
using UnityEngine;

[Serializable]
public class BlendShapeForm
{
	public Interval_Int valueInterval;

	public Interval_Float playInterval;

	public Interval_Float pauseInterval;

	public int[] shapeIndexes;

	public Coroutine routine;
}
