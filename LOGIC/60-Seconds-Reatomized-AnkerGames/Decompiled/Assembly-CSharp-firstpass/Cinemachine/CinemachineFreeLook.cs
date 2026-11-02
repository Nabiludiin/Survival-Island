using System;
using Cinemachine.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cinemachine;

[DocumentationSorting(11f, DocumentationSortingAttribute.Level.UserRef)]
[ExecuteInEditMode]
[DisallowMultipleComponent]
[AddComponentMenu("Cinemachine/CinemachineFreeLook")]
public class CinemachineFreeLook : CinemachineVirtualCameraBase
{
	[Serializable]
	public struct Orbit(float h, float r)
	{
		public float m_Height = h;

		public float m_Radius = r;
	}

	public delegate CinemachineVirtualCamera CreateRigDelegate(CinemachineFreeLook vcam, string name, CinemachineVirtualCamera copyFrom);

	public delegate void DestroyRigDelegate(GameObject rig);

	[Tooltip("Object for the camera children to look at (the aim target).")]
	[NoSaveDuringPlay]
	public Transform m_LookAt;

	[Tooltip("Object for the camera children wants to move with (the body target).")]
	[NoSaveDuringPlay]
	public Transform m_Follow;

	[Tooltip("If enabled, this lens setting will apply to all three child rigs, otherwise the child rig lens settings will be used")]
	[FormerlySerializedAs("m_UseCommonLensSetting")]
	public bool m_CommonLens = true;

	[FormerlySerializedAs("m_LensAttributes")]
	[Tooltip("Specifies the lens properties of this Virtual Camera.  This generally mirrors the Unity Camera's lens settings, and will be used to drive the Unity camera when the vcam is active")]
	[LensSettingsProperty]
	public LensSettings m_Lens = LensSettings.Default;

	[Header("Axis Control")]
	[Tooltip("The Vertical axis.  Value is 0..1.  Chooses how to blend the child rigs")]
	public AxisState m_YAxis = new AxisState(2f, 0.2f, 0.1f, 0.5f, "Mouse Y", invert: false);

	[Tooltip("The Horizontal axis.  Value is 0..359.  This is passed on to the rigs' OrbitalTransposer component")]
	public AxisState m_XAxis = new AxisState(300f, 0.1f, 0.1f, 0f, "Mouse X", invert: true);

	[Tooltip("The definition of Forward.  Camera will follow behind.")]
	public CinemachineOrbitalTransposer.Heading m_Heading = new CinemachineOrbitalTransposer.Heading(CinemachineOrbitalTransposer.Heading.HeadingDefinition.TargetForward, 4, 0f);

	[Tooltip("Controls how automatic recentering of the X axis is accomplished")]
	public CinemachineOrbitalTransposer.Recentering m_RecenterToTargetHeading = new CinemachineOrbitalTransposer.Recentering(enabled: false, 1f, 2f);

	[Header("Orbits")]
	[Tooltip("The coordinate space to use when interpreting the offset from the target.  This is also used to set the camera's Up vector, which will be maintained when aiming the camera.")]
	public CinemachineTransposer.BindingMode m_BindingMode = CinemachineTransposer.BindingMode.SimpleFollowWithWorldUp;

	[Tooltip("Controls how taut is the line that connects the rigs' orbits, which determines final placement on the Y axis")]
	[Range(0f, 1f)]
	[FormerlySerializedAs("m_SplineTension")]
	public float m_SplineCurvature = 0.2f;

	[Tooltip("The radius and height of the three orbiting rigs.")]
	public Orbit[] m_Orbits = new Orbit[3]
	{
		new Orbit(4.5f, 1.75f),
		new Orbit(2.5f, 3f),
		new Orbit(0.4f, 1.3f)
	};

	[SerializeField]
	[HideInInspector]
	[FormerlySerializedAs("m_HeadingBias")]
	private float m_LegacyHeadingBias = float.MaxValue;

	private bool mUseLegacyRigDefinitions;

	private bool mIsDestroyed;

	private CameraState m_State = CameraState.Default;

	[SerializeField]
	[HideInInspector]
	[NoSaveDuringPlay]
	private CinemachineVirtualCamera[] m_Rigs = new CinemachineVirtualCamera[3];

	private CinemachineOrbitalTransposer[] mOrbitals;

	private CinemachineBlend mBlendA;

