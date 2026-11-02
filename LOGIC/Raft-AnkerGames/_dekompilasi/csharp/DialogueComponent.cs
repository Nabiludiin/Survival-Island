using System;
using UnityEngine;

public class DialogueComponent : MonoBehaviour
{
	[SerializeField]
	private Dialogue dialogue;

	private void Start()
	{
		Dialogue dialogue = this.dialogue;
		dialogue.OnVoiceDataPlayed = (Action<SO_VoiceData.Instance_VoiceData>)Delegate.Combine(dialogue.OnVoiceDataPlayed, new Action<SO_VoiceData.Instance_VoiceData>(OnVoiceDataPlayed));
	}

	protected virtual void OnVoiceDataPlayed(SO_VoiceData.Instance_VoiceData voiceDataInstance)
	{
	}
}
