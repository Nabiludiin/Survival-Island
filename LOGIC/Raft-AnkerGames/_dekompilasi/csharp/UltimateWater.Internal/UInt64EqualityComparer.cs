using System.Collections.Generic;

namespace UltimateWater.Internal;

public class UInt64EqualityComparer : IEqualityComparer<ulong>
{
	private static UInt64EqualityComparer _DefaultInstance;

	public static UInt64EqualityComparer Default
	{
		get
		{
			if (_DefaultInstance == null)
			{
				return _DefaultInstance = new UInt64EqualityComparer();
			}
			return _DefaultInstance;
		}
	}

	public bool Equals(ulong x, ulong y)
	{
		return x == y;
	}

	public int GetHashCode(ulong obj)
	{
		return (int)(obj ^ (obj >> 32));
	}
}
