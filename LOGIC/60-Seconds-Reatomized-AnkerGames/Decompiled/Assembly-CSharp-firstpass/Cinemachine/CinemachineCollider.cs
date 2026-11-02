using System;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cinemachine;

[DocumentationSorting(15f, DocumentationSortingAttribute.Level.UserRef)]
[ExecuteInEditMode]
[AddComponentMenu("")]
[SaveDuringPlay]
public class CinemachineCollider : CinemachineExtension
{
	public enum ResolutionStrategy
	{
		PullCameraForward,
		PreserveCameraHeight,
		PreserveCameraDistance
	}

	private class VcamExtraState
	{
		public Vector3 m_previousDisplacement;

		public float colliderDisplacement;

		public bool targetObscured;

		public List<Vector3> debugResolutionPath;

		public void AddPointToDebugPath(Vector3 p)
		{
		}
	}

	[Header("Obstacle Detection")]
	[Tooltip("The Unity layer mask against which the collider will raycast")]
	public LayerMask m_CollideAgainst = 1;

	[TagField]
	[Tooltip("Obstacles with this tag will be ignored.  It is a good idea to set this field to the target's tag")]
	public string m_IgnoreTag = string.Empty;

	[Tooltip("Obstacles closer to the target than this will be ignored")]
	public float m_MinimumDistanceFromTarget = 0.1f;

	[Space]
	[Tooltip("When enabled, will attempt to resolve situations where the line of sight to the target is blocked by an obstacle")]
	[FormerlySerializedAs("m_PreserveLineOfSight")]
	public bool m_AvoidObstacles = true;

	[Tooltip("The maximum raycast distance when checking if the line of sight to this camera's target is clear.  If the setting is 0 or less, the current actual distance to target will be used.")]
	[FormerlySerializedAs("m_LineOfSightFeelerDistance")]
	public float m_DistanceLimit;

	[Tooltip("Camera will try to maintain this distance from any obstacle.  Try to keep this value small.  Increase it if you are seeing inside obstacles due to a large FOV on the camera.")]
	public float m_CameraRadius = 0.1f;

	[Tooltip("The way in which the Collider will attempt to preserve sight of the target.")]
	public ResolutionStrategy m_Strategy = ResolutionStrategy.PreserveCameraHeight;

	[Range(1f, 10f)]
	[Tooltip("Upper limit on how many obstacle hits to process.  Higher numbers may impact performance.  In most environments, 4 is enough.")]
	public int m_MaximumEffort = 4;

	[Range(0f, 10f)]
	[Tooltip("The gradualness of collision resolution.  Higher numbers will move the camera more gradually away from obstructions.")]
	[FormerlySerializedAs("m_Smoothing")]
	public float m_Damping;

	[Header("Shot Evaluation")]
	[Tooltip("If greater than zero, a higher score will be given to shots when the target is closer to this distance.  Set this to zero to disable this feature.")]
	public float m_OptimalTargetDistance;

	private const float PrecisionSlush = 0.001f;

	private RaycastHit[] m_CornerBuffer = new RaycastHit[4];

	private const float AngleThreshold = 0.1f;

	private Collider[] mColliderBuffer = new Collider[5];

	private SphereCollider mCameraCollider;

	private GameObject mCameraColliderGameObject;

	public List<List<Vector3>> DebugPaths
	{
		get
		{
			List<List<Vector3>> list = new List<List<Vector3>>();
			foreach (VcamExtraState allExtraState in GetAllExtraStates<VcamExtraState>())
			{
				if (allExtraState.debugResolutionPath != null)
				{
					list.Add(allExtraState.debugResolutionPath);
				}
			}
			return list;
		}
	}

	public bool IsTargetObscured(ICinemachineCamera vcam)
	{
		return GetExtraState<VcamExtraState>(vcam).targetObscured;
	}

	public bool CameraWasDisplaced(CinemachineVirtualCameraBase vcam)
	{
		return GetExtraState<VcamExtraState>(vcam).colliderDisplacement > 0f;
	}

	private void OnValidate()
	{
		m_DistanceLimit = Mathf.Max(0f, m_DistanceLimit);
		m_CameraRadius = Mathf.Max(0f, m_CameraRadius);
		m_MinimumDistanceFromTarget = Mathf.Max(0.01f, m_MinimumDistanceFromTarget);
		m_OptimalTargetDistance = Mathf.Max(0f, m_OptimalTargetDistance);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		CleanupCameraCollider();
	}

	protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
	{
		VcamExtraState extra = null;
		if (stage == CinemachineCore.Stage.Body)
		{
			extra = GetExtraState<VcamExtraState>(vcam);
			extra.targetObscured = false;
			extra.colliderDisplacement = 0f;
			extra.debugResolutionPath = null;
		}
		if (stage == CinemachineCore.Stage.Body && m_AvoidObstacles)
		{
			Vector3 vector = PreserveLignOfSight(ref state, ref extra);
			if (m_Damping > 0f && deltaTime >= 0f)
			{
				Vector3 initial = vector - extra.m_previousDisplacement;
				initial = Damper.Damp(initial, m_Damping, deltaTime);
				vector = extra.m_previousDisplacement + initial;
			}
			extra.m_previousDisplacement = vector;
			state.PositionCorrection += vector;
			extra.colliderDisplacement += vector.magnitude;
		}
		if (stage != CinemachineCore.Stage.Aim)
		{
			return;
		}
		extra = GetExtraState<VcamExtraState>(vcam);
		extra.targetObscured = CheckForTargetObstructions(state);
		if (extra.targetObscured)
		{
			state.ShotQuality *= 0.2f;
		}
		if (extra.colliderDisplacement > 0f)
		{
			state.ShotQuality *= 0.8f;
		}
		float num = 0f;
		if (!(m_OptimalTargetDistance > 0f) || !state.HasLookAt)
		{
			return;
		}
		float num2 = Vector3.Magnitude(state.ReferenceLookAt - state.FinalPosition);
		if (num2 <= m_OptimalTargetDistance)
		{
			float num3 = m_OptimalTargetDistance / 2f;
			if (num2 >= num3)
			{
				num = 0.2f * (num2 - num3) / (m_OptimalTargetDistance - num3);
			}
		}
		else
		{
			num2 -= m_OptimalTargetDistance;
			float num4 = m_OptimalTargetDistance * 3f;
			if (num2 < num4)
			{
				num = 0.2f * (1f - num2 / num4);
			}
		}
		state.ShotQuality *= 1f + num;
	}

	private Vector3 PreserveLignOfSight(ref CameraState state, ref VcamExtraState extra)
	{
		Vector3 result = Vector3.zero;
		if (state.HasLookAt)
		{
			Vector3 correctedPosition = state.CorrectedPosition;
			Vector3 referenceLookAt = state.ReferenceLookAt;
			Vector3 vector = correctedPosition;
			Vector3 vector2 = vector - referenceLookAt;
			float magnitude = vector2.magnitude;
			float num = Mathf.Max(m_MinimumDistanceFromTarget, 0.0001f);
			if (magnitude > num)
			{
				vector2.Normalize();
				float num2 = magnitude - num;
				if (m_DistanceLimit > 0.0001f)
				{
					num2 = Mathf.Min(m_DistanceLimit, num2);
				}
				Ray ray = new Ray(vector - num2 * vector2, vector2);
				num2 += 0.001f;
				if (num2 > 0.0001f && RaycastIgnoreTag(ray, out var hitInfo, num2))
				{
					float distance = Mathf.Max(0f, hitInfo.distance - 0.001f);
					vector = ray.GetPoint(distance);
					extra.AddPointToDebugPath(vector);
					if (m_Strategy != ResolutionStrategy.PullCameraForward)
					{
						vector = PushCameraBack(vector, vector2, hitInfo, referenceLookAt, new Plane(state.ReferenceUp, correctedPosition), magnitude, m_MaximumEffort, ref extra);
					}
				}
			}
			if (m_CameraRadius > 0.0001f)
			{
				vector += RespectCameraRadius(vector, state.ReferenceLookAt);
			}
			else if (mCameraColliderGameObject != null)
			{
				CleanupCameraCollider();
			}
			result = vector - correctedPosition;
		}
		return result;
	}

	private bool RaycastIgnoreTag(Ray ray, out RaycastHit hitInfo, float rayLength)
	{
		while (Physics.Raycast(ray, out hitInfo, rayLength, m_CollideAgainst.value, QueryTriggerInteraction.Ignore))
		{
			if (m_IgnoreTag.Length == 0 || !hitInfo.collider.CompareTag(m_IgnoreTag))
			{
				return true;
			}
			Ray ray2 = new Ray(ray.GetPoint(rayLength), -ray.direction);
			if (!hitInfo.collider.Raycast(ray2, out hitInfo, rayLength))
			{
				break;
			}
			rayLength = hitInfo.distance - 0.001f;
			if (rayLength < 0.0001f)
			{
				break;
			}
			ray.origin = ray2.GetPoint(rayLength);
		}
		return false;
	}