	private CinemachineBlend mBlendB;

	public static CreateRigDelegate CreateRigOverride;

	public static DestroyRigDelegate DestroyRigOverride;

	private Orbit[] m_CachedOrbits;

	private float m_CachedTension;

	private Vector4[] m_CachedKnots;

	private Vector4[] m_CachedCtrl1;

	private Vector4[] m_CachedCtrl2;

	public static string[] RigNames => new string[3] { "TopRig", "MiddleRig", "BottomRig" };

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

	public override ICinemachineCamera LiveChildOrSelf
	{
		get
		{
			if (m_Rigs == null || m_Rigs.Length != 3)
			{
				return this;
			}
			if (m_YAxis.Value < 0.33f)
			{
				return m_Rigs[2];
			}
			if (m_YAxis.Value > 0.66f)
			{
				return m_Rigs[0];
			}
			return m_Rigs[1];
		}
	}

	protected override void OnValidate()
	{
		base.OnValidate();
		if (m_LegacyHeadingBias != float.MaxValue)
		{
			m_Heading.m_HeadingBias = m_LegacyHeadingBias;
			m_LegacyHeadingBias = float.MaxValue;
			m_RecenterToTargetHeading.LegacyUpgrade(ref m_Heading.m_HeadingDefinition, ref m_Heading.m_VelocityFilterStrength);
			mUseLegacyRigDefinitions = true;
		}
		m_YAxis.Validate();
		m_XAxis.Validate();
		m_RecenterToTargetHeading.Validate();
		m_Lens.Validate();
		InvalidateRigCache();
	}

	public CinemachineVirtualCamera GetRig(int i)
	{
		UpdateRigCache();
		if (i >= 0 && i <= 2)
		{
			return m_Rigs[i];
		}
		return null;
	}

	protected override void OnEnable()
	{
		mIsDestroyed = false;
		base.OnEnable();
		InvalidateRigCache();
	}

	protected override void OnDestroy()
	{
		if (m_Rigs != null)
		{
			CinemachineVirtualCamera[] rigs = m_Rigs;
			for (int i = 0; i < rigs.Length; i++)
			{
				rigs[i].gameObject.hideFlags &= ~(HideFlags.HideInHierarchy | HideFlags.HideInInspector);
			}
		}
		mIsDestroyed = true;
		base.OnDestroy();
	}

	private void OnTransformChildrenChanged()
	{
		InvalidateRigCache();
	}

	private void Reset()
	{
		DestroyRigs();
	}

	public override bool IsLiveChild(ICinemachineCamera vcam)
	{
		if (m_Rigs == null || m_Rigs.Length != 3)
		{
			return false;
		}
		if (m_YAxis.Value < 0.33f)
		{
			return vcam == m_Rigs[2];
		}
		if (m_YAxis.Value > 0.66f)
		{
			return vcam == m_Rigs[0];
		}
		return vcam == m_Rigs[1];
	}

	public override void RemovePostPipelineStageHook(OnPostPipelineStageDelegate d)
	{
		base.RemovePostPipelineStageHook(d);
		UpdateRigCache();
		if (m_Rigs != null)
		{
			CinemachineVirtualCamera[] rigs = m_Rigs;
			for (int i = 0; i < rigs.Length; i++)
			{
				rigs[i].RemovePostPipelineStageHook(d);
			}
		}
	}

	public override void PreUpdateChildCameras(Vector3 worldUp, float deltaTime)
	{
		if (!PreviousStateIsValid)
		{
			deltaTime = -1f;
		}
		UpdateRigCache();
		int num;
		if (!(deltaTime >= 0f))
		{
			num = (CinemachineCore.Instance.IsLive(this) ? 1 : 0);
			if (num == 0)
			{
				goto IL_003b;
			}
		}
		else
		{
			num = 1;
		}
		m_YAxis.Update(deltaTime);
		goto IL_003b;
		IL_003b:
		PushSettingsToRigs();
		if (num != 0)
		{
			UpdateHeading(deltaTime, m_State.ReferenceUp);
		}
	}

