using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class MeshPath : MonoBehaviour, SceneEventInterface
{
	public static List<MeshPathConnection> PathConnections = new List<MeshPathConnection>();

	[SerializeField]
	protected bool drawGizmoPoints;

	[SerializeField]
	private float gizmosScale = 0.2f;

	[SerializeField]
	private float ropeWidth = 0.1f;

	public float maxLength = 1000f;

	[SerializeField]
	[Range(1f, 100f)]
	private float colliderRopeWidthMultiplier = 0.1f;

	[SerializeField]
	protected MeshFilter meshFilter;

	[HideInInspector]
	public float currentSlack = -0.5f;

	[HideInInspector]
	public List<Vector3> points = new List<Vector3>();

	[SerializeField]
	[EventRef]
	private string eventRef_createZipline = string.Empty;

	protected virtual void OnDrawGizmos()
	{
		if (drawGizmoPoints && points != null)
		{
			for (int i = 0; i < points.Count - 1; i++)
			{
				Vector3 vector = transform.TransformPoint(points[i]);
				Vector3 to = transform.TransformPoint(points[i + 1]);
				Gizmos.color = Color.green;
				Gizmos.DrawSphere(vector, 1f * gizmosScale);
				Gizmos.DrawLine(vector, to);
			}
		}
	}

	void SceneEventInterface.OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent == SceneEvent.LeaveGame && PathConnections != null)
		{
			PathConnections.Clear();
		}
	}

	public static MeshPathConnection GetBlockConnection(MeshPath path)
	{
		if (!PathConnections.ContainsItems())
		{
			return null;
		}
		for (int i = 0; i < PathConnections.Count; i++)
		{
			MeshPathConnection meshPathConnection = PathConnections[i];
			if (meshPathConnection != null && meshPathConnection.path == path)
			{
				return meshPathConnection;
			}
		}
		return null;
	}

	public static MeshPathConnection GetConnection(MeshPathBase baseA, MeshPathBase baseB)
	{
		if (!PathConnections.ContainsItems())
		{
			return null;
		}
		for (int i = 0; i < PathConnections.Count; i++)
		{
			MeshPathConnection meshPathConnection = PathConnections[i];
			if (meshPathConnection != null && (meshPathConnection.baseA == baseA || meshPathConnection.baseA == baseB) && (meshPathConnection.baseB == baseA || meshPathConnection.baseB == baseB))
			{
				return meshPathConnection;
			}
		}
		return null;
	}

	public static bool DoesConnectionExist(MeshPathBase baseA, MeshPathBase baseB)
	{
		if (!PathConnections.ContainsItems())
		{
			return false;
		}
		for (int i = 0; i < PathConnections.Count; i++)
		{
			MeshPathConnection meshPathConnection = PathConnections[i];
			if (meshPathConnection != null && (meshPathConnection.baseA == baseA || meshPathConnection.baseA == baseB) && (meshPathConnection.baseB == baseA || meshPathConnection.baseB == baseB))
			{
				return true;
			}
		}
		return false;
	}

	public static void RemovePathsFromBase(MeshPathBase baseA)
	{
		if (baseA == null || !PathConnections.ContainsItems())
		{
			return;
		}
		for (int i = 0; i < PathConnections.Count; i++)
		{
			MeshPathConnection meshPathConnection = PathConnections[i];
			if (meshPathConnection != null && (meshPathConnection.baseA == baseA || meshPathConnection.baseB == baseA))
			{
				if (meshPathConnection.path != null)
				{
					Object.Destroy(meshPathConnection.path.gameObject);
				}
				PathConnections.RemoveAt(i);
				i--;
			}
		}
	}

	public static bool RemovePath(MeshPath path)
	{
		if (!PathConnections.ContainsItems())
		{
			return false;
		}
		for (int i = 0; i < PathConnections.Count; i++)
		{
			MeshPathConnection meshPathConnection = PathConnections[i];
			if (meshPathConnection != null && meshPathConnection.path == path)
			{
				PathConnections.RemoveAt(i);
				meshPathConnection.path.OnPathRemoved();
				Object.Destroy(meshPathConnection.path.gameObject);
				return true;
			}
		}
		return false;
	}

	public static bool CreateMeshPath(MeshPathBase baseA, MeshPathBase baseB, float slack, bool replicating)
	{
		if (DoesConnectionExist(baseA, baseB))
		{
			return false;
		}
		MeshPath meshPath = CreateNewMeshPathGameObject(baseA.pathPrefab, baseA.connectPoint.position, baseA.transform.parent);
		if (meshPath != null)
		{
			meshPath.CreatePath(baseA.connectPoint.position, baseB.connectPoint.position, slack, replicating);
		}
		MeshPathConnection meshPathConnection = new MeshPathConnection(baseA, baseB, meshPath);
		PathConnections.Add(meshPathConnection);
		if (!replicating)
		{
			baseA.OnPathSucessfullyCreated(baseB, meshPathConnection);
		}
		return true;
	}

	private static MeshPath CreateNewMeshPathGameObject(MeshPath pathPrefab, Vector3 position, Transform parent)
	{
		MeshPath meshPath = Object.Instantiate(pathPrefab, position, Quaternion.identity, parent);
		meshPath.transform.localEulerAngles = Vector3.zero;
		return meshPath;
	}

	public virtual void CreatePath(Vector3 worldPointA, Vector3 worldPointB, float slack, bool replicating, int minPoints = 8, int maxPoints = 20)
	{
		if (points == null)
		{
			points = new List<Vector3>();
		}
		else
		{
			points.Clear();
		}
		currentSlack = slack;
		Vector3 vector = transform.InverseTransformPoint(worldPointA);
		Vector3 vector2 = transform.InverseTransformPoint(worldPointB);
		points = CreatePointsForPath(vector, vector2, slack, minPoints, maxPoints);
		MeshCollider componentInChildren = meshFilter.GetComponentInChildren<MeshCollider>();
		if (componentInChildren != null)
		{
			componentInChildren.sharedMesh = GenerateMeshFromPath(vector, vector2, ropeWidth * colliderRopeWidthMultiplier);
		}
		if (meshFilter != null)
		{
			meshFilter.sharedMesh = GenerateMeshFromPath(vector, vector2, ropeWidth);
		}
		if (!replicating)
		{
			RuntimeManager.PlayOneShot(eventRef_createZipline, worldPointA);
			RuntimeManager.PlayOneShot(eventRef_createZipline, worldPointB);
		}
	}

	public static List<Vector3> CreatePointsForPath(Vector3 localPointA, Vector3 localPointB, float slack, int minPoints = 8, int maxPoints = 20)
	{
		List<Vector3> list = new List<Vector3>();
		int num = Mathf.Clamp(Mathf.RoundToInt(localPointA.DistanceXZ(localPointB) / 2f), minPoints, maxPoints);
		list.Add(localPointA);
		for (float num2 = 1f; num2 < (float)num; num2++)
		{
			list.Add(Helper.GetBezierCurveValue(localPointA, localPointB, slack, num2 / (float)num));
		}
		list.Add(localPointB);
		return list;
	}

	public Vector3 GetDirectionFromStartPointToEndPoint()
	{
		return GetWorldPointPositionFromIndex(points.Count - 1) - GetWorldPointPositionFromIndex(0);
	}

	public Vector3 GetLocalPointPositionFromIndex(int index)
	{
		if (!points.ContainsItems())
		{
			return Vector3.zero;
		}
		if (index < 0 || index >= points.Count)
		{
			return Vector3.zero;
		}
		return points[index];
	}

	public Vector3 GetWorldPointPositionFromIndex(int index)
	{
		if (!points.ContainsItems())
		{
			return Vector3.zero;
		}
		if (index < 0 || index >= points.Count)
		{
			return Vector3.zero;
		}
		return transform.TransformPoint(points[index]);
	}

	public bool GetLocalPointPositionFromIndex(int index, ref Vector3 pointRef)
	{
		if (!points.ContainsItems())
		{
			return false;
		}
		if (index < 0 || index >= points.Count)
		{
			return false;
		}
		pointRef = GetLocalPointPositionFromIndex(index);
		return true;
	}

	public int GetClosestPointIndexFromWorldPosition(Vector3 worldPosition, bool includeEndPoints = false)
	{
		if (!points.ContainsItems())
		{
			return -1;
		}
		int num = -1;
		float num2 = float.MaxValue;
		worldPosition = transform.InverseTransformPoint(worldPosition);
		for (int i = 0; i < points.Count; i++)
		{
			float num3 = Vector3.Distance(points[i], worldPosition);
			if (num == -1 || num3 < num2)
			{
				num2 = num3;
				num = i;
			}
		}
		if (!includeEndPoints)
		{
			if (num == points.Count - 1)
			{
				num = points.Count - 2;
			}
			if (num == 0)
			{
				num = 1;
			}
			num = Mathf.Clamp(num, 0, points.Count - 1);
		}
		return num;
	}

	public virtual void OnPathRemoved()
	{
	}

	private Mesh GenerateMeshFromPath(Vector3 pointA, Vector3 pointB, float ropeWidth)
	{
		Mesh mesh = new Mesh();
		Vector3 vector = -Vector3.Cross((pointB - pointA).normalized, Vector3.up).normalized * ropeWidth;
		Vector3 vector2 = Vector3.up;
		Vector3[] array = new Vector3[points.Count * 5];
		List<int> list = new List<int>();
		Vector2[] array2 = new Vector2[array.Length];
		Vector3[] array3 = new Vector3[array.Length];
		float pathLength = GetPathLength();
		float num = 0.25f;
		for (int i = 0; i < points.Count; i++)
		{
			float num2 = (float)i / ((float)points.Count - 1f);
			if (i < points.Count - 1)
			{
				vector2 = Vector3.Cross((points[i + 1] - points[i]).normalized, vector).normalized * ropeWidth;
			}
			Vector3 vector3 = points[i];
			array[i] = vector3 - vector + vector2;
			array[i + points.Count] = vector3 + vector + vector2;
			array[i + points.Count * 2] = vector3 + vector - vector2;
			array[i + points.Count * 3] = vector3 - vector - vector2;
			array[i + points.Count * 4] = array[i];
			array2[i] = new Vector2(0f, pathLength * num2);
			array2[i + points.Count] = new Vector2(num, pathLength * num2);
			array2[i + points.Count * 2] = new Vector2(num * 2f, pathLength * num2);
			array2[i + points.Count * 3] = new Vector2(num * 3f, pathLength * num2);
			array2[i + points.Count * 4] = new Vector2(1f, pathLength * num2);
			array3[i] = vector2 - vector;
			array3[i + points.Count] = vector2 + vector;
			array3[i + points.Count * 2] = -vector2 + vector;
			array3[i + points.Count * 3] = -vector2 - vector;
			array3[i + points.Count * 4] = array3[i];
		}
		for (int j = 0; j < 5; j++)
		{
			for (int k = 0; k < points.Count - 1; k++)
			{
				int num3 = k + points.Count * j;
				list.Add(ClampIndex(num3));
				list.Add(ClampIndex(num3 + 1));
				list.Add(ClampIndex(num3 + points.Count));
				list.Add(ClampIndex(num3 + points.Count));
				list.Add(ClampIndex(num3 + 1));
				list.Add(ClampIndex(num3 + points.Count + 1));
			}
		}
		mesh.vertices = array;
		mesh.triangles = list.ToArray();
		mesh.normals = array3;
		mesh.uv = array2;
		mesh.RecalculateBounds();
		return mesh;
		int ClampIndex(int index)
		{
			if (index >= points.Count * 5)
			{
				return index - points.Count * 5;
			}
			return index;
		}
	}

	private float GetPathLength()
	{
		if (points.ContainsItems())
		{
			float num = 0f;
			for (int i = 0; i < points.Count - 1; i++)
			{
				num += Vector3.Distance(points[i], points[i + 1]);
			}
			return num;
		}
		return 0f;
	}
}