	private Vector3 PushCameraBack(Vector3 currentPos, Vector3 pushDir, RaycastHit obstacle, Vector3 lookAtPos, Plane startPlane, float targetDistance, int iterations, ref VcamExtraState extra)
	{
		Vector3 vector = currentPos;
		Vector3 outDir = Vector3.zero;
		if (!GetWalkingDirection(vector, pushDir, obstacle, ref outDir))
		{
			return vector;
		}
		Ray ray = new Ray(vector, outDir);
		float pushBackDistance = GetPushBackDistance(ray, startPlane, targetDistance, lookAtPos);
		if (pushBackDistance <= 0.0001f)
		{
			return vector;
		}
		float num = ClampRayToBounds(ray, pushBackDistance, obstacle.collider.bounds);
		pushBackDistance = Mathf.Min(pushBackDistance, num + 0.001f);
		if (RaycastIgnoreTag(ray, out var hitInfo, pushBackDistance))
		{
			float distance = hitInfo.distance - 0.001f;
			vector = ray.GetPoint(distance);
			extra.AddPointToDebugPath(vector);
			if (iterations > 1)
			{
				vector = PushCameraBack(vector, outDir, hitInfo, lookAtPos, startPlane, targetDistance, iterations - 1, ref extra);
			}
			return vector;
		}
		vector = ray.GetPoint(pushBackDistance);
		outDir = vector - lookAtPos;
		float magnitude = outDir.magnitude;
		if (magnitude < 0.0001f || RaycastIgnoreTag(new Ray(lookAtPos, outDir), out var _, magnitude - 0.001f))
		{
			return currentPos;
		}
		ray = new Ray(vector, outDir);
		extra.AddPointToDebugPath(vector);
		pushBackDistance = GetPushBackDistance(ray, startPlane, targetDistance, lookAtPos);
		if (pushBackDistance > 0.0001f)
		{
			if (!RaycastIgnoreTag(ray, out hitInfo, pushBackDistance))
			{
				vector = ray.GetPoint(pushBackDistance);
				extra.AddPointToDebugPath(vector);
			}
			else
			{
				float distance2 = hitInfo.distance - 0.001f;
				vector = ray.GetPoint(distance2);
				extra.AddPointToDebugPath(vector);
				if (iterations > 1)
				{
					vector = PushCameraBack(vector, outDir, hitInfo, lookAtPos, startPlane, targetDistance, iterations - 1, ref extra);
				}
			}
		}
		return vector;
	}

	private bool GetWalkingDirection(Vector3 pos, Vector3 pushDir, RaycastHit obstacle, ref Vector3 outDir)
	{
		Vector3 normal = obstacle.normal;
		float num = 0.0050000004f;
		int num2 = Physics.SphereCastNonAlloc(pos, num, pushDir.normalized, m_CornerBuffer, 0f, m_CollideAgainst.value, QueryTriggerInteraction.Ignore);
		if (num2 > 1)
		{
			for (int i = 0; i < num2; i++)
			{
				if (m_IgnoreTag.Length > 0 && m_CornerBuffer[i].collider.CompareTag(m_IgnoreTag))
				{
					continue;
				}
				Type type = m_CornerBuffer[i].collider.GetType();
				if (!(type == typeof(BoxCollider)) && !(type == typeof(SphereCollider)) && !(type == typeof(CapsuleCollider)))
				{
					continue;
				}
				Vector3 direction = m_CornerBuffer[i].collider.ClosestPoint(pos) - pos;
				if (direction.magnitude > 1E-05f && m_CornerBuffer[i].collider.Raycast(new Ray(pos, direction), out m_CornerBuffer[i], num))
				{
					if (!(m_CornerBuffer[i].normal - obstacle.normal).AlmostZero())
					{
						normal = m_CornerBuffer[i].normal;
					}
					break;
				}
			}
		}
		Vector3 vector = Vector3.Cross(obstacle.normal, normal);
		if (vector.AlmostZero())
		{
			vector = Vector3.ProjectOnPlane(pushDir, obstacle.normal);
		}
		else
		{
			float num3 = Vector3.Dot(vector, pushDir);
			if (Mathf.Abs(num3) < 0.0001f)
			{
				return false;
			}
			if (num3 < 0f)
			{
				vector = -vector;
			}
		}
		if (vector.AlmostZero())
		{
			return false;
		}
		outDir = vector.normalized;
		return true;
	}

