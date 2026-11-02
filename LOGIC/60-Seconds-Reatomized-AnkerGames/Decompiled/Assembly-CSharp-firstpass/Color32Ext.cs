using UnityEngine;

public static class Color32Ext
{
	public static bool IsEqualTo(this Color32 aCol, Color32 aRef)
	{
		if (aCol.r == aRef.r && aCol.g == aRef.g && aCol.b == aRef.b)
		{
			return aCol.a == aRef.a;
		}
		return false;
	}
}
