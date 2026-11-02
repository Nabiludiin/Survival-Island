using System.Collections.Generic;
using UnityEngine;

public class ExtrudeMesh : MonoBehaviour
{
	public interface OnGenerateMesh
	{
		void OnGenerateMesh();

		void OnClearMesh();
	}

	public struct OrientedPoint
	{
		public Vector3 pos;

		public Quaternion rot;

		public Vector3 LocalToWorld(Vector3 localSpacePos)
		{
			return pos + rot * localSpacePos;
		}

		public Vector3 LocalToWorldVec(Vector3 localSpacePos)
		{
			return rot * localSpacePos;
		}
	}

	[SerializeField]
	private SO_ExtrudeMeshShape shape;

	[SerializeField]
	private MeshFilter meshFilter;

	[SerializeField]
	private bool autoGenerate;

	[SerializeField]
	private float ringCountPerMeter = 1f;

	public float shapeScale = 1f;

	[SerializeField]
	private bool drawGizmos;

	[SerializeField]
	protected float pathLength = 2f;

	[SerializeField]
	private int ringCount = 2;

	private Mesh mesh;

	public float PathLength => pathLength;

	private bool CanGenerate()
	{
		if (shape != null)
		{
			return meshFilter != null;
		}
		return false;
	}

	public virtual OrientedPoint GetOrientedPoint(float t)
	{
		return new OrientedPoint
		{
			pos = Vector3.Lerp(transform.position, transform.position + transform.forward * 4f, t),
			rot = Quaternion.LookRotation(transform.forward, transform.up)
		};
	}

	protected virtual void SetPathLength()
	{
		pathLength = (GetOrientedPoint(1f).pos - GetOrientedPoint(0f).pos).magnitude;
	}

	private void Generate()
	{
		SetPathLength();
		GenerateMesh();
		CallOnGenerateMeshInterfaces();
	}

	private void ClearMesh()
	{
		if (mesh != null)
		{
			mesh.Clear();
		}
		CallOnClearMeshInterfaces();
	}

	private void GenerateMesh()
	{
		if (meshFilter == null || shape == null)
		{
			return;
		}
		ringCount = Mathf.Clamp(Mathf.RoundToInt(pathLength * ringCountPerMeter), 2, int.MaxValue);
		if (mesh == null)
		{
			mesh = new Mesh();
			mesh.name = "Test";
		}
		mesh.Clear();
		List<Vector3> list = new List<Vector3>();
		List<Vector3> list2 = new List<Vector3>();
		for (int i = 0; i < ringCount; i++)
		{
			float num = (float)i / ((float)ringCount - 1f);
			OrientedPoint orientedPoint = GetOrientedPoint(num);
			for (int j = 0; j < shape.VertexCount; j++)
			{
				SO_ExtrudeMeshShape.Vertex vertex = shape.vertices[j];
				list.Add(meshFilter.transform.InverseTransformPoint(orientedPoint.LocalToWorld(vertex.ScaledPoint(shapeScale))));
				list2.Add(new Vector2(vertex.ScaledU(shapeScale), pathLength * num));
			}
		}
		List<int> list3 = new List<int>();
		for (int k = 0; k < ringCount - 1; k++)
		{
			int num2 = k * shape.VertexCount;
			int num3 = (k + 1) * shape.VertexCount;
			for (int l = 0; l < shape.LineCount; l += 2)
			{
				int num4 = shape.lineIndices[l];
				int num5 = shape.lineIndices[l + 1];
				int item = num2 + num4;
				int item2 = num2 + num5;
				int item3 = num3 + num4;
				int item4 = num3 + num5;
				list3.Add(item);
				list3.Add(item3);
				list3.Add(item4);
				list3.Add(item);
				list3.Add(item4);
				list3.Add(item2);
			}
		}
		mesh.SetVertices(list);
		mesh.SetTriangles(list3, 0);
		mesh.RecalculateNormals();
		mesh.SetUVs(0, list2);
		if (shape.startPointSmooth)
		{
			Vector3[] normals = mesh.normals;
			int num6 = 0;
			int num7 = num6 + shape.VertexCount - 1;
			for (int m = 0; m < ringCount; m++)
			{
				int num8 = num6 + m * shape.VertexCount;
				int num9 = num7 + m * shape.VertexCount;
				Vector3 vector = normals[num8];
				Vector3 vector2 = normals[num9];
				normals[num8] = (normals[num9] = (vector + vector2).normalized);
			}
			mesh.normals = normals;
		}
		meshFilter.sharedMesh = mesh;
	}

	private void CallOnGenerateMeshInterfaces()
	{
		OnGenerateMesh[] componentsInChildren = GetComponentsInChildren<OnGenerateMesh>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i]?.OnGenerateMesh();
		}
	}

	private void CallOnClearMeshInterfaces()
	{
		OnGenerateMesh[] componentsInChildren = GetComponentsInChildren<OnGenerateMesh>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i]?.OnClearMesh();
		}
	}
}
