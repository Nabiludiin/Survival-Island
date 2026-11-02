using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace UltimateWater.Internal;

[Serializable]
public abstract class WaterPrimitiveBase
{
	protected class CachedMeshSet
	{
		public Mesh[] Meshes;

		public int LastFrameUsed;

		public CachedMeshSet(Mesh[] meshes)
		{
			Meshes = meshes;
			Update();
		}

		public void Update()
		{
			LastFrameUsed = Time.frameCount;
		}
	}

	protected Water _Water;

	protected Dictionary<int, CachedMeshSet> _Cache = new Dictionary<int, CachedMeshSet>(Int32EqualityComparer.Default);

	private List<int> _KeysToRemove;

	public virtual Mesh[] GetTransformedMeshes(Camera camera, out Matrix4x4 matrix, int vertexCount, bool volume)
	{
		matrix = ((camera != null) ? GetMatrix(camera) : Matrix4x4.identity);
		int num = vertexCount;
		if (volume)
		{
			num = -num;
		}
		if (!_Cache.TryGetValue(num, out var value))
		{
			value = (_Cache[num] = new CachedMeshSet(CreateMeshes(vertexCount, volume)));
		}
		else
		{
			value.Update();
		}
		return value.Meshes;
	}

	public void Dispose()
	{
		foreach (CachedMeshSet value in _Cache.Values)
		{
			Mesh[] meshes = value.Meshes;
			foreach (Mesh obj in meshes)
			{
				if (Application.isPlaying)
				{
					UnityEngine.Object.Destroy(obj);
				}
				else
				{
					UnityEngine.Object.DestroyImmediate(obj);
				}
			}
		}
		_Cache.Clear();
	}

	protected abstract Matrix4x4 GetMatrix(Camera camera);

	protected abstract Mesh[] CreateMeshes(int vertexCount, bool volume);

	protected Mesh CreateMesh(Vector3[] vertices, int[] indices, string name, bool triangular = false)
	{
		Mesh mesh = new Mesh();
		mesh.hideFlags = HideFlags.DontSave;
		mesh.name = name;
		mesh.vertices = vertices;
		mesh.SetIndices(indices, (!triangular) ? MeshTopology.Quads : MeshTopology.Triangles, 0);
		mesh.RecalculateBounds();
		mesh.UploadMeshData(markNoLongerReadable: true);
		return mesh;
	}

	protected Mesh CreateMesh32bit(Vector3[] vertices, int[] indices, string name, bool triangular = false)
	{
		Mesh mesh = new Mesh();
		mesh.hideFlags = HideFlags.DontSave;
		mesh.name = name;
		mesh.vertices = vertices;
		mesh.indexFormat = IndexFormat.UInt32;
		mesh.SetIndices(indices, (!triangular) ? MeshTopology.Quads : MeshTopology.Triangles, 0);
		mesh.RecalculateBounds();
		mesh.UploadMeshData(markNoLongerReadable: true);
		return mesh;
	}

	internal void Update()
	{
		int frameCount = Time.frameCount;
		if (_KeysToRemove == null)
		{
			_KeysToRemove = new List<int>();
		}
		Dictionary<int, CachedMeshSet>.Enumerator enumerator = _Cache.GetEnumerator();
		while (enumerator.MoveNext())
		{
			KeyValuePair<int, CachedMeshSet> current = enumerator.Current;
			if (frameCount - current.Value.LastFrameUsed <= 27)
			{
				continue;
			}
			_KeysToRemove.Add(current.Key);
			Mesh[] meshes = current.Value.Meshes;
			foreach (Mesh obj in meshes)
			{
				if (Application.isPlaying)
				{
					UnityEngine.Object.Destroy(obj);
				}
				else
				{
					UnityEngine.Object.DestroyImmediate(obj);
				}
			}
		}
		enumerator.Dispose();
		for (int j = 0; j < _KeysToRemove.Count; j++)
		{
			_Cache.Remove(_KeysToRemove[j]);
		}
		_KeysToRemove.Clear();
	}

	internal virtual void OnEnable(Water water)
	{
		_Water = water;
	}

	internal virtual void OnDisable()
	{
		Dispose();
	}

	internal virtual void AddToMaterial(Water water)
	{
	}

	internal virtual void RemoveFromMaterial(Water water)
	{
	}
}
