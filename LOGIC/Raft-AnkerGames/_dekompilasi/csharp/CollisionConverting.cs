using System.Collections.Generic;
using UnityEngine;

public static class CollisionConverting
{
	public static readonly int[] CUBE_INDECIS = new int[36]
	{
		0, 2, 1, 0, 3, 2, 2, 3, 4, 2,
		4, 5, 1, 2, 5, 1, 5, 6, 0, 7,
		4, 0, 4, 3, 5, 4, 7, 5, 7, 6,
		0, 6, 7, 0, 1, 6
	};

	public const int VERTICES_IN_QUAD = 4;

	public const int VERTICES_IN_CUBE = 8;

	public static void ConstructMeshFromBox(List<Vector3> vertices, List<int> indices, BoxCollider boxCollider, Transform parent)
	{
		ConstructMeshFromBox(vertices, indices, -1, boxCollider, parent);
	}

	public static void ConstructMeshFromBox(List<Vector3> vertices, List<int> indices, int verticesOffset, BoxCollider boxCollider, Transform parent)
	{
		Vector3 pos = parent.InverseTransformPoint(boxCollider.transform.position);
		Quaternion q = Quaternion.Inverse(parent.rotation) * boxCollider.transform.rotation;
		Vector3 s = default;
		s.x = boxCollider.transform.lossyScale.x / parent.lossyScale.x;
		s.y = boxCollider.transform.lossyScale.y / parent.lossyScale.y;
		s.z = boxCollider.transform.lossyScale.z / parent.lossyScale.z;
		Vector3 vector = boxCollider.size / 2f;
		Vector3[] array = new Vector3[8]
		{
			boxCollider.center + new Vector3(0f - vector.x, 0f - vector.y, 0f - vector.z),
			boxCollider.center + new Vector3(vector.x, 0f - vector.y, 0f - vector.z),
			boxCollider.center + new Vector3(vector.x, vector.y, 0f - vector.z),
			boxCollider.center + new Vector3(0f - vector.x, vector.y, 0f - vector.z),
			boxCollider.center + new Vector3(0f - vector.x, vector.y, vector.z),
			boxCollider.center + new Vector3(vector.x, vector.y, vector.z),
			boxCollider.center + new Vector3(vector.x, 0f - vector.y, vector.z),
			boxCollider.center + new Vector3(0f - vector.x, 0f - vector.y, vector.z)
		};
		Matrix4x4 matrix4x = Matrix4x4.TRS(pos, q, s);
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = matrix4x.MultiplyPoint(array[i]);
		}
		int num;
		if (verticesOffset == -1)
		{
			num = vertices.Count;
			vertices.AddRange(array);
		}
		else
		{
			num = verticesOffset;
			vertices.InsertRange(verticesOffset, array);
		}
		for (int j = 0; j < CUBE_INDECIS.Length; j++)
		{
			indices.Add(num + CUBE_INDECIS[j]);
		}
	}
}
