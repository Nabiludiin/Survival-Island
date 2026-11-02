using System.Collections.Generic;
using UnityEngine;

public class Unstuck_PointManager : MonoBehaviour
{
	public static List<Transform> viableUnstuckPoints = new List<Transform>();

	public static Vector3 GetClosestUnstuckPoint(Vector3 fromPoint)
	{
		float num = float.PositiveInfinity;
		Vector3 result = fromPoint;
		foreach (Transform viableUnstuckPoint in viableUnstuckPoints)
		{
			float num2 = Vector3.Distance(fromPoint, viableUnstuckPoint.position);
			if (num2 < num)
			{
				result = viableUnstuckPoint.position;
				num = num2;
			}
		}
		return result;
	}

	public static bool RemovePointFromList(Transform point)
	{
		if (viableUnstuckPoints.Contains(point))
		{
			viableUnstuckPoints.Remove(point);
			return true;
		}
		return false;
	}

	public static bool AddPointToList(Transform point)
	{
		if (!viableUnstuckPoints.Contains(point))
		{
			viableUnstuckPoints.Add(point);
			return true;
		}
		return false;
	}
}
