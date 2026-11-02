using System.Collections.Generic;
using UnityEngine;

namespace Cinemachine;

public sealed class CinemachineCore
{
	public enum Stage
	{
		Body,
		Aim,
		Noise
	}

	public delegate float AxisInputDelegate(string axisName);

	private struct UpdateStatus(int currentFrame)
	{
		private const int kWindowSize = 30;

		public int lastUpdateFrame = -1;

		public int lastUpdateSubframe = 0;

		public int windowStart = currentFrame;

		public int numWindowLateUpdateMoves = 0;

		public int numWindowFixedUpdateMoves = 0;

		public int numWindows = 0;

		public UpdateFilter preferredUpdate = UpdateFilter.Late;

		public Matrix4x4 targetPos = Matrix4x4.zero;

		public UpdateFilter ChoosePreferredUpdate(int currentFrame, Matrix4x4 pos, UpdateFilter updateFilter)
		{
			if (targetPos != pos)
			{
				if (updateFilter == UpdateFilter.Late)
				{
					numWindowLateUpdateMoves++;
				}
				else if (lastUpdateSubframe == 0)
				{
					numWindowFixedUpdateMoves++;
				}
				targetPos = pos;
			}
			UpdateFilter updateFilter2 = preferredUpdate;
			updateFilter2 = (((numWindowLateUpdateMoves > 0 && numWindowFixedUpdateMoves > 0) || numWindowLateUpdateMoves >= numWindowFixedUpdateMoves) ? UpdateFilter.Late : UpdateFilter.Fixed);
			if (numWindows == 0)
			{
				preferredUpdate = updateFilter2;
			}
			if (windowStart + 30 <= currentFrame)
			{
				preferredUpdate = updateFilter2;
				numWindows++;
				windowStart = currentFrame;
				numWindowLateUpdateMoves = (numWindowFixedUpdateMoves = 0);
			}
			return preferredUpdate;
		}
	}

	public enum UpdateFilter
	{
		Fixed,
		Late,
		Any
	}

	public static readonly int kStreamingVersion = 20170927;

	public static readonly string kVersionString = "2.1";

	private static CinemachineCore sInstance = null;

	public static bool sShowHiddenObjects = false;

	public static AxisInputDelegate GetInputAxis = Input.GetAxis;

	private List<CinemachineBrain> mActiveBrains = new List<CinemachineBrain>();

	private List<ICinemachineCamera> mActiveCameras = new List<ICinemachineCamera>();

	private List<List<ICinemachineCamera>> mChildCameras = new List<List<ICinemachineCamera>>();

	private Dictionary<ICinemachineCamera, UpdateStatus> mUpdateStatus;

	public static CinemachineCore Instance
	{
		get
		{
			if (sInstance == null)
			{
				sInstance = new CinemachineCore();
			}
			return sInstance;
		}
	}

	public int BrainCount => mActiveBrains.Count;

	public int VirtualCameraCount => mActiveCameras.Count;

	internal UpdateFilter CurrentUpdateFilter { get; set; }

	public CinemachineBrain GetActiveBrain(int index)
	{
		return mActiveBrains[index];
	}

	internal void AddActiveBrain(CinemachineBrain brain)
	{
		RemoveActiveBrain(brain);
		mActiveBrains.Insert(0, brain);
	}

	internal void RemoveActiveBrain(CinemachineBrain brain)
	{
		mActiveBrains.Remove(brain);
	}

	public ICinemachineCamera GetVirtualCamera(int index)
	{
		return mActiveCameras[index];
	}

	internal void AddActiveCamera(ICinemachineCamera vcam)
	{
		RemoveActiveCamera(vcam);
		int i;
		for (i = 0; i < mActiveCameras.Count && vcam.Priority < mActiveCameras[i].Priority; i++)
		{
		}
		mActiveCameras.Insert(i, vcam);
	}

	internal void RemoveActiveCamera(ICinemachineCamera vcam)
	{
		mActiveCameras.Remove(vcam);
	}

	internal void AddChildCamera(ICinemachineCamera vcam)
	{
		RemoveChildCamera(vcam);
		int num = 0;
		for (ICinemachineCamera cinemachineCamera = vcam; cinemachineCamera != null; cinemachineCamera = cinemachineCamera.ParentCamera)
		{
			num++;
		}
		while (mChildCameras.Count < num)
		{
			mChildCameras.Add(new List<ICinemachineCamera>());
		}
		mChildCameras[num - 1].Add(vcam);
	}

	internal void RemoveChildCamera(ICinemachineCamera vcam)
	{
		for (int i = 0; i < mChildCameras.Count; i++)
		{
			mChildCameras[i].Remove(vcam);
		}
	}

	internal void UpdateAllActiveVirtualCameras(Vector3 worldUp, float deltaTime)
	{
		int virtualCameraCount;
		if (CurrentUpdateFilter == UpdateFilter.Any || CurrentUpdateFilter == UpdateFilter.Late)
		{
			virtualCameraCount = VirtualCameraCount;
			for (int i = 0; i < virtualCameraCount; i++)
			{
				GetVirtualCamera(i).PreUpdateChildCameras(worldUp, deltaTime);
			}
			for (int j = 0; j < mChildCameras.Count - 1; j++)
			{
				virtualCameraCount = mChildCameras[j].Count;
				for (int k = 0; k < virtualCameraCount; k++)
				{
					mChildCameras[j][k].PreUpdateChildCameras(worldUp, deltaTime);
				}
			}
		}
		for (int num = mChildCameras.Count - 1; num >= 0; num--)
		{
			virtualCameraCount = mChildCameras[num].Count;
			for (int l = 0; l < virtualCameraCount; l++)
			{
				UpdateVirtualCamera(mChildCameras[num][l], worldUp, deltaTime);
			}
		}
		virtualCameraCount = VirtualCameraCount;
		for (int m = 0; m < virtualCameraCount; m++)
		{
			UpdateVirtualCamera(GetVirtualCamera(m), worldUp, deltaTime);
		}
	}

