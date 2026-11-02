using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace RG.Parsecs.Scavenge;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct ActorEventsComparer : IEqualityComparer<ActorEvents>
{
	public bool Equals(ActorEvents x, ActorEvents y)
	{
		return x == y;
	}

	public int GetHashCode(ActorEvents obj)
	{
		return (int)obj;
	}
}
