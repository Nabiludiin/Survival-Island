using System;
using Cinemachine.Utility;
using UnityEngine;

namespace Cinemachine;

[DocumentationSorting(19f, DocumentationSortingAttribute.Level.UserRef)]
[AddComponentMenu("Cinemachine/CinemachineTargetGroup")]
[SaveDuringPlay]
[ExecuteInEditMode]
public class CinemachineTargetGroup : MonoBehaviour
{
	[Serializable]
	[DocumentationSorting(19.1f, DocumentationSortingAttribute.Level.UserRef)]
	public struct Target
	{
		[Tooltip("The target objects.  This object's position and orientation will contribute to the group's average position and orientation, in accordance with its weight")]
		public Transform target;

		[Tooltip("How much weight to give the target when averaging.  Cannot be negative")]
		public float weight;

		[Tooltip("The radius of the target, used for calculating the bounding box.  Cannot be negative")]
		public float radius;
	}

	[DocumentationSorting(19.2f, DocumentationSortingAttribute.Level.UserRef)]
	public enum PositionMode
	{
		GroupCenter,
		GroupAverage
	}

	[DocumentationSorting(19.3f, DocumentationSortingAttribute.Level.UserRef)]
	public enum RotationMode
	{
		Manual,
		GroupAverage
	}

	public enum UpdateMethod
	{
		Update,
		FixedUpdate,
		LateUpdate
	}

	[Tooltip("How the group's position is calculated.  Select GroupCenter for the center of the bounding box, and GroupAverage for a weighted average of the positions of the members.")]
	public PositionMode m_PositionMode;

	[Tooltip("How the group's rotation is calculated.  Select Manual to use the value in the group's transform, and GroupAverage for a weighted average of the orientations of the members.")]
	public RotationMode m_RotationMode;

	[Tooltip("When to update the group's transform based on the position of the group members")]
	public UpdateMethod m_UpdateMethod = UpdateMethod.LateUpdate;

	[NoSaveDuringPlay]
	[Tooltip("The target objects, together with their weights and radii, that will contribute to the group's average position, orientation, and size.")]
	public Target[] m_Targets = new Target[0];

	private float m_lastRadius;

	public Bounds BoundingBox
	{
		get
		{
			Vector3 vector = CalculateAveragePosition(out var averageWeight);
			bool flag = false;
			Bounds result = new Bounds(vector, new Vector3(m_lastRadius * 2f, m_lastRadius * 2f, m_lastRadius * 2f));
			if (averageWeight > 0.0001f)
			{
				for (int i = 0; i < m_Targets.Length; i++)
				{
					if (m_Targets[i].target != null)
					{
						float weight = m_Targets[i].weight;
						weight = ((!(weight < averageWeight - 0.0001f)) ? 1f : (weight / averageWeight));
						float num = m_Targets[i].radius * 2f * weight;
						Vector3 center = Vector3.Lerp(vector, m_Targets[i].target.position, weight);
						Bounds bounds = new Bounds(center, new Vector3(num, num, num));
						if (!flag)
						{
							result = bounds;
						}
						else
						{
							result.Encapsulate(bounds);
						}
						flag = true;
					}
				}
			}
			Vector3 extents = result.extents;
			m_lastRadius = Mathf.Max(extents.x, Mathf.Max(extents.y, extents.z));
			return result;
		}
	}

	public bool IsEmpty
	{
		get
		{
			for (int i = 0; i < m_Targets.Length; i++)
			{
				if (m_Targets[i].target != null && m_Targets[i].weight > 0.0001f)
				{
					return false;
				}
			}
			return true;
		}
	}

	public Bounds GetViewSpaceBoundingBox(Matrix4x4 mView)
	{
		Matrix4x4 inverse = mView.inverse;
		Vector3 vector = inverse.MultiplyPoint3x4(CalculateAveragePosition(out var averageWeight));
		bool flag = false;
		Bounds result = new Bounds(vector, new Vector3(m_lastRadius * 2f, m_lastRadius * 2f, m_lastRadius * 2f));
		if (averageWeight > 0.0001f)
		{
			for (int i = 0; i < m_Targets.Length; i++)
			{
				if (m_Targets[i].target != null)
				{
					float weight = m_Targets[i].weight;
					weight = ((!(weight < averageWeight - 0.0001f)) ? 1f : (weight / averageWeight));
					float num = m_Targets[i].radius * 2f;
					Vector4 vector2 = inverse.MultiplyPoint3x4(m_Targets[i].target.position);
					vector2 = Vector3.Lerp(vector, vector2, weight);
					Bounds bounds = new Bounds(vector2, new Vector3(num, num, num));
					if (!flag)
					{
						result = bounds;
					}
					else
					{
						result.Encapsulate(bounds);
					}
					flag = true;
				}
			}
		}
		Vector3 extents = result.extents;
		m_lastRadius = Mathf.Max(extents.x, Mathf.Max(extents.y, extents.z));
		return result;
	}

	private Vector3 CalculateAveragePosition(out float averageWeight)
	{
		Vector3 zero = Vector3.zero;
		float num = 0f;
		int num2 = 0;
		for (int i = 0; i < m_Targets.Length; i++)
		{
			if (m_Targets[i].target != null && m_Targets[i].weight > 0.0001f)
			{
				num2++;
				num += m_Targets[i].weight;
				zero += m_Targets[i].target.position * m_Targets[i].weight;
			}
		}
		if (num > 0.0001f)
		{
			zero /= num;
		}
		if (num2 == 0)
		{
			averageWeight = 0f;
			return transform.position;
		}
		averageWeight = num / (float)num2;
		return zero;
	}

	private Quaternion CalculateAverageOrientation()
	{
		Quaternion q = Quaternion.identity;
		for (int i = 0; i < m_Targets.Length; i++)
		{
			if (m_Targets[i].target != null)
			{
				float weight = m_Targets[i].weight;
				Quaternion rotation = m_Targets[i].target.rotation;
				q = new Quaternion(q.x + rotation.x * weight, q.y + rotation.y * weight, q.z + rotation.z * weight, q.w + rotation.w * weight);
			}
		}
		return q.Normalized();
	}

	private void OnValidate()
	{
		for (int i = 0; i < m_Targets.Length; i++)
		{
			if (m_Targets[i].weight < 0f)
			{
				m_Targets[i].weight = 0f;
			}
			if (m_Targets[i].radius < 0f)
			{
				m_Targets[i].radius = 0f;
			}
		}
	}

	private void FixedUpdate()
	{
		if (m_UpdateMethod == UpdateMethod.FixedUpdate)
		{
			UpdateTransform();
		}
	}

	private void Update()
	{
		if (!Application.isPlaying || m_UpdateMethod == UpdateMethod.Update)
		{
			UpdateTransform();
		}
	}

	private void LateUpdate()
	{
		if (m_UpdateMethod == UpdateMethod.LateUpdate)
		{
			UpdateTransform();
		}
	}

	private void UpdateTransform()
	{
		if (!IsEmpty)
		{
			switch (m_PositionMode)
			{
			case PositionMode.GroupCenter:
				transform.position = BoundingBox.center;
				break;
			case PositionMode.GroupAverage:
			{
				transform.position = CalculateAveragePosition(out var _);
				break;
			}
			}
			RotationMode rotationMode = m_RotationMode;
			if (rotationMode != RotationMode.Manual && rotationMode == RotationMode.GroupAverage)
			{
				transform.rotation = CalculateAverageOrientation();
			}
		}
	}
}
