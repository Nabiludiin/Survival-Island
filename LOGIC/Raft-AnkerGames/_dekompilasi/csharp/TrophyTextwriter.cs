using Steamworks;
using UnityEngine.InputSystem;

public class TrophyTextwriter : TextWriterObject, IRaycastable
{
	public bool editWithMouseHover = true;

	private Raft_Network network;

	private Network_Player player;

	private ChatTextFieldController chat;

	protected override void Start()
	{
		base.Start();
		network = ComponentManager<Raft_Network>.Value;
		player = network.GetLocalPlayer();
		chat = ComponentManager<ChatTextFieldController>.Value;
	}

	void IRaycastable.OnIsRayed()
	{
		if (hasBeenInitialized && editWithMouseHover)
		{
			if (CanvasHelper.ActiveMenu != MenuType.None || PlayerItemManager.IsBusy)
			{
				canvas.displayTextManager.HideDisplayTexts();
			}
			else if (hasBeenInitialized && !GetIsBeingEdited() && canvas.CanOpenMenu && Helper.LocalPlayerIsWithinDistance(transform.position, Player.UseDistance))
			{
				DisplayRenamePrompt(value: true);
				CheckForInteractInput("Interact");
			}
			else
			{
				DisplayRenamePrompt(value: false);
			}
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

	public void CheckForInteractInput(string interactName)
	{
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(PlayerInput.GetPlayerByIndex(0).actions[(interactName == "Interact") ? "Interact" : "Remove"], interactName))
		{
			SendStartEditing();
		}
	}

	public void DisplayRenamePrompt(bool value, string keyName = "Interact", int displayIndex = 0, bool clearOtherText = true, string termDisplayText = "Game/RewriteSign")
	{
		if (value)
		{
			canvas.displayTextManager.ShowText(Helper.GetTerm(termDisplayText), PlayerInput.GetPlayerByIndex(0).actions[(keyName == "Interact") ? "Interact" : "Remove"].GetBindingDisplayString(), MyInput.Keybinds[keyName].MainKey, displayIndex, 0, clearOtherText);
		}
		else
		{
			canvas.displayTextManager.HideDisplayTexts();
		}
	}

	public void SendStartEditing()
	{
		Message_NetworkBehaviour_ID_SteamID message_NetworkBehaviour_ID_SteamID = new Message_NetworkBehaviour_ID_SteamID(Messages.Sign_StartEdit, network.NetworkIDManager, ObjectIndex, player.steamID);
		if (Raft_Network.IsHost)
		{
			network.RPC(message_NetworkBehaviour_ID_SteamID, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			StartEdit(network.GetPlayerFromID(message_NetworkBehaviour_ID_SteamID.SteamID));
		}
		else
		{
			network.SendP2P(network.HostID, message_NetworkBehaviour_ID_SteamID, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	public bool StartEdit(Network_Player p_player)
	{
		if (GetIsBeingEdited())
		{
			return false;
		}
		if (p_player.IsLocalPlayer)
		{
			chat.DisableChatTyping();
			canvas.OpenMenuCloseOther(MenuType.TextWriter, force: true);
			canvas.GetComponent<GamepadCursor>().SetLayer(GamepadCursorLayer.PopupWindows);
			textWriterMenu.StartWriting(this);
		}
		SetIsBeingEdited(p_value: true);
		return true;
	}

	public bool EndEdit()
	{
		if (!GetIsBeingEdited())
		{
			return false;
		}
		SetIsBeingEdited(p_value: false);
		return true;
	}

	public override void SendOnSignEdited(string p_newString)
	{
		base.SendOnSignEdited(p_newString);
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		if (network == null)
		{
			network = ComponentManager<Raft_Network>.Value;
		}
		switch (msg.Type)
		{
		case Messages.Sign_StartEdit:
		{
			Message_NetworkBehaviour_ID_SteamID message_NetworkBehaviour_ID_SteamID = msg as Message_NetworkBehaviour_ID_SteamID;
			return StartEdit(network.GetPlayerFromID(message_NetworkBehaviour_ID_SteamID.SteamID));
		}
		case Messages.Sign_EndEdit:
			SetIsBeingEdited(p_value: false);
			return true;
		default:
			return base.Deserialize(msg, remoteID);
		}
	}
}
