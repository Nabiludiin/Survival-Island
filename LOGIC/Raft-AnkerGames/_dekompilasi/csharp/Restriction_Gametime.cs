using UnityEngine;

[CreateAssetMenu(fileName = "Restriction_GameTime", menuName = "Raft/Restriction/GameTime")]
public class Restriction_Gametime : Restriction
{
	public float minGameTime = 10f;

	public float maxGameTime = float.PositiveInfinity;

	public override bool IsRestricted(WorldEvent we)
	{
		if (GameManager.TimePlayed < minGameTime)
		{
			return true;
		}
		if (GameManager.TimePlayed > maxGameTime)
		{
			return true;
		}
		return base.IsRestricted(we);
	}
}