	public override void UpdateCameraState(Vector3 worldUp, float deltaTime)
	{
		if (!PreviousStateIsValid)
		{
			deltaTime = -1f;
		}
		UpdateRigCache();
		if (deltaTime < 0f)
		{
			m_State = PullStateFromVirtualCamera(worldUp);
		}
		m_State = CalculateNewState(worldUp, deltaTime);
		if (Follow != null)
		{
			Vector3 vector = State.RawPosition - transform.position;
			transform.position = State.RawPosition;
			m_Rigs[0].transform.position -= vector;
			m_Rigs[1].transform.position -= vector;
			m_Rigs[2].transform.position -= vector;
		}
		PreviousStateIsValid = true;
	}

	public override void OnTransitionFromCamera(ICinemachineCamera fromCam, Vector3 worldUp, float deltaTime)
	{
		base.OnTransitionFromCamera(fromCam, worldUp, deltaTime);
		if (fromCam != null && fromCam is CinemachineFreeLook)
		{
			CinemachineFreeLook cinemachineFreeLook = fromCam as CinemachineFreeLook;
			if (cinemachineFreeLook.Follow == Follow)
			{
				m_XAxis.Value = cinemachineFreeLook.m_XAxis.Value;
				m_YAxis.Value = cinemachineFreeLook.m_YAxis.Value;
				UpdateCameraState(worldUp, deltaTime);
			}
		}
	}

	private void InvalidateRigCache()
	{
		mOrbitals = null;
	}

	private void DestroyRigs()
	{
		CinemachineVirtualCamera[] array = new CinemachineVirtualCamera[RigNames.Length];
		for (int i = 0; i < RigNames.Length; i++)
		{
			foreach (Transform item in base.transform)
			{
				if (item.gameObject.name == RigNames[i])
				{
					array[i] = item.GetComponent<CinemachineVirtualCamera>();
				}
			}
		}
		for (int j = 0; j < array.Length; j++)
		{
			if (array[j] != null)
			{
				if (DestroyRigOverride != null)
				{
					DestroyRigOverride(array[j].gameObject);
				}
				else
				{
					UnityEngine.Object.Destroy(array[j].gameObject);
				}
			}
		}
		m_Rigs = null;
		mOrbitals = null;
	}

	private CinemachineVirtualCamera[] CreateRigs(CinemachineVirtualCamera[] copyFrom)
	{
		mOrbitals = null;
		float[] array = new float[3] { 0.5f, 0.55f, 0.6f };
		CinemachineVirtualCamera[] array2 = new CinemachineVirtualCamera[3];
		for (int i = 0; i < RigNames.Length; i++)
		{
			CinemachineVirtualCamera cinemachineVirtualCamera = null;
			if (copyFrom != null && copyFrom.Length > i)
			{
				cinemachineVirtualCamera = copyFrom[i];
			}
			if (CreateRigOverride != null)
			{
				array2[i] = CreateRigOverride(this, RigNames[i], cinemachineVirtualCamera);
			}
			else
			{
				GameObject gameObject = new GameObject(RigNames[i]);
				gameObject.transform.parent = transform;
				array2[i] = gameObject.AddComponent<CinemachineVirtualCamera>();
				if (cinemachineVirtualCamera != null)
				{
					ReflectionHelpers.CopyFields(cinemachineVirtualCamera, array2[i]);
				}
				else
				{
					gameObject = array2[i].GetComponentOwner().gameObject;
					gameObject.AddComponent<CinemachineOrbitalTransposer>();
					gameObject.AddComponent<CinemachineComposer>();
				}
			}
			array2[i].InvalidateComponentPipeline();
			CinemachineOrbitalTransposer cinemachineOrbitalTransposer = array2[i].GetCinemachineComponent<CinemachineOrbitalTransposer>();
			if (cinemachineOrbitalTransposer == null)
			{
				cinemachineOrbitalTransposer = array2[i].AddCinemachineComponent<CinemachineOrbitalTransposer>();
			}
			if (cinemachineVirtualCamera == null)
			{
				cinemachineOrbitalTransposer.m_YawDamping = 0f;
				CinemachineComposer cinemachineComponent = array2[i].GetCinemachineComponent<CinemachineComposer>();
				if (cinemachineComponent != null)
				{
					cinemachineComponent.m_HorizontalDamping = (cinemachineComponent.m_VerticalDamping = 0f);
					cinemachineComponent.m_ScreenX = 0.5f;
					cinemachineComponent.m_ScreenY = array[i];
					cinemachineComponent.m_DeadZoneWidth = (cinemachineComponent.m_DeadZoneHeight = 0.1f);
					cinemachineComponent.m_SoftZoneWidth = (cinemachineComponent.m_SoftZoneHeight = 0.8f);
					cinemachineComponent.m_BiasX = (cinemachineComponent.m_BiasY = 0f);
				}
			}
		}
		return array2;
	}

