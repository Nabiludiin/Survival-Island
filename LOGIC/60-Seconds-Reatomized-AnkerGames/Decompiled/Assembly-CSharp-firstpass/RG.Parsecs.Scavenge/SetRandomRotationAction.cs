using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class SetRandomRotationAction : EventAction
{
	public bool useX;

	public Range rangeX;

	public bool useY;

	public Range rangeY;

	public bool useZ;

	public Range RangeZ;

	protected override void Act(EventActionArgs args = null)
	{
		Vector3 eulerAngles = transform.eulerAngles;
		if (useX)
		{
			eulerAngles.x = rangeX.RandomValue;
		}
		if (useY)
		{
			eulerAngles.y = rangeY.RandomValue;
		}
		if (useZ)
		{
			eulerAngles.z = RangeZ.RandomValue;
		}
		transform.eulerAngles = eulerAngles;
	}
}
