using System;
using System.Collections.Generic;
using FMODUnity;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

public class TradingPostUI : MonoBehaviour
{
	private int currentReputation;

	[SerializeField]
	private UI_Cost_Interactable leftPanelPrefab;

	[SerializeField]
	private Transform[] buyContentParent;

	[SerializeField]
	private Transform sellContentParent;

	[SerializeField]
	private GameObject rightPanel;

	[SerializeField]
	private UI_Cost rightPanelInfoCostUI;

	[SerializeField]
	private TabGroup buyAndSellTabgroup;

	[SerializeField]
	private Slider reputationSlider;

	[SerializeField]
	private Text reputationSliderText;

	[SerializeField]
	private Text reputationSliderTitle;

	[SerializeField]
	private TierUnlock tier2;

	[SerializeField]
	private TierUnlock tier3;

	[SerializeField]
	private GameObject buySection;

	[SerializeField]
	private Color color_inStock;

	[SerializeField]
	private Color color_outOfStock;

	[SerializeField]
	private Text text_inStock;

	[SerializeField]
	private UI_Cost[] rightPanelRequiredCostsUI;

	[SerializeField]
	private Button button_purchase;

	[SerializeField]
	private GameObject sellSection;

	[SerializeField]
	private UI_Cost reputationUICost;

	[SerializeField]
	private UI_Cost sellRewardUICost;

	[SerializeField]
	private Button button_sell;

	public EventReference er_clickedSound;

	[SerializeField]
	private GameObject sellText;

	[SerializeField]
	private GameObject buyText;

	private Network_Player localPlayer;

	private TradingPost currentTradingPost;

	private SO_TradingPost_Buyable.Instance selectedBuyableItem;

	private SO_TradingPost_Sellable selectedSellableItem;

	private List<UI_Cost_Interactable> buyableMenuItems = new List<UI_Cost_Interactable>();

	private Raft_Network network;

	private TradingPost.Tier currentTier;

	[SerializeField]
	private GameObject[] gamepadObjects;

	[SerializeField]
	private GameObject[] keyboardObjects;

	public TradingPost.Tier CurrentTier => currentTier;

	public int CurrentReputation
	{
		get
		{
			return currentReputation;
		}
		set
		{
			currentReputation = Mathf.Clamp(value, 0, tier3.limit);
			RefreshReputationUI();
		}
	}

	private void Awake()
	{
		ComponentManager<TradingPostUI>.Value = this;
	}

	private void Start()
	{
		localPlayer = ComponentManager<Network_Player>.Value;
		network = ComponentManager<Raft_Network>.Value;
		TabGroup tabGroup = buyAndSellTabgroup;
		tabGroup.OnNewTabOpen = (Action)Delegate.Combine(tabGroup.OnNewTabOpen, new Action(OnNewTabOpen));
		CustomInputConfig instance = SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance;
		instance.OnControlsChangedEvent = (Action<bool>)Delegate.Combine(instance.OnControlsChangedEvent, new Action<bool>(ControlsChangedEvent));
		ControlsChangedEvent(SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad);
	}

	private void OnDestroy()
	{
		if ((bool)SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance)
		{
			CustomInputConfig instance = SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance;
			instance.OnControlsChangedEvent = (Action<bool>)Delegate.Remove(instance.OnControlsChangedEvent, new Action<bool>(ControlsChangedEvent));
		}
	}

