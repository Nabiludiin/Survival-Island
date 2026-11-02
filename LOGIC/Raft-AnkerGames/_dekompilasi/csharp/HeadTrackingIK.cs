using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Animator))]
public class HeadTrackingIK : MonoBehaviour
{
	[Serializable]
	public class Bone
	{
		public Transform bone;

		[Range(0f, 1f)]
		public float weight;

		public Vector3 upAxis = Vector3.forward;

		public Vector3 offset;
	}

	private const float SnapRotationDuration = 0.3f;

	private const float WeightFadeDuration = 0.4f;

	private const float BlendToValidDuration = 0.5f;

	private const float MinHeadOffset = 0.8f;

	public Transform target;

	public Bone[] bones;

	public float maxTrackAngle = 90f;

	public Transform snapRotationRoot;

	public Vector3 snapRotationForward = Vector3.forward;

	public Vector3 snapRotationUp = Vector3.up;

	public bool allowSnapRotation = true;

	public float weight = 1f;

	private bool startEnabled;

	private bool startAllowSnapRotation;

	private Quaternion startRotation;

	private float recoveredTrackTime = -1f;

	private Vector3 lastTrackPosition;

	private Vector3? lastValidTrackPosition;

	private Vector3? startForwardLookPosition;

	private Quaternion fromRotation;

	private Quaternion toRotation;

	private float rotationTime;

	private bool fadingIn;

	private float fadeTargetTime;

	private void Start()
	{
		snapRotationForward.Normalize();
		snapRotationUp.Normalize();
		if (snapRotationRoot != null)
		{
			fromRotation = snapRotationRoot.localRotation;
			toRotation = snapRotationRoot.localRotation;
		}
	}

	private void LateUpdate()
	{
		Transform transform = target;
		float t = Mathf.Clamp01((fadeTargetTime - Time.time) / 0.4f);
		weight = Mathf.SmoothStep(1f, 0f, t);
		if (!fadingIn)
		{
			weight = 1f - weight;
		}
		if (Application.isPlaying && !fadingIn && weight <= 0f)
		{
			enabled = false;
		}
		if (transform == null)
		{
			return;
		}
		Vector3 vector = transform.position;
		if (!allowSnapRotation)
		{
			vector = AdjustFinalPositionAfterMaxAngle(transform);
		}
		if (allowSnapRotation && snapRotationRoot != null)
		{
			float t2 = Mathf.Clamp01((Time.time - rotationTime) / 0.3f);
			snapRotationRoot.localRotation = Quaternion.Slerp(fromRotation, toRotation, t2);
			Vector3 planeNormal = snapRotationRoot.rotation * snapRotationUp;
			Vector3 normalized = ProjectVectorOnPlane(planeNormal, snapRotationRoot.rotation * snapRotationForward).normalized;
			Vector3 normalized2 = ProjectVectorOnPlane(planeNormal, vector - snapRotationRoot.position).normalized;
			float num = Vector3.SignedAngle(normalized, normalized2, snapRotationRoot.up);
			if (Mathf.Abs(num) > maxTrackAngle)
			{
				fromRotation = snapRotationRoot.localRotation;
				toRotation = snapRotationRoot.localRotation * Quaternion.AngleAxis(num, snapRotationUp);
				rotationTime = Time.time;
			}
		}
		lastTrackPosition = vector;
		for (int i = 0; i < bones.Length; i++)
		{
			if (!(bones[i].bone == null))
			{
				Vector3 vector2 = vector + bones[i].bone.rotation * bones[i].offset;
				Vector3 vector3 = bones[i].bone.rotation * bones[i].upAxis;
				Vector3 vector4 = vector2 - bones[i].bone.position;
				Vector3 axis = Vector3.Cross(vector3, vector4);
				float num2 = Vector3.Angle(vector3, vector4);
				bones[i].bone.Rotate(axis, num2 * bones[i].weight * weight, Space.World);
			}
		}
	}

	public void InitializeStartState()
	{
		startEnabled = enabled;
		startAllowSnapRotation = allowSnapRotation;
		startRotation = snapRotationRoot.localRotation;
	}

	public void ResetToStartState()
	{
		enabled = startEnabled;
		allowSnapRotation = startAllowSnapRotation;
		snapRotationRoot.localRotation = startRotation;
	}

	public void EnableSnapRotation()
	{
		allowSnapRotation = true;
	}

	public void HeadTrackingOn()
	{
		fadingIn = true;
		if (!enabled)
		{
			fadeTargetTime = Time.time + 0.4f;
		}
		enabled = true;
		if (!startForwardLookPosition.HasValue)
		{
			Vector3 vector = bones[bones.Length - 1].bone.rotation * bones[bones.Length - 1].upAxis;
			startForwardLookPosition = bones[bones.Length - 1].bone.position + vector * 2f;
			lastTrackPosition = startForwardLookPosition.Value;
		}
		if (target != null && !IsAngleSolvable(target) && weight <= 0f)
		{
			lastValidTrackPosition = null;
			lastTrackPosition = startForwardLookPosition.Value;
		}
	}

	public void HeadTrackingOff()
	{
		fadingIn = false;
		fadeTargetTime = Time.time + 0.4f;
	}

	private Vector3 AdjustFinalPositionAfterMaxAngle(Transform target)
	{
		if (IsAngleSolvable(target))
		{
			if (lastValidTrackPosition.HasValue)
			{
				if (recoveredTrackTime == -1f)
				{
					recoveredTrackTime = Time.time;
					return lastValidTrackPosition.Value;
				}
				Vector3 position = bones[bones.Length - 1].bone.position;
				float num = Mathf.Clamp01((Time.time - recoveredTrackTime) / 0.5f);
				Vector3 vector = Vector3.Lerp(lastValidTrackPosition.Value, target.position, num);
				if (Vector3.Distance(vector, position) < 0.8f)
				{
					vector = position + (vector - position).normalized * 0.8f;
				}
				if (num >= 1f)
				{
					lastValidTrackPosition = null;
				}
				return vector;
			}
			return target.position;
		}
		if (!lastValidTrackPosition.HasValue)
		{
			recoveredTrackTime = -1f;
			lastValidTrackPosition = lastTrackPosition;
		}
		return lastValidTrackPosition.Value;
	}

	private bool IsAngleSolvable(Transform target)
	{
		int num = bones.Length - 1;
		if (bones[num].bone != null)
		{
			Vector3 vector = bones[num].bone.rotation * bones[num].upAxis;
			Vector3 to = target.position - bones[num].bone.position;
			return Vector3.Angle(vector, to) < maxTrackAngle;
		}
		return false;
	}

	private static Vector3 ProjectVectorOnPlane(Vector3 planeNormal, Vector3 vector)
	{
		return vector - Vector3.Dot(vector, planeNormal) * planeNormal;
	}
}
