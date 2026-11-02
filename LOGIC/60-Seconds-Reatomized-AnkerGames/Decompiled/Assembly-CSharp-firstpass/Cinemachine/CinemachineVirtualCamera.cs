using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cinemachine;

[DocumentationSorting(1f, DocumentationSortingAttribute.Level.UserRef)]
[ExecuteInEditMode]
[DisallowMultipleComponent]
[AddComponentMenu("Cinemachine/CinemachineVirtualCamera")]
public class CinemachineVirtualCamera : CinemachineVirtualCameraBase
{
	public delegate Transform CreatePipelineDelegate(CinemachineVirtualCamera vcam, string name, CinemachineComponentBase[] copyFrom);

	public delegate void DestroyPipelineDelegate(GameObject pipeline);

	[Tooltip("The object that the camera wants to look at (the Aim target).  If this is null, then the vcam's Transform orientation will define the camera's orientation.")]
	[NoSaveDuringPlay]
	public Transform m_LookAt;

	[Tooltip("The object that the camera wants to move with (the Body target).  If this is null, then the vcam's Transform position will define the camera's position.")]
	[NoSaveDuringPlay]
	public Transform m_Follow;

	[FormerlySerializedAs("m_LensAttributes")]
	[Tooltip("Specifies the lens properties of this Virtual Camera.  This generally mirrors the Unity Camera's lens settings, and will be used to drive the Unity camera when the vcam is active.")]
	[LensSettingsProperty]
	public LensSettings m_Lens = LensSettings.Default;

	public const string PipelineName = "cm";

	public static CreatePipelineDelegate CreatePipelineOverride;

	public static DestroyPipelineDelegate DestroyPipelineOverride;

	private CameraState m_State = CameraState.Default;

	private CinemachineComponentBase[] m_ComponentPipeline;

	[SerializeField]
	[HideInInspector]
	private Transform m_ComponentOwner;

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

	public bool UserIsDragging { get; set; }

	public override void UpdateCameraState(Vector3 worldUp, float deltaTime)
	{
		if (!PreviousStateIsValid)
		{
			deltaTime = -1f;
		}
		if (deltaTime < 0f)
		{
			m_State = PullStateFromVirtualCamera(worldUp);
		}
		m_State = CalculateNewState(worldUp, deltaTime);
		if (!UserIsDragging)
		{
			if (Follow != null)
			{
				transform.position = State.RawPosition;
			}
			if (LookAt != null)
			{
				transform.rotation = State.RawOrientation;
			}
		}
		PreviousStateIsValid = true;
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		InvalidateComponentPipeline();
		if (ValidatingStreamVersion < 20170927)
		{
			if (Follow != null && GetCinemachineComponent(CinemachineCore.Stage.Body) == null)
			{
				AddCinemachineComponent<CinemachineHardLockToTarget>();
			}
			if (LookAt != null && GetCinemachineComponent(CinemachineCore.Stage.Aim) == null)
			{
				AddCinemachineComponent<CinemachineHardLookAt>();
			}
		}
	}

	protected override void OnDestroy()
	{
		foreach (Transform item in base.transform)
		{
			if (item.GetComponent<CinemachinePipeline>() != null)
			{
				item.gameObject.hideFlags &= ~(HideFlags.HideInHierarchy | HideFlags.HideInInspector);
			}
		}
		base.OnDestroy();
	}

	protected override void OnValidate()
	{
		base.OnValidate();
		m_Lens.Validate();
	}

	private void OnTransformChildrenChanged()
	{
		InvalidateComponentPipeline();
	}

	private void Reset()
	{
		DestroyPipeline();
	}

	private void DestroyPipeline()
	{
		List<Transform> list = new List<Transform>();
		foreach (Transform item in base.transform)
		{
			if (item.GetComponent<CinemachinePipeline>() != null)
			{
				list.Add(item);
			}
		}
		foreach (Transform item2 in list)
		{
			if (DestroyPipelineOverride != null)
			{
				DestroyPipelineOverride(item2.gameObject);
			}
			else
			{
				Object.Destroy(item2.gameObject);
			}
		}
		m_ComponentOwner = null;
		PreviousStateIsValid = false;
	}

