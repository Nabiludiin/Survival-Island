using System;
using UnityEngine;

namespace UltimateWater;

public static class ColliderExtensions
{
	public static float ComputeVolume(this Collider that)
	{
		BoxCollider boxCollider = that as BoxCollider;
		if (boxCollider != null)
		{
			return boxCollider.ComputeVolume();
		}
		SphereCollider sphereCollider = that as SphereCollider;
		if (sphereCollider != null)
		{
			return sphereCollider.ComputeVolume();
		}
		MeshCollider meshCollider = that as MeshCollider;
		if (meshCollider != null)
		{
			return meshCollider.ComputeVolume();
		}
		CapsuleCollider capsuleCollider = that as CapsuleCollider;
		if (capsuleCollider != null)
		{
			return capsuleCollider.ComputeVolume();
		}
		throw new NotImplementedException("UltimateWater: Unknown collider type.");
	}

	public static float ComputeVolume(this BoxCollider that)
	{
		Vector3 size = that.size;
		Vector3 lossyScale = that.transform.lossyScale;
		return size.x * lossyScale.x * size.y * lossyScale.y * size.z * lossyScale.z;
	}

	public static float ComputeVolume(this SphereCollider that)
	{
		float radius = that.radius;
		Vector3 lossyScale = that.transform.lossyScale;
		return 4.1887903f * radius * radius * radius * lossyScale.x * lossyScale.y * lossyScale.z;
	}

	public static float ComputeVolume(this MeshCollider that)
	{
		float num = 0f;
		Mesh sharedMesh = that.sharedMesh;
		Vector3[] vertices = sharedMesh.vertices;
		int[] triangles = sharedMesh.triangles;
		int num2 = triangles.Length;
		Vector3 vector = that.transform.InverseTransformPoint(that.bounds.center);
		int num3 = 0;
		while (num3 < num2)
		{
			Vector3 p = vertices[triangles[num3++]] - vector;
			Vector3 p2 = vertices[triangles[num3++]] - vector;
			Vector3 p3 = vertices[triangles[num3++]] - vector;
			num += SignedVolumeOfTriangle(p, p2, p3);
		}
		Vector3 lossyScale = that.transform.lossyScale;
		return Mathf.Abs(num) * lossyScale.x * lossyScale.y * lossyScale.z;
	}

	public static float ComputeVolume(this CapsuleCollider that)
	{
		float radius = that.radius;
		float num = 4.1887903f * radius * radius * radius;
		float num2 = MathF.PI * radius * radius * that.height;
		Vector3 lossyScale = that.transform.lossyScale;
		return (num2 + num) * lossyScale.x * lossyScale.y * lossyScale.z;
	}

	public static float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
	{
		float num = p3.x * p2.y * p1.z;
		float num2 = p2.x * p3.y * p1.z;
		float num3 = p3.x * p1.y * p2.z;
		float num4 = p1.x * p3.y * p2.z;
		float num5 = p2.x * p1.y * p3.z;
		float num6 = p1.x * p2.y * p3.z;
		return 1f / 6f * (0f - num + num2 + num3 - num4 - num5 + num6);
	}

	public static float ComputeArea(this Collider that)
	{
		MeshCollider meshCollider = that as MeshCollider;
		if (meshCollider != null)
		{
			return meshCollider.ComputeArea();
		}
		BoxCollider boxCollider = that as BoxCollider;
		if (boxCollider != null)
		{
			return boxCollider.ComputeArea();
		}
		SphereCollider sphereCollider = that as SphereCollider;
		if (sphereCollider != null)
		{
			return sphereCollider.ComputeArea();
		}
		CapsuleCollider capsuleCollider = that as CapsuleCollider;
		if (capsuleCollider != null)
		{
			return capsuleCollider.ComputeArea();
		}
		throw new NotImplementedException("UltimateWater: Unknown collider type.");
	}

