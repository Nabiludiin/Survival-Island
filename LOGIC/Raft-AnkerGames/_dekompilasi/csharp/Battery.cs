using System;
using FMODUnity;
using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(RaycastInteractable))]
public class Battery : MonoBehaviour, IRaycastable
{
	public Action OnStateChange;

	[SerializeField]
	private int batteryIndex;

	public ItemObjectEnabler itemEnabler;

	[SerializeField]
	private Renderer batteryHolderRenderer;

	[SerializeField]
	[Range(0f, 1f)]
	private float greenLimit;

	[SerializeField]
	[Range(0f, 1f)]
	private float yellowLimit;

	[SerializeField]
	[Range(0f, 1f)]
	private float orangeLimit;

	[SerializeField]
	private Color green;

	[SerializeField]
	private Color yellow;

	[SerializeField]
	private Color orange;

	[SerializeField]
	private Color red;

	[SerializeField]
	private MonoBehaviour_ID_Network networkBehaviourID;

	public EventReference er_placeBatteryManually;

	private const int usesLostPerSecond = 1;

	private const float drainTime = 10f;

	private Network_Player localPlayer;

	private ItemInstance batteryInstance;

	private CanvasHelper canvas;

	private MaterialPropertyBlock mpb;

	private bool showingText;

	private bool on;

	private float timer;

	public MonoBehaviour_ID_Network NetworkMessageReciever => networkBehaviourID;

	public float NormalizedBatteryLeft
	{
		get
		{
			if (!BatterySlotIsEmpty)
			{
				return batteryInstance.NormalizedUses;
			}
			return 0f;
		}
	}

	public bool BatterySlotIsEmpty
	{
		get
		{
			if (batteryInstance != null)
			{
				return !batteryInstance.Valid;
			}
			return true;
		}
	}

	public bool CanGiveElectricity
	{
		get
		{
			if (!BatterySlotIsEmpty)
			{
				return batteryInstance.Uses > 0;
			}
			return false;
		}
	}

	public int BatteryUses
	{
		get
		{
			if (!BatterySlotIsEmpty)
			{
				return batteryInstance.Uses;
			}
			return 0;
		}
	}

	public bool On
	{
		get
		{
			return on;
		}
		set
		{
			if (on != value)
			{
				on = value;
				if (OnStateChange != null)
				{
					OnStateChange();
				}
			}
		}
	}

	public int MyProperty { get; set; }

	public int BatteryIndex => batteryIndex;

	private void Awake()
	{
		itemEnabler.DeActivateAllModels();
	}

	private void Start()
	{
		canvas = ComponentManager<CanvasHelper>.Value;
		localPlayer = ComponentManager<Network_Player>.Value;
		UpdateBatteryHolderLights();
	}