	private float GetPushBackDistance(Ray ray, Plane startPlane, float targetDistance, Vector3 lookAtPos)
	{
		float num = targetDistance - (ray.origin - lookAtPos).magnitude;
		if (num < 0.0001f)
		{
			return 0f;
		}
		if (m_Strategy == ResolutionStrategy.PreserveCameraDistance)
		{
			return num;
		}
		if (!startPlane.Raycast(ray, out var enter))
		{
			enter = 0f;
		}
		enter = Mathf.Min(num, enter);
		if (enter < 0.0001f)
		{
			return 0f;
		}
		float num2 = Mathf.Abs(Vector3.Angle(startPlane.normal, ray.direction) - 90f);
		if (num2 < 0.1f)
		{
			enter = Mathf.Lerp(0f, enter, num2 / 0.1f);
		}
		return enter;
	}

	private float ClampRayToBounds(Ray ray, float distance, Bounds bounds)
	{
		float enter;
		if (Vector3.Dot(ray.direction, Vector3.up) > 0f)
		{
			if (new Plane(Vector3.down, bounds.max).Raycast(ray, out enter) && enter > 0.0001f)
			{
				distance = Mathf.Min(distance, enter);
			}
		}
		else if (Vector3.Dot(ray.direction, Vector3.down) > 0f && new Plane(Vector3.up, bounds.min).Raycast(ray, out enter) && enter > 0.0001f)
		{
			distance = Mathf.Min(distance, enter);
		}
		if (Vector3.Dot(ray.direction, Vector3.right) > 0f)
		{
			if (new Plane(Vector3.left, bounds.max).Raycast(ray, out enter) && enter > 0.0001f)
			{
				distance = Mathf.Min(distance, enter);
			}
		}
		else if (Vector3.Dot(ray.direction, Vector3.left) > 0f && new Plane(Vector3.right, bounds.min).Raycast(ray, out enter) && enter > 0.0001f)
		{
			distance = Mathf.Min(distance, enter);
		}
		if (Vector3.Dot(ray.direction, Vector3.forward) > 0f)
		{
			if (new Plane(Vector3.back, bounds.max).Raycast(ray, out enter) && enter > 0.0001f)
			{
				distance = Mathf.Min(distance, enter);
			}
		}
		else if (Vector3.Dot(ray.direction, Vector3.back) > 0f && new Plane(Vector3.forward, bounds.min).Raycast(ray, out enter) && enter > 0.0001f)
		{
			distance = Mathf.Min(distance, enter);
		}
		return distance;
	}

	private Vector3 RespectCameraRadius(Vector3 cameraPos, Vector3 lookAtPos)
	{
		Vector3 zero = Vector3.zero;
		int num = Physics.OverlapSphereNonAlloc(cameraPos, m_CameraRadius, mColliderBuffer, m_CollideAgainst, QueryTriggerInteraction.Ignore);
		if (num > 0)
		{
			if (mCameraColliderGameObject == null)
			{
				mCameraColliderGameObject = new GameObject("Cinemachine Collider Collider");
				mCameraColliderGameObject.hideFlags = HideFlags.HideAndDontSave;
				mCameraColliderGameObject.transform.position = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
				mCameraColliderGameObject.SetActive(value: true);
				mCameraCollider = mCameraColliderGameObject.AddComponent<SphereCollider>();
			}
			mCameraCollider.radius = m_CameraRadius;
			for (int i = 0; i < num; i++)
			{
				Collider collider = mColliderBuffer[i];
				if ((m_IgnoreTag.Length <= 0 || !collider.CompareTag(m_IgnoreTag)) && Physics.ComputePenetration(mCameraCollider, cameraPos, Quaternion.identity, collider, collider.transform.position, collider.transform.rotation, out var direction, out var distance))
				{
					zero += direction * distance;
				}
			}
		}
		return zero;
	}

	private void CleanupCameraCollider()
	{
		if (mCameraColliderGameObject != null)
		{
			UnityEngine.Object.DestroyImmediate(mCameraColliderGameObject);
		}
		mCameraColliderGameObject = null;
		mCameraCollider = null;
	}

	private bool CheckForTargetObstructions(CameraState state)
	{
		if (state.HasLookAt)
		{
			Vector3 referenceLookAt = state.ReferenceLookAt;
			Vector3 correctedPosition = state.CorrectedPosition;
			Vector3 vector = referenceLookAt - correctedPosition;
			float magnitude = vector.magnitude;
			if (magnitude < Mathf.Max(m_MinimumDistanceFromTarget, 0.0001f))
			{
				return true;
			}
			Ray ray = new Ray(correctedPosition, vector.normalized);
			if (RaycastIgnoreTag(ray, out var _, magnitude - m_MinimumDistanceFromTarget))
			{
				return true;
			}
		}
		return false;
	}
}
