using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notification_CharacterUnlock : Notification_Animated
{
	public Queue<Notification_CharacterUnlock_Info> infoQue = new Queue<Notification_CharacterUnlock_Info>();

	[SerializeField]
	private Text text_characterName;

	[SerializeField]
	private Image image_characterImage;

	public override void Show()
	{
		Notification_CharacterUnlock_Info notification_CharacterUnlock_Info = infoQue.Dequeue();
		text_characterName.text = notification_CharacterUnlock_Info.characterName;
		image_characterImage.sprite = notification_CharacterUnlock_Info.characterSprite;
		base.Show();
	}
}
