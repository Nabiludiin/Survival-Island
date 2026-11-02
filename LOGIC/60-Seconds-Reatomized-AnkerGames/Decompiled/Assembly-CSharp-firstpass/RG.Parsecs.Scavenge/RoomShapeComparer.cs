using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace RG.Parsecs.Scavenge;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct RoomShapeComparer : IEqualityComparer<RoomShape>
{
	public bool Equals(RoomShape x, RoomShape y)
	{
		return x == y;
	}

	public int GetHashCode(RoomShape obj)
	{
		return (int)obj;
	}
}
