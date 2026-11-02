using System.Collections.Generic;

namespace UltimateWater.Internal;

public class Int32EqualityComparer : IEqualityComparer<int>
{
	private static Int32EqualityComparer _DefaultInstance;

	public static Int32EqualityComparer Default
	{
		get
		{
			if (_DefaultInstance == null)
			{
				return _DefaultInstance = new Int32EqualityComparer();
			}
			return _DefaultInstance;
		}
	}

	public bool Equals(int x, int y)
	{
		return x == y;
	}

	public int GetHashCode(int obj)
	{
		return obj;
	}
}
