using System;
using Cinemachine.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cinemachine;

[DocumentationSorting(6f, DocumentationSortingAttribute.Level.UserRef)]
[AddComponentMenu("")]
[RequireComponent(typeof(CinemachinePipeline))]
[SaveDuringPlay]
public class CinemachineOrbitalTransposer : CinemachineTransposer
{
	[Serializable]
	[DocumentationSorting(6.2f, DocumentationSortingAttribute.Level.UserRef)]
	public struct Heading(Heading.HeadingDefinition def, int filterStrength, float bias)
	{
		[DocumentationSorting(6.21f, DocumentationSortingAttribute.Level.UserRef)]
		public enum HeadingDefinition
		{
			PositionDelta,
			Velocity,
			TargetForward,
			WorldForward
		}

		[Tooltip("How 'forward' is defined.  The camera will be placed by default behind the target.  PositionDelta will consider 'forward' to be the direction in which the target is moving.")]
		public HeadingDefinition m_HeadingDefinition = def;

		[Range(0f, 10f)]
		[Tooltip("Size of the velocity sampling window for target heading filter.  This filters out irregularities in the target's movement.  Used only if deriving heading from target's movement (PositionDelta or Velocity)")]
		public int m_VelocityFilterStrength = filterStrength;

		[Range(-180f, 180f)]
		[Tooltip("Where the camera is placed when the X-axis value is zero.  This is a rotation in degrees around the Y axis.  When this value is 0, the camera will be placed behind the target.  Nonzero offsets will rotate the zero position around the target.")]
		public float m_HeadingBias = bias;
	}

	[Serializable]
	[DocumentationSorting(6.5f, DocumentationSortingAttribute.Level.UserRef)]
	public struct Recentering
	{
		[Tooltip("If checked, will enable automatic recentering of the camera based on the heading definition. If unchecked, recenting is disabled.")]
		public bool m_enabled;

		[Tooltip("If no input has been detected, the camera will wait this long in seconds before moving its heading to the zero position.")]
		public float m_RecenterWaitTime;

