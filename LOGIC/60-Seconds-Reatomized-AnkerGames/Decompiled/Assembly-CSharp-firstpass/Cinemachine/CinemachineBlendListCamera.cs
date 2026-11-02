using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cinemachine;

[DocumentationSorting(13f, DocumentationSortingAttribute.Level.UserRef)]
[ExecuteInEditMode]
[DisallowMultipleComponent]
[AddComponentMenu("Cinemachine/CinemachineBlendListCamera")]
public class CinemachineBlendListCamera : CinemachineVirtualCameraBase
{
	[Serializable]
	public struct Instruction
	{
		[Tooltip("The virtual camera to activate when this instruction becomes active")]
		public CinemachineVirtualCameraBase m_VirtualCamera;

		[Tooltip("How long to wait (in seconds) before activating the next virtual camera in the list (if any)")]
		public float m_Hold;

		[CinemachineBlendDefinitionProperty]
		[Tooltip("How to blend to the next virtual camera in the list (if any)")]
		public CinemachineBlendDefinition m_Blend;
	}

	[Tooltip("Default object for the camera children to look at (the aim target), if not specified in a child camera.  May be empty if all of the children define targets of their own.")]
	[NoSaveDuringPlay]
	public Transform m_LookAt;

	[Tooltip("Default object for the camera children wants to move with (the body target), if not specified in a child camera.  May be empty if all of the children define targets of their own.")]
	[NoSaveDuringPlay]
	public Transform m_Follow;

	[Tooltip("When enabled, the current child camera and blend will be indicated in the game window, for debugging")]
	public bool m_ShowDebugText;

	[Tooltip("Force all child cameras to be enabled.  This is useful if animating them in Timeline, but consumes extra resources")]
	public bool m_EnableAllChildCameras;

	[SerializeField]
	[HideInInspector]
	[NoSaveDuringPlay]
	public CinemachineVirtualCameraBase[] m_ChildCameras;

	[Tooltip("The set of instructions for enabling child cameras.")]
	public Instruction[] m_Instructions;

	private CameraState m_State = CameraState.Default;

	private float mActivationTime = -1f;

	private int mCurrentInstruction;

	private CinemachineBlend mActiveBlend;

	public override string Description
	{
		get
		{
			ICinemachineCamera liveChild = LiveChild;
			if (mActiveBlend == null)
			{
				if (liveChild == null)
				{
					return "(none)";
				}
				return "[" + liveChild.Name + "]";
			}
			return mActiveBlend.Description;
		}
	}

	public ICinemachineCamera LiveChild { get; set; }

	public override ICinemachineCamera LiveChildOrSelf => LiveChild;

	public override CameraState State => m_State;

	public override Transform LookAt
	{
		get
		{
			return ResolveLookAt(m_LookAt);
		}
		set
		{
			m_LookAt = value;
		}
	}

	public override Transform Follow
	{
		get
		{
			return ResolveFollow(m_Follow);
		}
		set
		{
			m_Follow = value;
		}
	}

	public CinemachineVirtualCameraBase[] ChildCameras
	{
		get
		{
			UpdateListOfChildren();
			return m_ChildCameras;
		}
	}

	public bool IsBlending => mActiveBlend != null;

	public override bool IsLiveChild(ICinemachineCamera vcam)
	{
		if (vcam != LiveChild)
		{
			if (mActiveBlend != null)
			{
				if (vcam != mActiveBlend.CamA)
				{
					return vcam == mActiveBlend.CamB;
				}
				return true;
			}
			return false;
		}
		return true;
	}

	public override void RemovePostPipelineStageHook(OnPostPipelineStageDelegate d)
	{
		base.RemovePostPipelineStageHook(d);
		UpdateListOfChildren();
		CinemachineVirtualCameraBase[] childCameras = m_ChildCameras;
		for (int i = 0; i < childCameras.Length; i++)
		{
			childCameras[i].RemovePostPipelineStageHook(d);
		}
	}

	public override void OnTransitionFromCamera(ICinemachineCamera fromCam, Vector3 worldUp, float deltaTime)
	{
		base.OnTransitionFromCamera(fromCam, worldUp, deltaTime);
		mActivationTime = Time.time;
		mCurrentInstruction = -1;
		LiveChild = null;
		mActiveBlend = null;
		UpdateCameraState(worldUp, deltaTime);
	}

