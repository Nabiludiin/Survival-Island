using RG.Core.Base;
using UnityEngine;

namespace RG.Parsecs.Survival;

[CreateAssetMenu(menuName = "60 Parsecs!/Lengths Ranges")]
public class LengthsRanges : RGScriptableObject
{
	[SerializeField]
	private LengthsRange[] _ranges;

	public int GetAmountOfLengthsIconsForRange(int min, int max)
	{
		int num = 0;
		for (int i = 0; i < _ranges.Length; i++)
		{
			if (_ranges[i].Matches(min, max))
			{
				num = i + 1;
			}
		}
		if (num == 0)
		{
			if (min > _ranges[_ranges.Length - 1].Max)
			{
				num = _ranges.Length;
			}
			else if (max < _ranges[0].Min)
			{
				num = 1;
			}
			else
			{
				Debug.LogErrorFormat("Haven't found correct length for min: {0} max: {1} in \"{2}\" Lengths Icon Ranges file. Falling back to 1.", min, max, name);
				num = 1;
			}
		}
		return num;
	}
}
