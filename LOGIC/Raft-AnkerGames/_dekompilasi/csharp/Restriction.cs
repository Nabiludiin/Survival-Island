using UnityEngine;

public class Restriction : ScriptableObject
{
	public virtual bool IsRestricted(WorldEvent we)
	{
		return false;
	}
}
