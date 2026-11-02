using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace RG.Parsecs.Scavenge;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct GameObjectEventsComparer : IEqualityComparer<GameObjectEvents>
{
	public bool Equals(GameObjectEvents x, GameObjectEvents y)
	{
		return x == y;
	}

	public int GetHashCode(GameObjectEvents obj)
	{
		return (int)obj;
	}
}