	public override void UpdateCameraState(Vector3 worldUp, float deltaTime)
	{
		if (!PreviousStateIsValid)
		{
			deltaTime = -1f;
		}
		UpdateListOfChildren();
		AdvanceCurrentInstruction();
		CinemachineVirtualCameraBase cinemachineVirtualCameraBase = null;
		if (mCurrentInstruction >= 0 && mCurrentInstruction < m_Instructions.Length)
		{
			cinemachineVirtualCameraBase = m_Instructions[mCurrentInstruction].m_VirtualCamera;
		}
		if (m_ChildCameras != null)
		{
			for (int i = 0; i < m_ChildCameras.Length; i++)
			{
				CinemachineVirtualCameraBase cinemachineVirtualCameraBase2 = m_ChildCameras[i];
				if (!(cinemachineVirtualCameraBase2 != null))
				{
					continue;
				}
				bool flag = m_EnableAllChildCameras || cinemachineVirtualCameraBase2 == cinemachineVirtualCameraBase;
				if (flag != cinemachineVirtualCameraBase2.VirtualCameraGameObject.activeInHierarchy)
				{
					cinemachineVirtualCameraBase2.gameObject.SetActive(flag);
					if (flag)
					{
						CinemachineCore.Instance.UpdateVirtualCamera(cinemachineVirtualCameraBase2, worldUp, deltaTime);
					}
				}
			}
		}
		if (cinemachineVirtualCameraBase != null)
		{
			ICinemachineCamera liveChild = LiveChild;
			LiveChild = cinemachineVirtualCameraBase;
			if (liveChild != null && LiveChild != null && liveChild != LiveChild && mCurrentInstruction > 0)
			{
				mActiveBlend = CreateBlend(liveChild, LiveChild, m_Instructions[mCurrentInstruction].m_Blend.BlendCurve, m_Instructions[mCurrentInstruction].m_Blend.m_Time, mActiveBlend, deltaTime);
				LiveChild.OnTransitionFromCamera(liveChild, worldUp, deltaTime);
				CinemachineCore.Instance.GenerateCameraActivationEvent(LiveChild);
				if (mActiveBlend == null)
				{
					CinemachineCore.Instance.GenerateCameraCutEvent(LiveChild);
				}
			}
		}
		if (mActiveBlend != null)
		{
			mActiveBlend.TimeInBlend += ((deltaTime >= 0f) ? deltaTime : mActiveBlend.Duration);
			if (mActiveBlend.IsComplete)
			{
				mActiveBlend = null;
			}
		}
		if (mActiveBlend != null)
		{
			mActiveBlend.UpdateCameraState(worldUp, deltaTime);
			m_State = mActiveBlend.State;
		}
		else if (LiveChild != null)
		{
			m_State = LiveChild.State;
		}
		PreviousStateIsValid = true;
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		InvalidateListOfChildren();
		mActiveBlend = null;
	}

	public void OnTransformChildrenChanged()
	{
		InvalidateListOfChildren();
	}

	private void InvalidateListOfChildren()
	{
		m_ChildCameras = null;
		LiveChild = null;
	}

	private void UpdateListOfChildren()
	{
		if (m_ChildCameras != null)
		{
			return;
		}
		List<CinemachineVirtualCameraBase> list = new List<CinemachineVirtualCameraBase>();
		CinemachineVirtualCameraBase[] componentsInChildren = GetComponentsInChildren<CinemachineVirtualCameraBase>(includeInactive: true);
		foreach (CinemachineVirtualCameraBase cinemachineVirtualCameraBase in componentsInChildren)
		{
			if (cinemachineVirtualCameraBase.transform.parent == transform)
			{
				list.Add(cinemachineVirtualCameraBase);
			}
		}
		m_ChildCameras = list.ToArray();
		ValidateInstructions();
	}

	public void ValidateInstructions()
	{
		if (m_Instructions == null)
		{
			m_Instructions = new Instruction[0];
		}
		for (int i = 0; i < m_Instructions.Length; i++)
		{
			if (m_Instructions[i].m_VirtualCamera != null && m_Instructions[i].m_VirtualCamera.transform.parent != transform)
			{
				m_Instructions[i].m_VirtualCamera = null;
			}
		}
		mActiveBlend = null;
	}

	private void AdvanceCurrentInstruction()
	{
		if (m_ChildCameras == null || m_ChildCameras.Length == 0 || mActivationTime < 0f || m_Instructions.Length == 0)
		{
			mActivationTime = -1f;
			mCurrentInstruction = -1;
			mActiveBlend = null;
			return;
		}
		if (mCurrentInstruction >= m_Instructions.Length - 1)
		{
			mCurrentInstruction = m_Instructions.Length - 1;
			return;
		}
		float time = Time.time;
		if (mCurrentInstruction < 0)
		{
			mActivationTime = time;
			mCurrentInstruction = 0;
		}
		else if (time - mActivationTime > Mathf.Max(0f, m_Instructions[mCurrentInstruction].m_Hold))
		{
			mActivationTime = time;
			mCurrentInstruction++;
		}
	}

	private CinemachineBlend CreateBlend(ICinemachineCamera camA, ICinemachineCamera camB, AnimationCurve blendCurve, float duration, CinemachineBlend activeBlend, float deltaTime)
	{
		if (blendCurve == null || duration <= 0f || (camA == null && camB == null))
		{
			return null;
		}
		if (camA == null || activeBlend != null)
		{
			camA = new StaticPointVirtualCamera(activeBlend?.State ?? State, (activeBlend != null) ? "Mid-blend" : "(none)");
		}
		return new CinemachineBlend(camA, camB, blendCurve, duration, 0f);
	}
}
