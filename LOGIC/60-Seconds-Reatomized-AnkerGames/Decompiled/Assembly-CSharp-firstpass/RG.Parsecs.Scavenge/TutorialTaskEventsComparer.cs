using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace RG.Parsecs.Scavenge;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct TutorialTaskEventsComparer : IEqualityComparer<TutorialTaskEvents>
{
	public bool Equals(TutorialTaskEvents x, TutorialTaskEvents y)
	{
		return x == y;
	}

	public int GetHashCode(TutorialTaskEvents obj)
	{
		return (int)obj;
	}
}
