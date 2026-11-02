using System;
using UnityEngine;

public class DialogueHeadtrackingIKConnection : MonoBehaviour
{
	public HeadTrackingIK headIK;

	public Dialogue dialogue;

	public bool autoStart;

	private void OnDestroy()
	{
		if (this.dialogue != null)
		{
			Dialogue dialogue = this.dialogue;
			dialogue.OnVoiceLinePlayed = (Action<SO_VoiceLine>)Delegate.Remove(dialogue.OnVoiceLinePlayed, new Action<SO_VoiceLine>(StartIK));
			Dialogue dialogue2 = this.dialogue;
			dialogue2.OnVoiceDataCancelled = (Action<SO_VoiceData.Instance_VoiceData>)Delegate.Remove(dialogue2.OnVoiceDataCancelled, new Action<SO_VoiceData.Instance_VoiceData>(StopIK));
		}
	}

	private void Start()
	{
		if (autoStart && dialogue != null)
		{
			ManuallStart(dialogue);
		}
	}

	public void ManuallStart(Dialogue dialogue)
	{
		if (dialogue != null)
		{
			this.dialogue = dialogue;
			dialogue.OnVoiceLinePlayed = (Action<SO_VoiceLine>)Delegate.Combine(dialogue.OnVoiceLinePlayed, new Action<SO_VoiceLine>(StartIK));
			dialogue.OnVoiceDataCancelled = (Action<SO_VoiceData.Instance_VoiceData>)Delegate.Combine(dialogue.OnVoiceDataCancelled, new Action<SO_VoiceData.Instance_VoiceData>(StopIK));
		}
	}

	private void StartIK(SO_VoiceLine vl)
	{
		if (headIK == null)
		{
			return;
		}
		headIK.HeadTrackingOn();
		if (dialogue != null && dialogue.currentDialogueInstance != null && dialogue.currentDialogueInstance.startedPlayer != null)
		{
			headIK.target = dialogue.currentDialogueInstance.startedPlayer.CameraTransform;
			return;
		}
		Network_Player value = ComponentManager<Network_Player>.Value;
		if (value != null)
		{
			headIK.target = value.CameraTransform;
		}
	}

	private void StopIK(SO_VoiceData.Instance_VoiceData vdInstance)
	{
		if (!(headIK == null))
		{
			headIK.HeadTrackingOff();
		}
	}
}
