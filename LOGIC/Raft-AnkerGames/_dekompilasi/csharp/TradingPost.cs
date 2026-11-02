using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;

public class TradingPost : MonoBehaviour_ID_Network, IRaycastable
{
	public enum Tier
	{
		Tier1,
		Tier2,
		Tier3
	}

	public static List<SO_TradingPost_Sellable> SellableItems = new List<SO_TradingPost_Sellable>();

	private static List<SO_TradingPost_Buyable> startingStock = new List<SO_TradingPost_Buyable>();

	private static TradingPostUI tradingPostUI;

	private static CanvasHelper canvasHelper;

	public List<SO_TradingPost_Buyable.Instance> buyableItems = new List<SO_TradingPost_Buyable.Instance>();

	[SerializeField]
	private Animator animator;

	[SerializeField]
	public StudioEventEmitter radioMusic;

	public EventReference er_StartTrading;

	public EventReference er_StopTrading;

	public EventReference er_BuyItem;

	public EventReference er_SellItem;

	public EventReference er_UnlockTier;

	private void Start()
	{
		if (tradingPostUI == null)
		{
			tradingPostUI = ComponentManager<TradingPostUI>.Value;
		}
		if (canvasHelper == null)
		{
			canvasHelper = ComponentManager<CanvasHelper>.Value;
		}
	}

	private void Update()
	{
		if (CanvasHelper.ActiveMenu == MenuType.TradingPost)
		{
			InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Inventory"];
			InputAction action2 = PlayerInput.GetPlayerByIndex(0).actions["Cancel"];
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasReleasedThisFrame(action, "Inventory") || SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasReleasedThisFrame(action2, "Cancel"))
			{
				CloseUI();
			}
		}
	}

	void IRaycastable.OnIsRayed()
	{
		if (CanvasHelper.ActiveMenu == MenuType.None && !PlayerItemManager.IsBusy)
		{
			InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
			canvasHelper.displayTextManager.ShowText(Helper.GetTerm("Game/Open"), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
			{
				OpenUI();
			}
		}
	}

	void IRaycastable.OnRayEnter()
	{
	}

	void IRaycastable.OnRayExit()
	{
		if (canvasHelper != null)
		{
			canvasHelper.displayTextManager.HideDisplayTexts();
		}
	}

	public bool PurchaseItem(int uniqueItemIndex, Network_Player purchasingPlayer)
	{
		SO_TradingPost_Buyable.Instance instance = null;
		foreach (SO_TradingPost_Buyable.Instance buyableItem in buyableItems)
		{
			if (buyableItem != null && !(buyableItem.reward.item == null) && buyableItem.reward.item.UniqueIndex == uniqueItemIndex)
			{
				instance = buyableItem;
				break;
			}
		}
		if (instance == null || instance.stock <= 0)
		{
			return false;
		}
		instance.stock--;
		if (purchasingPlayer != null && purchasingPlayer.IsLocalPlayer)
		{
			for (int i = 0; i < instance.cost.Length; i++)
			{
				Cost cost = instance.cost[i];
				if (cost != null)
				{
					purchasingPlayer.Inventory.RemoveItem(cost.item.UniqueName, cost.amount);
				}
			}
			purchasingPlayer.Inventory.AddItem(instance.reward.item.UniqueName, instance.reward.amount);
		}
		PlayBuySound();
		if (animator != null)
		{
			animator.SetTrigger("Buy");
		}
		tradingPostUI.RefreshUI();
		AchievementHandler.UnlockAchievement(AchievementType.ach_firstPurchaseAtTrader);
		return true;
	}

	public void ResetItemsForSale()
	{
		if (startingStock.Count == 0)
		{
			startingStock = Resources.LoadAll<SO_TradingPost_Buyable>("SO_TradingPost").ToList();
			startingStock = startingStock.OrderBy((SO_TradingPost_Buyable b) => b.sortingOrder).ToList();
		}
		if (SellableItems.Count == 0)
		{
			SellableItems = Resources.LoadAll<SO_TradingPost_Sellable>("SO_TradingPost").ToList();
			SellableItems = SellableItems.OrderBy((SO_TradingPost_Sellable b) => b.sortingOrder).ToList();
		}
		if (buyableItems == null)
		{
			buyableItems = new List<SO_TradingPost_Buyable.Instance>();
		}
		buyableItems.Clear();
		for (int num = 0; num < startingStock.Count; num++)
		{
			buyableItems.Add(startingStock[num].CreateInstance());
		}
	}

	public void PlayBuySound()
	{
		if (!er_BuyItem.IsNull)
		{
			RuntimeManager.PlayOneShot(er_BuyItem, animator.transform.position);
		}
	}

	public void PlaySellSound()
	{
		if (!er_SellItem.IsNull)
		{
			RuntimeManager.PlayOneShot(er_SellItem, animator.transform.position);
		}
	}

	public void PlayTierUpSound()
	{
		if (!er_UnlockTier.IsNull)
		{
			RuntimeManager.PlayOneShot(er_UnlockTier, animator.transform.position);
		}
	}

	public void CloseUI()
	{
		if (!er_StopTrading.IsNull)
		{
			RuntimeManager.PlayOneShot(er_StopTrading, animator.transform.position);
		}
		if (radioMusic != null)
		{
			radioMusic.SetParameter("trading", 0f);
		}
		canvasHelper.CloseMenu(MenuType.TradingPost);
		canvasHelper.GetComponent<GamepadCursor>().SetLayer(GamepadCursorLayer.Inventory);
	}

	private void OpenUI()
	{
		if (!er_StopTrading.IsNull)
		{
			RuntimeManager.PlayOneShot(er_StopTrading, animator.transform.position);
		}
		if (radioMusic != null)
		{
			radioMusic.SetParameter("trading", 1f);
		}
		canvasHelper.OpenMenu(MenuType.TradingPost);
		canvasHelper.GetComponent<GamepadCursor>().SetLayer(GamepadCursorLayer.Trader);
		tradingPostUI.Open(this);
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.TradingPost_Purchase:
		{
			Message_TradingPost message_TradingPost2 = msg as Message_TradingPost;
			Network_Player playerFromID = ComponentManager<Raft_Network>.Value.GetPlayerFromID(message_TradingPost2.SteamID);
			return PurchaseItem(message_TradingPost2.value, playerFromID);
		}
		case Messages.TradingPost_Reputation:
		{
			Message_TradingPost message_TradingPost = msg as Message_TradingPost;
			if (tradingPostUI != null)
			{
				if (Raft_Network.IsHost)
				{
					int num = message_TradingPost.value + tradingPostUI.CurrentReputation;
					tradingPostUI.CheckAndRunTierUp(num);
					tradingPostUI.CurrentReputation = num;
					message_TradingPost.value = tradingPostUI.CurrentReputation;
				}
				else
				{
					tradingPostUI.CheckAndRunTierUp(message_TradingPost.value);
					tradingPostUI.CurrentReputation = message_TradingPost.value;
				}
			}
			return true;
		}
		default:
			return base.Deserialize(msg, remoteID);
		}
	}
}
