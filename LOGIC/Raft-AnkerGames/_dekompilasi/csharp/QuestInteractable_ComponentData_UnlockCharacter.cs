using System;
using UnityEngine;

[Serializable]
public class QuestInteractable_ComponentData_UnlockCharacter : QuestInteractable_ComponentData
{
	public SO_Character characterToUnlock;

	public override void OnFinishedInteraction(Network_Player player, bool atStart, bool successfull)
	{
		base.OnFinishedInteraction(player, atStart, successfull);
		if (characterToUnlock == null)
		{
			Debug.LogError("Can't unlock character because 'characterToUnlock' reference is null");
		}
		else if (!atStart || Raft_Network.IsHost)
		{
			CharacterManager.UnlockCharacterByIndex(characterToUnlock.index);
		}
	}
}
