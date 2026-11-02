using System;

namespace RG.Parsecs.Scavenge;

[Serializable]
public class SpaceDoorGroup
{
	[NonSerialized]
	public bool available = true;

	public string GroupName = "";

	public SpaceDoors[] doors;
}
