using UnityEngine;
using UnityEngine.Serialization;

public class QuestInteractable_ComponentData
{
	public QuestInteractable_TriggeredAt triggeredAt;

	[Tooltip("-1 means all indexes triggers the component")]
	public int interactionIndex = -1;

	public SO_VoiceData voiceData;

	[FormerlySerializedAs("interactionTriggerDelay")]
	public float delay;

	public virtual void OnFinishedInteraction(Network_Player player, bool atStart, bool successfull)
	{
	}
}
