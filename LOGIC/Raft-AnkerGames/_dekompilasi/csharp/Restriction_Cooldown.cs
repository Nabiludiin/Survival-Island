using UnityEngine;

[CreateAssetMenu(fileName = "Restriction_Cooldown", menuName = "Raft/Restriction/Cooldown")]
public class Restriction_Cooldown : Restriction
{
	public float cooldownSeconds;

	public override bool IsRestricted(WorldEvent we)
	{
		return IsInCooldown(we);
	}

	private bool IsInCooldown(WorldEvent we)
	{
		if (we == null)
		{
			return true;
		}
		return we.inactivityProgress < cooldownSeconds;
	}
}
