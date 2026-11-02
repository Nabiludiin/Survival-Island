using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace RG.Parsecs.Scavenge;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct ScavengeItemEventsComparer : IEqualityComparer<ScavengeItemEvents>
{
	public bool Equals(ScavengeItemEvents x, ScavengeItemEvents y)
	{
		return x == y;
	}

	public int GetHashCode(ScavengeItemEvents obj)
	{
		return (int)obj;
	}
}
