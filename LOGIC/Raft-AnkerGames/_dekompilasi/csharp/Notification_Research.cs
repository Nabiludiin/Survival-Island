using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notification_Research : Notification_Animated
{
	public Queue<Notification_Research_Info> researchInfoQue = new Queue<Notification_Research_Info>();

	[SerializeField]
	private Text text_learnedItem;

	[SerializeField]
	private Text text_researchedBy;

	[SerializeField]
	private Image image_item;

	public override void Show()
	{
		Notification_Research_Info notification_Research_Info = researchInfoQue.Dequeue();
		LocalizationParameters.remoteSteamID = notification_Research_Info.researcher;
		text_researchedBy.text = Helper.GetTerm("Game/ResearchedBy", applyParameters: true);
		text_learnedItem.text = notification_Research_Info.itemName;
		image_item.sprite = notification_Research_Info.itemSprite;
		base.Show();
	}
}
