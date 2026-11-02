using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace RG.Parsecs.Scavenge;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct ScavengeManagerEventsComparer : IEqualityComparer<ScavengeManagerEvents>
{
	public bool Equals(ScavengeManagerEvents x, ScavengeManagerEvents y)
	{
		return x == y;
	}

	public int GetHashCode(ScavengeManagerEvents obj)
	{
		return (int)obj;
	}
}