	private void ControlsChangedEvent(bool gamepad)
	{
		GameObject[] array = gamepadObjects;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(gamepad);
		}
		array = keyboardObjects;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(!gamepad);
		}
		RefreshUI();
	}

	public void Button_Close()
	{
		if (currentTradingPost != null)
		{
			currentTradingPost.CloseUI();
			return;
		}
		ComponentManager<CanvasHelper>.Value.CloseMenu(MenuType.TradingPost);
		ComponentManager<CanvasHelper>.Value.GetComponent<GamepadCursor>().SetLayer(GamepadCursorLayer.Inventory);
	}

	public void Button_Buy()
	{
		if (!(currentTradingPost != null) || selectedBuyableItem == null || selectedBuyableItem.stock <= 0)
		{
			return;
		}
		int uniqueIndex = selectedBuyableItem.reward.item.UniqueIndex;
		if (uniqueIndex >= 0)
		{
			Message_TradingPost message = new Message_TradingPost(Messages.TradingPost_Purchase, network.NetworkIDManager, currentTradingPost.ObjectIndex, localPlayer.steamID, uniqueIndex);
			if (Raft_Network.IsHost)
			{
				if (currentTradingPost.PurchaseItem(uniqueIndex, localPlayer))
				{
					network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				}
			}
			else
			{
				network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
		RefreshUI();
	}

	public void Button_Sell()
	{
		if (!(currentTradingPost != null) || !(selectedSellableItem != null))
		{
			return;
		}
		int itemCount = localPlayer.Inventory.GetItemCount(selectedSellableItem.sellableItem);
		if (itemCount <= 0)
		{
			return;
		}
		currentTradingPost.PlaySellSound();
		localPlayer.Inventory.RemoveItem(selectedSellableItem.sellableItem.UniqueName, 1);
		localPlayer.Inventory.AddItem(selectedSellableItem.reward.item.UniqueName, selectedSellableItem.reward.amount);
		Message_TradingPost message_TradingPost = new Message_TradingPost(Messages.TradingPost_Reputation, network.NetworkIDManager, currentTradingPost.ObjectIndex, localPlayer.steamID, 0);
		if (Raft_Network.IsHost)
		{
			bool num = currentTier == TradingPost.Tier.Tier1 && currentReputation + selectedSellableItem.reputationReward >= tier2.limit;
			bool flag = currentTier == TradingPost.Tier.Tier2 && currentReputation + selectedSellableItem.reputationReward >= tier3.limit - 5;
			CurrentReputation += selectedSellableItem.reputationReward;
			message_TradingPost.value = CurrentReputation;
			network.RPC(message_TradingPost, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			if (num | flag)
			{
				ReputationTierUp(currentTier);
			}
		}
		else
		{
			bool num2 = currentTier == TradingPost.Tier.Tier1 && currentReputation + selectedSellableItem.reputationReward >= tier2.limit;
			bool flag2 = currentTier == TradingPost.Tier.Tier2 && currentReputation + selectedSellableItem.reputationReward >= tier3.limit - 5;
			message_TradingPost.value = selectedSellableItem.reputationReward;
			network.SendP2P(network.HostID, message_TradingPost, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			if (num2 | flag2)
			{
				ReputationTierUp(currentTier);
			}
		}
		if (itemCount == 0)
		{
			selectedSellableItem = null;
		}
		Open(currentTradingPost);
	}

	private void OnNewTabOpen()
	{
		selectedBuyableItem = null;
		selectedSellableItem = null;
		RefreshUI();
	}

	public void Open(TradingPost tradingPost)
	{
		currentTradingPost = tradingPost;
		for (int i = 0; i < buyContentParent.Length; i++)
		{
			for (int j = 0; j < buyContentParent[i].childCount; j++)
			{
				UnityEngine.Object.Destroy(buyContentParent[i].GetChild(j).gameObject);
			}
		}
		for (int k = 0; k < sellContentParent.childCount; k++)
		{
			UnityEngine.Object.Destroy(sellContentParent.GetChild(k).gameObject);
		}
		buyableMenuItems.Clear();
		foreach (SO_TradingPost_Buyable.Instance tradingCost in tradingPost.buyableItems)
		{
			Transform transform = buyContentParent[(int)tradingCost.tier];
			UI_Cost_Interactable uI_Cost_Interactable = UnityEngine.Object.Instantiate(leftPanelPrefab, transform);
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
			{
				uI_Cost_Interactable.GetComponentInChildren<UiMagnet>().Viewport = transform.parent.GetComponent<RectTransform>();
			}
			uI_Cost_Interactable.Refresh(tradingCost.reward);
			uI_Cost_Interactable.backgroundButton.interactable = tradingCost.stock > 0 && currentTier >= tradingCost.tier;
			uI_Cost_Interactable.OnPointerClick = (Action<UI_Cost_Interactable>)Delegate.Combine(uI_Cost_Interactable.OnPointerClick, (Action<UI_Cost_Interactable>)((UI_Cost_Interactable box) =>
			{
				selectedSellableItem = null;
				selectedBuyableItem = tradingCost;
				RefreshUI();
				PlayClickSound();
			}));
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
			{
				uI_Cost_Interactable.OnPointerExit = (Action<UI_Cost_Interactable>)Delegate.Combine(uI_Cost_Interactable.OnPointerExit, (Action<UI_Cost_Interactable>)((UI_Cost_Interactable box) =>
				{
					if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
					{
						selectedBuyableItem = null;
						RefreshUI();
					}
				}));
			}
			buyableMenuItems.Add(uI_Cost_Interactable);
		}
		foreach (SO_TradingPost_Sellable sellableItem in TradingPost.SellableItems)
		{
			int itemCount = localPlayer.Inventory.GetItemCount(sellableItem.sellableItem);
			UI_Cost_Interactable uI_Cost_Interactable2 = UnityEngine.Object.Instantiate(leftPanelPrefab, sellContentParent);
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
			{
				uI_Cost_Interactable2.GetComponentInChildren<UiMagnet>().Viewport = sellContentParent.parent.GetComponent<RectTransform>();
			}
			uI_Cost_Interactable2.Refresh(new Cost(sellableItem.sellableItem, itemCount));
			uI_Cost_Interactable2.backgroundButton.interactable = itemCount > 0;
			uI_Cost_Interactable2.OnPointerClick = (Action<UI_Cost_Interactable>)Delegate.Combine(uI_Cost_Interactable2.OnPointerClick, (Action<UI_Cost_Interactable>)((UI_Cost_Interactable box) =>
			{
				selectedBuyableItem = null;
				selectedSellableItem = sellableItem;
				RefreshUI();
				PlayClickSound();
			}));
			if (!SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
			{
				continue;
			}
			uI_Cost_Interactable2.OnPointerExit = (Action<UI_Cost_Interactable>)Delegate.Combine(uI_Cost_Interactable2.OnPointerExit, (Action<UI_Cost_Interactable>)((UI_Cost_Interactable box) =>
			{
				if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
				{
					selectedSellableItem = null;
					RefreshUI();
				}
			}));
		}
		RefreshUI();
	}

	public void RefreshUI()
	{
		if (localPlayer == null)
		{
			localPlayer = ComponentManager<Network_Player>.Value;
		}
		rightPanel.SetActiveSafe(selectedBuyableItem != null || selectedSellableItem != null);
		buySection.SetActiveSafe(selectedBuyableItem != null);
		sellSection.SetActiveSafe(selectedSellableItem != null);
		RefreshReputationUI();
		if (buyAndSellTabgroup.SelectedTabButton != null && buyAndSellTabgroup.SelectedTabButton.tabIndex == 0)
		{
			if (selectedBuyableItem != null)
			{
				bool flag = selectedBuyableItem.stock > 0;
				rightPanelInfoCostUI.Refresh(selectedBuyableItem.reward);
				bool flag2 = true;
				for (int i = 0; i < rightPanelRequiredCostsUI.Length; i++)
				{
					UI_Cost uI_Cost = rightPanelRequiredCostsUI[i];
					uI_Cost.gameObject.SetActiveSafe(i < selectedBuyableItem.cost.Length);
					if (uI_Cost.gameObject.activeSelf)
					{
						int itemCount = localPlayer.Inventory.GetItemCount(selectedBuyableItem.cost[i].item);
						if (itemCount < selectedBuyableItem.cost[i].amount)
						{
							flag2 = false;
						}
						uI_Cost.Refresh(selectedBuyableItem.cost[i], itemCount);
					}
				}
				if (currentTier < selectedBuyableItem.tier)
				{
					text_inStock.text = Helper.GetTerm("Game/NotEnoughReputation");
					text_inStock.color = color_outOfStock;
				}
				else if (flag)
				{
					LocalizationParameters.itemX = selectedBuyableItem.stock.ToString();
					text_inStock.text = Helper.GetTerm("Game/InStock", applyParameters: true);
					text_inStock.color = color_inStock;
				}
				else
				{
					text_inStock.text = Helper.GetTerm("Game/OutOfStock");
					text_inStock.color = color_outOfStock;
				}
				button_purchase.interactable = (flag2 & flag) && currentTier >= selectedBuyableItem.tier;
				if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
				{
					sellText.SetActive(value: false);
					buyText.SetActive(button_purchase.interactable);
				}
			}
			else if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
			{
				sellText.SetActive(value: false);
				buyText.SetActive(value: false);
			}
		}
		else if (selectedSellableItem != null)
		{
			rightPanelInfoCostUI.Refresh(new Cost(selectedSellableItem.sellableItem, 0));
			sellRewardUICost.Refresh(selectedSellableItem.reward);
			reputationUICost.Refresh(new Cost(null, selectedSellableItem.reputationReward));
			int itemCount2 = localPlayer.Inventory.GetItemCount(selectedSellableItem.sellableItem);
			button_sell.interactable = itemCount2 > 0;
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
			{
				buyText.SetActive(value: false);
				sellText.SetActive(button_sell.interactable);
			}
		}
		else if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
		{
			sellText.SetActive(value: false);
			buyText.SetActive(value: false);
		}
	}

	private void PlayClickSound()
	{
		if (!er_clickedSound.IsNull)
		{
			RuntimeManager.PlayOneShot(er_clickedSound);
		}
	}

	private void RefreshReputationUI()
	{
		currentTier = TradingPost.Tier.Tier1;
		if (CurrentReputation >= tier2.limit && CurrentReputation < tier3.limit)
		{
			currentTier = TradingPost.Tier.Tier2;
		}
		else if (CurrentReputation >= tier3.limit)
		{
			currentTier = TradingPost.Tier.Tier3;
		}
		bool flag = currentTier == TradingPost.Tier.Tier1;
		tier2.lockImage.gameObject.SetActiveSafe(flag);
		tier2.text.text = (flag ? "T2     " : (Helper.GetTerm("Game/Tier") + " 2"));
		bool flag2 = currentTier == TradingPost.Tier.Tier1 || currentTier == TradingPost.Tier.Tier2;
		tier3.lockImage.gameObject.SetActiveSafe(flag2);
		tier3.text.text = (flag2 ? "T3     " : (Helper.GetTerm("Game/Tier") + " 3"));
		int limit = tier2.limit;
		if (currentTier == TradingPost.Tier.Tier2 || currentTier == TradingPost.Tier.Tier3)
		{
			limit = tier3.limit;
		}
		float value = Mathf.Clamp01((float)CurrentReputation / (float)limit);
		reputationSlider.value = value;
		reputationSliderTitle.text = Helper.GetTerm("Game/Tier") + " " + (int)(currentTier + 1);
		reputationSliderText.text = CurrentReputation + "/" + limit;
	}

	public void CheckAndRunTierUp(int totalRep)
	{
		bool num = currentTier == TradingPost.Tier.Tier1 && totalRep >= tier2.limit;
		bool flag = currentTier == TradingPost.Tier.Tier2 && totalRep >= tier3.limit;
		if (num)
		{
			ReputationTierUp(TradingPost.Tier.Tier2);
		}
		if (flag)
		{
			ReputationTierUp(TradingPost.Tier.Tier3);
		}
	}

	private void ReputationTierUp(TradingPost.Tier tier)
	{
		if (currentTradingPost != null)
		{
			currentTradingPost.PlayTierUpSound();
		}
		switch (tier)
		{
		case TradingPost.Tier.Tier2:
			AchievementHandler.UnlockAchievement(AchievementType.ach_tier2Trader);
			break;
		case TradingPost.Tier.Tier3:
			AchievementHandler.UnlockAchievement(AchievementType.ach_tier2Trader);
			AchievementHandler.UnlockAchievement(AchievementType.ach_tier3Trader);
			break;
		}
	}
}
