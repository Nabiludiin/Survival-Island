using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace UltimateWater;

[Serializable]
public class WaterCustomSurfaceMeshes
{
	private struct Edge
	{
		public int VertexIndex0;

		public int VertexIndex1;

		public int FaceIndex0;

		public int FaceIndex1;
	}

	[SerializeField]
	[FormerlySerializedAs("customMeshes")]
	private Mesh[] _CustomMeshes;

	private Water _Water;

	private Mesh[] _UsedMeshesCache;

	private Mesh[] _VolumeMeshes;

	public Mesh[] VolumeMeshes
	{
		get
		{
			if (_VolumeMeshes == null)
			{
				Mesh[] usedMeshes = _UsedMeshes;
				List<Mesh> list = new List<Mesh>();
				Mesh[] array = usedMeshes;
				foreach (Mesh mesh in array)
				{
					list.Add(mesh);
					list.Add(CreateBoundaryMesh(mesh));
				}
				_VolumeMeshes = list.ToArray();
			}
			return _VolumeMeshes;
		}
	}

	public bool Triangular
	{
		get
		{
			if (_CustomMeshes != null && _UsedMeshes.Length != 0)
			{
				return _UsedMeshes[0].GetTopology(0) == MeshTopology.Triangles;
			}
			return true;
		}
	}

	public Mesh[] Meshes
	{
		get
		{
			return _CustomMeshes;
		}
		set
		{
			_CustomMeshes = value;
			_UsedMeshesCache = null;
			_VolumeMeshes = null;
		}
	}

	private Mesh[] _UsedMeshes
	{
		get
		{
			if (_UsedMeshesCache == null)
			{
				List<Mesh> list = new List<Mesh>();
				Mesh[] customMeshes = _CustomMeshes;
				foreach (Mesh mesh in customMeshes)
				{
					if (mesh != null)
					{
						list.Add(mesh);
					}
				}
				_UsedMeshesCache = list.ToArray();
			}
			return _UsedMeshesCache;
		}
	}

	public Mesh[] GetTransformedMeshes(Camera camera, out Matrix4x4 matrix, bool volume)
	{
		matrix = _Water.transform.localToWorldMatrix;
		if (volume)
		{
			return VolumeMeshes;
		}
		return _UsedMeshes;
	}

	public void Dispose()
	{
		if (_VolumeMeshes != null)
		{
			for (int i = 1; i < _VolumeMeshes.Length; i += 2)
			{
				_VolumeMeshes[i].Destroy();
			}
			_VolumeMeshes = null;
		}
		_UsedMeshesCache = null;
	}

	private Mesh CreateBoundaryMesh(Mesh sourceMesh)
	{
		Mesh mesh = new Mesh
		{
			hideFlags = HideFlags.DontSave
		};
		Vector3[] vertices = sourceMesh.vertices;
		List<Vector3> list = new List<Vector3>();
		List<int> list2 = new List<int>();
		Edge[] array = BuildManifoldEdges(sourceMesh);
		Vector3 item = default;
		int item2 = array.Length * 4;
		for (int i = 0; i < array.Length; i++)
		{
			int count = list.Count;
			Vector3 vector = vertices[array[i].VertexIndex0];
			Vector3 vector2 = vertices[array[i].VertexIndex1];
			list.Add(vector);
			list.Add(vector2);
			vector.y -= 1000f;
			vector2.y -= 1000f;
			list.Add(vector);
			list.Add(vector2);
			list2.Add(count + 3);
			list2.Add(count + 2);
			list2.Add(count);
			list2.Add(count + 3);
			list2.Add(count);
			list2.Add(count + 1);
			list2.Add(count + 3);
			list2.Add(count + 2);
			list2.Add(item2);
			item += vector;
			item += vector2;
		}
		int num = list.Count / 2;
		item /= (float)num;
		list.Add(item);
		mesh.vertices = list.ToArray();
		mesh.SetIndices(list2.ToArray(), MeshTopology.Triangles, 0);
		return mesh;
	}

	private static Edge[] BuildManifoldEdges(Mesh mesh)
	{
		Edge[] array = BuildEdges(mesh.vertexCount, mesh.triangles);
		List<Edge> list = new List<Edge>();
		Edge[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			Edge item = array2[i];
			if (item.FaceIndex0 == item.FaceIndex1)
			{
				list.Add(item);
			}
		}
		return list.ToArray();
	}

	private static Edge[] BuildEdges(int vertexCount, int[] triangleArray)
	{
		int num = triangleArray.Length;
		int[] array = new int[vertexCount + num];
		int num2 = triangleArray.Length / 3;
		for (int i = 0; i < vertexCount; i++)
		{
			array[i] = -1;
		}
		Edge[] array2 = new Edge[num];
		int num3 = 0;
		for (int j = 0; j < num2; j++)
		{
			int num4 = triangleArray[j * 3 + 2];
			for (int k = 0; k < 3; k++)
			{
				int num5 = triangleArray[j * 3 + k];
				if (num4 < num5)
				{
					Edge edge = new Edge
					{
						VertexIndex0 = num4,
						VertexIndex1 = num5,
						FaceIndex0 = j,
						FaceIndex1 = j
					};
					array2[num3] = edge;
					int num6 = array[num4];
					if (num6 == -1)
					{
						array[num4] = num3;
					}
					else
					{
						while (true)
						{
							int num7 = array[vertexCount + num6];
							if (num7 == -1)
							{
								break;
							}
							num6 = num7;
						}
						array[vertexCount + num6] = num3;
					}
					array[vertexCount + num3] = -1;
					num3++;
				}
				num4 = num5;
			}
		}
		for (int l = 0; l < num2; l++)
		{
			int num8 = triangleArray[l * 3 + 2];
			for (int m = 0; m < 3; m++)
			{
				int num9 = triangleArray[l * 3 + m];
				if (num8 > num9)
				{
					bool flag = false;
					for (int num10 = array[num9]; num10 != -1; num10 = array[vertexCount + num10])
					{
						Edge edge2 = array2[num10];
						if (edge2.VertexIndex1 == num8 && edge2.FaceIndex0 == edge2.FaceIndex1)
						{
							array2[num10].FaceIndex1 = l;
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						Edge edge3 = new Edge
						{
							VertexIndex0 = num8,
							VertexIndex1 = num9,
							FaceIndex0 = l,
							FaceIndex1 = l
						};
						array2[num3] = edge3;
						num3++;
					}
				}
				num8 = num9;
			}
		}
		Edge[] array3 = new Edge[num3];
		for (int n = 0; n < num3; n++)
		{
			array3[n] = array2[n];
		}
		return array3;
	}

	internal virtual void OnEnable(Water water)
	{
		_Water = water;
	}

	internal virtual void OnDisable()
	{
		Dispose();
	}
}
