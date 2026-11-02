using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace RG.Parsecs.Scavenge;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct HandUIEventsComparer : IEqualityComparer<HandUIEvents>
{
	public bool Equals(HandUIEvents x, HandUIEvents y)
	{
		return x == y;
	}

	public int GetHashCode(HandUIEvents obj)
	{
		return (int)obj;
	}
}
