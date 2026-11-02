using System.Collections.Generic;

namespace RG.Parsecs.Common;

public struct ScavengeEntry
{
	public int TotalSoups;

	public int TotalItems;

	public bool IsSuccessful;

	public int LeftForDead;

	public List<string> ItemsIds;
}
