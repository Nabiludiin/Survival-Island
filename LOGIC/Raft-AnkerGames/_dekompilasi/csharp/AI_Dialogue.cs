using System;
using UnityEngine;

public class AI_Dialogue : Dialogue
{
	[SerializeField]
	private TriggerBoxEnterAction enterZone;

	[SerializeField]
	private SO_AI_DialogueSettings ai_dialogueSettings;

	public override void Awake()
	{
		base.Awake();
		if (Raft_Network.IsHost)
		{
			TriggerBoxEnterAction triggerBoxEnterAction = enterZone;
			triggerBoxEnterAction.OnTriggerAction = (Action<Collider>)Delegate.Combine(triggerBoxEnterAction.OnTriggerAction, new Action<Collider>(PlayerEnteredArea));
		}
	}

	private void PlayerEnteredArea(Collider other)
	{
		if (!IsVoiceActive)
		{
			Network_Player componentInParent = other.GetComponentInParent<Network_Player>();
			if (!(componentInParent == null) && UnityEngine.Random.Range(0, 100) < ai_dialogueSettings.enterAreaSpeechPrevelancyPercentage)
			{
				PlayNextVoice_Networked(componentInParent);
			}
		}
	}
}
