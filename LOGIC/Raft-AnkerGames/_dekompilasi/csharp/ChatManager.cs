using Steamworks;
using UnityEngine;

public class ChatManager : MonoBehaviour_Network
{
	public delegate void OnRecieveChatMessageAction();

	[HideInInspector]
	public Raft_Network network;

	public ChatTextFieldController chatFieldController;

	public static event OnRecieveChatMessageAction OnRecieveChatMessage;

	private void Awake()
	{
		ComponentManager<ChatManager>.Value = this;
	}

	private void Start()
	{
		network = ComponentManager<Raft_Network>.Value;
		chatFieldController = GetComponent<ChatTextFieldController>();
		if (Raft_Network.IsHost && GameManager.IsInNewGame)
		{
			NetworkUpdateManager.SetIndexForBehaviour(this);
			NetworkUpdateManager.AddBehaviour(this);
		}
	}

	public override RGD Serialize_Save()
	{
		return new RGD_NetworkBehaviour(RGDType.NetworkBehaviour_ChatManager, this);
	}

	public override void Serialize_Create()
	{
		AddMessage(new Message_NetworkBehaviour(Messages.Create_Chat_Manager, this));
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		if (msg.Type == Messages.Ingame_Chat_Message)
		{
			Message_IngameChat message_IngameChat = msg as Message_IngameChat;
			if (!network.IsUserBlocked(remoteID))
			{
				HandleChatMessageInput(message_IngameChat.messageText, message_IngameChat.SteamID);
			}
			return true;
		}
		return base.Deserialize(msg, remoteID);
	}

	public static void SendDebugChatMessage(string message)
	{
		ChatManager chatManager = null;
		Network_Player network_Player = null;
		if (chatManager == null)
		{
			chatManager = ComponentManager<ChatManager>.Value;
		}
		if (network_Player == null)
		{
			network_Player = ComponentManager<Network_Player>.Value;
		}
		chatManager.SendChatMessage(message, network_Player.steamID);
	}

	public static void LocalDebugChatMessage(string message)
	{
		ChatManager chatManager = null;
		Network_Player network_Player = null;
		if (chatManager == null)
		{
			chatManager = ComponentManager<ChatManager>.Value;
		}
		if (network_Player == null)
		{
			network_Player = ComponentManager<Network_Player>.Value;
		}
		chatManager.HandleChatMessageInput(message, network_Player.steamID);
	}

	public void SendChatMessage(string p_message, Network_UserId p_steamID)
	{
		if (!(network == null))
		{
			Message_IngameChat message = new Message_IngameChat(Messages.Ingame_Chat_Message, this, p_steamID, p_message);
			if (Raft_Network.IsHost)
			{
				HandleChatMessageInput(p_message, p_steamID);
				network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
			else
			{
				network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
	}

	public void HandleChatMessageInput(string text, Network_UserId textWriterSteamID)
	{
		bool flag = chatFieldController.HandleChatMessageAsCheat(text, textWriterSteamID);
		bool flag2 = chatFieldController.HandleChatMessageAsTerminalCommando(text, textWriterSteamID);
		if (!flag && !flag2)
		{
			chatFieldController.AttemptToAddUITextMessage(text, textWriterSteamID);
		}
		SendRecieveChatMessageAction();
	}

	private void SendRecieveChatMessageAction()
	{
		if (OnRecieveChatMessage != null)
		{
			OnRecieveChatMessage();
		}
	}
}
