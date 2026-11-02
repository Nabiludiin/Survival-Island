using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BingoMenuItem : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	[HideInInspector]
	public ResearchMenuItem researchMenuItem;

	[SerializeField]
	private Image itemImage;

	[SerializeField]
	private GameObject researchCheckbox;

	private bool bingoState;

	private Item_Base bingoItem;

	public bool BingoState => bingoState;

	public Item_Base BingoItem
	{
		get
		{
			return bingoItem;
		}
		set
		{
			bingoItem = value;
		}
	}

	public void SetBingoState(bool state)
	{
		bingoState = state;
		if (BingoState)
		{
			itemImage.color = Color.white;
		}
		researchCheckbox.SetActive(state);
	}

	public void SetSprite(Sprite sprite)
	{
		itemImage.sprite = sprite;
		itemImage.color = new Color(1f, 1f, 1f, 0.5f);
		researchCheckbox.SetActive(value: false);
	}

	void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
	{
		if (researchMenuItem.localPlayer == null)
		{
			researchMenuItem.localPlayer = ComponentManager<Network_Player>.Value;
		}
		researchMenuItem.localPlayer.Inventory.SetItemDescription(BingoItem);
	}

	void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
	{
		if (researchMenuItem.localPlayer == null)
		{
			researchMenuItem.localPlayer = ComponentManager<Network_Player>.Value;
		}
		researchMenuItem.localPlayer.Inventory.SetItemDescription(null);
	}
}
