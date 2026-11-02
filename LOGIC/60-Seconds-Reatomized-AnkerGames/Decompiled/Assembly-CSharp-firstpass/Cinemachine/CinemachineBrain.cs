using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Cinemachine;

[DocumentationSorting(0f, DocumentationSortingAttribute.Level.UserRef)]
[ExecuteInEditMode]
[DisallowMultipleComponent]
[AddComponentMenu("Cinemachine/CinemachineBrain")]
[SaveDuringPlay]
public class CinemachineBrain : MonoBehaviour
{
	[DocumentationSorting(0.1f, DocumentationSortingAttribute.Level.UserRef)]
	public enum UpdateMethod
	{
		FixedUpdate,
		LateUpdate,
		SmartUpdate
	}

	[Serializable]
	public class BrainEvent : UnityEvent<CinemachineBrain>
	{
	}

	[Serializable]
	public class VcamEvent : UnityEvent<ICinemachineCamera>
	{
	}

	private class OverrideStackFrame
	{
		public int id;

		public ICinemachineCamera camera;

		public CinemachineBlend blend;

		public float deltaTime;

		public float timeOfOverride;

		public bool Active => camera != null;

		public bool Expired
		{
			get
			{
				if (!Application.isPlaying)
				{
					return Time.realtimeSinceStartup - timeOfOverride > Time.maximumDeltaTime;
				}
				return false;
			}
		}
	}

	[Tooltip("When enabled, the current camera and blend will be indicated in the game window, for debugging")]
	public bool m_ShowDebugText;

	[Tooltip("When enabled, the camera's frustum will be shown at all times in the scene view")]
	public bool m_ShowCameraFrustum = true;

	[Tooltip("When enabled, the cameras will always respond in real-time to user input and damping, even if the game is running in slow motion")]
	public bool m_IgnoreTimeScale;

	[Tooltip("If set, this object's Y axis will define the worldspace Up vector for all the virtual cameras.  This is useful for instance in top-down game environments.  If not set, Up is worldspace Y.  Setting this appropriately is important, because Virtual Cameras don't like looking straight up or straight down.")]
	public Transform m_WorldUpOverride;

	[Tooltip("Use FixedUpdate if all your targets are animated during FixedUpdate (e.g. RigidBodies), LateUpdate if all your targets are animated during the normal Update loop, and SmartUpdate if you want Cinemachine to do the appropriate thing on a per-target basis.  SmartUpdate is the recommended setting")]
	public UpdateMethod m_UpdateMethod = UpdateMethod.SmartUpdate;