		[Tooltip("Maximum angular speed of recentering.  Will accelerate into and decelerate out of this.")]
		public float m_RecenteringTime;

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("m_HeadingDefinition")]
		private int m_LegacyHeadingDefinition;

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("m_VelocityFilterStrength")]
		private int m_LegacyVelocityFilterStrength;

		public Recentering(bool enabled, float recenterWaitTime, float recenteringSpeed)
		{
			m_enabled = enabled;
			m_RecenterWaitTime = recenterWaitTime;
			m_RecenteringTime = recenteringSpeed;
			m_LegacyHeadingDefinition = (m_LegacyVelocityFilterStrength = -1);
		}

		public void Validate()
		{
			m_RecenterWaitTime = Mathf.Max(0f, m_RecenterWaitTime);
			m_RecenteringTime = Mathf.Max(0f, m_RecenteringTime);
		}

		internal bool LegacyUpgrade(ref Heading.HeadingDefinition heading, ref int velocityFilter)
		{
			if (m_LegacyHeadingDefinition != -1 && m_LegacyVelocityFilterStrength != -1)
			{
				heading = (Heading.HeadingDefinition)m_LegacyHeadingDefinition;
				velocityFilter = m_LegacyVelocityFilterStrength;
				m_LegacyHeadingDefinition = (m_LegacyVelocityFilterStrength = -1);
				return true;
			}
			return false;
		}
	}

	private class HeadingTracker
	{
		private struct Item
		{
			public Vector3 velocity;

			public float weight;

			public float time;
		}

		private Item[] mHistory;

		private int mTop;

		private int mBottom;

		private int mCount;

		private Vector3 mHeadingSum;

		private float mWeightSum;

		private float mWeightTime;

		private Vector3 mLastGoodHeading = Vector3.zero;

		private static float mDecayExponent;

		public int FilterSize => mHistory.Length;

		public HeadingTracker(int filterSize)
		{
			mHistory = new Item[filterSize];
			float num = (float)filterSize / 5f;
			mDecayExponent = (0f - Mathf.Log(2f)) / num;
			ClearHistory();
		}

		private void ClearHistory()
		{
			mTop = (mBottom = (mCount = 0));
			mWeightSum = 0f;
			mHeadingSum = Vector3.zero;
		}

		private static float Decay(float time)
		{
			return Mathf.Exp(time * mDecayExponent);
		}

		public void Add(Vector3 velocity)
		{
			if (FilterSize == 0)
			{
				mLastGoodHeading = velocity;
				return;
			}
			float magnitude = velocity.magnitude;
			if (magnitude > 0.0001f)
			{
				Item item = new Item
				{
					velocity = velocity,
					weight = magnitude,
					time = Time.time
				};
				if (mCount == FilterSize)
				{
					PopBottom();
				}
				mCount++;
				mHistory[mTop] = item;
				if (++mTop == FilterSize)
				{
					mTop = 0;
				}
				mWeightSum *= Decay(item.time - mWeightTime);
				mWeightTime = item.time;
				mWeightSum += magnitude;
				mHeadingSum += item.velocity;
			}
		}

		private void PopBottom()
		{
			if (mCount > 0)
			{
				float time = Time.time;
				Item item = mHistory[mBottom];
				if (++mBottom == FilterSize)
				{
					mBottom = 0;
				}
				mCount--;
				float num = Decay(time - item.time);
				mWeightSum -= item.weight * num;
				mHeadingSum -= item.velocity * num;
				if (mWeightSum <= 0.0001f || mCount == 0)
				{
					ClearHistory();
				}
			}
		}

		public void DecayHistory()
		{
			float time = Time.time;
			float num = Decay(time - mWeightTime);
			mWeightSum *= num;
			mWeightTime = time;
			if (mWeightSum < 0.0001f)
			{
				ClearHistory();
			}
			else
			{
				mHeadingSum *= num;
			}
		}

		public Vector3 GetReliableHeading()
		{
			if (mWeightSum > 0.0001f && (mCount == mHistory.Length || mLastGoodHeading.AlmostZero()))
			{
				Vector3 v = mHeadingSum / mWeightSum;
				if (!v.AlmostZero())
				{
					mLastGoodHeading = v.normalized;
				}
			}
			return mLastGoodHeading;
		}
	}

	[Space]
	[Tooltip("The definition of Forward.  Camera will follow behind.")]
	public Heading m_Heading = new Heading(Heading.HeadingDefinition.TargetForward, 4, 0f);

	[Tooltip("Automatic heading recentering.  The settings here defines how the camera will reposition itself in the absence of player input.")]
	public Recentering m_RecenterToTargetHeading = new Recentering(enabled: true, 1f, 2f);

	[Tooltip("Heading Control.  The settings here control the behaviour of the camera in response to the player's input.")]
	public AxisState m_XAxis = new AxisState(300f, 2f, 1f, 0f, "Mouse X", invert: true);

	[SerializeField]
	[HideInInspector]
	[FormerlySerializedAs("m_Radius")]
	private float m_LegacyRadius = float.MaxValue;

	[SerializeField]
	[HideInInspector]
	[FormerlySerializedAs("m_HeightOffset")]
	private float m_LegacyHeightOffset = float.MaxValue;

	[SerializeField]
	[HideInInspector]
	[FormerlySerializedAs("m_HeadingBias")]
	private float m_LegacyHeadingBias = float.MaxValue;

	[HideInInspector]
	[NoSaveDuringPlay]
	public bool m_HeadingIsSlave;

	private float mLastHeadingAxisInputTime;

	private float mHeadingRecenteringVelocity;

	private Vector3 mLastTargetPosition = Vector3.zero;

	private HeadingTracker mHeadingTracker;

	private Rigidbody mTargetRigidBody;

	private Quaternion mHeadingPrevFrame = Quaternion.identity;

	private Vector3 mOffsetPrevFrame = Vector3.zero;

	private Transform PreviousTarget { get; set; }

	protected override void OnValidate()
	{
		if (m_LegacyRadius != float.MaxValue && m_LegacyHeightOffset != float.MaxValue && m_LegacyHeadingBias != float.MaxValue)
		{
			m_FollowOffset = new Vector3(0f, m_LegacyHeightOffset, 0f - m_LegacyRadius);
			m_LegacyHeightOffset = (m_LegacyRadius = float.MaxValue);
			m_Heading.m_HeadingBias = m_LegacyHeadingBias;
			m_XAxis.m_MaxSpeed /= 10f;
			m_XAxis.m_AccelTime /= 10f;
			m_XAxis.m_DecelTime /= 10f;
			m_LegacyHeadingBias = float.MaxValue;
			m_RecenterToTargetHeading.LegacyUpgrade(ref m_Heading.m_HeadingDefinition, ref m_Heading.m_VelocityFilterStrength);
		}
		m_XAxis.Validate();
		m_RecenterToTargetHeading.Validate();
		base.OnValidate();
	}

	public void UpdateHeading(float deltaTime, Vector3 up)
	{
		if ((deltaTime >= 0f || CinemachineCore.Instance.IsLive(VirtualCamera)) && m_XAxis.Update(deltaTime))
		{
			mLastHeadingAxisInputTime = Time.time;
			mHeadingRecenteringVelocity = 0f;
		}
		float targetHeading = GetTargetHeading(m_XAxis.Value, GetReferenceOrientation(up), deltaTime);
		if (deltaTime < 0f)
		{
			mHeadingRecenteringVelocity = 0f;
			if (m_RecenterToTargetHeading.m_enabled)
			{
				m_XAxis.Value = targetHeading;
			}
		}
		else
		{
			if (m_BindingMode == BindingMode.SimpleFollowWithWorldUp || !m_RecenterToTargetHeading.m_enabled || !(Time.time > mLastHeadingAxisInputTime + m_RecenterToTargetHeading.m_RecenterWaitTime))
			{
				return;
			}
			float num = m_RecenterToTargetHeading.m_RecenteringTime / 3f;
			if (num <= deltaTime)
			{
				m_XAxis.Value = targetHeading;
				return;
			}
			float f = Mathf.DeltaAngle(m_XAxis.Value, targetHeading);
			float num2 = Mathf.Abs(f);
			if (num2 < 0.0001f)
			{
				m_XAxis.Value = targetHeading;
				mHeadingRecenteringVelocity = 0f;
				return;
			}
			float num3 = deltaTime / num;
			float num4 = Mathf.Sign(f) * Mathf.Min(num2, num2 * num3);
			float num5 = num4 - mHeadingRecenteringVelocity;
			if ((num4 < 0f && num5 < 0f) || (num4 > 0f && num5 > 0f))
			{
				num4 = mHeadingRecenteringVelocity + num4 * num3;
			}
			m_XAxis.Value += num4;
			mHeadingRecenteringVelocity = num4;
		}
	}

	private void OnVlaidate()
	{
		m_XAxis.Validate();
	}

	private void OnEnable()
	{
		m_XAxis.SetThresholds(0f, 360f, wrapAround: true);
		PreviousTarget = null;
		mLastTargetPosition = Vector3.zero;
	}

	public override void MutateCameraState(ref CameraState curState, float deltaTime)
	{
		InitPrevFrameStateInfo(ref curState, deltaTime);
		if (FollowTarget != PreviousTarget)
		{
			PreviousTarget = FollowTarget;
			mTargetRigidBody = ((PreviousTarget == null) ? null : PreviousTarget.GetComponent<Rigidbody>());
			mLastTargetPosition = ((PreviousTarget == null) ? Vector3.zero : PreviousTarget.position);
			mHeadingTracker = null;
		}
		if (!m_HeadingIsSlave)
		{
			UpdateHeading(deltaTime, curState.ReferenceUp);
		}
		if (IsValid)
		{
			mLastTargetPosition = FollowTarget.position;
			float num = m_XAxis.Value;
			if (m_BindingMode == BindingMode.SimpleFollowWithWorldUp)
			{
				m_XAxis.Value = 0f;
			}
			else
			{
				num += m_Heading.m_HeadingBias;
			}
			Quaternion quaternion = Quaternion.AngleAxis(num, curState.ReferenceUp);
			Vector3 effectiveOffset = EffectiveOffset;
			TrackTarget(deltaTime, curState.ReferenceUp, quaternion * effectiveOffset, out var outTargetPosition, out var outTargetOrient);
			curState.ReferenceUp = outTargetOrient * Vector3.up;
			if (deltaTime >= 0f)
			{
				Vector3 vector = quaternion * effectiveOffset - mHeadingPrevFrame * mOffsetPrevFrame;
				vector = outTargetOrient * vector;
				curState.PositionDampingBypass = vector;
			}
			outTargetOrient *= quaternion;
			curState.RawPosition = outTargetPosition + outTargetOrient * effectiveOffset;
			mHeadingPrevFrame = ((m_BindingMode == BindingMode.SimpleFollowWithWorldUp) ? Quaternion.identity : quaternion);
			mOffsetPrevFrame = effectiveOffset;
		}
	}

	public override void OnPositionDragged(Vector3 delta)
	{
		Vector3 vector = Quaternion.Inverse(GetReferenceOrientation(VcamState.ReferenceUp)) * delta;
		vector.x = 0f;
		m_FollowOffset += vector;
		m_FollowOffset = EffectiveOffset;
	}

	private static string GetFullName(GameObject current)
	{
		if (current == null)
		{
			return "";
		}
		if (current.transform.parent == null)
		{
			return "/" + current.name;
		}
		return GetFullName(current.transform.parent.gameObject) + "/" + current.name;
	}

	private float GetTargetHeading(float currentHeading, Quaternion targetOrientation, float deltaTime)
	{
		if (m_BindingMode == BindingMode.SimpleFollowWithWorldUp)
		{
			return 0f;
		}
		if (FollowTarget == null)
		{
			return currentHeading;
		}
		if (m_Heading.m_HeadingDefinition == Heading.HeadingDefinition.Velocity && mTargetRigidBody == null)
		{
			Debug.Log($"Attempted to use HeadingDerivationMode.Velocity to calculate heading for {GetFullName(VirtualCamera.VirtualCameraGameObject)}. No RigidBody was present on '{FollowTarget}'. Defaulting to position delta");
			m_Heading.m_HeadingDefinition = Heading.HeadingDefinition.PositionDelta;
		}
		Vector3 zero = Vector3.zero;
		switch (m_Heading.m_HeadingDefinition)
		{
		case Heading.HeadingDefinition.PositionDelta:
			zero = FollowTarget.position - mLastTargetPosition;
			break;
		case Heading.HeadingDefinition.Velocity:
			zero = mTargetRigidBody.velocity;
			break;
		case Heading.HeadingDefinition.TargetForward:
			zero = FollowTarget.forward;
			break;
		default:
			return 0f;
		}
		int num = m_Heading.m_VelocityFilterStrength * 5;
		if (mHeadingTracker == null || mHeadingTracker.FilterSize != num)
		{
			mHeadingTracker = new HeadingTracker(num);
		}
		mHeadingTracker.DecayHistory();
		Vector3 vector = targetOrientation * Vector3.up;
		zero = zero.ProjectOntoPlane(vector);
		if (!zero.AlmostZero())
		{
			mHeadingTracker.Add(zero);
		}
		zero = mHeadingTracker.GetReliableHeading();
		if (!zero.AlmostZero())
		{
			return UnityVectorExtensions.SignedAngle(targetOrientation * Vector3.forward, zero, vector);
		}
		return currentHeading;
	}
}
