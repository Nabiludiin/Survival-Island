using UnityEngine;

namespace RG.Core.Extensions;

public static class TransformExtensions
{
	public static void SetX(this Transform transform, float x)
	{
		Vector3 position = new Vector3(x, transform.position.y, transform.position.z);
		transform.position = position;
	}

	public static void SetY(this Transform transform, float y)
	{
		Vector3 position = new Vector3(transform.position.x, y, transform.position.z);
		transform.position = position;
	}

	public static void SetZ(this Transform transform, float z)
	{
		Vector3 position = new Vector3(transform.position.x, transform.position.y, z);
		transform.position = position;
	}
}