	[CinemachineBlendDefinitionProperty]
	[Tooltip("The blend that is used in cases where you haven't explicitly defined a blend between two Virtual Cameras")]
	public CinemachineBlendDefinition m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, 2f);

	[Tooltip("This is the asset that contains custom settings for blends between specific virtual cameras in your scene")]
	public CinemachineBlenderSettings m_CustomBlends;

	private Camera m_OutputCamera;

	[Tooltip("This event will fire whenever a virtual camera goes live and there is no blend")]
	public BrainEvent m_CameraCutEvent = new BrainEvent();

	[Tooltip("This event will fire whenever a virtual camera goes live.  If a blend is involved, then the event will fire on the first frame of the blend.")]
	public VcamEvent m_CameraActivatedEvent = new VcamEvent();

	internal static BrainEvent sPostProcessingHandler = new BrainEvent();

	private ICinemachineCamera mActiveCameraPreviousFrame;

	private ICinemachineCamera mOutgoingCameraPreviousFrame;

	private CinemachineBlend mActiveBlend;

	private bool mPreviousFrameWasOverride;

	private List<OverrideStackFrame> mOverrideStack = new List<OverrideStackFrame>();

	private int mNextOverrideId = 1;

	private OverrideStackFrame mOverrideBlendFromNothing = new OverrideStackFrame();

	private WaitForFixedUpdate mWaitForFixedUpdate = new WaitForFixedUpdate();

	private static int msCurrentFrame;

	private static int msFirstBrainObjectId;

	private static int msSubframes;

	public Camera OutputCamera
	{
		get
		{
			if (m_OutputCamera == null)
			{
				m_OutputCamera = GetComponent<Camera>();
			}
			return m_OutputCamera;
		}
	}

	internal Component PostProcessingComponent { get; set; }

	public static ICinemachineCamera SoloCamera { get; set; }

	public Vector3 DefaultWorldUp
	{
		get
		{
			if (!(m_WorldUpOverride != null))
			{
				return Vector3.up;
			}
			return m_WorldUpOverride.transform.up;
		}
	}

	public bool IsBlending
	{
		get
		{
			if (ActiveBlend != null)
			{
				return ActiveBlend.IsValid;
			}
			return false;
		}
	}

	public CinemachineBlend ActiveBlend
	{
		get
		{
			if (SoloCamera != null)
			{
				return null;
			}
			OverrideStackFrame activeOverride = GetActiveOverride();
			if (activeOverride == null || activeOverride.blend == null)
			{
				return mActiveBlend;
			}
			return activeOverride.blend;
		}
	}

	public ICinemachineCamera ActiveVirtualCamera
	{
		get
		{
			if (SoloCamera != null)
			{
				return SoloCamera;
			}
			OverrideStackFrame activeOverride = GetActiveOverride();
			if (activeOverride == null || activeOverride.camera == null)
			{
				return TopCameraFromPriorityQueue();
			}
			return activeOverride.camera;
		}
	}

	public CameraState CurrentCameraState { get; private set; }

	public static Color GetSoloGUIColor()
	{
		return Color.Lerp(Color.red, Color.yellow, 0.8f);
	}

	private OverrideStackFrame GetOverrideFrame(int id)
	{
		int count = mOverrideStack.Count;
		for (int i = 0; i < count; i++)
		{
			if (mOverrideStack[i].id == id)
			{
				return mOverrideStack[i];
			}
		}
		OverrideStackFrame overrideStackFrame = new OverrideStackFrame();
		overrideStackFrame.id = id;
		mOverrideStack.Insert(0, overrideStackFrame);
		return overrideStackFrame;
	}

	private OverrideStackFrame GetNextActiveFrame(int overrideId)
	{
		bool flag = false;
		int count = mOverrideStack.Count;
		for (int i = 0; i < count; i++)
		{
			if (mOverrideStack[i].id == overrideId)
			{
				flag = true;
			}
			else if (mOverrideStack[i].Active & flag)
			{
				return mOverrideStack[i];
			}
		}
		mOverrideBlendFromNothing.camera = TopCameraFromPriorityQueue();
		mOverrideBlendFromNothing.blend = mActiveBlend;
		return mOverrideBlendFromNothing;
	}

	private OverrideStackFrame GetActiveOverride()
	{
		int count = mOverrideStack.Count;
		for (int i = 0; i < count; i++)
		{
			if (mOverrideStack[i].Active)
			{
				return mOverrideStack[i];
			}
		}
		return null;
	}

	internal int SetCameraOverride(int overrideId, ICinemachineCamera camA, ICinemachineCamera camB, float weightB, float deltaTime)
	{
		if (overrideId < 0)
		{
			overrideId = mNextOverrideId++;
		}
		OverrideStackFrame overrideFrame = GetOverrideFrame(overrideId);
		overrideFrame.camera = null;
		overrideFrame.deltaTime = deltaTime;
		overrideFrame.timeOfOverride = Time.realtimeSinceStartup;
		if (camA != null || camB != null)
		{
			if (weightB <= 0.0001f)
			{
				overrideFrame.blend = null;
				if (camA != null)
				{
					overrideFrame.camera = camA;
				}
			}
			else if (weightB >= 0.9999f)
			{
				overrideFrame.blend = null;
				if (camB != null)
				{
					overrideFrame.camera = camB;
				}
			}
			else
			{
				if (camB == null)
				{
					ICinemachineCamera cinemachineCamera = camB;
					camB = camA;
					camA = cinemachineCamera;
					weightB = 1f - weightB;
				}
				if (camA == null)
				{
					OverrideStackFrame nextActiveFrame = GetNextActiveFrame(overrideId);
					camA = ((nextActiveFrame.blend == null) ? ((nextActiveFrame.camera != null) ? nextActiveFrame.camera : camB) : new BlendSourceVirtualCamera(nextActiveFrame.blend, deltaTime));
				}
				if (overrideFrame.blend == null)
				{
					overrideFrame.blend = new CinemachineBlend(camA, camB, AnimationCurve.Linear(0f, 0f, 1f, 1f), 1f, weightB);
				}
				overrideFrame.blend.CamA = camA;
				overrideFrame.blend.CamB = camB;
				overrideFrame.blend.TimeInBlend = weightB;
				overrideFrame.camera = camB;
			}
		}
		return overrideId;
	}

	internal void ReleaseCameraOverride(int overrideId)
	{
		int count = mOverrideStack.Count;
		for (int i = 0; i < count; i++)
		{
			if (mOverrideStack[i].id == overrideId)
			{
				mOverrideStack.RemoveAt(i);
				break;
			}
		}
	}

	private void OnEnable()
	{
		mActiveBlend = null;
		mActiveCameraPreviousFrame = null;
		mOutgoingCameraPreviousFrame = null;
		mPreviousFrameWasOverride = false;
		CinemachineCore.Instance.AddActiveBrain(this);
	}

	private void OnDisable()
	{
		CinemachineCore.Instance.RemoveActiveBrain(this);
		mActiveBlend = null;
		mActiveCameraPreviousFrame = null;
		mOutgoingCameraPreviousFrame = null;
		mPreviousFrameWasOverride = false;
		mOverrideStack.Clear();
	}

	private void Start()
	{
		UpdateVirtualCameras(CinemachineCore.UpdateFilter.Any, -1f);
		StartCoroutine(AfterPhysics());
	}

	private IEnumerator AfterPhysics()
	{
		while (true)
		{
			yield return mWaitForFixedUpdate;
			if (m_UpdateMethod == UpdateMethod.SmartUpdate)
			{
				AddSubframe();
				UpdateVirtualCameras(CinemachineCore.UpdateFilter.Fixed, GetEffectiveDeltaTime(fixedDelta: true));
			}
			else if (m_UpdateMethod == UpdateMethod.LateUpdate)
			{
				msSubframes = 1;
			}
			else
			{
				AddSubframe();
				UpdateVirtualCameras(CinemachineCore.UpdateFilter.Any, GetEffectiveDeltaTime(fixedDelta: true));
			}
		}
	}

	private void LateUpdate()
	{
		float effectiveDeltaTime = GetEffectiveDeltaTime(fixedDelta: false);
		if (m_UpdateMethod == UpdateMethod.SmartUpdate)
		{
			UpdateVirtualCameras(CinemachineCore.UpdateFilter.Late, effectiveDeltaTime);
		}
		else if (m_UpdateMethod == UpdateMethod.LateUpdate)
		{
			UpdateVirtualCameras(CinemachineCore.UpdateFilter.Any, effectiveDeltaTime);
		}
		ProcessActiveCamera(GetEffectiveDeltaTime(fixedDelta: false));
	}

	private float GetEffectiveDeltaTime(bool fixedDelta)
	{
		if (SoloCamera != null)
		{
			return Time.unscaledDeltaTime;
		}
		OverrideStackFrame activeOverride = GetActiveOverride();
		if (activeOverride != null)
		{
			if (!activeOverride.Expired)
			{
				return activeOverride.deltaTime;
			}
			return -1f;
		}
		if (!Application.isPlaying)
		{
			return -1f;
		}
		if (m_IgnoreTimeScale)
		{
			if (!fixedDelta)
			{
				return Time.unscaledDeltaTime;
			}
			return Time.fixedDeltaTime;
		}
		if (!fixedDelta)
		{
			return Time.deltaTime;
		}
		return Time.fixedDeltaTime * Time.timeScale;
	}

	private void UpdateVirtualCameras(CinemachineCore.UpdateFilter updateFilter, float deltaTime)
	{
		CinemachineCore.Instance.CurrentUpdateFilter = updateFilter;
		CinemachineCore.Instance.UpdateAllActiveVirtualCameras(DefaultWorldUp, deltaTime);
		ICinemachineCamera activeVirtualCamera = ActiveVirtualCamera;
		if (activeVirtualCamera != null)
		{
			CinemachineCore.Instance.UpdateVirtualCamera(activeVirtualCamera, DefaultWorldUp, deltaTime);
		}
		ActiveBlend?.UpdateCameraState(DefaultWorldUp, deltaTime);
		CinemachineCore.Instance.CurrentUpdateFilter = CinemachineCore.UpdateFilter.Any;
	}

	private void ProcessActiveCamera(float deltaTime)
	{
		if (!isActiveAndEnabled)
		{
			mActiveCameraPreviousFrame = null;
			mOutgoingCameraPreviousFrame = null;
			mPreviousFrameWasOverride = false;
			return;
		}
		OverrideStackFrame activeOverride = GetActiveOverride();
		ICinemachineCamera activeVirtualCamera = ActiveVirtualCamera;
		if (activeVirtualCamera == null || activeVirtualCamera.VirtualCameraGameObject == null)
		{
			mOutgoingCameraPreviousFrame = null;
		}
		else
		{
			if (activeOverride != null)
			{
				mActiveBlend = null;
			}
			CinemachineBlend cinemachineBlend = ActiveBlend;
			if (mActiveCameraPreviousFrame != null && mActiveCameraPreviousFrame.VirtualCameraGameObject == null)
			{
				mActiveCameraPreviousFrame = null;
			}
			if (mActiveCameraPreviousFrame != activeVirtualCamera)
			{
				if (mActiveCameraPreviousFrame != null && !mPreviousFrameWasOverride && activeOverride == null && deltaTime >= 0f)
				{
					float duration = 0f;
					AnimationCurve blendCurve = LookupBlendCurve(mActiveCameraPreviousFrame, activeVirtualCamera, out duration);
					cinemachineBlend = CreateBlend(mActiveCameraPreviousFrame, activeVirtualCamera, blendCurve, duration, mActiveBlend);
				}
				if (activeVirtualCamera != mOutgoingCameraPreviousFrame)
				{
					activeVirtualCamera.OnTransitionFromCamera(mActiveCameraPreviousFrame, DefaultWorldUp, deltaTime);
					if (!activeVirtualCamera.VirtualCameraGameObject.activeInHierarchy && (cinemachineBlend == null || !cinemachineBlend.Uses(activeVirtualCamera)))
					{
						activeVirtualCamera.UpdateCameraState(DefaultWorldUp, -1f);
					}
					if (m_CameraActivatedEvent != null)
					{
						m_CameraActivatedEvent.Invoke(activeVirtualCamera);
					}
				}
				if ((cinemachineBlend == null || (cinemachineBlend.CamA != mActiveCameraPreviousFrame && cinemachineBlend.CamB != mActiveCameraPreviousFrame && cinemachineBlend.CamA != mOutgoingCameraPreviousFrame && cinemachineBlend.CamB != mOutgoingCameraPreviousFrame)) && m_CameraCutEvent != null)
				{
					m_CameraCutEvent.Invoke(this);
				}
			}
			if (cinemachineBlend != null)
			{
				if (activeOverride == null)
				{
					cinemachineBlend.TimeInBlend += ((deltaTime >= 0f) ? deltaTime : cinemachineBlend.Duration);
				}
				if (cinemachineBlend.IsComplete)
				{
					cinemachineBlend = null;
				}
			}
			if (activeOverride == null)
			{
				mActiveBlend = cinemachineBlend;
			}
			CameraState state = activeVirtualCamera.State;
			if (cinemachineBlend != null)
			{
				state = cinemachineBlend.State;
			}
			PushStateToUnityCamera(state, activeVirtualCamera);
			mOutgoingCameraPreviousFrame = null;
			if (cinemachineBlend != null)
			{
				mOutgoingCameraPreviousFrame = cinemachineBlend.CamB;
			}
		}
		mActiveCameraPreviousFrame = activeVirtualCamera;
		mPreviousFrameWasOverride = activeOverride != null;
		if (mPreviousFrameWasOverride && activeOverride.blend != null)
		{
			if (activeOverride.blend.BlendWeight < 0.5f)
			{
				mActiveCameraPreviousFrame = activeOverride.blend.CamA;
				mOutgoingCameraPreviousFrame = activeOverride.blend.CamB;
			}
			else
			{
				mActiveCameraPreviousFrame = activeOverride.blend.CamB;
				mOutgoingCameraPreviousFrame = activeOverride.blend.CamA;
			}
		}
	}

	public bool IsLive(ICinemachineCamera vcam)
	{
		if (IsLiveItself(vcam))
		{
			return true;
		}
		ICinemachineCamera parentCamera = vcam.ParentCamera;
		while (parentCamera != null && parentCamera.IsLiveChild(vcam))
		{
			if (IsLiveItself(parentCamera))
			{
				return true;
			}
			vcam = parentCamera;
			parentCamera = vcam.ParentCamera;
		}
		return false;
	}

	private bool IsLiveItself(ICinemachineCamera vcam)
	{
		if (mActiveCameraPreviousFrame == vcam)
		{
			return true;
		}
		if (ActiveVirtualCamera == vcam)
		{
			return true;
		}
		if (IsBlending && ActiveBlend.Uses(vcam))
		{
			return true;
		}
		return false;
	}

	private ICinemachineCamera TopCameraFromPriorityQueue()
	{
		Camera outputCamera = OutputCamera;
		int num = ((outputCamera == null) ? (-1) : outputCamera.cullingMask);
		int virtualCameraCount = CinemachineCore.Instance.VirtualCameraCount;
		for (int i = 0; i < virtualCameraCount; i++)
		{
			ICinemachineCamera virtualCamera = CinemachineCore.Instance.GetVirtualCamera(i);
			GameObject gameObject = virtualCamera?.VirtualCameraGameObject;
			if (gameObject != null && (num & (1 << gameObject.layer)) != 0)
			{
				return virtualCamera;
			}
		}
		return null;
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

	private CinemachineBlend CreateBlend(ICinemachineCamera camA, ICinemachineCamera camB, AnimationCurve blendCurve, float duration, CinemachineBlend activeBlend)
	{
		if (blendCurve == null || duration <= 0f || (camA == null && camB == null))
		{
			return null;
		}
		if (camA == null || activeBlend != null)
		{
			CameraState state = CameraState.Default;
			if (activeBlend != null)
			{
				state = activeBlend.State;
			}
			else
			{
				state.RawPosition = transform.position;
				state.RawOrientation = transform.rotation;
				state.Lens = LensSettings.FromCamera(OutputCamera);
			}
			camA = new StaticPointVirtualCamera(state, (activeBlend == null) ? "(none)" : "Mid-blend");
		}
		return new CinemachineBlend(camA, camB, blendCurve, duration, 0f);
	}

	private void PushStateToUnityCamera(CameraState state, ICinemachineCamera vcam)
	{
		CurrentCameraState = state;
		transform.position = state.FinalPosition;
		transform.rotation = state.FinalOrientation;
		Camera outputCamera = OutputCamera;
		if (outputCamera != null)
		{
			outputCamera.fieldOfView = state.Lens.FieldOfView;
			outputCamera.orthographicSize = state.Lens.OrthographicSize;
			outputCamera.nearClipPlane = state.Lens.NearClipPlane;
			outputCamera.farClipPlane = state.Lens.FarClipPlane;
		}
		if (sPostProcessingHandler != null)
		{
			sPostProcessingHandler.Invoke(this);
		}
	}

	private void AddSubframe()
	{
		int frameCount = Time.frameCount;
		if (frameCount == msCurrentFrame)
		{
			if (msFirstBrainObjectId == GetInstanceID())
			{
				msSubframes++;
			}
		}
		else
		{
			msCurrentFrame = frameCount;
			msFirstBrainObjectId = GetInstanceID();
			msSubframes = 1;
		}
	}

	internal static int GetSubframeCount()
	{
		return Math.Max(1, msSubframes);
	}
}