	private Transform CreatePipeline(CinemachineVirtualCamera copyFrom)
	{
		CinemachineComponentBase[] array = null;
		if (copyFrom != null)
		{
			copyFrom.InvalidateComponentPipeline();
			array = copyFrom.GetComponentPipeline();
		}
		Transform transform = null;
		if (CreatePipelineOverride != null)
		{
			transform = CreatePipelineOverride(this, "cm", array);
		}
		else
		{
			GameObject gameObject = new GameObject("cm");
			gameObject.transform.parent = base.transform;
			gameObject.AddComponent<CinemachinePipeline>();
			transform = gameObject.transform;
			if (array != null)
			{
				CinemachineComponentBase[] array2 = array;
				foreach (Component component in array2)
				{
					ReflectionHelpers.CopyFields(component, gameObject.AddComponent(component.GetType()));
				}
			}
		}
		PreviousStateIsValid = false;
		return transform;
	}

	public void InvalidateComponentPipeline()
	{
		m_ComponentPipeline = null;
	}

	public Transform GetComponentOwner()
	{
		UpdateComponentPipeline();
		return m_ComponentOwner;
	}

	public CinemachineComponentBase[] GetComponentPipeline()
	{
		UpdateComponentPipeline();
		return m_ComponentPipeline;
	}

	public CinemachineComponentBase GetCinemachineComponent(CinemachineCore.Stage stage)
	{
		CinemachineComponentBase[] componentPipeline = GetComponentPipeline();
		if (componentPipeline != null)
		{
			CinemachineComponentBase[] array = componentPipeline;
			foreach (CinemachineComponentBase cinemachineComponentBase in array)
			{
				if (cinemachineComponentBase.Stage == stage)
				{
					return cinemachineComponentBase;
				}
			}
		}
		return null;
	}

	public T GetCinemachineComponent<T>() where T : CinemachineComponentBase
	{
		CinemachineComponentBase[] componentPipeline = GetComponentPipeline();
		if (componentPipeline != null)
		{
			CinemachineComponentBase[] array = componentPipeline;
			foreach (CinemachineComponentBase cinemachineComponentBase in array)
			{
				if (cinemachineComponentBase is T)
				{
					return cinemachineComponentBase as T;
				}
			}
		}
		return null;
	}

	public T AddCinemachineComponent<T>() where T : CinemachineComponentBase
	{
		Transform componentOwner = GetComponentOwner();
		CinemachineComponentBase[] components = componentOwner.GetComponents<CinemachineComponentBase>();
		T val = componentOwner.gameObject.AddComponent<T>();
		if (val != null && components != null)
		{
			CinemachineCore.Stage stage = val.Stage;
			for (int num = components.Length - 1; num >= 0; num--)
			{
				if (components[num].Stage == stage)
				{
					components[num].enabled = false;
					Object.DestroyImmediate(components[num]);
				}
			}
		}
		InvalidateComponentPipeline();
		return val;
	}

	public void DestroyCinemachineComponent<T>() where T : CinemachineComponentBase
	{
		CinemachineComponentBase[] componentPipeline = GetComponentPipeline();
		if (componentPipeline == null)
		{
			return;
		}
		CinemachineComponentBase[] array = componentPipeline;
		foreach (CinemachineComponentBase cinemachineComponentBase in array)
		{
			if (cinemachineComponentBase is T)
			{
				cinemachineComponentBase.enabled = false;
				Object.DestroyImmediate(cinemachineComponentBase);
				InvalidateComponentPipeline();
			}
		}
	}

	public void OnPositionDragged(Vector3 delta)
	{
		CinemachineComponentBase[] componentPipeline = GetComponentPipeline();
		if (componentPipeline != null)
		{
			for (int i = 0; i < componentPipeline.Length; i++)
			{
				componentPipeline[i].OnPositionDragged(delta);
			}
		}
	}

