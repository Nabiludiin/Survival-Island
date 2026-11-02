using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

public class Block_Wardrobe : Block, IRaycastable
{
	[SerializeField]
	private Placeable_Wardrobe placeableWardrobe;

	private CanvasHelper canvas;

	private bool inputReleased;

	protected override void Start()
	{
		base.Start();
		canvas = ComponentManager<CanvasHelper>.Value;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (hasBeenPlaced && !GameManager.IsLeavingGame && CanvasHelper.ActiveMenu == MenuType.Wardrobe && canvas != null)
		{
			Close();
		}
	}

	private void Update()
	{
		if (WardrobeMenu.wardrobeOpened != this || !(canvas != null) || CanvasHelper.ActiveMenu != MenuType.Wardrobe)
		{
			return;
		}
		InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
		InputAction action2 = PlayerInput.GetPlayerByIndex(0).actions["Inventory"];
		InputAction action3 = PlayerInput.GetPlayerByIndex(0).actions["Cancel"];
		bool flag = SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action3, "Cancel") || SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action2, "Inventory") || SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact");
		if (inputReleased)
		{
			if (flag)
			{
				Close();
			}
		}
		else if (!flag)
		{
			inputReleased = true;
		}
	}

	void IRaycastable.OnIsRayed()
	{
		if (hasBeenPlaced && CanvasHelper.ActiveMenu == MenuType.None && !PlayerItemManager.IsBusy && !(canvas == null) && canvas.CanOpenMenu && Helper.LocalPlayerIsWithinDistance(transform.position, Player.UseDistance))
		{
			InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
			canvas.displayTextManager.ShowText(Helper.GetTerm("Game/Open"), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
			{
				canvas.displayTextManager.HideDisplayTexts();
				canvas.OpenMenuCloseOther(MenuType.Wardrobe);
				canvas.GetComponent<GamepadCursor>().SetLayer(GamepadCursorLayer.Wardrobe);
				WardrobeMenu.wardrobeOpened = this;
				placeableWardrobe.network.SendP2P(placeableWardrobe.network.HostID, new Message_NetworkBehaviour_ID_SteamID(Messages.Wardrobe_Open, placeableWardrobe.network.NetworkIDManager, placeableWardrobe.ObjectIndex, placeableWardrobe.network.GetLocalPlayer().steamID), EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				inputReleased = false;
			}
		}
	}

	void IRaycastable.OnRayEnter()
	{
	}

	void IRaycastable.OnRayExit()
	{
		if (hasBeenPlaced)
		{
			canvas.displayTextManager.HideDisplayTexts();
		}
	}

	public void Close()
	{
		canvas.CloseMenu(MenuType.Wardrobe);
		canvas.GetComponent<GamepadCursor>().SetLayer(GamepadCursorLayer.Inventory);
		placeableWardrobe.network.SendP2P(placeableWardrobe.network.HostID, new Message_NetworkBehaviour_ID_SteamID(Messages.Wardrobe_Close, placeableWardrobe.network.NetworkIDManager, placeableWardrobe.ObjectIndex, placeableWardrobe.network.GetLocalPlayer().steamID), EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
	}

	public override RGD Serialize_Save()
	{
		if (IsValidSerialization() && buildableItem != null)
		{
			return new RGD_Block(RGDType.Block, this);
		}
		return null;
	}
}
