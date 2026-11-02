using System;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
public class LengthsRange
{
	[SerializeField]
	[Range(0f, 999f)]
	private int _min;

	[SerializeField]
	[Range(0f, 999f)]
	private int _max;

	public int Min => _min;

	public int Max => _max;

	public bool Matches(int min, int max)
	{
		if (_min > _max)
		{
			Debug.LogErrorFormat("Lengths Range: Min: {0} is bigger that Max: {1}", _min, max);
		}
		if (min > max)
		{
			Debug.LogErrorFormat("Tested Range: Min: {0} is bigger that Max: {1}", _min, max);
		}
		if (min <= _max)
		{
			return max >= _min;
		}
		return false;
	}
}
