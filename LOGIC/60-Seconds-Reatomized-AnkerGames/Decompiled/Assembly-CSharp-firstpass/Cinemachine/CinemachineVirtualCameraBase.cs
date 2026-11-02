using System;
using UnityEngine;

namespace Cinemachine;

[SaveDuringPlay]
public abstract class CinemachineVirtualCameraBase : MonoBehaviour, ICinemachineCamera
{
	public delegate void OnPostPipelineStageDelegate(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState newState, float deltaTime);

	[HideInInspector]
	[NoSaveDuringPlay]
	public Action CinemachineGUIDebuggerCallback;

	[HideInInspector]
	[SerializeField]
	[NoSaveDuringPlay]
	public string[] m_ExcludedPropertiesInInspector = new string[1] { "m_Script" };

	[HideInInspector]
	[SerializeField]
	[NoSaveDuringPlay]
	public CinemachineCore.Stage[] m_LockStageInInspector;

	private int m_ValidatingStreamVersion;

	private bool m_OnValidateCalled;

	[HideInInspector]
	[SerializeField]
	[NoSaveDuringPlay]
	private int m_StreamingVersion;

	[NoSaveDuringPlay]
	[Tooltip("The priority will determine which camera becomes active based on the state of other cameras and this camera.  Higher numbers have greater priority.")]
	public int m_Priority = 10;

	protected OnPostPipelineStageDelegate OnPostPipelineStage;

	private bool m_previousStateIsValid;

	private Transform m_previousLookAtTarget;

	private Transform m_previousFollowTarget;

	private bool mSlaveStatusUpdated;

	private CinemachineVirtualCameraBase m_parentVcam;

	private int m_QueuePriority = int.MaxValue;

	public int ValidatingStreamVersion
	{
		get
		{
			if (!m_OnValidateCalled)
			{
				return CinemachineCore.kStreamingVersion;
			}
			return m_ValidatingStreamVersion;
		}
		private set
		{
			m_ValidatingStreamVersion = value;
		}
	}

	public string Name => name;

	public virtual string Description => "";

	public int Priority
	{
		get
		{
			return m_Priority;
		}
		set
		{
			m_Priority = value;
		}
	}

	public GameObject VirtualCameraGameObject
	{
		get
		{
			if (this == null)
			{
				return null;
			}
			return gameObject;
		}
	}

	public abstract CameraState State { get; }

	public virtual ICinemachineCamera LiveChildOrSelf => this;

	public ICinemachineCamera ParentCamera
	{
		get
		{
			if (!mSlaveStatusUpdated || !Application.isPlaying)
			{
				UpdateSlaveStatus();
			}
			return m_parentVcam;
		}
	}

	public abstract Transform LookAt { get; set; }

	public abstract Transform Follow { get; set; }

	public bool PreviousStateIsValid
	{
		get
		{
			if (LookAt != m_previousLookAtTarget)
			{
				m_previousLookAtTarget = LookAt;
				m_previousStateIsValid = false;
			}
			if (Follow != m_previousFollowTarget)
			{
				m_previousFollowTarget = Follow;
				m_previousStateIsValid = false;
			}
			return m_previousStateIsValid;
		}
		set
		{
			m_previousStateIsValid = value;
		}
	}

	public virtual void AddPostPipelineStageHook(OnPostPipelineStageDelegate d)
	{
		OnPostPipelineStage = (OnPostPipelineStageDelegate)Delegate.Remove(OnPostPipelineStage, d);
		OnPostPipelineStage = (OnPostPipelineStageDelegate)Delegate.Combine(OnPostPipelineStage, d);
	}

	public virtual void RemovePostPipelineStageHook(OnPostPipelineStageDelegate d)
	{
		OnPostPipelineStage = (OnPostPipelineStageDelegate)Delegate.Remove(OnPostPipelineStage, d);
	}

	protected void InvokePostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState newState, float deltaTime)
	{
		if (OnPostPipelineStage != null)
		{
			OnPostPipelineStage(vcam, stage, ref newState, deltaTime);
		}
		CinemachineVirtualCameraBase cinemachineVirtualCameraBase = ParentCamera as CinemachineVirtualCameraBase;
		if (cinemachineVirtualCameraBase != null)
		{
			cinemachineVirtualCameraBase.InvokePostPipelineStageCallback(vcam, stage, ref newState, deltaTime);
		}
	}

	public virtual bool IsLiveChild(ICinemachineCamera vcam)
	{
		return false;
	}

	public virtual void PreUpdateChildCameras(Vector3 worldUp, float deltaTime)
	{
	}

	public abstract void UpdateCameraState(Vector3 worldUp, float deltaTime);

	public virtual void OnTransitionFromCamera(ICinemachineCamera fromCam, Vector3 worldUp, float deltaTime)
	{
		if (!gameObject.activeInHierarchy)
		{
			PreviousStateIsValid = false;
		}
	}

	protected virtual void Start()
	{
	}

	protected virtual void OnDestroy()
	{
		CinemachineCore.Instance.RemoveActiveCamera(this);
	}

	protected virtual void OnValidate()
	{
		m_OnValidateCalled = true;
		ValidatingStreamVersion = m_StreamingVersion;
		m_StreamingVersion = CinemachineCore.kStreamingVersion;
	}

	protected virtual void OnEnable()
	{
		CinemachineVirtualCameraBase[] components = GetComponents<CinemachineVirtualCameraBase>();
		for (int i = 0; i < components.Length; i++)
		{
			if (components[i].enabled && components[i] != this)
			{
				Debug.LogError(Name + " has multiple CinemachineVirtualCameraBase-derived components.  Disabling " + GetType().Name + ".");
				enabled = false;
			}
		}
		UpdateSlaveStatus();
		UpdateVcamPoolStatus();
		PreviousStateIsValid = false;
	}

	protected virtual void OnDisable()
	{
		UpdateVcamPoolStatus();
	}

	protected virtual void Update()
	{
		if (m_Priority != m_QueuePriority)
		{
			UpdateVcamPoolStatus();
		}
	}

	protected virtual void OnTransformParentChanged()
	{
		UpdateSlaveStatus();
		UpdateVcamPoolStatus();
	}

	private void UpdateSlaveStatus()
	{
		mSlaveStatusUpdated = true;
		m_parentVcam = null;
		Transform parent = transform.parent;
		if (parent != null)
		{
			m_parentVcam = parent.GetComponent<CinemachineVirtualCameraBase>();
		}
	}

	protected Transform ResolveLookAt(Transform localLookAt)
	{
		Transform transform = localLookAt;
		if (transform == null && ParentCamera != null)
		{
			transform = ParentCamera.LookAt;
		}
		return transform;
	}

	protected Transform ResolveFollow(Transform localFollow)
	{
		Transform transform = localFollow;
		if (transform == null && ParentCamera != null)
		{
			transform = ParentCamera.Follow;
		}
		return transform;
	}

	private void UpdateVcamPoolStatus()
	{
		m_QueuePriority = int.MaxValue;
		CinemachineCore.Instance.RemoveActiveCamera(this);
		CinemachineCore.Instance.RemoveChildCamera(this);
		if (m_parentVcam == null)
		{
			if (isActiveAndEnabled)
			{
				CinemachineCore.Instance.AddActiveCamera(this);
				m_QueuePriority = m_Priority;
			}
		}
		else if (isActiveAndEnabled)
		{
			CinemachineCore.Instance.AddChildCamera(this);
		}
	}

	public void MoveToTopOfPrioritySubqueue()
	{
		UpdateVcamPoolStatus();
	}
}
