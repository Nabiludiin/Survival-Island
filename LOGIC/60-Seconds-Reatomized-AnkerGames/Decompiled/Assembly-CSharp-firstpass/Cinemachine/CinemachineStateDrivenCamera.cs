using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cinemachine;

[DocumentationSorting(13f, DocumentationSortingAttribute.Level.UserRef)]
[ExecuteInEditMode]
[DisallowMultipleComponent]
[AddComponentMenu("Cinemachine/CinemachineStateDrivenCamera")]
public class CinemachineStateDrivenCamera : CinemachineVirtualCameraBase
{
	[Serializable]
	public struct Instruction
	{
		[Tooltip("The full hash of the animation state")]
		public int m_FullHash;

		[Tooltip("The virtual camera to activate whrn the animation state becomes active")]
		public CinemachineVirtualCameraBase m_VirtualCamera;

		[Tooltip("How long to wait (in seconds) before activating the virtual camera. This filters out very short state durations")]
		public float m_ActivateAfter;

		[Tooltip("The minimum length of time (in seconds) to keep a virtual camera active")]
		public float m_MinDuration;
	}

	[Serializable]
	[DocumentationSorting(13.2f, DocumentationSortingAttribute.Level.Undoc)]
	public struct ParentHash(int h, int p)
	{
		public int m_Hash = h;

		public int m_ParentHash = p;
	}

	[Tooltip("Default object for the camera children to look at (the aim target), if not specified in a child camera.  May be empty if all of the children define targets of their own.")]
	[NoSaveDuringPlay]
	public Transform m_LookAt;

	[Tooltip("Default object for the camera children wants to move with (the body target), if not specified in a child camera.  May be empty if all of the children define targets of their own.")]
	[NoSaveDuringPlay]
	public Transform m_Follow;

	[Space]
	[Tooltip("The state machine whose state changes will drive this camera's choice of active child")]
	public Animator m_AnimatedTarget;

	[Tooltip("Which layer in the target state machine to observe")]
	public int m_LayerIndex;

	[Tooltip("When enabled, the current child camera and blend will be indicated in the game window, for debugging")]
	public bool m_ShowDebugText;

	[Tooltip("Force all child cameras to be enabled.  This is useful if animating them in Timeline, but consumes extra resources")]
	public bool m_EnableAllChildCameras;

	[SerializeField]
	[HideInInspector]
	[NoSaveDuringPlay]
	public CinemachineVirtualCameraBase[] m_ChildCameras;

	[Tooltip("The set of instructions associating virtual cameras with states.  These instructions are used to choose the live child at any given moment")]
	public Instruction[] m_Instructions;

