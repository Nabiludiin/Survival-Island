using System;
using System.Collections.Generic;
using PlayFab.Party;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : MenuBox
{
	private const int TabIndex_Lobby = 0;

	private const int TabIndex_Friends = 1;

	private const int TabIndex_Blocked = 2;

	public static Action OnSocialClosed;

	[SerializeField]
	private TabGroup socialTabGroup;

	[SerializeField]
	private Lobby_Selection socialSelectionPrefab;

	[SerializeField]
	private GameObject profileText;

	[SerializeField]
	public ScrollRect lobbyScrollRect;

	[SerializeField]
	private Selectable lobbyListSelectable;

	private List<Lobby_Selection> lobbyMembers = new List<Lobby_Selection>();

	[HideInInspector]
	public Lobby_Selection selectedLobbyMember;

	[SerializeField]
	private GameObject lobbyOptions;

	[SerializeField]
	private Text selectedName;

	[SerializeField]
	private Toggle muteToggle;

	[SerializeField]
	private GameObject separator;

	[SerializeField]
	private Slider volumeSlider;

	[SerializeField]
	private GameObject separator2;

	[SerializeField]
	private Button profileButton;

	[SerializeField]
	private Button blockButton;

	[SerializeField]
	private Button reportButton;

	[SerializeField]
	private Toggle disableVoiceChatToggle;

	[SerializeField]
	private Toggle muteMicrophoneToggle;

	[SerializeField]
	public ScrollRect friendsScrollRect;

	[SerializeField]
	private Selectable friendsListSelectable;

	[SerializeField]
	private Button inviteFriendButton;

	[SerializeField]
	private GameObject inviteText;

	[SerializeField]
	private GameObject noFriendsText;

	private List<Lobby_Selection> friendMembers = new List<Lobby_Selection>();

	[HideInInspector]
	public Lobby_Selection selectedFriendMember;

	[SerializeField]
	public ScrollRect blockedScrollRect;

	[SerializeField]
	private Selectable blockedListSelectable;

	[SerializeField]
	private GameObject unblockText;

	[SerializeField]
	private Button unblockButton;

	private List<Lobby_Selection> blockedMembers = new List<Lobby_Selection>();

	[HideInInspector]
	public Lobby_Selection selectedBlockedMember;

	private Raft_Network network;

	public override bool IsOpen => gameObject.activeInHierarchy;

	public new void OnDestroy()
	{
		Raft_Network value = ComponentManager<Raft_Network>.Value;
		value.OnPlayerJoinOrLeaves = (Action)Delegate.Remove(value.OnPlayerJoinOrLeaves, new Action(RefreshLobbyMembers));
	}

	protected override void OnControlChanged(bool gamepad)
	{
		base.OnControlChanged(gamepad);
		foreach (Lobby_Selection lobbyMember in lobbyMembers)
		{
			lobbyMember.EnableTriggers(gamepad);
		}
		foreach (Lobby_Selection friendMember in friendMembers)
		{
			friendMember.EnableTriggers(gamepad);
		}
		foreach (Lobby_Selection blockedMember in blockedMembers)
		{
			blockedMember.EnableTriggers(gamepad);
		}
	}

	private void OnTabOpen()
	{
		RefreshUI();
	}

	private void WhatShouldBeVisible()
	{
		socialTabGroup.tabButtons[0].SetInteractability(network.PlayerCount > 1);
		socialTabGroup.tabButtons[2].SetInteractability(network.BlockedUserIds.Count > 0);
		if (!socialTabGroup.tabButtons[2].IsInteractable)
		{
			socialTabGroup.tabButtons[2].OnPointerExit(forceExit: true);
		}
		if (network.PlayerCount > 1)
		{
			if (socialTabGroup.SelectedTabButton != socialTabGroup.tabButtons[0])
			{
				socialTabGroup.SelectedTabButton.OnPointerExit(forceExit: true);
				socialTabGroup.SelectTab(0);
			}
			else
			{
				RefreshUI();
			}
		}
		else if (socialTabGroup.SelectedTabButton != socialTabGroup.tabButtons[1])
		{
			socialTabGroup.SelectedTabButton.OnPointerExit(forceExit: true);
			socialTabGroup.SelectTab(1);
		}
		else
		{
			RefreshUI();
		}
	}

	public override void Open()
	{
		base.Open();
		if (!network)
		{
			network = ComponentManager<Raft_Network>.Value;
		}
		TabGroup tabGroup = socialTabGroup;
		tabGroup.OnNewTabOpen = (Action)Delegate.Combine(tabGroup.OnNewTabOpen, new Action(OnTabOpen));
		CanvasHelper value = ComponentManager<CanvasHelper>.Value;
		if (value != null)
		{
			value.GetComponent<GamepadCursor>().EnableCursor(_useCursor: false);
		}
		RefreshBlockList();
		RefreshLobbyMembers();
		WhatShouldBeVisible();
		if (friendMembers.Count > 0)
		{
			foreach (Lobby_Selection friendMember in friendMembers)
			{
				friendMember.inviteBlocked = false;
			}
		}
		Raft_Network value2 = ComponentManager<Raft_Network>.Value;
		value2.OnPlayerJoinOrLeaves = (Action)Delegate.Combine(value2.OnPlayerJoinOrLeaves, new Action(RefreshLobbyMembers));
	}

	public override void Close()
	{
		base.Close();
		CanvasHelper value = ComponentManager<CanvasHelper>.Value;
		if (value != null)
		{
			value.GetComponent<GamepadCursor>().EnableCursor(_useCursor: true);
		}
		if (OnSocialClosed != null)
		{
			OnSocialClosed();
		}
		TabGroup tabGroup = socialTabGroup;
		tabGroup.OnNewTabOpen = (Action)Delegate.Remove(tabGroup.OnNewTabOpen, new Action(OnTabOpen));
		Raft_Network value2 = ComponentManager<Raft_Network>.Value;
		value2.OnPlayerJoinOrLeaves = (Action)Delegate.Remove(value2.OnPlayerJoinOrLeaves, new Action(RefreshLobbyMembers));
		ComponentManager<Settings>.Value.ForceSaveAudio();
	}

	private void RefreshUI()
	{
		if (socialTabGroup.SelectedTabButton.tabIndex == 0)
		{
			lobbyListSelectable.Select();
			RefreshLobbyMembers();
		}
		else if (socialTabGroup.SelectedTabButton.tabIndex == 1)
		{
			friendsListSelectable.Select();
			RefreshFriends();
		}
		else if (socialTabGroup.SelectedTabButton.tabIndex == 2)
		{
			blockedListSelectable.Select();
			RefreshBlockList();
		}
	}

	public override void Update()
	{
		base.Update();
		if (CanvasHelper.ActiveMenu == MenuType.DeathMenu)
		{
			Close();
		}
		if (socialTabGroup.SelectedTabButton.tabIndex == 0)
		{
			selectedName.text = selectedLobbyMember.OnlineId;
			bool flag = !network.VoiceDisabled && selectedLobbyMember != null && !network.IsUserBlocked(selectedLobbyMember.Player.EntityKey.Id);
			bool flag2 = !network.VoiceDisabled && selectedLobbyMember != null && !selectedLobbyMember.Player.IsMuted;
			bool flag3 = selectedLobbyMember != null && !selectedLobbyMember.IsCrossPlatformPlayer;
			bool flag4 = selectedLobbyMember != null;
			bool flag5 = selectedLobbyMember != null;
			bool flag6 = flag4 && !blockedMembers.Exists((Lobby_Selection selection) => selection.PlatformId == selectedLobbyMember.Player.EntityKey.Id);
			bool flag7 = flag5 && !network.IsUserReported(selectedLobbyMember.Player.EntityKey.Id);
			if (flag)
			{
				muteToggle.SetIsOnWithoutNotify(selectedLobbyMember.Player.IsMuted);
			}
			disableVoiceChatToggle.SetIsOnWithoutNotify(ComponentManager<Settings>.Value.audioBox.VoicechatDisabled);
			muteMicrophoneToggle.SetIsOnWithoutNotify(ComponentManager<Settings>.Value.audioBox.IsMuted);
			muteToggle.transform.parent.gameObject.SetActive(flag);
			separator.SetActive(flag & flag2);
			volumeSlider.transform.parent.gameObject.SetActive(flag2);
			separator2.SetActive(flag | flag2);
			blockButton.gameObject.SetActive(flag6);
			reportButton.gameObject.SetActive(flag5);
			profileButton.interactable = flag3;
			profileText.SetActive(flag3);
			reportButton.gameObject.SetActive(flag7);
			inviteFriendButton.interactable = false;
			inviteText.SetActive(value: false);
			unblockButton.interactable = false;
			unblockText.SetActive(value: false);
			Navigation navigation = lobbyListSelectable.navigation;
			Selectable selectOnRight;
			if (flag)
			{
				selectOnRight = muteToggle;
			}
			else if (flag2)
			{
				selectOnRight = volumeSlider;
			}
			else if (flag6)
			{
				selectOnRight = blockButton;
			}
			else
			{
				selectOnRight = (reportButton.interactable ? reportButton : null);
			}
			navigation.selectOnRight = selectOnRight;
			lobbyListSelectable.navigation = navigation;
			Navigation navigation2 = volumeSlider.navigation;
			navigation2.selectOnUp = (flag ? muteToggle : lobbyListSelectable);
			Selectable selectOnDown;
			if (flag6)
			{
				selectOnDown = blockButton;
			}
			else
			{
				selectOnDown = (reportButton.interactable ? reportButton : lobbyListSelectable);
			}
			navigation2.selectOnDown = selectOnDown;
			volumeSlider.navigation = navigation2;
			Navigation navigation3 = blockButton.navigation;
			Selectable selectOnUp;
			if (flag2)
			{
				selectOnUp = volumeSlider;
			}
			else
			{
				selectOnUp = (flag ? muteToggle : lobbyListSelectable);
			}
			navigation3.selectOnUp = selectOnUp;
			navigation3.selectOnDown = (flag7 ? reportButton : lobbyListSelectable);
			blockButton.navigation = navigation3;
			Navigation navigation4 = muteToggle.navigation;
			Selectable selectOnDown2;
			if (flag2)
			{
				selectOnDown2 = volumeSlider;
			}
			else if (flag6)
			{
				selectOnDown2 = blockButton;
			}
			else
			{
				selectOnDown2 = (reportButton.interactable ? reportButton : lobbyListSelectable);
			}
			navigation4.selectOnDown = selectOnDown2;
			muteToggle.navigation = navigation4;
		}
		else if (socialTabGroup.SelectedTabButton.tabIndex == 1)
		{
			bool flag8 = false;
			if (selectedFriendMember != null && !selectedFriendMember.inviteBlocked)
			{
				flag8 = !lobbyMembers.Exists((Lobby_Selection selection) => selection.PlatformId == selectedFriendMember.PlatformId);
			}
			bool flag9 = selectedFriendMember != null;
			profileButton.interactable = flag9;
			profileText.SetActive(flag9);
			inviteFriendButton.interactable = flag8;
			inviteText.SetActive(flag8);
			unblockButton.interactable = false;
			unblockText.SetActive(value: false);
		}
		else if (socialTabGroup.SelectedTabButton.tabIndex == 2)
		{
			profileButton.interactable = false;
			profileText.SetActive(value: false);
			inviteFriendButton.interactable = false;
			inviteText.SetActive(value: false);
			bool flag10 = selectedBlockedMember != null;
			unblockButton.interactable = flag10;
			unblockText.SetActive(flag10 && SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad);
		}
	}

	public void Button_OpenProfile()
	{
		ComponentManager<SoundManager>.Value.PlayUI_Click();
		Lobby_Selection lobby_Selection = null;
		SteamFriends.ActivateGameOverlayToUser("steamid", new CSteamID(ulong.Parse((socialTabGroup.SelectedTabButton.tabIndex switch
		{
			0 => selectedLobbyMember, 
			1 => selectedFriendMember, 
			2 => selectedBlockedMember, 
			_ => throw new ArgumentOutOfRangeException(), 
		}).PlatformId)));
		if (socialTabGroup.SelectedTabButton.tabIndex == 0)
		{
			OnLobbyListSelected(lobbyScrollRect);
		}
		else if (socialTabGroup.SelectedTabButton.tabIndex == 1)
		{
			OnFriendsListSelected(friendsScrollRect);
		}
	}

	public void UnblockPlayer()
	{
		Debug.Log($"Unblock {selectedBlockedMember}");
		network.UnblockUser(selectedBlockedMember.PlatformId);
		RefreshBlockList();
	}

	public void BlockPlayer()
	{
		Debug.Log($"Block {selectedLobbyMember}");
		selectedLobbyMember.Player.IsMuted = true;
		network.AddToBlockList(selectedLobbyMember.OnlineId, selectedLobbyMember.Player.EntityKey.Id);
		RefreshBlockList();
		lobbyListSelectable.Select();
	}

	public void ReportPlayer()
	{
		Debug.Log($"Report {selectedLobbyMember}");
		network.ReportUser(selectedLobbyMember.Player.EntityKey.Id);
		OnLobbyListSelected(lobbyScrollRect);
	}

	public void InviteFriend()
	{
		ComponentManager<SoundManager>.Value.PlayUI_Click();
		Debug.Log($"Invite {selectedFriendMember}");
		string text = network.CurrentSteamHost.ToString();
		string text2 = (GameManager.HasPassword ? Raft_Network.PASSWORD_DELIMITER : string.Empty);
		string pchConnectString = Raft_Network.CONNECT_DELIMITER + " " + text + text2;
		SteamFriends.InviteUserToGame(new CSteamID(ulong.Parse(selectedFriendMember.PlatformId)), pchConnectString);
		selectedFriendMember.PreventInviteSpam();
	}

	private void RefreshLobbyMembers()
	{
		for (int i = 0; i < lobbyMembers.Count; i++)
		{
			UnityEngine.Object.Destroy(lobbyMembers[i].gameObject);
		}
		lobbyMembers.Clear();
		lobbyScrollRect.content.DetachChildren();
		if (PlayFabMultiplayerManager.Get().State == PlayFabMultiplayerManagerState.ConnectedToNetwork)
		{
			foreach (PlayFabPlayer remotePlayer in PlayFabMultiplayerManager.Get().RemotePlayers)
			{
				Network_Player playerFromID = network.GetPlayerFromID(remotePlayer.EntityKey.Id);
				if (playerFromID != null)
				{
					network.GetPlayerInfoFromNetworkId(remotePlayer.EntityKey.Id, out var playerInfo);
					Lobby_Selection lobby_Selection = UnityEngine.Object.Instantiate(socialSelectionPrefab, lobbyScrollRect.content);
					lobby_Selection.SetLobbyMember(this, playerFromID, remotePlayer, playerInfo.platformId);
					lobby_Selection.EnableTriggers(SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad);
					lobbyMembers.Add(lobby_Selection);
				}
			}
		}
		socialTabGroup.tabButtons[0].SetInteractability(lobbyMembers.Count > 0);
		if (socialTabGroup.SelectedTabButton.tabIndex == 0)
		{
			if (lobbyMembers.Count == 0)
			{
				socialTabGroup.SelectedTabButton.OnPointerExit(forceExit: true);
				socialTabGroup.SelectTab(1);
			}
		}
		else
		{
			socialTabGroup.tabButtons[0].OnPointerExit(forceExit: true);
		}
		for (int j = 0; j < lobbyScrollRect.content.childCount - 1; j++)
		{
			Button component = lobbyScrollRect.content.GetChild(j).GetComponent<Button>();
			Button component2 = lobbyScrollRect.content.GetChild(j + 1).GetComponent<Button>();
			Navigation navigation = component.navigation;
			navigation.mode = Navigation.Mode.Explicit;
			navigation.selectOnDown = component2;
			component.navigation = navigation;
			Navigation navigation2 = component2.navigation;
			navigation2.mode = Navigation.Mode.Explicit;
			navigation2.selectOnUp = component;
			component2.navigation = navigation2;
		}
		if (lobbyScrollRect.content.childCount > 0)
		{
			lobbyScrollRect.content.GetChild(0).GetComponent<Lobby_Selection>().OnSelected();
		}
	}

	private void RefreshFriendsInternal()
	{
		for (int i = 0; i < friendMembers.Count; i++)
		{
			UnityEngine.Object.Destroy(friendMembers[i].gameObject);
		}
		friendMembers.Clear();
		friendsScrollRect.content.DetachChildren();
		int friendCount = SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagAll);
		for (int j = 0; j < friendCount; j++)
		{
			CSteamID friendByIndex = SteamFriends.GetFriendByIndex(j, EFriendFlags.k_EFriendFlagAll);
			if (SteamFriends.GetFriendPersonaState(friendByIndex) == EPersonaState.k_EPersonaStateOnline)
			{
				string friendPersonaName = SteamFriends.GetFriendPersonaName(friendByIndex);
				Lobby_Selection lobby_Selection = UnityEngine.Object.Instantiate(socialSelectionPrefab, friendsScrollRect.content);
				lobby_Selection.SetFriend(this, friendPersonaName, string.Empty, friendByIndex.ToString());
				lobby_Selection.DownloadAvatarWhenActive(friendByIndex.ToString());
				lobby_Selection.EnableTriggers(SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad);
				friendMembers.Add(lobby_Selection);
			}
		}
		if (friendMembers.Count == 0)
		{
			friendsListSelectable.Select();
		}
		noFriendsText.SetActive(friendMembers.Count == 0);
		for (int k = 0; k < friendsScrollRect.content.childCount - 1; k++)
		{
			Button component = friendsScrollRect.content.GetChild(k).GetComponent<Button>();
			Button component2 = friendsScrollRect.content.GetChild(k + 1).GetComponent<Button>();
			Navigation navigation = component.navigation;
			navigation.mode = Navigation.Mode.Explicit;
			navigation.selectOnDown = component2;
			component.navigation = navigation;
			Navigation navigation2 = component2.navigation;
			navigation2.mode = Navigation.Mode.Explicit;
			navigation2.selectOnUp = component;
			component2.navigation = navigation2;
		}
		if (friendsScrollRect.content.childCount > 0)
		{
			friendsScrollRect.content.GetChild(0).GetComponent<Lobby_Selection>().OnSelected();
		}
		OnFriendsListSelected(friendsScrollRect);
	}

	private void RefreshFriends()
	{
		RefreshFriendsInternal();
	}

	private void RefreshBlockList()
	{
		for (int i = 0; i < blockedMembers.Count; i++)
		{
			UnityEngine.Object.Destroy(blockedMembers[i].gameObject);
		}
		blockedMembers.Clear();
		blockedScrollRect.content.DetachChildren();
		foreach (KeyValuePair<Network_UserId, string> blockedUserId in network.BlockedUserIds)
		{
			Lobby_Selection lobby_Selection = UnityEngine.Object.Instantiate(socialSelectionPrefab, blockedScrollRect.content);
			lobby_Selection.SetBlocked(this, blockedUserId.Value, blockedUserId.Key);
			lobby_Selection.EnableTriggers(SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad);
			blockedMembers.Add(lobby_Selection);
		}
		socialTabGroup.tabButtons[2].SetInteractability(blockedMembers.Count > 0);
		if (socialTabGroup.SelectedTabButton.tabIndex == 2)
		{
			if (blockedMembers.Count == 0)
			{
				socialTabGroup.SelectedTabButton.OnPointerExit(forceExit: true);
				socialTabGroup.SelectTab(1);
			}
		}
		else
		{
			socialTabGroup.tabButtons[2].OnPointerExit(forceExit: true);
		}
		for (int j = 0; j < blockedScrollRect.content.childCount - 1; j++)
		{
			Button component = blockedScrollRect.content.GetChild(j).GetComponent<Button>();
			Button component2 = blockedScrollRect.content.GetChild(j + 1).GetComponent<Button>();
			Navigation navigation = component.navigation;
			navigation.mode = Navigation.Mode.Explicit;
			navigation.selectOnDown = component2;
			component.navigation = navigation;
			Navigation navigation2 = component2.navigation;
			navigation2.mode = Navigation.Mode.Explicit;
			navigation2.selectOnUp = component;
			component2.navigation = navigation2;
		}
		if (blockedScrollRect.content.childCount > 0)
		{
			blockedScrollRect.content.GetChild(0).GetComponent<Lobby_Selection>().OnSelected();
		}
		OnBlockedListSelected(blockedScrollRect);
	}

	public void Button_SelectMember(Lobby_Selection selection, Lobby_Selection.Context context)
	{
		switch (context)
		{
		case Lobby_Selection.Context.Lobby:
			selectedLobbyMember = selection;
			if (selectedLobbyMember != null)
			{
				muteToggle.SetIsOnWithoutNotify(selectedLobbyMember.Player.IsMuted);
				volumeSlider.SetValueWithoutNotify(selectedLobbyMember.Player.VoiceLevel);
			}
			break;
		case Lobby_Selection.Context.Friends:
			selectedFriendMember = selection;
			break;
		case Lobby_Selection.Context.Blocked:
			selectedBlockedMember = selection;
			break;
		default:
			throw new ArgumentOutOfRangeException("context", context, null);
		}
	}

	public void Checkbox_MuteMicrophone(bool value)
	{
		ComponentManager<Settings>.Value.audioBox.IsMuted = value;
	}

	public void Checkbox_DisableVoiceChat(bool value)
	{
		ComponentManager<Settings>.Value.audioBox.VoicechatDisabled = value;
		if (value)
		{
			foreach (Lobby_Selection lobbyMember in lobbyMembers)
			{
				lobbyMember.Player._wasMuted = lobbyMember.Player.IsMuted;
				lobbyMember.Player.IsMuted = true;
			}
			return;
		}
		foreach (Lobby_Selection lobbyMember2 in lobbyMembers)
		{
			lobbyMember2.Player.IsMuted = lobbyMember2.Player._wasMuted;
		}
	}

	public void Checkbox_Mute()
	{
		if (selectedLobbyMember != null)
		{
			selectedLobbyMember.Player.IsMuted = !selectedLobbyMember.Player.IsMuted;
		}
	}

	public void Slider_VolumeChanged(float value)
	{
		if (selectedLobbyMember != null)
		{
			selectedLobbyMember.Player.VoiceLevel = value;
		}
	}

	public void OnLobbyListSelected(ScrollRect scrollRect)
	{
		for (int i = 0; i < scrollRect.content.childCount; i++)
		{
			Lobby_Selection component = scrollRect.content.GetChild(i).GetComponent<Lobby_Selection>();
			if (selectedLobbyMember == component)
			{
				scrollRect.content.GetChild(i).GetComponent<Button>().Select();
			}
		}
	}

	public void OnFriendsListSelected(ScrollRect scrollRect)
	{
		for (int i = 0; i < scrollRect.content.childCount; i++)
		{
			Lobby_Selection component = scrollRect.content.GetChild(i).GetComponent<Lobby_Selection>();
			if (selectedFriendMember == component)
			{
				scrollRect.content.GetChild(i).GetComponent<Button>().Select();
			}
		}
	}

	public void OnBlockedListSelected(ScrollRect scrollRect)
	{
		for (int i = 0; i < scrollRect.content.childCount; i++)
		{
			Lobby_Selection component = scrollRect.content.GetChild(i).GetComponent<Lobby_Selection>();
			if (selectedBlockedMember == component)
			{
				scrollRect.content.GetChild(i).GetComponent<Button>().Select();
			}
		}
	}
}
