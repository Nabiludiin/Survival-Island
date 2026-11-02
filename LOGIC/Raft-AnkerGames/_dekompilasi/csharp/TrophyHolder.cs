using FMODUnity;
using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

public class TrophyHolder : MonoBehaviour_ID_Network, IBlockPlaced, IRaycastable
{
	[SerializeField]
	private ItemObjectEnabler itemObjectEnabler;

	private Network_Player player;

	private CanvasHelper canvas;

	private bool isShowingText;

	private Item_Base currentObject;

	private BoxCollider trophyTrigger;

	private Raft_Network network;

	[SerializeField]
	private Vector3 colliderOccupiedOffset = Vector3.zero;

	[SerializeField]
	private Vector3 colliderEmptyOffset = Vector3.zero;

	[SerializeField]
	private Vector3 colliderOccupiedScale = Vector3.zero;

	[SerializeField]
	private Vector3 colliderEmptyScale = Vector3.zero;

	public EventReference er_PlaceTrophySound;

	public bool IsOccupied => currentObject != null;

	protected void Start()
	{
		player = ComponentManager<Network_Player>.Value;
		canvas = ComponentManager<CanvasHelper>.Value;
		network = ComponentManager<Raft_Network>.Value;
	}

	void IBlockPlaced.OnBlockPlacedInterface()
	{
		ObjectIndex = GetComponentInParent<Block>().ObjectIndex;
		NetworkIDManager.AddNetworkID(this);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		NetworkIDManager.RemoveNetworkID(this);
	}

	private void OnEnable()
	{
		player = ComponentManager<Network_Player>.Value;
		canvas = ComponentManager<CanvasHelper>.Value;
		trophyTrigger = GetComponent<BoxCollider>();
	}

	public void OnIsRayed()
	{
		if (CanvasHelper.ActiveMenu != MenuType.None || !Helper.LocalPlayerIsWithinDistance(transform.position, Player.UseDistance))
		{
			if (isShowingText)
			{
				canvas.displayTextManager.HideDisplayTexts();
				isShowingText = false;
			}
			return;
		}
		InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
		if (!IsOccupied)
		{
			Slot selectedHotbarSlot = player.Inventory.GetSelectedHotbarSlot();
			if (!(selectedHotbarSlot != null) || !selectedHotbarSlot.HasValidItemInstance())
			{
				return;
			}
			Item_Base baseItem = selectedHotbarSlot.itemInstance.baseItem;
			if (itemObjectEnabler.DoesAcceptItem(baseItem))
			{
				LocalizationParameters.itemX = baseItem.settings_Inventory.DisplayName;
				canvas.displayTextManager.ShowText(Helper.GetTerm("Game/PlaceItemX", applyParameters: true), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
				isShowingText = true;
				if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
				{
					canvas.displayTextManager.HideDisplayTexts();
					isShowingText = false;
					PlaceTrophyNetworked(network.GetLocalPlayer(), baseItem);
				}
			}
		}
		else
		{
			if (!IsOccupied)
			{
				return;
			}
			LocalizationParameters.itemX = currentObject.settings_Inventory.DisplayName;
			canvas.displayTextManager.ShowText(Helper.GetTerm("Game/PickUpX", applyParameters: true), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
			isShowingText = true;
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
			{
				canvas.displayTextManager.HideDisplayTexts();
				isShowingText = false;
				Message_PickupTrophyOnBoard message = new Message_PickupTrophyOnBoard(Messages.TrophyHolder_Pickup, network.LocalSteamID, network.NetworkIDManager, ObjectIndex);
				if (Raft_Network.IsHost)
				{
					network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
					RemoveTrophy(network.GetLocalPlayer());
				}
				else
				{
					network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				}
			}
		}
	}

	public void OnRayEnter()
	{
	}

	public void OnRayExit()
	{
		canvas.displayTextManager.HideDisplayTexts();
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		if (network == null)
		{
			network = ComponentManager<Raft_Network>.Value;
		}
		switch (msg.Type)
		{
		case Messages.TrophyHolder_Pickup:
		{
			Message_PickupTrophyOnBoard message_PickupTrophyOnBoard = msg as Message_PickupTrophyOnBoard;
			return RemoveTrophy(network.GetPlayerFromID(message_PickupTrophyOnBoard.SteamID));
		}
		case Messages.TrophyHolder_Place:
		{
			Message_PlaceTrophyOnBoard message_PlaceTrophyOnBoard = msg as Message_PlaceTrophyOnBoard;
			return PlaceTrophy(network.GetPlayerFromID(message_PlaceTrophyOnBoard.SteamID), ItemManager.GetItemByIndex(message_PlaceTrophyOnBoard.itemIndex));
		}
		default:
			return false;
		}
	}

	public void SetCurrentItem(Item_Base p_item)
	{
		currentObject = p_item;
	}

	public Item_Base GetCurrentItem()
	{
		return currentObject;
	}

	public bool PlaceTrophy(Network_Player p_player, Item_Base p_obj)
	{
		if ((IsOccupied && currentObject != null) || p_obj == null)
		{
			return false;
		}
		currentObject = p_obj;
		if (p_player != null && p_player.IsLocalPlayer)
		{
			player.Inventory.RemoveItem(currentObject.UniqueName, 1);
			p_player.Animator.SetAnimation(PlayerAnimation.Trigger_Plant);
		}
		if (itemObjectEnabler != null)
		{
			itemObjectEnabler.ActivateModel(currentObject);
		}
		if (p_player != null && !er_PlaceTrophySound.IsNull)
		{
			RuntimeManager.PlayOneShot(er_PlaceTrophySound, transform.position);
		}
		return true;
	}

	public void PlaceTrophyNetworked(Network_Player player, Item_Base selectedItem)
	{
		if (network == null)
		{
			network = ComponentManager<Raft_Network>.Value;
		}
		Message_PlaceTrophyOnBoard message = new Message_PlaceTrophyOnBoard(Messages.TrophyHolder_Place, (player != null) ? player.steamID : ((Network_UserId)0uL), network.NetworkIDManager, ObjectIndex, selectedItem.UniqueIndex);
		if (Raft_Network.IsHost)
		{
			network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			PlaceTrophy(player, selectedItem);
		}
		else
		{
			network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	private bool RemoveTrophy(Network_Player p_player)
	{
		if ((!IsOccupied && currentObject == null) || p_player == null)
		{
			return false;
		}
		if (p_player.IsLocalPlayer)
		{
			p_player.Animator.SetAnimation(PlayerAnimation.Trigger_GrabItem);
			p_player.Inventory.AddItem(currentObject.UniqueName, 1);
		}
		currentObject = null;
		itemObjectEnabler.DeActivateAllModels();
		return true;
	}

	private void ModifyColliderScaleAndPos(bool p_isOccupied)
	{
		if (p_isOccupied)
		{
			trophyTrigger.size = colliderOccupiedScale;
			trophyTrigger.center = colliderOccupiedOffset;
		}
		else
		{
			trophyTrigger.size = colliderEmptyScale;
			trophyTrigger.center = colliderEmptyOffset;
		}
	}

	public override RGD Serialize_Save()
	{
		return new RGDTrophyHolder(RGDType.ID_TrophyHolder, this);
	}
}
