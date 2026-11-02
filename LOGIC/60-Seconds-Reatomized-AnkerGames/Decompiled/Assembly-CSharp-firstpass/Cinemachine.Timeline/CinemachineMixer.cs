using UnityEngine;
using UnityEngine.Playables;

namespace Cinemachine.Timeline;

public sealed class CinemachineMixer : PlayableBehaviour
{
	private CinemachineBrain mBrain;

	private int mBrainOverrideId = -1;

	private bool mPlaying;

	private float mLastOverrideFrame;

	public override void OnGraphStop(Playable playable)
	{
		if (mBrain != null)
		{
			mBrain.ReleaseCameraOverride(mBrainOverrideId);
		}
		mBrainOverrideId = -1;
	}

	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		base.ProcessFrame(playable, info, playerData);
		GameObject gameObject = playerData as GameObject;
		if (gameObject == null)
		{
			mBrain = (CinemachineBrain)playerData;
		}
		else
		{
			mBrain = gameObject.GetComponent<CinemachineBrain>();
		}
		if (mBrain == null)
		{
			return;
		}
		int num = 0;
		ICinemachineCamera cinemachineCamera = null;
		ICinemachineCamera camA = null;
		float weightB = 1f;
		for (int i = 0; i < playable.GetInputCount(); i++)
		{
			CinemachineShotPlayable behaviour = ((ScriptPlayable<CinemachineShotPlayable>)playable.GetInput(i)).GetBehaviour();
			float inputWeight = playable.GetInputWeight(i);
			if (behaviour != null && behaviour.VirtualCamera != null && playable.GetPlayState() == PlayState.Playing && inputWeight > 0.0001f)
			{
				if (num == 1)
				{
					camA = cinemachineCamera;
				}
				weightB = inputWeight;
				cinemachineCamera = behaviour.VirtualCamera;
				num++;
				if (num == 2)
				{
					break;
				}
			}
		}
		float deltaTime = info.deltaTime;
		if (!mPlaying)
		{
			if (mBrainOverrideId < 0)
			{
				mLastOverrideFrame = -1f;
			}
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			deltaTime = Time.unscaledDeltaTime;
			if (!Application.isPlaying && (mLastOverrideFrame < 0f || realtimeSinceStartup - mLastOverrideFrame > Time.maximumDeltaTime))
			{
				deltaTime = -1f;
			}
			mLastOverrideFrame = realtimeSinceStartup;
		}
		mBrainOverrideId = mBrain.SetCameraOverride(mBrainOverrideId, camA, cinemachineCamera, weightB, deltaTime);
	}

	public override void PrepareFrame(Playable playable, FrameData info)
	{
		mPlaying = info.evaluationType == FrameData.EvaluationType.Playback;
	}
}