	[CinemachineBlendDefinitionProperty]
	[Tooltip("The blend which is used if you don't explicitly define a blend between two Virtual Camera children")]
	public CinemachineBlendDefinition m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, 0.5f);

	[Tooltip("This is the asset which contains custom settings for specific child blends")]
	public CinemachineBlenderSettings m_CustomBlends;

	[HideInInspector]
	[SerializeField]
	public ParentHash[] m_ParentHash;

	private CameraState m_State = CameraState.Default;

	private float mActivationTime;

	private Instruction mActiveInstruction;

	private float mPendingActivationTime;

	private Instruction mPendingInstruction;

	private CinemachineBlend mActiveBlend;

	private Dictionary<int, int> mInstructionDictionary;

	private Dictionary<int, int> mStateParentLookup;

	private List<AnimatorClipInfo> m_clipInfoList = new List<AnimatorClipInfo>();

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

	public override void UpdateCameraState(Vector3 worldUp, float deltaTime)
	{
		if (!PreviousStateIsValid)
		{
			deltaTime = -1f;
		}
		UpdateListOfChildren();
		CinemachineVirtualCameraBase cinemachineVirtualCameraBase = ChooseCurrentCamera(deltaTime);
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
		ICinemachineCamera liveChild = LiveChild;
		LiveChild = cinemachineVirtualCameraBase;
		if (liveChild != null && LiveChild != null && liveChild != LiveChild)
		{
			float duration = 0f;
			AnimationCurve blendCurve = LookupBlendCurve(liveChild, LiveChild, out duration);
			mActiveBlend = CreateBlend(liveChild, LiveChild, blendCurve, duration, mActiveBlend, deltaTime);
			LiveChild.OnTransitionFromCamera(liveChild, worldUp, deltaTime);
			CinemachineCore.Instance.GenerateCameraActivationEvent(LiveChild);
			if (mActiveBlend == null)
			{
				CinemachineCore.Instance.GenerateCameraCutEvent(LiveChild);
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

	public static string CreateFakeHashName(int parentHash, string stateName)
	{
		return parentHash + "_" + stateName;
	}

	private void InvalidateListOfChildren()
	{
		m_ChildCameras = null;
		LiveChild = null;
	}

	private void UpdateListOfChildren()
	{
		if (m_ChildCameras != null && mInstructionDictionary != null && mStateParentLookup != null)
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
		mInstructionDictionary = new Dictionary<int, int>();
		for (int i = 0; i < m_Instructions.Length; i++)
		{
			if (m_Instructions[i].m_VirtualCamera != null && m_Instructions[i].m_VirtualCamera.transform.parent != transform)
			{
				m_Instructions[i].m_VirtualCamera = null;
			}
			mInstructionDictionary[m_Instructions[i].m_FullHash] = i;
		}
		mStateParentLookup = new Dictionary<int, int>();
		if (m_ParentHash != null)
		{
			ParentHash[] parentHash = m_ParentHash;
			for (int j = 0; j < parentHash.Length; j++)
			{
				ParentHash parentHash2 = parentHash[j];
				mStateParentLookup[parentHash2.m_Hash] = parentHash2.m_ParentHash;
			}
		}
		mActivationTime = (mPendingActivationTime = 0f);
		mActiveBlend = null;
	}

	private CinemachineVirtualCameraBase ChooseCurrentCamera(float deltaTime)
	{
		if (m_ChildCameras == null || m_ChildCameras.Length == 0)
		{
			mActivationTime = 0f;
			return null;
		}
		CinemachineVirtualCameraBase cinemachineVirtualCameraBase = m_ChildCameras[0];
		if (m_AnimatedTarget == null || !m_AnimatedTarget.gameObject.activeSelf || m_AnimatedTarget.runtimeAnimatorController == null || m_LayerIndex < 0 || m_LayerIndex >= m_AnimatedTarget.layerCount)
		{
			mActivationTime = 0f;
			return cinemachineVirtualCameraBase;
		}
		int num;
		if (m_AnimatedTarget.IsInTransition(m_LayerIndex))
		{
			AnimatorStateInfo nextAnimatorStateInfo = m_AnimatedTarget.GetNextAnimatorStateInfo(m_LayerIndex);
			num = nextAnimatorStateInfo.fullPathHash;
			if (m_AnimatedTarget.GetNextAnimatorClipInfoCount(m_LayerIndex) > 1)
			{
				m_AnimatedTarget.GetNextAnimatorClipInfo(m_LayerIndex, m_clipInfoList);
				num = GetClipHash(nextAnimatorStateInfo.fullPathHash, m_clipInfoList);
			}
		}
		else
		{
			AnimatorStateInfo currentAnimatorStateInfo = m_AnimatedTarget.GetCurrentAnimatorStateInfo(m_LayerIndex);
			num = currentAnimatorStateInfo.fullPathHash;
			if (m_AnimatedTarget.GetCurrentAnimatorClipInfoCount(m_LayerIndex) > 1)
			{
				m_AnimatedTarget.GetCurrentAnimatorClipInfo(m_LayerIndex, m_clipInfoList);
				num = GetClipHash(currentAnimatorStateInfo.fullPathHash, m_clipInfoList);
			}
		}
		while (num != 0 && !mInstructionDictionary.ContainsKey(num))
		{
			num = (mStateParentLookup.ContainsKey(num) ? mStateParentLookup[num] : 0);
		}
		float time = Time.time;
		if (mActivationTime != 0f)
		{
			if (mActiveInstruction.m_FullHash == num)
			{
				mPendingActivationTime = 0f;
				return mActiveInstruction.m_VirtualCamera;
			}
			if (deltaTime >= 0f && mPendingActivationTime != 0f && mPendingInstruction.m_FullHash == num)
			{
				if (time - mPendingActivationTime > mPendingInstruction.m_ActivateAfter && (time - mActivationTime > mActiveInstruction.m_MinDuration || mPendingInstruction.m_VirtualCamera.Priority > mActiveInstruction.m_VirtualCamera.Priority))
				{
					mActiveInstruction = mPendingInstruction;
					mActivationTime = time;
					mPendingActivationTime = 0f;
				}
				return mActiveInstruction.m_VirtualCamera;
			}
		}
		mPendingActivationTime = 0f;
		if (!mInstructionDictionary.ContainsKey(num))
		{
			if (mActivationTime != 0f)
			{
				return mActiveInstruction.m_VirtualCamera;
			}
			return cinemachineVirtualCameraBase;
		}
		Instruction instruction = m_Instructions[mInstructionDictionary[num]];
		if (instruction.m_VirtualCamera == null)
		{
			instruction.m_VirtualCamera = cinemachineVirtualCameraBase;
		}
		if (deltaTime >= 0f && mActivationTime > 0f && (instruction.m_ActivateAfter > 0f || (time - mActivationTime < mActiveInstruction.m_MinDuration && instruction.m_VirtualCamera.Priority <= mActiveInstruction.m_VirtualCamera.Priority)))
		{
			mPendingInstruction = instruction;
			mPendingActivationTime = time;
			if (mActivationTime != 0f)
			{
				return mActiveInstruction.m_VirtualCamera;
			}
			return cinemachineVirtualCameraBase;
		}
		mActiveInstruction = instruction;
		mActivationTime = time;
		return mActiveInstruction.m_VirtualCamera;
	}

	private int GetClipHash(int hash, List<AnimatorClipInfo> clips)
	{
		if (clips.Count > 1)
		{
			int num = -1;
			for (int i = 0; i < clips.Count; i++)
			{
				if (num < 0 || clips[i].weight > clips[num].weight)
				{
					num = i;
				}
			}
			if (num >= 0 && clips[num].weight > 0f)
			{
				hash = Animator.StringToHash(CreateFakeHashName(hash, clips[num].clip.name));
			}
		}
		return hash;
	}

	private AnimationCurve LookupBlendCurve(ICinemachineCamera fromKey, ICinemachineCamera toKey, out float duration)
	{
		AnimationCurve animationCurve = m_DefaultBlend.BlendCurve;
		if (m_CustomBlends != null)
		{
			string fromCameraName = ((fromKey != null) ? fromKey.Name : string.Empty);
			string toCameraName = ((toKey != null) ? toKey.Name : string.Empty);
			animationCurve = m_CustomBlends.GetBlendCurveForVirtualCameras(fromCameraName, toCameraName, animationCurve);
		}
		Keyframe[] keys = animationCurve.keys;
		duration = ((keys == null || keys.Length == 0) ? 0f : keys[keys.Length - 1].time);
		return animationCurve;
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
