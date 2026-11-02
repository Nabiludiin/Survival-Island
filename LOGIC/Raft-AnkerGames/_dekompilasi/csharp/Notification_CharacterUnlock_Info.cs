using UnityEngine;

public class Notification_CharacterUnlock_Info
{
	public string characterName;

	public Sprite characterSprite;

	public Notification_CharacterUnlock_Info(SO_Character characterData)
	{
		characterName = characterData.displayName;
		characterSprite = characterData.sprite;
	}
}
