using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace RG.Parsecs.Scavenge;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct ClockEventsComparer : IEqualityComparer<ClockEvents>
{
	public bool Equals(ClockEvents x, ClockEvents y)
	{
		return x == y;
	}

	public int GetHashCode(ClockEvents obj)
	{
		return (int)obj;
	}
}
