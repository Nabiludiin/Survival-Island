using UnityEngine;

namespace RG.Parsecs.Scavenge;

public static class IActorBrainExtensions
{
	public static Vector3 GetPosition(this IActorBrain brain)
	{
		if (brain.PositionMarker == null)
		{
			return brain.Transform.position;
		}
		return brain.PositionMarker.position;
	}
}