	private void UpdateRigCache()
	{
		if (mIsDestroyed)
		{
			return;
		}
		if (m_Rigs != null && m_Rigs.Length == 3 && m_Rigs[0] != null && m_Rigs[0].transform.parent != transform)
		{
			DestroyRigs();
			m_Rigs = CreateRigs(m_Rigs);
		}
		if (mOrbitals == null || mOrbitals.Length != 3)
		{
			if (LocateExistingRigs(RigNames, forceOrbital: false) != 3)
			{
				DestroyRigs();
				m_Rigs = CreateRigs(null);
				LocateExistingRigs(RigNames, forceOrbital: true);
			}
			CinemachineVirtualCamera[] rigs = m_Rigs;
			foreach (CinemachineVirtualCamera cinemachineVirtualCamera in rigs)
			{
				cinemachineVirtualCamera.m_ExcludedPropertiesInInspector = (m_CommonLens ? new string[6] { "m_Script", "Header", "Extensions", "m_Priority", "m_Follow", "m_Lens" } : new string[5] { "m_Script", "Header", "Extensions", "m_Priority", "m_Follow" });
				cinemachineVirtualCamera.m_LockStageInInspector = new CinemachineCore.Stage[1];
			}
			mBlendA = new CinemachineBlend(m_Rigs[1], m_Rigs[0], AnimationCurve.Linear(0f, 0f, 1f, 1f), 1f, 0f);
			mBlendB = new CinemachineBlend(m_Rigs[2], m_Rigs[1], AnimationCurve.Linear(0f, 0f, 1f, 1f), 1f, 0f);
			m_XAxis.SetThresholds(0f, 360f, wrapAround: true);
			m_YAxis.SetThresholds(0f, 1f, wrapAround: false);
		}
	}

	private int LocateExistingRigs(string[] rigNames, bool forceOrbital)
	{
		mOrbitals = new CinemachineOrbitalTransposer[rigNames.Length];
		m_Rigs = new CinemachineVirtualCamera[rigNames.Length];
		int num = 0;
		foreach (Transform item in base.transform)
		{
			CinemachineVirtualCamera component = item.GetComponent<CinemachineVirtualCamera>();
			if (!(component != null))
			{
				continue;
			}
			GameObject gameObject = item.gameObject;
			for (int i = 0; i < rigNames.Length; i++)
			{
				if (mOrbitals[i] == null && gameObject.name == rigNames[i])
				{
					mOrbitals[i] = component.GetCinemachineComponent<CinemachineOrbitalTransposer>();
					if ((mOrbitals[i] == null) & forceOrbital)
					{
						mOrbitals[i] = component.AddCinemachineComponent<CinemachineOrbitalTransposer>();
					}
					if (mOrbitals[i] != null)
					{
						mOrbitals[i].m_HeadingIsSlave = true;
						m_Rigs[i] = component;
						num++;
					}
				}
			}
		}
		return num;
	}

	private void PushSettingsToRigs()
	{
		UpdateRigCache();
		for (int i = 0; i < m_Rigs.Length; i++)
		{
			if (m_Rigs[i] == null)
			{
				continue;
			}
			if (m_CommonLens)
			{
				m_Rigs[i].m_Lens = m_Lens;
			}
			if (mUseLegacyRigDefinitions)
			{
				mUseLegacyRigDefinitions = false;
				m_Orbits[i].m_Height = mOrbitals[i].m_FollowOffset.y;
				m_Orbits[i].m_Radius = 0f - mOrbitals[i].m_FollowOffset.z;
				if (m_Rigs[i].Follow != null)
				{
					Follow = m_Rigs[i].Follow;
				}
			}
			m_Rigs[i].Follow = null;
			if (CinemachineCore.sShowHiddenObjects)
			{
				m_Rigs[i].gameObject.hideFlags &= ~(HideFlags.HideInHierarchy | HideFlags.HideInInspector);
			}
			else
			{
				m_Rigs[i].gameObject.hideFlags |= HideFlags.HideInHierarchy | HideFlags.HideInInspector;
			}
			mOrbitals[i].m_FollowOffset = GetLocalPositionForCameraFromInput(m_YAxis.Value);
			mOrbitals[i].m_BindingMode = m_BindingMode;
			mOrbitals[i].m_Heading = m_Heading;
			mOrbitals[i].m_HeadingIsSlave = true;
			mOrbitals[i].m_XAxis = m_XAxis;
			mOrbitals[i].m_RecenterToTargetHeading = m_RecenterToTargetHeading;
			if (i > 0)
			{
				mOrbitals[i].m_RecenterToTargetHeading.m_enabled = false;
			}
			if (m_BindingMode == CinemachineTransposer.BindingMode.SimpleFollowWithWorldUp)
			{
				m_Rigs[i].SetStateRawPosition(State.RawPosition);
			}
		}
	}

