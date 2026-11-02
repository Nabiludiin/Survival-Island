using UnityEngine;

public class DialogueComponent_UnlockCharacter : DialogueComponent
{
	[SerializeField]
	private SO_Character characterToUnlock;

	protected override void OnVoiceDataPlayed(SO_VoiceData.Instance_VoiceData voiceDataInstance)
	{
		base.OnVoiceDataPlayed(voiceDataInstance);
		if (characterToUnlock == null)
		{
			Debug.LogError("Can't unlock character because 'characterToUnlock' reference is null");
		}
		else
		{
			CharacterManager.UnlockCharacterByIndex(characterToUnlock.index);
		}
	}
}
