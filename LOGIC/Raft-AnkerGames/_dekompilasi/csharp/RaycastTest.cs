using UnityEngine;

[ExecuteInEditMode]
public class RaycastTest : MonoBehaviour
{
	public MeshRenderer mr;

	public Collider collider;

	public LayerMask layerMask;

	public bool createTriangle;

	public int triangleIndex;

	private RaycastHit hitInfo;

	public bool step2;

	public bool drawTriangle;

	private void Update()
	{
		if (createTriangle)
		{
			createTriangle = false;
			CreateTriangle();
		}
	}

	private void CreateTriangle()
	{
		Mesh mesh = new Mesh();
		Vector3 vector = new Vector3(0f, 0f, 0f);
		Vector3 vector2 = new Vector3(0f, 0f, 1f);
		Vector3 vector3 = new Vector3(0f, 1f, 0f);
		float x = 0.01f;
		Vector3 vector4 = new Vector3(x, 0f, 0f);
		Vector3 vector5 = new Vector3(x, 0f, 1f);
		Vector3 vector6 = new Vector3(x, 1f, 0f);
		Vector3[] vertices = new Vector3[6] { vector, vector2, vector3, vector6, vector5, vector4 };
		int[] triangles = new int[6] { 0, 1, 2, 3, 4, 5 };
		mesh.name = "Triangle";
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		GetComponent<MeshFilter>().sharedMesh = mesh;
		GetComponent<MeshCollider>().sharedMesh = mesh;
	}

	private void Swap<T>(ref T v1, ref T v2)
	{
		T val = v1;
		v1 = v2;
		v2 = val;
	}

	private void OnDrawGizmos()
	{
		if (!mr)
		{
			return;
		}
		Vector3 position = transform.position;
		_ = mr.bounds.min;
		Vector3 left = Vector3.left;
		Physics.queriesHitBackfaces = true;
		_ = Time.realtimeSinceStartup;
		if (Physics.Raycast(new Ray
		{
			origin = position,
			direction = left
		}, out hitInfo, 10000f))
		{
			if (Vector3.Dot(left, hitInfo.normal) >= 0f)
			{
				Gizmos.color = Color.green;
			}
			else
			{
				Gizmos.color = Color.red;
			}
			Gizmos.DrawLine(hitInfo.point, hitInfo.point + hitInfo.normal);
			Gizmos.color = Color.white;
			Gizmos.DrawLine(position, hitInfo.point);
		}
		else
		{
			Gizmos.color = Color.red;
			Gizmos.DrawLine(position, position + left.normalized * 1000f);
		}
	}
}
