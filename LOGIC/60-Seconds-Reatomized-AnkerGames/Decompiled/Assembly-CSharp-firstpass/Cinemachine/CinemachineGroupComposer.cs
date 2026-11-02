using System;
using Cinemachine.Utility;
using UnityEngine;

namespace Cinemachine;

[DocumentationSorting(4f, DocumentationSortingAttribute.Level.UserRef)]
[ExecuteInEditMode]
[AddComponentMenu("")]
[RequireComponent(typeof(CinemachinePipeline))]
[SaveDuringPlay]
public class CinemachineGroupComposer : CinemachineComposer
{
	[DocumentationSorting(4.01f, DocumentationSortingAttribute.Level.UserRef)]
	public enum FramingMode
	{
		Horizontal,
		Vertical,
		HorizontalAndVertical
	}

	public enum AdjustmentMode
	{
		ZoomOnly,
		DollyOnly,
		DollyThenZoom
	}

	[Space]
	[Tooltip("The bounding box of the targets should occupy this amount of the screen space.  1 means fill the whole screen.  0.5 means fill half the screen, etc.")]
	public float m_GroupFramingSize = 0.8f;

	[Tooltip("What screen dimensions to consider when framing.  Can be Horizontal, Vertical, or both")]
	public FramingMode m_FramingMode = FramingMode.HorizontalAndVertical;

	[Range(0f, 20f)]
	[Tooltip("How aggressively the camera tries to frame the group. Small numbers are more responsive, rapidly adjusting the camera to keep the group in the frame.  Larger numbers give a more heavy slowly responding camera.")]
	public float m_FrameDamping = 2f;

	[Tooltip("How to adjust the camera to get the desired framing.  You can zoom, dolly in/out, or do both.")]
	public AdjustmentMode m_AdjustmentMode = AdjustmentMode.DollyThenZoom;

	[Tooltip("The maximum distance toward the target that this behaviour is allowed to move the camera.")]
	public float m_MaxDollyIn = 5000f;

	[Tooltip("The maximum distance away the target that this behaviour is allowed to move the camera.")]
	public float m_MaxDollyOut = 5000f;

	[Tooltip("Set this to limit how close to the target the camera can get.")]
	public float m_MinimumDistance = 1f;

	[Tooltip("Set this to limit how far from the target the camera can get.")]
	public float m_MaximumDistance = 5000f;

	[Range(1f, 179f)]
	[Tooltip("If adjusting FOV, will not set the FOV lower than this.")]
	public float m_MinimumFOV = 3f;

	[Range(1f, 179f)]
	[Tooltip("If adjusting FOV, will not set the FOV higher than this.")]
	public float m_MaximumFOV = 60f;

	[Tooltip("If adjusting Orthographic Size, will not set it lower than this.")]
	public float m_MinimumOrthoSize = 1f;

	[Tooltip("If adjusting Orthographic Size, will not set it higher than this.")]
	public float m_MaximumOrthoSize = 100f;

	private float m_prevTargetHeight;

	public CinemachineTargetGroup TargetGroup
	{
		get
		{
			Transform lookAtTarget = LookAtTarget;
			if (lookAtTarget != null)
			{
				return lookAtTarget.GetComponent<CinemachineTargetGroup>();
			}
			return null;
		}
	}

	public Bounds m_LastBounds { get; private set; }

	public Matrix4x4 m_lastBoundsMatrix { get; private set; }

	private void OnValidate()
	{
		m_GroupFramingSize = Mathf.Max(0.0001f, m_GroupFramingSize);
		m_MaxDollyIn = Mathf.Max(0f, m_MaxDollyIn);
		m_MaxDollyOut = Mathf.Max(0f, m_MaxDollyOut);
		m_MinimumDistance = Mathf.Max(0f, m_MinimumDistance);
		m_MaximumDistance = Mathf.Max(m_MinimumDistance, m_MaximumDistance);
		m_MinimumFOV = Mathf.Max(1f, m_MinimumFOV);
		m_MaximumFOV = Mathf.Clamp(m_MaximumFOV, m_MinimumFOV, 179f);
		m_MinimumOrthoSize = Mathf.Max(0.01f, m_MinimumOrthoSize);
		m_MaximumOrthoSize = Mathf.Max(m_MinimumOrthoSize, m_MaximumOrthoSize);
	}

