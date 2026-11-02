using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace RG.Parsecs.Scavenge;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct RescurePodEntranceEventsComparer : IEqualityComparer<RescurePodEntranceEvents>
{
	public bool Equals(RescurePodEntranceEvents x, RescurePodEntranceEvents y)
	{
		return x == y;
	}

	public int GetHashCode(RescurePodEntranceEvents obj)
	{
		return (int)obj;
	}
}
