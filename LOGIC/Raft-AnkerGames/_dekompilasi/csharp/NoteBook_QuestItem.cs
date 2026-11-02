using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NoteBook_QuestItem : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	[SerializeField]
	private Image backgroundImage;

	[SerializeField]
	private Image itemImage;

	[SerializeField]
	private Image textBackground;

	[SerializeField]
	private TextMeshProUGUI itemAmount;

	[SerializeField]
	private TextMeshProUGUI onHoverDisplayText;

	private Animator anim;

	private QuestItem questItem;

	private void Awake()
	{
		anim = GetComponent<Animator>();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (anim != null)
		{
			anim.SetTrigger("Highlighted");
		}
		if (onHoverDisplayText != null)
		{
			onHoverDisplayText.text = questItem.SOQuestItem.DisplayName;
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (anim != null)
		{
			anim.SetTrigger("Normal");
		}
		if (onHoverDisplayText != null)
		{
			onHoverDisplayText.text = string.Empty;
		}
	}

	public void SetItem(QuestItem questItem, Color backgroundColor)
	{
		this.questItem = questItem;
		backgroundImage.color = backgroundColor;
		itemImage.sprite = questItem.SOQuestItem.itemImage;
		itemAmount.text = questItem.itemCount.ToString();
		bool value = questItem.itemCount > 1;
		itemAmount.gameObject.SetActiveSafe(value);
		textBackground.gameObject.SetActiveSafe(value);
	}
}
