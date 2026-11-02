using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class SetRandomPositionAction : EventAction
{
	public bool useX;

	public Range rangeX;

	public bool useY;

	public Range rangeY;

	public bool useZ;

	public Range RangeZ;

	protected override void Act(EventActionArgs args = null)
	{
		Vector3 position = transform.position;
		if (useX)
		{
			position.x = rangeX.RandomValue;
		}
		if (useY)
		{
			position.y = rangeY.RandomValue;
		}
		if (useZ)
		{
			position.z = RangeZ.RandomValue;
		}
		transform.position = position;
	}
}
