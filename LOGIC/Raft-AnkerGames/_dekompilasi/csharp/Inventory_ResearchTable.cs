using System;
using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_ResearchTable : Inventory, SceneEventInterface
{
	public static bool hasBeenSaved;

	[SerializeField]
	private ResearchMenuItem menuItemPrefab;

	[SerializeField]
	private RectTransform content;

	[SerializeField]
	private RectTransform researchItemContent;

	[SerializeField]
	private Button researchButton;

	[SerializeField]
	private Text researchedText;

	[SerializeField]
	private Text cantBeResearchedText;

	[SerializeField]
	private float menuItemHeight = 60f;

	[SerializeField]
	private AvaialableResearchItem availableItemPrefab;

	[SerializeField]
	private SO_MysteryPackageLoot mysteryPackageLoot;

	[SerializeField]
	private Scrollbar scrollbar;

	public EventReference er_research;

	public EventReference er_learn;

	private List<ResearchMenuItem> menuItems = new List<ResearchMenuItem>();

	private List<Item_Base> researchedItems = new List<Item_Base>();

	private Dictionary<Item_Base, AvaialableResearchItem> availableResearchItems = new Dictionary<Item_Base, AvaialableResearchItem>();

	public ResearchTable activeTable;

	[HideInInspector]
	public Raft_Network network;

	private NotificationManager notificationManager;

	public static Item_Base ResearchItem;

	protected override void Awake()
	{
		base.Awake();
		Raft_Network.OnWorldReceivedLate = (Action)Delegate.Combine(Raft_Network.OnWorldReceivedLate, new Action(OnWorldRecievedLate));
	}

	protected override void Start()
	{
		base.Start();
		notificationManager = ComponentManager<NotificationManager>.Value;
		network = ComponentManager<Raft_Network>.Value;
		CreateMenuItems(UnityEngine.Object.FindObjectOfType<CraftingMenu>());
	}

	private void OnEnable()
	{
		scrollbar.interactable = !SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance || !SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad;
	}

	private void OnDestroy()
	{
		ResearchItem = null;
	}

	private void OnWorldRecievedLate()
	{
		if (GameModeValueManager.GetCurrentGameModeValue().playerSpecificVariables.learnAllRecipiesAtStart)
		{
			LearnAllRecipesInstantly();
		}
	}

	protected override void Update()
	{
		base.Update();
		ResearchItem = null;
		if (activeTable == null || !activeTable.IsOccupied || activeTable.slot == null)
		{
			return;
		}
		if (activeTable.slot.IsEmpty)
		{
			researchButton.interactable = false;
			researchButton.gameObject.SetActive(value: false);
			researchedText.gameObject.SetActive(value: false);
			cantBeResearchedText.gameObject.SetActive(value: false);
			return;
		}
		Item_Base baseItem = activeTable.slot.itemInstance.baseItem;
		if (HasBeenResearched(baseItem))
		{
			researchedText.gameObject.SetActive(value: true);
			if (!SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
			{
				researchButton.gameObject.SetActive(value: false);
			}
			else
			{
				cantBeResearchedText.gameObject.SetActive(value: false);
			}
			return;
		}
		researchButton.interactable = CanResearchItem(baseItem);
		if (!SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
		{
			researchButton.gameObject.SetActive(value: true);
		}
		else
		{
			researchButton.gameObject.SetActive(value: false);
			cantBeResearchedText.gameObject.SetActive(!researchButton.interactable);
		}
		researchedText.gameObject.SetActive(value: false);
		ResearchItem = (researchButton.interactable ? baseItem : null);
	}

	void SceneEventInterface.OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent == SceneEvent.LeaveGame)
		{
			hasBeenSaved = false;
		}
	}

	public void LearnAllRecipesInstantly()
	{
		for (int i = 0; i < menuItems.Count; i++)
		{
			menuItems[i].LearnInstantly();
		}
	}

	public bool LearnItem(Item_Base item, Network_UserId researcherID)
	{
		bool result = false;
		for (int i = 0; i < menuItems.Count; i++)
		{
			if (!menuItems[i].Learned && menuItems[i].GetItem().UniqueIndex == item.UniqueIndex)
			{
				RuntimeManager.PlayOneShot(er_learn);
				(notificationManager.ShowNotification("Research") as Notification_Research).researchInfoQue.Enqueue(new Notification_Research_Info(item.settings_Inventory.DisplayName, researcherID, item.settings_Inventory.Sprite));
				menuItems[i].Learn();
				if (mysteryPackageLoot != null)
				{
					AchievementHandler.CheckMysterPackageAchievement(mysteryPackageLoot.possibleYield);
				}
				result = true;
				break;
			}
		}
		SortMenuItems();
		return result;
	}

	public void LearnItemNetworked(Item_Base item, Network_UserId researcherID)
	{
		Message_ResearchTable_ResearchOrLearn message = new Message_ResearchTable_ResearchOrLearn(Messages.ResearchTable_Learn, network.GetPlayerFromID(researcherID), researcherID, item.UniqueIndex);
		if (Raft_Network.IsHost)
		{
			if (LearnItem(item, researcherID))
			{
				network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
		else
		{
			network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	public void ResearchButton()
	{
		if (network == null)
		{
			network = ComponentManager<Raft_Network>.Value;
		}
		Item_Base baseItem = activeTable.slot.itemInstance.baseItem;
		Message_ResearchTable_ResearchOrLearn message = new Message_ResearchTable_ResearchOrLearn(Messages.ResearchTable_Research, activeTable, network.GetLocalPlayer().steamID, baseItem.UniqueIndex);
		if (Raft_Network.IsHost)
		{
			network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			Research(baseItem);
			activeTable.slot.RemoveItem(1);
		}
		else
		{
			network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	public bool Research(Item_Base item, bool autoLearnRecipe = false)
	{
		if (CanResearchItem(item))
		{
			RuntimeManager.PlayOneShot(er_research);
			researchedItems.Add(item);
			if (item.settings_recipe.IsBlueprint)
			{
				List<int> blueprintItemIndexes = item.settings_recipe.GetBlueprintItemIndexes();
				if (blueprintItemIndexes != null)
				{
					foreach (int item2 in blueprintItemIndexes)
					{
						for (int i = 0; i < menuItems.Count; i++)
						{
							ResearchMenuItem researchMenuItem = menuItems[i];
							if (researchMenuItem.GetItem().UniqueIndex == item2)
							{
								researchMenuItem.gameObject.SetActive(value: true);
								break;
							}
						}
					}
				}
			}
			else
			{
				if (availableResearchItems.ContainsKey(item))
				{
					availableResearchItems[item].SetResearchedState(researched: true);
				}
				for (int j = 0; j < menuItems.Count; j++)
				{
					menuItems[j].Research(item);
				}
				TestResearcherAchievement(autoLearnRecipe);
			}
			SortMenuItems();
			return true;
		}
		return false;
	}

	public void TestResearcherAchievement(bool autoLearnRecipe)
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < menuItems.Count; i++)
		{
			bool learnedViaBlueprint = menuItems[i].GetItem().settings_recipe.LearnedViaBlueprint;
			bool flag = learnedViaBlueprint && menuItems[i].GetItem().settings_recipe.NewCost.Length == 0;
			if (menuItems[i].GetItem().settings_recipe.HiddenInResearchTable | flag)
			{
				continue;
			}
			num++;
			if (!menuItems[i].gameObject.activeSelf)
			{
				continue;
			}
			float sortBingoPercent = menuItems[i].SortBingoPercent;
			if (sortBingoPercent == 1f && autoLearnRecipe)
			{
				menuItems[i].LearnInstantly();
			}
			if (learnedViaBlueprint)
			{
				if (menuItems[i].Learned)
				{
					num2++;
				}
			}
			else if (sortBingoPercent == 1f || menuItems[i].Learned)
			{
				num2++;
			}
		}
		if (num > 0 && num2 == num && GameModeValueManager.GetCurrentGameModeValue().achievementVariables.trackAchievments)
		{
			AchievementHandler.UnlockAchievement(AchievementType.ach_researcher);
		}
	}

	public bool ResearchBlueprint(Item_Base blueprintItem)
	{
		if (blueprintItem == null)
		{
			return false;
		}
		bool flag = false;
		List<int> blueprintItemIndexes = blueprintItem.settings_recipe.GetBlueprintItemIndexes();
		if (blueprintItemIndexes != null)
		{
			foreach (int item in blueprintItemIndexes)
			{
				for (int i = 0; i < menuItems.Count; i++)
				{
					ResearchMenuItem researchMenuItem = menuItems[i];
					if (researchMenuItem.GetItem().UniqueIndex == item)
					{
						if (!researchMenuItem.gameObject.activeSelf)
						{
							flag = true;
							researchMenuItem.gameObject.SetActiveSafe(value: true);
						}
						break;
					}
				}
			}
		}
		if (flag)
		{
			researchedItems.Add(blueprintItem);
			RuntimeManager.PlayOneShot(er_research);
			List<int> blueprintItemIndexes2 = blueprintItem.settings_recipe.GetBlueprintItemIndexes();
			if (blueprintItemIndexes2 != null)
			{
				for (int j = 0; j < blueprintItemIndexes2.Count; j++)
				{
					Item_Base itemByIndex = ItemManager.GetItemByIndex(blueprintItemIndexes2[j]);
					if (!(itemByIndex == null))
					{
						(ComponentManager<NotificationManager>.Value.ShowNotification("ResearchBlueprint") as Notification_ResearchBlueprint).infoQue.Enqueue(new Notification_ResearchBlueprint_Info(itemByIndex));
					}
				}
			}
			SortMenuItems();
			Network_Player localPlayer = network.GetLocalPlayer();
			Message_ResearchTable_ResearchOrLearn message = new Message_ResearchTable_ResearchOrLearn(Messages.ResearchBlueprint, localPlayer, localPlayer.steamID, blueprintItem.UniqueIndex);
			if (Raft_Network.IsHost)
			{
				network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
			else
			{
				network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
		return flag;
	}

	public void CreateMenuItems(CraftingMenu craftingMenu)
	{
		List<Item_Base> allRecipes = craftingMenu.AllRecipes;
		List<Item_Base> recipesLearnedBeginning = craftingMenu.RecipesLearnedBeginning;
		List<Item_Base> list = new List<Item_Base>();
		int num = 0;
		for (int i = 0; i < allRecipes.Count; i++)
		{
			Item_Base item_Base = allRecipes[i];
			if (recipesLearnedBeginning.Contains(item_Base) || item_Base.settings_recipe.IsBlueprint || item_Base.settings_recipe.CraftingCategory == CraftingCategory.Hidden || item_Base.settings_recipe.CraftingCategory == CraftingCategory.CreativeMode)
			{
				continue;
			}
			num++;
			ResearchMenuItem researchMenuItem = UnityEngine.Object.Instantiate(menuItemPrefab, content);
			researchMenuItem.SetupViewport(content.transform.parent.GetComponent<RectTransform>());
			RectTransform component = researchMenuItem.transform.GetComponent<RectTransform>();
			component.sizeDelta = new Vector2(component.sizeDelta.x, menuItemHeight);
			researchMenuItem.transform.position = Vector3.zero;
			researchMenuItem.Initialize(craftingMenu, this);
			researchMenuItem.SetItem(item_Base);
			researchMenuItem.SetBackgroundAlpha((num % 2 == 0) ? 125f : 50f);
			researchMenuItem.OnLearnedRecipeEvent += SortMenuItems;
			menuItems.Add(researchMenuItem);
			if (item_Base.settings_recipe.LearnedViaBlueprint || item_Base.settings_recipe.HiddenInResearchTable)
			{
				researchMenuItem.gameObject.SetActiveSafe(value: false);
			}
			CostMultiple[] newCost = item_Base.settings_recipe.NewCost;
			foreach (CostMultiple costMultiple in newCost)
			{
				for (int k = 0; k < costMultiple.items.Length; k++)
				{
					Item_Base item_Base2 = costMultiple.items[k];
					if (!list.Contains(item_Base2))
					{
						list.Add(item_Base2);
						AvaialableResearchItem avaialableResearchItem = UnityEngine.Object.Instantiate(availableItemPrefab, researchItemContent);
						avaialableResearchItem.SetItem(item_Base2);
						avaialableResearchItem.transform.position = Vector3.zero;
						availableResearchItems.Add(item_Base2, avaialableResearchItem);
					}
				}
			}
		}
	}

	public void SetTable(ResearchTable table)
	{
		activeTable = table;
	}

	public List<Item_Base> GetResearchedItems()
	{
		return researchedItems;
	}

	public List<ResearchMenuItem> GetMenuItems()
	{
		return menuItems;
	}

	public bool CanResearchBlueprint(Item_Base blueprintItem)
	{
		if (blueprintItem == null)
		{
			return false;
		}
		List<int> blueprintItemIndexes = blueprintItem.settings_recipe.GetBlueprintItemIndexes();
		if (blueprintItemIndexes != null)
		{
			foreach (int item in blueprintItemIndexes)
			{
				for (int i = 0; i < menuItems.Count; i++)
				{
					ResearchMenuItem researchMenuItem = menuItems[i];
					if (researchMenuItem.GetItem().UniqueIndex == item)
					{
						if (researchMenuItem.gameObject.activeSelf)
						{
							break;
						}
						return true;
					}
				}
			}
		}
		return false;
	}

	public bool CanResearchItem(Item_Base item)
	{
		if (HasBeenResearched(item))
		{
			return false;
		}
		if (item.settings_recipe.IsBlueprint)
		{
			return true;
		}
		for (int i = 0; i < menuItems.Count; i++)
		{
			if (menuItems[i].ContainsItem(item))
			{
				return true;
			}
		}
		return false;
	}

	public bool HasBeenResearched(Item_Base item)
	{
		return researchedItems.Contains(item);
	}

	public void SortMenuItems()
	{
		menuItems = menuItems.OrderBy((ResearchMenuItem i) => i.SortBingoPercent).ToList();
		menuItems.Reverse();
		for (int num = 0; num < menuItems.Count; num++)
		{
			menuItems[num].transform.SetSiblingIndex(num);
			menuItems[num].SetBackgroundAlpha((num % 2 == 0) ? 125f : 50f);
		}
	}
}
