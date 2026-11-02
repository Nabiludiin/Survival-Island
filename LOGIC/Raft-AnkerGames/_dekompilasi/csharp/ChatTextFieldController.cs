using System;
using System.Collections.Generic;
using System.Linq;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

public class ChatTextFieldController : MonoBehaviour
{
	public ChatManager chatManager;

	public Scrollbar scrollBarChatContentBar;

	public Transform chatMessagePoolParent;

	public ChatVisibilityController chatVisiblityController;

	public InputField chatInputField;

	public VerticalLayoutGroup chatLayoutGroup;

	[SerializeField]
	private RectTransform chatMessageParent;

	[SerializeField]
	private bool autoScrollChatToBottomOnMessage = true;

	[Range(0f, 200f)]
	[SerializeField]
	private int chatMessageLimit = 10;

	[SerializeField]
	private List<string> previousMessages = new List<string>();

	[SerializeField]
	private int currentPrevMessageIndex;

	[SerializeField]
	private string commandDelimeter = "/";

	[SerializeField]
	private string terminalCommandDelimeter = "#";

	[SerializeField]
	private ObjectPool chatMessagePool = new ObjectPool();

	[SerializeField]
	private Color adminHost = Color.white;

	[SerializeField]
	private Color adminClient = Color.white;

	[SerializeField]
	private Color normalHost = Color.white;

	[SerializeField]
	private Color normalClient = Color.white;

	[Range(0f, 1f)]
	private float currentScrollValue;

	private List<RectTransform> chatMessageList = new List<RectTransform>();

	private Network_Player localPlayer;

	private Raft_Network network;

	private GameManager gameManager;

	private CanvasHelper canvas;

	private int storedMessageLimitCount = 5;

	private static bool isChatWindowSelected;

	public static bool IsChatWindowSelected
	{
		get
		{
			return isChatWindowSelected;
		}
		set
		{
			isChatWindowSelected = value;
		}
	}

	private void Awake()
	{
		ComponentManager<ChatTextFieldController>.Value = this;
	}

	private void OnDestroy()
	{
		Raft_Network.OnWorldReceived = (Action)Delegate.Remove(Raft_Network.OnWorldReceived, new Action(OnWorldRecieved));
		Raft_Network.OnDisconnectEvent -= CreatePlayerLeaveText;
	}

	private void Start()
	{
		if (chatInputField == null)
		{
			chatInputField = transform.Find("InputField").GetComponent<InputField>();
		}
		chatManager = GetComponent<ChatManager>();
		chatVisiblityController = GetComponent<ChatVisibilityController>();
		network = ComponentManager<Raft_Network>.Value;
		canvas = ComponentManager<CanvasHelper>.Value;
		Raft_Network.OnWorldReceived = (Action)Delegate.Combine(Raft_Network.OnWorldReceived, new Action(OnWorldRecieved));
		chatMessagePool.Initialize();
	}

	private void Update()
	{
		if (!(localPlayer == null))
		{
			HandleInput();
		}
	}

	public void OnChatInputfieldDeselect()
	{
		if (IsChatWindowSelected)
		{
			DisableChatTyping();
		}
	}

	public void PreHandleChatMessage(string p_message)
	{
		if (string.IsNullOrEmpty(p_message) || p_message.All(char.IsWhiteSpace))
		{
			HandleOnSendMessageInputField();
			return;
		}
		if (p_message != string.Empty)
		{
			currentPrevMessageIndex = 0;
			StoreMessageToPreviousMessages(p_message);
			chatManager.SendChatMessage(p_message, localPlayer.steamID);
		}
		ClearPreviousMessagesStorage();
		HandleOnSendMessageInputField();
	}

	public bool HandleChatMessageAsCheat(string p_message, Network_UserId p_steamID)
	{
		bool flag = CheckIfStringIsACommand(p_message);
		if (flag || p_message == string.Empty)
		{
			Network_Player playerFromID = network.GetPlayerFromID(p_steamID);
			if (playerFromID != null)
			{
				CSteamID cSteamID = ((!playerFromID.IsLocalPlayer) ? new CSteamID(network.GetPlatformSpecificId(p_steamID)) : SteamUser.GetSteamID());
				if (Cheat.AllowCheatsForUser(cSteamID) && p_message != string.Empty)
				{
					currentPrevMessageIndex = 0;
					if (localPlayer.steamID == p_steamID)
					{
						StoreMessageToPreviousMessages(p_message);
					}
					Cheat.HandleCheatCode(p_message, commandDelimeter, p_steamID);
				}
				chatVisiblityController.StartChatActivityCoroutine1();
			}
		}
		return flag;
	}

	public bool HandleChatMessageAsTerminalCommando(string p_message, Network_UserId p_steamID)
	{
		bool flag = CheckIfStringIsTerminalCommand(p_message);
		if (flag)
		{
			Cheat.HandleTerminalCommand(p_message, terminalCommandDelimeter, p_steamID);
		}
		return flag;
	}

	public void AttemptToAddUITextMessage(string p_ChatMessage, Network_UserId p_steamID)
	{
		AddUITextMessage(p_ChatMessage, p_steamID);
	}

