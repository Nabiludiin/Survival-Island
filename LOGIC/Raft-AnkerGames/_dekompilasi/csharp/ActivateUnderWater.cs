using System;
using UltimateWater;
using UnityEngine;

public class ActivateUnderWater : MonoBehaviour, SceneEventInterface
{
	public bool activeScript = true;

	public bool reverse;

	private SubmersionState submersionState;

	public void OnSceneEvent(SceneEvent sceneEvent)
	{
		switch (sceneEvent)
		{
		case SceneEvent.JoinGame:
			PersonController.CameraSubmersionAction = (Action<SubmersionState>)Delegate.Combine(PersonController.CameraSubmersionAction, new Action<SubmersionState>(PersonController_CameraSubmersionStateEvent));
			break;
		case SceneEvent.LeaveGame:
			PersonController.CameraSubmersionAction = (Action<SubmersionState>)Delegate.Remove(PersonController.CameraSubmersionAction, new Action<SubmersionState>(PersonController_CameraSubmersionStateEvent));
			break;
		}
	}

	private void OnDestroy()
	{
		PersonController.CameraSubmersionAction = (Action<SubmersionState>)Delegate.Remove(PersonController.CameraSubmersionAction, new Action<SubmersionState>(PersonController_CameraSubmersionStateEvent));
	}

	private void OnEnable()
	{
		SetActiveState();
	}

	private void PersonController_CameraSubmersionStateEvent(SubmersionState state)
	{
		submersionState = state;
		SetActiveState();
	}

	private void SetActiveState()
	{
		if (!(this == null) && !(gameObject == null) && activeScript)
		{
			bool flag = submersionState == SubmersionState.Full;
			if (reverse)
			{
				flag = !flag;
			}
			gameObject.SetActiveSafe(flag);
		}
	}
}
