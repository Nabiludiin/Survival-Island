using System.Collections;
using Steamworks;
using UnityEngine;

[RequireComponent(typeof(UseItemController))]
public class PlayerItemManager : MonoBehaviour_Network
{
	private enum PlayerState
	{
		Nothing,
		Using
	}

	private static bool isBusy;

	public static bool throwableAnchorBusy;

	[SerializeField]
	private Network_Player playerNetwork;

	private PlayerState state;

	[HideInInspector]
	public UseItemController useItemController;

	public static bool IsBusy
	{
		get
		{
			return isBusy;
		}
		set
		{
			isBusy = value;
		}
	}

	private void Awake()
	{
		useItemController = GetComponent<UseItemController>();
	}

	public bool CanSwitch()
	{
		return !IsBusy;
	}

	public void SelectUsable(Item_Base item)
	{
		if (playerNetwork.IsLocalPlayer)
		{
			if (item != null)
			{
				SwitchState(PlayerState.Using);
				useItemController.StartUsing(item);
			}
			else
			{
				SwitchState(PlayerState.Nothing);
			}
			Item_Base currentItemInHand = useItemController.GetCurrentItemInHand();
			int itemIndex = ((currentItemInHand == null) ? (-1) : currentItemInHand.UniqueIndex);
			playerNetwork.Network.RPCExclude(new Message_SelectItem(Messages.ItemManager_SelectItem, this, itemIndex), Target.Other, playerNetwork.steamID, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	public static bool ShouldItBeBusy()
	{
		Network_Player value = ComponentManager<Network_Player>.Value;
		if (value == null)
		{
			return false;
		}
		bool result = false;
		if (value.BedComponent.Sleeping)
		{
			result = true;
		}
		else if (value.CarryingComponent.IsCarrying)
		{
			result = true;
		}
		else if (value.ZiplinePlayer.IsAttachedToZipline)
		{
			result = true;
		}
		else if (value.PlayerNetworkManager.IsAttached)
		{
			result = true;
		}
		return result;
	}

	public void HideItemInHand()
	{
		useItemController.Deselect();
	}

	private void SwitchState(PlayerState newState)
	{
		SwitchOut(state);
		SwitchIn(newState);
	}

	private void SwitchOut(PlayerState stateOut)
	{
		if (stateOut != PlayerState.Nothing && stateOut == PlayerState.Using)
		{
			useItemController.Deselect();
		}
	}

	private void SwitchIn(PlayerState stateIn)
	{
		if (playerNetwork.IsLocalPlayer)
		{
			IsBusy = false;
		}
		switch (stateIn)
		{
		case PlayerState.Nothing:
			if (playerNetwork.Animator != null)
			{
				playerNetwork.Animator.SetAnimation(PlayerAnimation.Index_0_Idle);
			}
			break;
		}
		state = stateIn;
	}

	public override void Serialize_Create()
	{
		if (useItemController != null)
		{
			Item_Base currentItemInHand = useItemController.GetCurrentItemInHand();
			int itemIndex = ((currentItemInHand == null) ? (-1) : currentItemInHand.UniqueIndex);
			if (currentItemInHand != null)
			{
				AddMessage(new Message_SelectItem(Messages.ItemManager_Create, this, itemIndex));
			}
		}
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.ItemManager_Create:
		{
			if (playerNetwork.IsLocalPlayer)
			{
				return false;
			}
			Message_SelectItem message_SelectItem = msg as Message_SelectItem;
			Item_Base item = ((message_SelectItem.itemIndex == -1) ? null : ItemManager.GetItemByIndex(message_SelectItem.itemIndex));
			StartCoroutine(SwitchItem(item, 0.5f));
			return false;
		}
		case Messages.ItemManager_SelectItem:
		{
			Message_SelectItem message_SelectItem = msg as Message_SelectItem;
			Item_Base item = ((message_SelectItem.itemIndex == -1) ? null : ItemManager.GetItemByIndex(message_SelectItem.itemIndex));
			SelectUsableClient(item);
			if (Raft_Network.IsHost)
			{
				playerNetwork.Network.RPCExclude(msg, Target.Other, remoteID, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
			return false;
		}
		default:
			return false;
		}
	}

	private void SelectUsableClient(Item_Base item)
	{
		if (item != null)
		{
			SwitchState(PlayerState.Using);
			useItemController.StartUsing(item);
		}
		else
		{
			SwitchState(PlayerState.Nothing);
		}
	}

	private IEnumerator SwitchItem(Item_Base newItem, float timeDelay)
	{
		yield return new WaitForSeconds(timeDelay);
		SelectUsableClient(newItem);
	}
}
