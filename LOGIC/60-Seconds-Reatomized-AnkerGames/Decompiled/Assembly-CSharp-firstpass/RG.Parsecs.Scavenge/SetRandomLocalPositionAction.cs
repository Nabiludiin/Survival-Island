using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class SetRandomLocalPositionAction : EventAction
{
	public bool useX;

	public Range rangeX;

	public bool useY;

	public Range rangeY;

	public bool useZ;

	public Range RangeZ;

	protected override void Act(EventActionArgs args = null)
	{
		Vector3 localPosition = transform.localPosition;
		if (useX)
		{
			localPosition.x = rangeX.RandomValue;
		}
		if (useY)
		{
			localPosition.y = rangeY.RandomValue;
		}
		if (useZ)
		{
			localPosition.z = RangeZ.RandomValue;
		}
		transform.localPosition = localPosition;
	}
}