	private CameraState CalculateNewState(Vector3 worldUp, float deltaTime)
	{
		CameraState result = PullStateFromVirtualCamera(worldUp);
		float value = m_YAxis.Value;
		if (value > 0.5f)
		{
			if (mBlendA != null)
			{
				mBlendA.TimeInBlend = (value - 0.5f) * 2f;
				mBlendA.UpdateCameraState(worldUp, deltaTime);
				result = mBlendA.State;
			}
		}
		else if (mBlendB != null)
		{
			mBlendB.TimeInBlend = value * 2f;
			mBlendB.UpdateCameraState(worldUp, deltaTime);
			result = mBlendB.State;
		}
		return result;
	}

	private void UpdateHeading(float deltaTime, Vector3 up)
	{
		if (mOrbitals[0] != null)
		{
			mOrbitals[0].UpdateHeading(deltaTime, up);
			m_XAxis = mOrbitals[0].m_XAxis;
		}
		for (int i = 1; i < mOrbitals.Length; i++)
		{
			if (mOrbitals[i] != null)
			{
				mOrbitals[i].m_XAxis = m_XAxis;
			}
		}
		if (m_BindingMode == CinemachineTransposer.BindingMode.SimpleFollowWithWorldUp)
		{
			m_XAxis.Value = 0f;
		}
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

	public Vector3 GetLocalPositionForCameraFromInput(float t)
	{
		if (mOrbitals == null)
		{
			return Vector3.zero;
		}
		UpdateCachedSpline();
		int num = 1;
		if (t > 0.5f)
		{
			t -= 0.5f;
			num = 2;
		}
		return SplineHelpers.Bezier3(t * 2f, m_CachedKnots[num], m_CachedCtrl1[num], m_CachedCtrl2[num], m_CachedKnots[num + 1]);
	}

	private void UpdateCachedSpline()
	{
		bool flag = m_CachedOrbits != null && m_CachedTension == m_SplineCurvature;
		for (int i = 0; (i < 3) & flag; i++)
		{
			flag = m_CachedOrbits[i].m_Height == m_Orbits[i].m_Height && m_CachedOrbits[i].m_Radius == m_Orbits[i].m_Radius;
		}
		if (!flag)
		{
			float splineCurvature = m_SplineCurvature;
			m_CachedKnots = new Vector4[5];
			m_CachedCtrl1 = new Vector4[5];
			m_CachedCtrl2 = new Vector4[5];
			m_CachedKnots[1] = new Vector4(0f, m_Orbits[2].m_Height, 0f - m_Orbits[2].m_Radius, 0f);
			m_CachedKnots[2] = new Vector4(0f, m_Orbits[1].m_Height, 0f - m_Orbits[1].m_Radius, 0f);
			m_CachedKnots[3] = new Vector4(0f, m_Orbits[0].m_Height, 0f - m_Orbits[0].m_Radius, 0f);
			m_CachedKnots[0] = Vector4.Lerp(m_CachedKnots[1], Vector4.zero, splineCurvature);
			m_CachedKnots[4] = Vector4.Lerp(m_CachedKnots[3], Vector4.zero, splineCurvature);
			SplineHelpers.ComputeSmoothControlPoints(ref m_CachedKnots, ref m_CachedCtrl1, ref m_CachedCtrl2);
			m_CachedOrbits = new Orbit[3];
			for (int j = 0; j < 3; j++)
			{
				m_CachedOrbits[j] = m_Orbits[j];
			}
			m_CachedTension = m_SplineCurvature;
		}
	}
}
