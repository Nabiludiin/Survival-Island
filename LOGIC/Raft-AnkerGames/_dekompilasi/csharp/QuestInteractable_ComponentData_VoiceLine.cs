using System;

[Serializable]
public class QuestInteractable_ComponentData_VoiceLine : QuestInteractable_ComponentData
{
	public Dialogue dialogue;

	public SO_VoiceData voiceLine;

	public HeadTrackingIK headIK;

	public bool stopOnDialogStart;

	public bool keepTrackingAfterDialogStoped;

	public override void OnFinishedInteraction(Network_Player player, bool atStart, bool successfull)
	{
		if (headIK != null)
		{
			if (stopOnDialogStart)
			{
				headIK.HeadTrackingOff();
			}
			else
			{
				Dialogue dialogue = this.dialogue;
				dialogue.OnVoiceLinePlayed = (Action<SO_VoiceLine>)Delegate.Combine(dialogue.OnVoiceLinePlayed, new Action<SO_VoiceLine>(StartIK));
				Dialogue dialogue2 = this.dialogue;
				dialogue2.OnVoiceDataCancelled = (Action<SO_VoiceData.Instance_VoiceData>)Delegate.Combine(dialogue2.OnVoiceDataCancelled, new Action<SO_VoiceData.Instance_VoiceData>(StopIK));
			}
		}
		base.OnFinishedInteraction(player, atStart, successfull);
		this.dialogue.PlayVoice(voiceLine, null);
	}

	private void StartIK(SO_VoiceLine vo)
	{
		Network_Player value = ComponentManager<Network_Player>.Value;
		if (headIK != null && value != null)
		{
			headIK.target = value.CameraTransform;
			headIK.HeadTrackingOn();
		}
	}

	private void StopIK(SO_VoiceData.Instance_VoiceData voInstance)
	{
		if (headIK.target != null && !keepTrackingAfterDialogStoped)
		{
			headIK.HeadTrackingOff();
		}
	}
}
