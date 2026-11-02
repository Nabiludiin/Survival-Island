using System;

namespace RG.Parsecs.Scavenge;

[Serializable]
public struct BaseSeed
{
	public int RoomSeed;

	public int CorridorSeed;

	public int LeftModuleSeed;

	public int RightModuleSeed;

	public int ItemSeed;

	public void Setup()
	{
		RoomSeed = -1;
		CorridorSeed = -1;
		LeftModuleSeed = -1;
		RightModuleSeed = -1;
		ItemSeed = -1;
	}
}