	public void AddUITextMessage(string p_ChatMessage, Network_UserId p_steamID)
	{
		GameObject gameObject = CreateChatMessagePanel();
		ChatMessagePanel component = gameObject.GetComponent<ChatMessagePanel>();
		SetChatMessagePanelsUsername(component.usernameText, p_steamID);
		SetChatMessagePanelsText(component, p_ChatMessage);
		SetChatMessagePanelsImage(component.uiImage, p_steamID);
		chatMessageList.Add(gameObject.GetComponent<RectTransform>());
		CheckMessageLimit();
		ScrollContentToBottom();
	}

	public void CreateLocalChatMessage(string message)
	{
		if (localPlayer == null)
		{
			Debug.Log("Can't write local chat message because localplayer reference is null");
			return;
		}
		AttemptToAddUITextMessage(message, localPlayer.steamID);
		chatVisiblityController.UpdateChatVisiblity();
	}

	public void CreatePlayerJoinText(Network_Player p_player)
	{
		if (!(p_player.steamID == 0uL) && Raft_Network.WorldHasBeenRecieved && p_player != network.GetLocalPlayer())
		{
			string empty = string.Empty;
			empty = Helper.GetTerm("Game/PlayerJoined");
			AddUITextMessage(empty, p_player.steamID);
			chatVisiblityController.UpdateChatVisiblity();
		}
	}

	public void CreatePlayerLeaveText(Network_UserId steamID, DisconnectReason reason)
	{
		if (network.GetPlayerFromID(steamID) != null)
		{
			string empty = string.Empty;
			empty = Helper.GetTerm("Game/PlayerDisconnected");
			AddUITextMessage(empty, steamID);
			chatVisiblityController.UpdateChatVisiblity();
		}
	}

	public void DisableChatTyping()
	{
		if (localPlayer != null && !localPlayer.BedComponent.Sleeping && !localPlayer.PlayerNetworkManager.IsAttached)
		{
			if (!localPlayer.CarryingComponent.IsCarrying)
			{
				PlayerItemManager.IsBusy = false;
			}
			if (!localPlayer.flightCamera.enabled)
			{
				localPlayer.PersonController.IsMovementFree = true;
			}
		}
		IsChatWindowSelected = false;
		SetChatFieldVisible(value: false);
		chatInputField.DeactivateInputField();
		chatVisiblityController.StartChatActivityCoroutine1();
	}

	private void OnWorldRecieved()
	{
		Cheat.Initialize();
		localPlayer = ComponentManager<Network_Player>.Value;
		Raft_Network.OnDisconnectEvent += CreatePlayerLeaveText;
	}

	private bool CheckIfStringIsACommand(string p_string)
	{
		if (p_string == string.Empty)
		{
			return false;
		}
		if (p_string.StartsWith(commandDelimeter))
		{
			return true;
		}
		return false;
	}

	private bool CheckIfStringIsTerminalCommand(string p_string)
	{
		if (p_string == string.Empty)
		{
			return false;
		}
		if (p_string.StartsWith(terminalCommandDelimeter))
		{
			return true;
		}
		return false;
	}

