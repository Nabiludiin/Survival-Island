using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

public class ResearchTable : MonoBehaviour_Network, SceneEventInterface, IRaycastable
{
	[SerializeField]
	private Animator animator;

	private static Inventory_ResearchTable inventoryReference;

	private CanvasHelper canvas;

	private Raft_Network network;

	private Network_Player localPlayer;

	private Network_UserId occupyingPlayerID;

	private bool hasBeenPlaced;

	private bool canClose;

	public Slot slot;

	public Inventory_ResearchTable InventoryReference => inventoryReference;

	public Slot Slot => slot;

	public bool IsOccupied
	{
		get
		{
			if (occupyingPlayerID == null)
			{
				return false;
			}
			if (occupyingPlayerID.IsValid())
			{
				return true;
			}
			return false;
		}
	}

	public void Start()
	{
	}

	public void OnBlockPlaced()
	{
		hasBeenPlaced = true;
		network = ComponentManager<Raft_Network>.Value;
		canvas = ComponentManager<CanvasHelper>.Value;
		inventoryReference = ComponentManager<Inventory_ResearchTable>.Value;
		localPlayer = network.GetLocalPlayer();
		Raft_Network.OnDisconnectEvent += OnClientDisconnect;
		CanvasHelper.MenuCloseEvent += OnMenuClose;
		GameObject gameObject = Object.Instantiate(inventoryReference.slotPrefab, inventoryReference.gridLayoutGroup.transform);
		RectTransform component = gameObject.GetComponent<RectTransform>();
		gameObject.name = "ResearchSlot " + ObjectIndex;
		gameObject.transform.localScale = inventoryReference.slotPrefab.transform.localScale;
		component.localPosition = new Vector3(0f, 0f);
		component.sizeDelta = inventoryReference.gridLayoutGroup.cellSize;
		slot = gameObject.GetComponent<Slot>();
		slot.InitializeEventListeners(inventoryReference);
		slot.gameObject.SetActive(value: false);
		inventoryReference.allSlots.Add(slot);
	}