	private void Update()
	{
		if (localPlayer == null)
		{
			localPlayer = ComponentManager<Network_Player>.Value;
		}
		else if (Raft_Network.IsHost && On && CanGiveElectricity)
		{
			timer += Time.deltaTime;
			if (timer >= 10f)
			{
				timer = 0f;
				Update(batteryInstance.Uses - 1);
				localPlayer.Network.RPC(new Message_Battery(Messages.Battery_Update, localPlayer.Network.NetworkIDManager, localPlayer.steamID, networkBehaviourID.ObjectIndex, batteryInstance.Uses, batteryIndex), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
	}

	public bool OnBatteryMessage(Message_NetworkBehaviour msg)
	{
		if (localPlayer == null)
		{
			localPlayer = ComponentManager<Network_Player>.Value;
		}
		switch (msg.Type)
		{
		case Messages.Battery_Insert:
		{
			Message_Battery message_Battery2 = msg as Message_Battery;
			Network_Player playerFromID = localPlayer.Network.GetPlayerFromID(message_Battery2.SteamID);
			if (playerFromID != null)
			{
				return Insert(playerFromID, message_Battery2.batteryUsesLeft, message_Battery2.batteryUniqueIndex);
			}
			break;
		}
		case Messages.Battery_Take:
		{
			Message_Battery message_Battery3 = msg as Message_Battery;
			Network_Player playerFromID2 = localPlayer.Network.GetPlayerFromID(message_Battery3.SteamID);
			if (playerFromID2 != null)
			{
				return Take(playerFromID2, message_Battery3.batteryUsesLeft);
			}
			break;
		}
		case Messages.Battery_Update:
		{
			Message_Battery message_Battery = msg as Message_Battery;
			Update(message_Battery.batteryUsesLeft);
			return true;
		}
		case Messages.Battery_OnOff:
		{
			if (BatterySlotIsEmpty)
			{
				break;
			}
			Message_Battery_OnOff message_Battery_OnOff = msg as Message_Battery_OnOff;
			if (Raft_Network.IsHost)
			{
				if (message_Battery_OnOff.on && CanGiveElectricity)
				{
					On = true;
					return true;
				}
				if (!message_Battery_OnOff.on)
				{
					On = false;
					return true;
				}
				return false;
			}
			On = message_Battery_OnOff.on;
			break;
		}
		}
		return false;
	}

	public bool RemoveBatteryUsesNetworked(int usesToRemove)
	{
		if (Raft_Network.IsHost && CanGiveElectricity)
		{
			Update(batteryInstance.Uses - usesToRemove);
			localPlayer.Network.RPC(new Message_Battery(Messages.Battery_Update, localPlayer.Network.NetworkIDManager, localPlayer.steamID, networkBehaviourID.ObjectIndex, batteryInstance.Uses, batteryIndex), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			return true;
		}
		return false;
	}

	public bool AddBatteryUsesNetworked(int usesToAdd)
	{
		if (Raft_Network.IsHost && !BatterySlotIsEmpty)
		{
			Update(batteryInstance.Uses + usesToAdd);
			localPlayer.Network.RPC(new Message_Battery(Messages.Battery_Update, localPlayer.Network.NetworkIDManager, localPlayer.steamID, networkBehaviourID.ObjectIndex, batteryInstance.Uses, batteryIndex), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			return true;
		}
		return false;
	}

	public void Update(int batteryUses)
	{
		if (!BatterySlotIsEmpty)
		{
			batteryInstance.Uses = batteryUses;
			if (batteryUses == 0)
			{
				On = false;
			}
		}
		UpdateBatteryHolderLights();
	}

	public bool Insert(Network_Player player, int batteryUses, int uniqueItemIndex = 176)
	{
		if (BatterySlotIsEmpty)
		{
			RuntimeManager.PlayOneShot(er_placeBatteryManually, transform.position);
			if (player != null && player.IsLocalPlayer)
			{
				localPlayer.Inventory.RemoveSelectedHotSlotItem(1);
			}
			Item_Base itemByIndex = ItemManager.GetItemByIndex(uniqueItemIndex);
			if (itemByIndex == null)
			{
				Debug.Log("Unable to find item with index " + uniqueItemIndex);
				return false;
			}
			batteryInstance = new ItemInstance(itemByIndex, 1, batteryUses);
			itemEnabler.ActivateModel(itemByIndex);
			UpdateBatteryHolderLights();
			return true;
		}
		return false;
	}

	public bool Take(Network_Player player, int batteryUsesLeft)
	{
		if (!BatterySlotIsEmpty)
		{
			if (player.IsLocalPlayer)
			{
				localPlayer.Inventory.AddItem(new ItemInstance(batteryInstance.baseItem, 1, batteryUsesLeft));
			}
			On = false;
			batteryInstance = null;
			itemEnabler.DeActivateAllModels();
			UpdateBatteryHolderLights();
			return true;
		}
		return false;
	}

	public ItemInstance GetIsBatteryValid()
	{
		if (this != null)
		{
			ItemInstance itemInstance = GetBatteryInstance();
			if (itemInstance != null)
			{
				return itemInstance;
			}
		}
		return null;
	}

	public ItemInstance GetBatteryInstance()
	{
		return batteryInstance;
	}

	public void OnIsRayed()
	{
		if (canvas == null)
		{
			canvas = ComponentManager<CanvasHelper>.Value;
			return;
		}
		if (localPlayer == null)
		{
			localPlayer = ComponentManager<Network_Player>.Value;
			return;
		}
		if (CanvasHelper.ActiveMenu != MenuType.None || !Helper.LocalPlayerIsWithinDistance(transform.position, Player.UseDistance))
		{
			if (showingText)
			{
				canvas.displayTextManager.HideDisplayTexts();
				showingText = false;
			}
			return;
		}
		InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
		if (BatterySlotIsEmpty)
		{
			Slot selectedHotbarSlot = localPlayer.Inventory.GetSelectedHotbarSlot();
			if (!(selectedHotbarSlot != null) || !selectedHotbarSlot.HasValidItemInstance() || !itemEnabler.DoesAcceptItem(selectedHotbarSlot.itemInstance.baseItem))
			{
				return;
			}
			Item_Base itemBase = selectedHotbarSlot.GetItemBase();
			LocalizationParameters.itemX = itemBase.settings_Inventory.DisplayName;
			canvas.displayTextManager.ShowText(Helper.GetTerm("Game/PlaceItemX", applyParameters: true), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
			showingText = true;
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
			{
				canvas.displayTextManager.HideDisplayTexts();
				showingText = false;
				Message_Battery message = new Message_Battery(Messages.Battery_Insert, localPlayer.Network.NetworkIDManager, localPlayer.steamID, networkBehaviourID.ObjectIndex, selectedHotbarSlot.itemInstance.Uses, batteryIndex, itemBase.UniqueIndex);
				if (Raft_Network.IsHost)
				{
					Insert(localPlayer, selectedHotbarSlot.itemInstance.Uses, itemBase.UniqueIndex);
					localPlayer.Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				}
				else
				{
					localPlayer.SendP2P(message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				}
			}
			return;
		}
		LocalizationParameters.itemX = ItemManager.GetItemByIndex(batteryInstance.UniqueIndex).settings_Inventory.DisplayName;
		canvas.displayTextManager.ShowText(Helper.GetTerm("Game/PickUpX", applyParameters: true), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
		showingText = true;
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
		{
			canvas.displayTextManager.HideDisplayTexts();
			showingText = false;
			Message_Battery message2 = new Message_Battery(Messages.Battery_Take, localPlayer.Network.NetworkIDManager, localPlayer.steamID, networkBehaviourID.ObjectIndex, batteryInstance.Uses, batteryIndex);
			if (Raft_Network.IsHost)
			{
				Take(localPlayer, batteryInstance.Uses);
				localPlayer.Network.RPC(message2, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
			else
			{
				localPlayer.SendP2P(message2, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
	}

	public void OnRayEnter()
	{
	}

	void IRaycastable.OnRayExit()
	{
		canvas.displayTextManager.HideDisplayTexts();
		showingText = false;
	}

	private void UpdateBatteryHolderLights()
	{
		if (batteryHolderRenderer == null)
		{
			return;
		}
		if (mpb == null)
		{
			mpb = new MaterialPropertyBlock();
		}
		batteryHolderRenderer.GetPropertyBlock(mpb);
		if (BatterySlotIsEmpty)
		{
			mpb.SetColor("_EmissionColor", Color.black);
		}
		else
		{
			float num = (BatterySlotIsEmpty ? 0f : batteryInstance.NormalizedUses);
			Color value = red;
			if (num > greenLimit)
			{
				value = green;
			}
			else if (num > yellowLimit)
			{
				value = yellow;
			}
			else if (num > orangeLimit)
			{
				value = orange;
			}
			mpb.SetColor("_EmissionColor", value);
		}
		batteryHolderRenderer.SetPropertyBlock(mpb);
	}
}