	private void HandleInput()
	{
		bool flag = true;
		flag = localPlayer.PlayerScript.IsDead || ((localPlayer.PlayerNetworkManager.IsAttached || localPlayer.BedComponent.Sleeping || localPlayer.RessurectComponent.IsCarrying) ? (CanvasHelper.ActiveMenu == MenuType.None) : (!PlayerItemManager.IsBusy && CanvasHelper.ActiveMenu == MenuType.None));
		if (GeneralSettingsBox.ChatVisible && IsChatWindowSelected)
		{
			if (MyInput.GetButtonDown("PreviousChatMessageIDUp"))
			{
				currentPrevMessageIndex++;
				if (previousMessages.Count >= currentPrevMessageIndex)
				{
					if (previousMessages[previousMessages.Count - currentPrevMessageIndex] != string.Empty)
					{
						chatInputField.text = previousMessages[previousMessages.Count - currentPrevMessageIndex];
					}
				}
				else
				{
					currentPrevMessageIndex = previousMessages.Count;
				}
			}
			else if (MyInput.GetButtonDown("PreviousChatMessageIDDown"))
			{
				currentPrevMessageIndex--;
				if (previousMessages.Count >= currentPrevMessageIndex && currentPrevMessageIndex > 0)
				{
					if (previousMessages[previousMessages.Count - currentPrevMessageIndex] != string.Empty)
					{
						chatInputField.text = previousMessages[previousMessages.Count - currentPrevMessageIndex];
					}
				}
				else
				{
					currentPrevMessageIndex = 0;
				}
			}
			else if (Input.GetKey(KeyCode.PageUp))
			{
				ScrollChatUpwards();
			}
			else if (Input.GetKey(KeyCode.PageDown))
			{
				ScrollChatDownwards();
			}
		}
		if (!GeneralSettingsBox.ChatVisible)
		{
			return;
		}
		if (MyInput.GetButtonDown("Chat"))
		{
			if (IsChatWindowSelected)
			{
				chatVisiblityController.ResetAutoHideTimer();
				PreHandleChatMessage(GetInputFieldsText());
			}
			else if (flag)
			{
				chatVisiblityController.ResetAutoHideTimer();
				ActivateChatTyping();
			}
		}
		else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.KeypadEnter))
		{
			OnChatInputfieldDeselect();
		}
	}

	private void SetChatFieldVisible(bool value)
	{
		if (canvas == null)
		{
			canvas = ComponentManager<CanvasHelper>.Value;
		}
		if (canvas != null)
		{
			if (value)
			{
				canvas.OpenMenu(MenuType.ChatField, force: true);
			}
			else
			{
				canvas.CloseMenu(MenuType.ChatField);
			}
		}
	}

	private void ScrollChatUpwards()
	{
		currentScrollValue += Time.deltaTime;
		if (currentScrollValue > 1f)
		{
			currentScrollValue = 1f;
		}
		SetScrollbarScroll(currentScrollValue);
	}

	private void ScrollChatDownwards()
	{
		currentScrollValue -= Time.deltaTime;
		if (currentScrollValue < 0f)
		{
			currentScrollValue = 0f;
		}
		SetScrollbarScroll(currentScrollValue);
	}

	private GameObject CreateChatMessagePanel()
	{
		GameObject gameObject = chatMessagePool.GetObject(returnObjectActive: true);
		gameObject.transform.SetParent(chatMessageParent);
		gameObject.transform.localScale = Vector3.one;
		return gameObject;
	}

	private void SetChatMessagePanelsUsername(Text userNameText, Network_UserId p_steamID)
	{
		Network_Player playerFromID = network.GetPlayerFromID(p_steamID);
		if (playerFromID != null)
		{
			string visualName = playerFromID.visualName;
			userNameText.text = visualName;
			userNameText.color = GetPlayerTextColor(p_steamID);
		}
	}

	private Color GetPlayerTextColor(Network_UserId p_steamID)
	{
		if (Cheat.IsUserDev(p_steamID))
		{
			if (network.HostID == p_steamID)
			{
				return adminHost;
			}
			return adminClient;
		}
		if (network.HostID == p_steamID)
		{
			return normalHost;
		}
		return normalClient;
	}

	private string GetProperColorCode(Network_UserId p_steamID)
	{
		string empty = string.Empty;
		if (Cheat.IsUserDev(p_steamID))
		{
			if (network.HostID == p_steamID)
			{
				return "<color=#00ff00ff>";
			}
			return "color=#ff0000ff>";
		}
		if (network.HostID == p_steamID)
		{
			return "<color=#008000ff>";
		}
		return "color=#800000ff>";
	}

	private void SetChatMessagePanelsText(ChatMessagePanel p_panel, string p_newText)
	{
		p_panel.SetText(p_newText);
	}

	private void SetChatMessagePanelsImage(Image p_chatPanelImage, Network_UserId p_steamID)
	{
		Network_Player playerFromID = network.GetPlayerFromID(p_steamID);
		if (playerFromID != null)
		{
			p_chatPanelImage.sprite = playerFromID.GetSmallSteamProfile();
		}
	}

	private string GetInputFieldsText()
	{
		return chatInputField.text;
	}

	private void ScrollContentToBottom()
	{
		LayoutRebuilder.ForceRebuildLayoutImmediate(chatMessageParent);
		if (autoScrollChatToBottomOnMessage)
		{
			scrollBarChatContentBar.value = 0f;
		}
	}

	private void SetScrollbarScroll(float p_value)
	{
		scrollBarChatContentBar.value = p_value;
	}

	private void CheckMessageLimit()
	{
		if (chatMessageList.Count > chatMessageLimit)
		{
			RemoveExcessiveMessages();
		}
	}

	private void RemoveExcessiveMessages()
	{
		for (int num = chatMessageList.Count - 1; num >= chatMessageLimit; num--)
		{
			chatMessageList[num - chatMessageLimit].gameObject.transform.SetParent(chatMessagePoolParent);
			chatMessageList[num - chatMessageLimit].gameObject.SetActive(value: false);
			chatMessageList.RemoveAt(num - chatMessageLimit);
		}
	}

	private void StoreMessageToPreviousMessages(string message)
	{
		if (previousMessages.Count == 0)
		{
			previousMessages.Add(message);
		}
		else if (previousMessages[previousMessages.Count - 1] != message)
		{
			previousMessages.Add(message);
		}
	}

	private void ClearPreviousMessagesStorage()
	{
		for (int num = previousMessages.Count - 1; num >= storedMessageLimitCount; num--)
		{
			previousMessages.RemoveAt(num - storedMessageLimitCount);
		}
	}

	private void ActivateChatTyping()
	{
		chatVisiblityController.SetChatVisible(p_value: true);
		SetChatFieldVisible(value: true);
		chatInputField.ActivateInputField();
		PlayerItemManager.IsBusy = true;
		IsChatWindowSelected = true;
		if (localPlayer != null)
		{
			localPlayer.PersonController.IsMovementFree = false;
		}
	}

	private void HandleOnSendMessageInputField()
	{
		chatInputField.text = string.Empty;
		DisableChatTyping();
	}
}