	public override void MutateCameraState(ref CameraState curState, float deltaTime)
	{
		CinemachineTargetGroup targetGroup = TargetGroup;
		if (targetGroup == null)
		{
			base.MutateCameraState(ref curState, deltaTime);
			return;
		}
		if (!IsValid || !curState.HasLookAt)
		{
			m_prevTargetHeight = 0f;
			return;
		}
		curState.ReferenceLookAt = GetLookAtPointAndSetTrackedPoint(targetGroup.transform.position);
		Vector3 v = TrackedPoint - curState.RawPosition;
		float magnitude = v.magnitude;
		if (magnitude < 0.0001f)
		{
			return;
		}
		Vector3 vector = (v.AlmostZero() ? Vector3.forward : v.normalized);
		Bounds boundingBox = targetGroup.BoundingBox;
		m_lastBoundsMatrix = Matrix4x4.TRS(boundingBox.center - vector * boundingBox.extents.magnitude, Quaternion.LookRotation(vector, curState.ReferenceUp), Vector3.one);
		m_LastBounds = targetGroup.GetViewSpaceBoundingBox(m_lastBoundsMatrix);
		float num = GetTargetHeight(m_LastBounds);
		Vector3 vector2 = m_lastBoundsMatrix.MultiplyPoint3x4(m_LastBounds.center);
		if (deltaTime >= 0f)
		{
			float initial = num - m_prevTargetHeight;
			initial = Damper.Damp(initial, m_FrameDamping, deltaTime);
			num = m_prevTargetHeight + initial;
		}
		m_prevTargetHeight = num;
		if (!curState.Lens.Orthographic && m_AdjustmentMode != AdjustmentMode.ZoomOnly)
		{
			float fieldOfView = curState.Lens.FieldOfView;
			float value = num / (2f * Mathf.Tan(fieldOfView * ((float)Math.PI / 180f) / 2f)) + m_LastBounds.extents.z;
			value = Mathf.Clamp(value, magnitude - m_MaxDollyIn, magnitude + m_MaxDollyOut);
			value = Mathf.Clamp(value, m_MinimumDistance, m_MaximumDistance);
			curState.PositionCorrection += vector2 - vector * value - curState.RawPosition;
		}
		if (curState.Lens.Orthographic || m_AdjustmentMode != AdjustmentMode.DollyOnly)
		{
			float num2 = (TrackedPoint - curState.CorrectedPosition).magnitude - m_LastBounds.extents.z;
			float value2 = 179f;
			if (num2 > 0.0001f)
			{
				value2 = 2f * Mathf.Atan(num / (2f * num2)) * 57.29578f;
			}
			LensSettings lens = curState.Lens;
			lens.FieldOfView = Mathf.Clamp(value2, m_MinimumFOV, m_MaximumFOV);
			lens.OrthographicSize = Mathf.Clamp(num / 2f, m_MinimumOrthoSize, m_MaximumOrthoSize);
			curState.Lens = lens;
		}
		base.MutateCameraState(ref curState, deltaTime);
	}

	private float GetTargetHeight(Bounds b)
	{
		float num = Mathf.Max(0.0001f, m_GroupFramingSize);
		return m_FramingMode switch
		{
			FramingMode.Horizontal => Mathf.Max(0.0001f, b.size.x) / (num * VcamState.Lens.Aspect), 
			FramingMode.Vertical => Mathf.Max(0.0001f, b.size.y) / num, 
			_ => Mathf.Max(Mathf.Max(0.0001f, b.size.x) / (num * VcamState.Lens.Aspect), Mathf.Max(0.0001f, b.size.y) / num), 
		};
	}
}
