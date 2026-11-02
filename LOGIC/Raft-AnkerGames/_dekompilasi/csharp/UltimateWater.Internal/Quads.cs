using UnityEngine;

namespace UltimateWater.Internal;

public class Quads
{
	private static Mesh _BipolarXY;

	private static Mesh _BipolarXInversedY;

	private static Mesh _BipolarXZ;

	private static Mesh _TriangleMesh;

	private static Mesh _TriangleMeshInversedY;

	private static bool _Initialized;

	public static Mesh BipolarXY
	{
		get
		{
			if (!_Initialized)
			{
				CreateMeshes();
			}
			return _BipolarXY;
		}
	}

	public static Mesh BipolarXInversedY
	{
		get
		{
			if (!_Initialized)
			{
				CreateMeshes();
			}
			return _BipolarXInversedY;
		}
	}

	public static Mesh BipolarXZ
	{
		get
		{
			if (!_Initialized)
			{
				CreateMeshes();
			}
			return _BipolarXZ;
		}
	}

	public static Mesh TriangleMesh
	{
		get
		{
			if (!_Initialized)
			{
				CreateMeshes();
			}
			return _TriangleMesh;
		}
	}

	public static Mesh TriangleMeshInversedY
	{
		get
		{
			if (!_Initialized)
			{
				CreateMeshes();
			}
			return _TriangleMeshInversedY;
		}
	}

	private static void CreateMeshes()
	{
		_BipolarXY = CreateBipolarXY(inversedY: false);
		_BipolarXInversedY = CreateBipolarXY(inversedY: true);
		_BipolarXZ = CreateBipolarXZ();
		_TriangleMesh = CreateFullscreenTriangle(inverted: false);
		_TriangleMeshInversedY = CreateFullscreenTriangle(inverted: true);
		_Initialized = true;
	}

	private static Mesh CreateBipolarXY(bool inversedY)
	{
		Mesh mesh = new Mesh();
		mesh.hideFlags = HideFlags.DontSave;
		mesh.vertices = new Vector3[4]
		{
			new Vector3(-1f, -1f, 0f),
			new Vector3(1f, -1f, 0f),
			new Vector3(1f, 1f, 0f),
			new Vector3(-1f, 1f, 0f)
		};
		mesh.uv = new Vector2[4]
		{
			new Vector2(0f, inversedY ? 1f : 0f),
			new Vector2(1f, inversedY ? 1f : 0f),
			new Vector2(1f, inversedY ? 0f : 1f),
			new Vector2(0f, inversedY ? 0f : 1f)
		};
		Mesh mesh2 = mesh;
		mesh2.SetTriangles(new int[6] { 0, 1, 2, 0, 2, 3 }, 0);
		mesh2.UploadMeshData(markNoLongerReadable: true);
		return mesh2;
	}

	private static Mesh CreateBipolarXZ()
	{
		Mesh mesh = new Mesh();
		mesh.name = "Shoreline Quad Mesh";
		mesh.hideFlags = HideFlags.DontSave;
		mesh.vertices = new Vector3[4]
		{
			new Vector3(-1f, 0f, -1f),
			new Vector3(-1f, 0f, 1f),
			new Vector3(1f, 0f, 1f),
			new Vector3(1f, 0f, -1f)
		};
		mesh.uv = new Vector2[4]
		{
			new Vector2(0f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f)
		};
		Mesh mesh2 = mesh;
		mesh2.SetIndices(new int[4] { 0, 1, 2, 3 }, MeshTopology.Quads, 0);
		mesh2.UploadMeshData(markNoLongerReadable: true);
		return mesh2;
	}

	private static Mesh CreateFullscreenTriangle(bool inverted)
	{
		Mesh mesh = new Mesh();
		mesh.hideFlags = HideFlags.DontSave;
		mesh.vertices = new Vector3[3]
		{
			new Vector3(-1f, -1f, 0f),
			new Vector3(3f, -1f, 0f),
			new Vector3(-1f, 3f, 0f)
		};
		mesh.uv = new Vector2[3]
		{
			new Vector2(0f, inverted ? 1f : 0f),
			new Vector2(2f, inverted ? 1f : 0f),
			new Vector2(0f, inverted ? (-1f) : 2f)
		};
		Mesh mesh2 = mesh;
		mesh2.SetTriangles(new int[3] { 0, 1, 2 }, 0);
		mesh2.UploadMeshData(markNoLongerReadable: true);
		return mesh2;
	}
}