	private void UpdateComponentPipeline()
	{
		if (m_ComponentOwner != null && m_ComponentOwner.parent != base.transform)
		{
			CinemachineVirtualCamera copyFrom = ((m_ComponentOwner.parent != null) ? m_ComponentOwner.parent.gameObject.GetComponent<CinemachineVirtualCamera>() : null);
			DestroyPipeline();
			m_ComponentOwner = CreatePipeline(copyFrom);
		}
		if (m_ComponentOwner != null && m_ComponentPipeline != null)
		{
			return;
		}
		m_ComponentOwner = null;
		List<CinemachineComponentBase> list = new List<CinemachineComponentBase>();
		foreach (Transform item2 in base.transform)
		{
			if (item2.GetComponent<CinemachinePipeline>() != null)
			{
				m_ComponentOwner = item2;
				CinemachineComponentBase[] components = item2.GetComponents<CinemachineComponentBase>();
				foreach (CinemachineComponentBase item in components)
				{
					list.Add(item);
				}
			}
		}
		if (m_ComponentOwner == null)
		{
			m_ComponentOwner = CreatePipeline(null);
		}
		if (CinemachineCore.sShowHiddenObjects)
		{
			m_ComponentOwner.gameObject.hideFlags &= ~(HideFlags.HideInHierarchy | HideFlags.HideInInspector);
		}
		else
		{
			m_ComponentOwner.gameObject.hideFlags |= HideFlags.HideInHierarchy | HideFlags.HideInInspector;
		}
		list.Sort((CinemachineComponentBase c1, CinemachineComponentBase c2) => c1.Stage - c2.Stage);
		m_ComponentPipeline = list.ToArray();
	}

	private CameraState CalculateNewState(Vector3 worldUp, float deltaTime)
	{
		CameraState state = PullStateFromVirtualCamera(worldUp);
		if (LookAt != null)
		{
			state.ReferenceLookAt = LookAt.position;
		}
		CinemachineCore.Stage curStage = CinemachineCore.Stage.Body;
		UpdateComponentPipeline();
		if (m_ComponentPipeline != null)
		{
			for (int i = 0; i < m_ComponentPipeline.Length; i++)
			{
				m_ComponentPipeline[i].PrePipelineMutateCameraState(ref state);
			}
			for (int j = 0; j < m_ComponentPipeline.Length; j++)
			{
				curStage = AdvancePipelineStage(ref state, deltaTime, curStage, (int)m_ComponentPipeline[j].Stage);
				m_ComponentPipeline[j].MutateCameraState(ref state, deltaTime);
			}
		}
		int maxStage = 3;
		AdvancePipelineStage(ref state, deltaTime, curStage, maxStage);
		return state;
	}

	private CinemachineCore.Stage AdvancePipelineStage(ref CameraState state, float deltaTime, CinemachineCore.Stage curStage, int maxStage)
	{
		while ((int)curStage < maxStage)
		{
			InvokePostPipelineStageCallback(this, curStage, ref state, deltaTime);
			curStage++;
		}
		return curStage;
	}

	private CameraState PullStateFromVirtualCamera(Vector3 worldUp)
	{
		CameraState result = CameraState.Default;
		result.RawPosition = transform.position;
		result.RawOrientation = transform.rotation;
		result.ReferenceUp = worldUp;
		CinemachineBrain cinemachineBrain = CinemachineCore.Instance.FindPotentialTargetBrain(this);
		m_Lens.Aspect = ((cinemachineBrain != null) ? cinemachineBrain.OutputCamera.aspect : 1f);
		m_Lens.Orthographic = cinemachineBrain != null && cinemachineBrain.OutputCamera.orthographic;
		result.Lens = m_Lens;
		return result;
	}

	internal void SetStateRawPosition(Vector3 pos)
	{
		m_State.RawPosition = pos;
	}
}
