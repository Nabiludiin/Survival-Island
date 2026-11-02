using Steamworks;
using UnityEngine.InputSystem;

public class MeshPath_Zipline : MeshPath, IRaycastable
{
	private CanvasHelper canvas;

	private Network_Player localPlayer;

	private void Start()
	{
		canvas = ComponentManager<CanvasHelper>.Value;
		localPlayer = ComponentManager<Network_Player>.Value;
	}

	void IRaycastable.OnIsRayed()
	{
		if (localPlayer == null || PlayerItemManager.IsBusy || CanvasHelper.ActiveMenu != MenuType.None)
		{
			return;
		}
		if (!localPlayer.Inventory.HasEquipmentOfTypeEquipped(EquipSlotType.ZiplineTool))
		{
			canvas.displayTextManager.ShowText(Helper.GetTerm("Game/Zipline/RequiresTool"));
			return;
		}
		if (localPlayer.ZiplinePlayer.IsAttachedToZipline || MeshPathBase.RecentlyCreatedMeshPath)
		{
			canvas.displayTextManager.HideDisplayTexts();
			return;
		}
		InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
		canvas.displayTextManager.ShowText(Helper.GetTerm("Game/Zipline/Attach"), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
		{
			canvas.displayTextManager.HideDisplayTexts();
			AttachPlayerToZipline();
		}
	}

	void IRaycastable.OnRayEnter()
	{
	}

	void IRaycastable.OnRayExit()
	{
		if (canvas != null)
		{
			canvas.displayTextManager.HideDisplayTexts();
		}
	}

	private void AttachPlayerToZipline()
	{
		if (localPlayer == null)
		{
			localPlayer = ComponentManager<Network_Player>.Value;
		}
		if (localPlayer != null && localPlayer.ZiplinePlayer.AttachToZipline(this))
		{
			MeshPathConnection blockConnection = MeshPath.GetBlockConnection(this);
			localPlayer.Network.SendP2P(localPlayer.Network.HostID, new Message_Zipline_Attach(Messages.ZiplineAttach, localPlayer, blockConnection), EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}
}
