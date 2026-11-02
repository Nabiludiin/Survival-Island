using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingUI_CostBox_Sub : MonoBehaviour
{
	public Image itemImage;

	public Image backgroundImage;

	[HideInInspector]
	public Item_Base item;

	protected virtual void Start()
	{
		EventTrigger componentInChildren = GetComponentInChildren<EventTrigger>();
		BuildingUI_CostBox costBox = GetComponentInParent<BuildingUI_CostBox>();
		if (componentInChildren != null && costBox != null)
		{
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerEnter;
			entry.callback.AddListener((BaseEventData eventData) =>
			{
				costBox.PointerEnterSub(this);
			});
			componentInChildren.triggers.Add(entry);
			entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerExit;
			entry.callback.AddListener((BaseEventData eventData) =>
			{
				costBox.PointerExitSub(this);
			});
			componentInChildren.triggers.Add(entry);
		}
	}
}