	public static float ComputeArea(this MeshCollider that)
	{
		float num = 0f;
		Mesh sharedMesh = that.sharedMesh;
		Vector3[] vertices = sharedMesh.vertices;
		int[] triangles = sharedMesh.triangles;
		int num2 = triangles.Length;
		Vector3 lossyScale = that.transform.lossyScale;
		int num3 = 0;
		while (num3 < num2)
		{
			Vector3 vector = vertices[triangles[num3++]];
			Vector3 lhs = vertices[triangles[num3++]] - vector;
			Vector3 rhs = vertices[triangles[num3++]] - vector;
			lhs.Scale(lossyScale);
			rhs.Scale(lossyScale);
			num += Vector3.Cross(lhs, rhs).magnitude;
		}
		return num * 0.5f;
	}

	public static float ComputeArea(this BoxCollider that)
	{
		Vector3 size = that.size;
		size.Scale(that.transform.lossyScale);
		return 2f * (size.x * size.y + size.y * size.z + size.x * size.z);
	}

	public static float ComputeArea(this SphereCollider that)
	{
		float magnitude = that.transform.lossyScale.magnitude;
		float num = that.radius * magnitude;
		return MathF.PI * 4f * num * num;
	}

	public static float ComputeArea(this CapsuleCollider that)
	{
		Vector3 lossyScale = that.transform.lossyScale;
		float num = that.radius * lossyScale.magnitude;
		float height = that.height;
		return MathF.PI * 2f * num * (2f * num + that.direction switch
		{
			0 => height * lossyScale.x, 
			1 => height * lossyScale.y, 
			2 => height * lossyScale.z, 
			_ => throw new InvalidOperationException(), 
		});
	}

	public static Vector3 RandomPoint(this Collider that)
	{
		MeshCollider meshCollider = that as MeshCollider;
		if (meshCollider != null)
		{
			return meshCollider.RandomPoint();
		}
		BoxCollider boxCollider = that as BoxCollider;
		if (boxCollider != null)
		{
			return boxCollider.RandomPoint();
		}
		CapsuleCollider capsuleCollider = that as CapsuleCollider;
		if (capsuleCollider != null)
		{
			return capsuleCollider.RandomPoint();
		}
		SphereCollider sphereCollider = that as SphereCollider;
		if (sphereCollider != null)
		{
			return sphereCollider.RandomPoint();
		}
		throw new NotImplementedException("UltimateWater: Unknown collider type.");
	}

	public static Vector3 RandomPoint(this MeshCollider that)
	{
		Bounds bounds = that.sharedMesh.bounds;
		Vector3 min = bounds.min;
		Vector3 vector = bounds.max - min;
		Vector3 vector2 = default;
		for (int i = 0; i < 40; i++)
		{
			vector2.x = min.x + UnityEngine.Random.value * vector.x;
			vector2.y = min.y + UnityEngine.Random.value * vector.y;
			vector2.z = min.z + UnityEngine.Random.value * vector.z;
			if (that.IsPointInside(that.transform.TransformPoint(vector2)))
			{
				break;
			}
		}
		return vector2;
	}

	public static Vector3 RandomPoint(this BoxCollider that)
	{
		Vector3 center = that.center;
		Vector3 vector = that.size * 0.5f;
		float x = center.x + UnityEngine.Random.Range(0f - vector.x, vector.x);
		float y = center.y + UnityEngine.Random.Range(0f - vector.y, vector.y);
		float z = center.z + UnityEngine.Random.Range(0f - vector.z, vector.z);
		return new Vector3(x, y, z);
	}

	public static Vector3 RandomPoint(this CapsuleCollider that)
	{
		float radius = that.radius;
		float height = that.height;
		float num = MathF.PI * radius * radius * height;
		float num2 = 4.1887903f * radius * radius * radius;
		Vector3 result;
		if (UnityEngine.Random.Range(0f, num + num2) < num)
		{
			result = RandomPointInCircle(radius);
			result.z = result.y;
			result.y = UnityEngine.Random.Range((0f - height) * 0.5f, height * 0.5f);
		}
		else
		{
			result = RandomPointInSphere(radius);
			if (result.y < 0f)
			{
				result.y -= height * 0.5f;
			}
			else
			{
				result.y += height * 0.5f;
			}
		}
		switch (that.direction)
		{
		case 0:
		{
			float y2 = result.y;
			result.y = result.x;
			result.x = y2;
			break;
		}
		case 2:
		{
			float y = result.y;
			result.y = result.z;
			result.z = y;
			break;
		}
		}
		return result;
	}

