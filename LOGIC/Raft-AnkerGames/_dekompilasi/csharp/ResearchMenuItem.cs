using System.Collections.Generic;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

public class ResearchMenuItem : MonoBehaviour
{
	public delegate void OnLearnedRecipe();

	[SerializeField]
	private Image itemImage;

	[SerializeField]
	private Image backgroundImage;

	[SerializeField]
	private Image backgroundItemImage;

	[SerializeField]
	private BingoMenuItem bingoItemPrefab;

	[SerializeField]
	private RectTransform bingoParent;

	[SerializeField]
	private Button learnButton;

	[SerializeField]
	private Text learnedText;

	[SerializeField]
	private Text learnText;

	[SerializeField]
	private CanvasGroup canvasgroup;

	[HideInInspector]
	public Network_Player localPlayer;

	[SerializeField]
	private GameObject gamepadHelp;

	private CraftingMenu craftingMenu;

	private Item_Base item;

	private List<BingoMenuItem> bingoMenuItems = new List<BingoMenuItem>();

	private bool learned;

	private Inventory_ResearchTable inventoryRef;

	[SerializeField]
	private UiMagnet iconMagnet;

	public bool Learned => learned;

	public float SortBingoPercent
	{
		get
		{
			if (learned)
			{
				return -1f;
			}
			float num = 0f;
			for (int i = 0; i < bingoMenuItems.Count; i++)
			{
				if (bingoMenuItems[i] == null)
				{
					num++;
				}
				else if (bingoMenuItems[i].BingoState)
				{
					num++;
				}
			}
			return num / (float)bingoMenuItems.Count;
		}
	}

	public event OnLearnedRecipe OnLearnedRecipeEvent;

	private void Start()
	{
		localPlayer = ComponentManager<Network_Player>.Value;
	}

	private void OnEnable()
	{
		if (!SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
		{
			backgroundItemImage.sprite = ComponentManager<CraftingMenu>.Value.spriteNormal;
			gamepadHelp.SetActive(value: false);
		}
	}

	private void OnDestroy()
	{
		bingoMenuItems.Clear();
		localPlayer = null;
		inventoryRef = null;
		craftingMenu = null;
	}

	public void Initialize(CraftingMenu craftingMenu, Inventory_ResearchTable inventoryRef)
	{
		this.inventoryRef = inventoryRef;
		this.craftingMenu = craftingMenu;
		learnedText.gameObject.SetActive(value: false);
		learnButton.gameObject.SetActive(value: false);
		learnText.gameObject.SetActive(value: false);
	}

	public void LearnInstantly()
	{
		for (int i = 0; i < bingoMenuItems.Count; i++)
		{
			Research(bingoMenuItems[i].BingoItem);
		}
		Learn();
		itemImage.sprite = item.settings_Inventory.Sprite;
	}

	public void LearnButton()
	{
		if (item.settings_recipe.Learned)
		{
			return;
		}
		if (localPlayer == null)
		{
			localPlayer = ComponentManager<Network_Player>.Value;
		}
		Message_ResearchTable_ResearchOrLearn message = new Message_ResearchTable_ResearchOrLearn(Messages.ResearchTable_Learn, localPlayer, localPlayer.steamID, item.UniqueIndex);
		if (Raft_Network.IsHost)
		{
			inventoryRef.network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			inventoryRef.LearnItem(item, localPlayer.steamID);
			if (OnLearnedRecipeEvent != null)
			{
				OnLearnedRecipeEvent();
			}
		}
		else
		{
			inventoryRef.network.SendP2P(inventoryRef.network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
		inventoryRef.TestResearcherAchievement(autoLearnRecipe: false);
	}

	public void Learn()
	{
		learned = true;
		canvasgroup.alpha = 0.5f;
		item.settings_recipe.Learned = true;
		learnButton.gameObject.SetActive(value: false);
		learnedText.gameObject.SetActive(value: true);
		learnText.gameObject.SetActive(value: false);
		if (CanvasHelper.ActiveMenu == MenuType.Inventory)
		{
			craftingMenu.ReselectCategory();
		}
	}

	private bool GotBingo()
	{
		int num = 0;
		foreach (BingoMenuItem bingoMenuItem in bingoMenuItems)
		{
			if (bingoMenuItem.BingoState)
			{
				num++;
			}
		}
		return num == bingoMenuItems.Count;
	}

	public void Research(Item_Base item)
	{
		if (SortBingoPercent == 1f)
		{
			return;
		}
		int num = 0;
		foreach (BingoMenuItem bingoMenuItem in bingoMenuItems)
		{
			if (bingoMenuItem.BingoItem == item)
			{
				bingoMenuItem.SetBingoState(state: true);
			}
			if (bingoMenuItem.BingoState)
			{
				num++;
			}
		}
		if (num == bingoMenuItems.Count)
		{
			Bingo();
		}
	}

	public void SetBackgroundAlpha(float alpha)
	{
		Color color = backgroundImage.color;
		color.a = alpha / 255f;
		backgroundImage.color = color;
	}

	public void SetItem(Item_Base item)
	{
		this.item = item;
		itemImage.sprite = item.settings_Inventory.Sprite;
		itemImage.color = new Color(1f, 1f, 1f, 0.5f);
		CostMultiple[] newCost = item.settings_recipe.NewCost;
		for (int i = 0; i < newCost.Length; i++)
		{
			BingoMenuItem bingoMenuItem = Object.Instantiate(bingoItemPrefab, bingoParent);
			bingoMenuItem.transform.position = Vector3.zero;
			bingoMenuItem.SetSprite(newCost[i].items[0].settings_Inventory.Sprite);
			bingoMenuItem.BingoItem = newCost[i].items[0];
			bingoMenuItem.researchMenuItem = this;
			bingoMenuItems.Add(bingoMenuItem);
		}
	}

	public Item_Base GetItem()
	{
		return item;
	}

	public void Bingo()
	{
		if (!learned)
		{
			learnButton.interactable = true;
			itemImage.sprite = item.settings_Inventory.Sprite;
			if (!SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
			{
				learnButton.gameObject.SetActive(value: true);
				learnText.gameObject.SetActive(value: false);
			}
			else
			{
				learnButton.gameObject.SetActive(value: false);
				learnText.gameObject.SetActive(value: true);
			}
			itemImage.color = Color.white;
		}
	}

	public bool ContainsItem(Item_Base item)
	{
		for (int i = 0; i < bingoMenuItems.Count; i++)
		{
			if (bingoMenuItems[i].BingoItem == item)
			{
				return true;
			}
		}
		return false;
	}

	public void PointerEnter()
	{
		if (localPlayer == null)
		{
			localPlayer = ComponentManager<Network_Player>.Value;
		}
		localPlayer.Inventory.SetItemDescription(item);
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
		{
			backgroundItemImage.sprite = ComponentManager<CraftingMenu>.Value.spriteHoover;
			gamepadHelp.SetActive(GotBingo() && !learnedText.gameObject.activeSelf);
		}
	}

	public void PointerExit()
	{
		if (localPlayer == null)
		{
			localPlayer = ComponentManager<Network_Player>.Value;
		}
		localPlayer.Inventory.SetItemDescription(null);
		backgroundItemImage.sprite = ComponentManager<CraftingMenu>.Value.spriteNormal;
		gamepadHelp.SetActive(value: false);
	}

	public void SetupViewport(RectTransform rectTransform)
	{
		iconMagnet.Viewport = rectTransform;
	}
}
