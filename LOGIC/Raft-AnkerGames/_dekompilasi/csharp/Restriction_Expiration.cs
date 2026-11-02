using UnityEngine;

[CreateAssetMenu(fileName = "Restriction_Expiration", menuName = "Raft/Restriction/Expiration")]
public class Restriction_Expiration : Restriction
{
	public float duration = 10f;

	public override bool IsRestricted(WorldEvent we = null)
	{
		return HasExpired(we);
	}

	private bool HasExpired(WorldEvent we)
	{
		if (we == null || we.progress >= duration)
		{
			return true;
		}
		return false;
	}
}