	public static Vector3 RandomPoint(this SphereCollider that)
	{
		return RandomPointInSphere(that.radius);
	}

	public static Vector3 RandomPointInSphere(float radius)
	{
		float f = Mathf.Asin(UnityEngine.Random.Range(-1f, 1f));
		float f2 = MathF.PI * 2f * UnityEngine.Random.Range(0f, 1f);
		float num = 3f * Mathf.Pow(UnityEngine.Random.Range(0f, 1f), 1f / 3f);
		float num2 = Mathf.Sin(f);
		return new Vector3(num * num2 * Mathf.Cos(f2), num * num2 * Mathf.Sin(f2), num * Mathf.Cos(f));
	}

	public static Vector2 RandomPointInCircle(float radius)
	{
		float f = MathF.PI * 2f * UnityEngine.Random.Range(0f, 1f);
		float num = UnityEngine.Random.Range(0f, 1f) + UnityEngine.Random.Range(0f, 1f);
		float num2 = ((num > 1f) ? (2f - num) : num) * radius;
		return new Vector2(num2 * Mathf.Cos(f), num2 * Mathf.Sin(f));
	}

	public static void GetLocalMinMax(Collider collider, out Vector3 min, out Vector3 max)
	{
		MeshCollider meshCollider = collider as MeshCollider;
		if (meshCollider != null)
		{
			Bounds bounds = meshCollider.sharedMesh.bounds;
			min = bounds.min;
			max = bounds.max;
			return;
		}
		BoxCollider boxCollider = collider as BoxCollider;
		if (boxCollider != null)
		{
			BoxCollider boxCollider2 = boxCollider;
			min = boxCollider2.center - boxCollider2.size * 0.5f;
			max = boxCollider2.center + boxCollider2.size * 0.5f;
			return;
		}
		SphereCollider sphereCollider = collider as SphereCollider;
		if (sphereCollider != null)
		{
			Vector3 center = sphereCollider.center;
			float num = sphereCollider.radius * 0.5f;
			min = new Vector3(center.x - num, center.y - num, center.z - num);
			max = new Vector3(center.x + num, center.y + num, center.z + num);
			return;
		}
		CapsuleCollider capsuleCollider = collider as CapsuleCollider;
		if (capsuleCollider != null)
		{
			Vector3 center2 = capsuleCollider.center;
			float num2 = capsuleCollider.radius * 0.5f;
			float num3 = capsuleCollider.height * 0.5f + num2;
			switch (capsuleCollider.direction)
			{
			case 0:
				min = new Vector3(center2.x - num3, center2.y - num2, center2.z - num2);
				max = new Vector3(center2.x + num3, center2.y + num2, center2.z + num2);
				break;
			case 1:
				min = new Vector3(center2.x - num2, center2.y - num3, center2.z - num2);
				max = new Vector3(center2.x + num2, center2.y + num3, center2.z + num2);
				break;
			case 2:
				min = new Vector3(center2.x - num2, center2.y - num2, center2.z - num3);
				max = new Vector3(center2.x + num2, center2.y + num2, center2.z + num3);
				break;
			default:
				throw new InvalidOperationException();
			}
			return;
		}
		throw new InvalidOperationException();
	}

	public static bool IsPointInside(this Collider convex, Vector3 point)
	{
		Bounds bounds = convex.bounds;
		if (!bounds.Contains(point))
		{
			return false;
		}
		Vector3 direction = bounds.center - point;
		float magnitude = direction.magnitude;
		if (magnitude < 1E-05f)
		{
			return true;
		}
		RaycastHit hitInfo;
		return !convex.Raycast(new Ray(point, direction), out hitInfo, magnitude);
	}
}
