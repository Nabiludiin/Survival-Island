using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_ExtrudeMeshShape", menuName = "ExtrudeMeshShape")]
public class SO_ExtrudeMeshShape : ScriptableObject
{
	[Serializable]
	public class Vertex
	{
		public Vector2 point;

		[HideInInspector]
		public float scale = 1f;

		[HideInInspector]
		public int ogIndex = -1;

		[HideInInspector]
		public float u;

		public Vertex(Vector2 point, int ogIndex)
		{
			this.point = point;
			this.ogIndex = ogIndex;
		}

		public Vertex()
		{
		}

		public float ScaledU(float scaled)
		{
			return u * scale;
		}

		public Vector2 ScaledPoint(float scale)
		{
			return point * scale;
		}
	}

	[Serializable]
	public class TriangleInfo
	{
		public int vertA;

		public int vertB;

		public TriangleInfo(int vertA, int vertB)
		{
			this.vertA = vertA;
			this.vertB = vertB;
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !GetType().Equals(obj.GetType()))
			{
				return false;
			}
			TriangleInfo triangleInfo = (TriangleInfo)obj;
			if (vertA == triangleInfo.vertA)
			{
				return vertB == triangleInfo.vertB;
			}
			return false;
		}

		public bool IsNeighbour(TriangleInfo other)
		{
			if (vertA == other.vertA && vertB == other.vertB)
			{
				return false;
			}
			if (vertA == other.vertA || vertA == other.vertB || vertB == other.vertA || vertB == other.vertB)
			{
				return true;
			}
			return false;
		}
	}

	public Mesh meshReference;

	public bool startPointSmooth;

	public bool flipVertexDiretions;

	public Vertex[] vertices;

	public int[] lineIndices;

	public int VertexCount => vertices.Length;

	public int LineCount => lineIndices.Length;

	private void GetVerticesFromMesh()
	{
		Vector3[] verts = meshReference.vertices;
		int[] triangles = meshReference.triangles;
		TriangleInfo triangleInfo = null;
		List<Vertex> list = new List<Vertex>();
		for (int i = 0; i < triangles.Length; i += 3)
		{
			Vector3 vector = verts[triangles[i]];
			Vector3 vector2 = verts[triangles[i + 1]];
			Vector3 vector3 = verts[triangles[i + 2]];
			List<Vertex> list2 = new List<Vertex>();
			if (Mathf.Round(vector.z) == 0f)
			{
				list2.Add(new Vertex(vector, triangles[i]));
			}
			if (Mathf.Round(vector2.z) == 0f)
			{
				list2.Add(new Vertex(vector2, triangles[i + 1]));
			}
			if (Mathf.Round(vector3.z) == 0f)
			{
				list2.Add(new Vertex(vector3, triangles[i + 2]));
			}
			if (list2.Count == 2)
			{
				triangleInfo = new TriangleInfo(list2[0].ogIndex, list2[1].ogIndex);
				break;
			}
		}
		list.Add(new Vertex(verts[triangleInfo.vertA], triangleInfo.vertA));
		list.Add(new Vertex(verts[triangleInfo.vertB], triangleInfo.vertB));
		int excludeIndex = triangleInfo.vertA;
		int num = triangleInfo.vertB;
		int num2 = verts.Length / 2;
		while (list.Count < num2)
		{
			TriangleInfo triangleInfo2 = FindNeighbourTriangle(num, excludeIndex);
			if (triangleInfo2 == null)
			{
				int num3 = GetVertIndexFromPosition(num);
				if (num3 < 0)
				{
					Debug.LogError("Can't find correct index");
					break;
				}
				list.Add(new Vertex(verts[num3], num3));
				triangleInfo2 = FindNeighbourTriangle(num3, -1);
			}
			if (triangleInfo2 != null)
			{
				int ogIndex = list.Last().ogIndex;
				int num4 = ((triangleInfo2.vertA == ogIndex) ? triangleInfo2.vertB : triangleInfo2.vertA);
				list.Add(new Vertex(verts[num4], num4));
				excludeIndex = list[list.Count - 2].ogIndex;
				num = num4;
			}
		}
		if (flipVertexDiretions)
		{
			list.Reverse();
		}
		if (list.First().ogIndex != list.Last().ogIndex && GetVertCountFromPosition(list[0].ogIndex) == 1)
		{
			list.Add(new Vertex(list[0].point, -1));
		}
		vertices = list.ToArray();
		List<int> list3 = new List<int>();
		for (int j = 0; j < VertexCount - 1; j++)
		{
			Vertex obj = vertices[j];
			Vertex vertex = vertices[j + 1];
			if (obj.point != vertex.point)
			{
				list3.Add(j);
				list3.Add(j + 1);
			}
		}
		lineIndices = list3.ToArray();
		CalculateUV();
		TriangleInfo FindNeighbourTriangle(int sharedIndex, int num5)
		{
			for (int k = 0; k < triangles.Length; k += 3)
			{
				Vector3 vector4 = verts[triangles[k]];
				Vector3 vector5 = verts[triangles[k + 1]];
				Vector3 vector6 = verts[triangles[k + 2]];
				List<Vertex> list4 = new List<Vertex>();
				if (Mathf.Round(vector4.z) == 0f)
				{
					list4.Add(new Vertex(vector4, triangles[k]));
				}
				if (Mathf.Round(vector5.z) == 0f)
				{
					list4.Add(new Vertex(vector5, triangles[k + 1]));
				}
				if (Mathf.Round(vector6.z) == 0f)
				{
					list4.Add(new Vertex(vector6, triangles[k + 2]));
				}
				if (list4.Count == 2)
				{
					TriangleInfo triangleInfo3 = new TriangleInfo(list4[0].ogIndex, list4[1].ogIndex);
					if (triangleInfo3.vertA == sharedIndex || triangleInfo3.vertB == sharedIndex)
					{
						if (num5 < 0)
						{
							return triangleInfo3;
						}
						if (triangleInfo3.vertA != num5 && triangleInfo3.vertB != num5)
						{
							return triangleInfo3;
						}
					}
				}
			}
			return null;
		}
		int GetVertCountFromPosition(int index)
		{
			Vector3 vector4 = verts[index];
			int num5 = 0;
			for (int k = 0; k < verts.Length; k++)
			{
				if (verts[k] == vector4)
				{
					num5++;
				}
			}
			return num5;
		}
		int GetVertIndexFromPosition(int num5)
		{
			Vector3 vector4 = verts[num5];
			for (int k = 0; k < verts.Length; k++)
			{
				if (verts[k] == vector4 && k != num5)
				{
					return k;
				}
			}
			return -1;
		}
	}

	private void CalculateUV()
	{
		float num = 0f;
		for (int i = 0; i < LineCount; i += 2)
		{
			Vertex vertex = vertices[lineIndices[i]];
			Vertex vertex2 = vertices[lineIndices[i + 1]];
			num += Vector3.Distance(vertex.point, vertex2.point);
			vertex.u = (vertex2.u = -1f);
		}
		float num2 = 0f;
		for (int j = 0; j < LineCount; j += 2)
		{
			Vertex vertex3 = vertices[lineIndices[j]];
			Vertex vertex4 = vertices[lineIndices[j + 1]];
			float num3 = Mathf.Clamp01(Vector3.Distance(vertex3.point, vertex4.point) / num);
			if (vertex3.u == -1f)
			{
				vertex3.u = num2;
			}
			if (vertex4.u == -1f)
			{
				vertex4.u = num2 + num3;
				num2 = vertex4.u;
			}
		}
	}
}