	internal bool UpdateVirtualCamera(ICinemachineCamera vcam, Vector3 worldUp, float deltaTime)
	{
		int frameCount = Time.frameCount;
		bool flag = CurrentUpdateFilter != UpdateFilter.Any;
		bool flag2 = CurrentUpdateFilter == UpdateFilter.Late;
		if (mUpdateStatus == null)
		{
			mUpdateStatus = new Dictionary<ICinemachineCamera, UpdateStatus>();
		}
		if (vcam.VirtualCameraGameObject == null)
		{
			if (mUpdateStatus.ContainsKey(vcam))
			{
				mUpdateStatus.Remove(vcam);
			}
			return false;
		}
		if (!mUpdateStatus.TryGetValue(vcam, out var value))
		{
			value = new UpdateStatus(frameCount);
			mUpdateStatus.Add(vcam, value);
		}
		int num = (flag2 ? 1 : CinemachineBrain.GetSubframeCount());
		if (value.lastUpdateFrame != frameCount)
		{
			value.lastUpdateSubframe = 0;
		}
		bool flag3 = !flag;
		if (flag)
		{
			flag3 = (GetTargetPosition(vcam, out var targetPos) ? (value.ChoosePreferredUpdate(frameCount, targetPos, CurrentUpdateFilter) == CurrentUpdateFilter) : flag2);
		}
		if (flag3)
		{
			value.preferredUpdate = CurrentUpdateFilter;
			while (value.lastUpdateSubframe < num)
			{
				vcam.UpdateCameraState(worldUp, deltaTime);
				value.lastUpdateSubframe++;
			}
			value.lastUpdateFrame = frameCount;
		}
		mUpdateStatus[vcam] = value;
		return true;
	}

	private static bool GetTargetPosition(ICinemachineCamera vcam, out Matrix4x4 targetPos)
	{
		ICinemachineCamera liveChildOrSelf = vcam.LiveChildOrSelf;
		if (liveChildOrSelf == null || liveChildOrSelf.VirtualCameraGameObject == null)
		{
			targetPos = Matrix4x4.identity;
			return false;
		}
		targetPos = liveChildOrSelf.VirtualCameraGameObject.transform.localToWorldMatrix;
		if (liveChildOrSelf.LookAt != null)
		{
			targetPos = liveChildOrSelf.LookAt.localToWorldMatrix;
			return true;
		}
		if (liveChildOrSelf.Follow != null)
		{
			targetPos = liveChildOrSelf.Follow.localToWorldMatrix;
			return true;
		}
		targetPos = vcam.VirtualCameraGameObject.transform.localToWorldMatrix;
		return true;
	}

	public UpdateFilter GetVcamUpdateStatus(ICinemachineCamera vcam)
	{
		if (mUpdateStatus == null || !mUpdateStatus.TryGetValue(vcam, out var value))
		{
			return UpdateFilter.Any;
		}
		return value.preferredUpdate;
	}

	public bool IsLive(ICinemachineCamera vcam)
	{
		if (vcam != null)
		{
			for (int i = 0; i < BrainCount; i++)
			{
				CinemachineBrain activeBrain = GetActiveBrain(i);
				if (activeBrain != null && activeBrain.IsLive(vcam))
				{
					return true;
				}
			}
		}
		return false;
	}

	public void GenerateCameraActivationEvent(ICinemachineCamera vcam)
	{
		if (vcam == null)
		{
			return;
		}
		for (int i = 0; i < BrainCount; i++)
		{
			CinemachineBrain activeBrain = GetActiveBrain(i);
			if (activeBrain != null && activeBrain.IsLive(vcam))
			{
				activeBrain.m_CameraActivatedEvent.Invoke(vcam);
			}
		}
	}

	public void GenerateCameraCutEvent(ICinemachineCamera vcam)
	{
		if (vcam == null)
		{
			return;
		}
		for (int i = 0; i < BrainCount; i++)
		{
			CinemachineBrain activeBrain = GetActiveBrain(i);
			if (activeBrain != null && activeBrain.IsLive(vcam))
			{
				activeBrain.m_CameraCutEvent.Invoke(activeBrain);
			}
		}
	}

	public CinemachineBrain FindPotentialTargetBrain(ICinemachineCamera vcam)
	{
		int brainCount = BrainCount;
		if (vcam != null && brainCount > 1)
		{
			for (int i = 0; i < brainCount; i++)
			{
				CinemachineBrain activeBrain = GetActiveBrain(i);
				if (activeBrain != null && activeBrain.OutputCamera != null && activeBrain.IsLive(vcam))
				{
					return activeBrain;
				}
			}
		}
		for (int j = 0; j < brainCount; j++)
		{
			CinemachineBrain activeBrain2 = GetActiveBrain(j);
			if (activeBrain2 != null && activeBrain2.OutputCamera != null)
			{
				return activeBrain2;
			}
		}
		return null;
	}
}