	public void OnIsRayed()
	{
		if (!hasBeenPlaced)
		{
			return;
		}
		if (IsOccupied)
		{
			canvas.displayTextManager.HideDisplayTexts();
		}
		else
		{
			if (PlayerItemManager.IsBusy || !canvas.CanOpenMenu || !Helper.LocalPlayerIsWithinDistance(transform.position, Player.UseDistance))
			{
				return;
			}
			InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
			canvas.displayTextManager.ShowText(Helper.GetTerm("Game/Open"), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
			{
				canvas.displayTextManager.HideDisplayTexts();
				Message_NetworkBehaviour_SteamID message = new Message_NetworkBehaviour_SteamID(Messages.ResearchTable_Open, this, localPlayer.steamID);
				if (Raft_Network.IsHost)
				{
					network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
					Open(asLocalPlayer: true, localPlayer.steamID);
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

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (hasBeenPlaced)
		{
			Raft_Network.OnDisconnectEvent -= OnClientDisconnect;
			CanvasHelper.MenuCloseEvent -= OnMenuClose;
		}
		Close();
		if (slot != null && inventoryReference != null && inventoryReference.allSlots.Contains(slot))
		{
			inventoryReference.allSlots.Remove(slot);
			Object.Destroy(slot.gameObject);
		}
	}

	void SceneEventInterface.OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent == SceneEvent.LeaveGame)
		{
			inventoryReference = null;
		}
	}

	private void Update()
	{
		if (!IsOccupied || !canClose)
		{
			return;
		}
		InputAction inputAction = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
		InputAction inputAction2 = PlayerInput.GetPlayerByIndex(0).actions["Cancel"];
		bool flag = ((!SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad) ? (MyInput.GetButtonDown("Interact") || MyInput.GetButtonDown("Cancel") || MyInput.GetButtonDown("Inventory")) : (inputAction.WasPressedThisFrame() || inputAction2.WasPressedThisFrame()));
		if ((occupyingPlayerID == localPlayer.steamID) & flag)
		{
			Message_ResearchTable_Close message = new Message_ResearchTable_Close(Messages.ResearchTable_Close, this, slot);
			if (Raft_Network.IsHost)
			{
				network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				Close();
			}
			else
			{
				network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
	}

	private bool Open(bool asLocalPlayer, Network_UserId playerID)
	{
		if (IsOccupied)
		{
			return false;
		}
		occupyingPlayerID = playerID;
		animator.SetBool("Open", value: true);
		if (asLocalPlayer)
		{
			inventoryReference.SetTable(this);
			slot.gameObject.SetActive(value: true);
			canvas.OpenMenuCloseOther(MenuType.Inventory);
			inventoryReference.Show();
			PlayerItemManager.IsBusy = true;
			canClose = false;
			Invoke("AllowCloseWithUseButton", 0.1f);
			localPlayer.Inventory.secondInventory = inventoryReference;
			inventoryReference.secondInventory = localPlayer.Inventory;
			enabled = true;
		}
		return true;
	}

	private void Close()
	{
		if (!IsOccupied)
		{
			return;
		}
		if (localPlayer.steamID == occupyingPlayerID)
		{
			localPlayer.Inventory.secondInventory = (inventoryReference.secondInventory = null);
			if (slot != null)
			{
				slot.gameObject.SetActive(value: false);
			}
			inventoryReference.SetTable(null);
			inventoryReference.Hide();
			canvas.CloseMenu(MenuType.Inventory);
			PlayerItemManager.IsBusy = false;
			canClose = false;
			CancelInvoke();
			enabled = false;
		}
		animator.SetBool("Open", value: false);
		occupyingPlayerID.Clear();
	}

	private void AllowCloseWithUseButton()
	{
		canClose = true;
	}

	private void OnClientDisconnect(Network_UserId steamID, DisconnectReason reason)
	{
		if ((reason == DisconnectReason.Timeout || reason == DisconnectReason.SelfDisconnected) && occupyingPlayerID.IsValid() && occupyingPlayerID == steamID)
		{
			Close();
		}
	}

	private void OnMenuClose(MenuType menuType)
	{
		if (menuType == MenuType.Inventory && occupyingPlayerID == localPlayer.steamID)
		{
			Message_ResearchTable_Close message = new Message_ResearchTable_Close(Messages.ResearchTable_Close, this, slot);
			if (Raft_Network.IsHost)
			{
				network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				Close();
			}
			else
			{
				network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
	}

	public override RGD Serialize_Save()
	{
		return new RGD_ResearchTable(RGDType.Block_ResearchTable, GetComponent<Block>(), this);
	}

	public override void Serialize_Create()
	{
		AddMessage(new Message_ResearchTable_Create(Messages.ResearchTable_Create, this, occupyingPlayerID, this));
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.ResearchTable_Create:
		{
			Message_ResearchTable_Create obj2 = msg as Message_ResearchTable_Create;
			Network_UserId steamID = obj2.SteamID;
			if (steamID.IsValid())
			{
				Open(asLocalPlayer: false, steamID);
			}
			obj2.rgdSlot.RestoreSlot(slot);
			return false;
		}
		case Messages.ResearchTable_Open:
		{
			Network_UserId steamID2 = (msg as Message_NetworkBehaviour_SteamID).SteamID;
			if (steamID2.IsValid())
			{
				return Open(steamID2 == localPlayer.steamID, steamID2);
			}
			return false;
		}
		case Messages.ResearchTable_Close:
		{
			Message_ResearchTable_Close obj = msg as Message_ResearchTable_Close;
			Close();
			obj.rgdSlot.RestoreSlot(slot);
			return true;
		}
		case Messages.ResearchTable_Research:
		{
			Item_Base itemByIndex = ItemManager.GetItemByIndex((msg as Message_ResearchTable_ResearchOrLearn).itemIndex);
			if (itemByIndex != null && inventoryReference.Research(itemByIndex))
			{
				if (IsOccupied && occupyingPlayerID == localPlayer.steamID)
				{
					slot.RemoveItem(1);
				}
				return true;
			}
			return false;
		}
		default:
			return false;
		}
	}
}
