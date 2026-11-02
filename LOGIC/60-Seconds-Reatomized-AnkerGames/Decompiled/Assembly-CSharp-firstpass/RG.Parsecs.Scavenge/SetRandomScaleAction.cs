using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class SetRandomScaleAction : EventAction
{
	public bool useX;

	public Range rangeX;

	public bool useY;

	public Range rangeY;

	public bool useZ;

	public Range RangeZ;

	public bool useUniform;

	public Range RangeUniform;

	protected override void Act(EventActionArgs args = null)
	{
		Vector3 localScale = transform.localScale;
		if (useUniform)
		{
			localScale.z = (localScale.y = (localScale.x = RangeUniform.RandomValue));
		}
		else
		{
			if (useX)
			{
				localScale.x = rangeX.RandomValue;
			}
			if (useY)
			{
				localScale.y = rangeY.RandomValue;
			}
			if (useZ)
			{
				localScale.z = RangeZ.RandomValue;
			}
		}
		transform.localScale = localScale;
	}
}
