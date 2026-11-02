using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

public class JoinGameBoxConsole : MenuBox
{
	[SerializeField]
	private Button profileButton;

	[SerializeField]
	private GameObject profileText;

	[SerializeField]
	private ScrollRect scrollRect;

	[SerializeField]
	private Button joinGameButton;

	[SerializeField]
	private GameObject noGamesFoundsText;

	[SerializeField]
	private GameObject onlineFeaturesNotAvailable;

	[SerializeField]
	private Selectable gameList;

	[SerializeField]
	private Button crossPlatformButton;

	[SerializeField]
	private Button refreshButton;

	[SerializeField]
	private JoinGameConsole_Selection gameSelectionPrefab;

	[SerializeField]
	private GameObject joinGameText;

	[SerializeField]
	private GameObject worldButtonText;

	[SerializeField]
	private GameObject refreshText;

	[SerializeField]
	private GameObject joinCrossPlatform;

	[SerializeField]
	private InputField worldCodeField;

	[SerializeField]
	private InputField worldCodePasswordField;

	[SerializeField]
	private Text worldCodeErrorText;

	[SerializeField]
	private ConnectingBox connectingBox;

	[SerializeField]
	private JoinPasswordBox joinPasswordBox;

	private JoinGameConsole_Selection selectedGame;

	private List<JoinGameConsole_Selection> joinGameSelections = new List<JoinGameConsole_Selection>();

	private TouchScreenKeyboard keyboard;

	private bool multiplayerAllowed;

	private bool crossplayAllowed;

	private bool refreshBlocked;

	private bool joinBlocked;

	public override void Update()
	{
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
		{
			gamepadObjects[0].SetActive(!joinCrossPlatform.activeSelf);
		}
		if (multiplayerAllowed)
		{
			multiplayerAllowed = Raft_Network.SignedIntoPlayfab;
			crossPlatformButton.interactable = multiplayerAllowed && !joinBlocked;
			worldButtonText.SetActive(crossPlatformButton.interactable);
			refreshButton.interactable = !refreshBlocked;
			refreshText.SetActive(refreshButton.interactable);
			joinGameButton.interactable = selectedGame != null && !joinBlocked;
			joinGameText.SetActive(joinGameButton.interactable);
			Navigation navigation = gameList.navigation;
			Button selectOnDown;
			if (joinGameButton.interactable)
			{
				selectOnDown = joinGameButton;
			}
			else
			{
				selectOnDown = (crossPlatformButton.gameObject.activeSelf ? crossPlatformButton : refreshButton);
			}
			navigation.selectOnDown = selectOnDown;
			gameList.navigation = navigation;
		}
	}

	public void CloseJoinWorldDialog()
	{
		joinCrossPlatform.SetActive(value: false);
		if (scrollRect.content.childCount > 0)
		{
			OnGameListSelected(scrollRect);
		}
		else
		{
			gameList.Select();
		}
	}

	public void ClosePasswordBox()
	{
		OnGameListSelected(scrollRect);
	}

	public void JoinWorldThroughDialog()
	{
		string text = worldCodeField.text;
		string text2 = worldCodePasswordField.text;
		if (text.Length >= 4)
		{
			GameManager.Password = text2;
			SimpleMonoBehaviourSingleton<PlayFabManager>.Instance.JoinWorld(text.TrimEnd().ToUpper());
		}
		CloseJoinWorldDialog();
		PreventJoinSpam();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		Raft_Network value = ComponentManager<Raft_Network>.Value;
		value.OnCrossplayJoinComplete = (Action<CrossplayJoinErrorCode>)Delegate.Remove(value.OnCrossplayJoinComplete, new Action<CrossplayJoinErrorCode>(CrossplayJoinComplete));
	}

	public override void Open()
	{
		base.Open();
		Raft_Network value = ComponentManager<Raft_Network>.Value;
		value.OnCrossplayJoinComplete = (Action<CrossplayJoinErrorCode>)Delegate.Combine(value.OnCrossplayJoinComplete, new Action<CrossplayJoinErrorCode>(CrossplayJoinComplete));
		multiplayerAllowed = Raft_Network.SignedIntoPlayfab;
		crossplayAllowed = multiplayerAllowed;
		joinGameButton.interactable = multiplayerAllowed;
		crossPlatformButton.interactable = crossplayAllowed;
		refreshButton.interactable = multiplayerAllowed;
		joinGameText.SetActive(joinGameButton.interactable);
		worldButtonText.SetActive(crossPlatformButton.interactable);
		refreshText.SetActive(refreshButton.interactable);
		noGamesFoundsText.SetActive(value: false);
		if (!multiplayerAllowed)
		{
			onlineFeaturesNotAvailable.SetActive(value: true);
			return;
		}
		onlineFeaturesNotAvailable.SetActive(value: false);
		RefreshGames();
	}

	private void CrossplayJoinComplete(CrossplayJoinErrorCode ec)
	{
		worldCodeErrorText.gameObject.SetActive(value: true);
		worldCodeErrorText.text = ((ec == CrossplayJoinErrorCode.WorldCodeIncorrect) ? Helper.GetTerm("Menu/ConnectFail") : Helper.GetTerm("Menu/ConnectFail_Password"));
		joinCrossPlatform.SetActive(value: true);
		worldCodeField.Select();
	}

	public override void Close()
	{
		base.Close();
		Raft_Network value = ComponentManager<Raft_Network>.Value;
		value.OnCrossplayJoinComplete = (Action<CrossplayJoinErrorCode>)Delegate.Remove(value.OnCrossplayJoinComplete, new Action<CrossplayJoinErrorCode>(CrossplayJoinComplete));
	}

	private IEnumerator SetIDToSelection(JoinableSession session, JoinGameConsole_Selection selection, bool passwordProtected)
	{
		while (selection != null && !selection.gameObject.activeInHierarchy)
		{
			yield return null;
		}
		string gameInfoString = SteamFriends.GetFriendRichPresence(session.steamID, Raft_Network.PresenceKeyGameInfo);
		if (gameInfoString == string.Empty)
		{
			yield return null;
		}
		if (selection != null)
		{
			selection.SetInfo(session, passwordProtected);
			GameInfo gameInfo = Raft_Network.DecodeGameInfo(gameInfoString);
			selection.SetSessionDetails(new SessionDetails
			{
				gameInfo = gameInfo
			});
		}
	}

	private void ClearGameList()
	{
		for (int i = 0; i < joinGameSelections.Count; i++)
		{
			UnityEngine.Object.Destroy(joinGameSelections[i].gameObject);
		}
		joinGameSelections.Clear();
		scrollRect.content.DetachChildren();
		selectedGame = null;
	}

	private IEnumerator WaitForRefreshToBeReady()
	{
		yield return new WaitForSeconds(10f);
		refreshBlocked = false;
	}

	private IEnumerator WaitFoJoinToBeReady()
	{
		yield return new WaitForSeconds(5f);
		joinBlocked = false;
	}

	public void PreventRefreshSpam()
	{
		refreshBlocked = true;
		StartCoroutine(WaitForRefreshToBeReady());
	}

	public void PreventJoinSpam()
	{
		joinBlocked = true;
		StartCoroutine(WaitFoJoinToBeReady());
	}

	private void GetFriendsInMultiplayer()
	{
		int friendCount = SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagAll);
		for (int i = 0; i < friendCount; i++)
		{
			CSteamID friendByIndex = SteamFriends.GetFriendByIndex(i, EFriendFlags.k_EFriendFlagAll);
			string friendRichPresence = SteamFriends.GetFriendRichPresence(friendByIndex, Raft_Network.PresenceKeyStatus);
			if (friendRichPresence.Contains(Raft_Network.STATUS_Hosting, StringComparison.OrdinalIgnoreCase))
			{
				JoinableSession session = new JoinableSession
				{
					onlineId = SteamFriends.GetFriendPersonaName(friendByIndex),
					steamID = friendByIndex
				};
				JoinGameConsole_Selection joinGameConsole_Selection = UnityEngine.Object.Instantiate(gameSelectionPrefab, scrollRect.content);
				StartCoroutine(SetIDToSelection(session, joinGameConsole_Selection, friendRichPresence == Raft_Network.STATUS_Hosting_Password));
				joinGameSelections.Add(joinGameConsole_Selection);
			}
		}
		if (scrollRect.content.childCount > 0)
		{
			scrollRect.content.GetChild(0).GetComponent<JoinGameConsole_Selection>().OnSelected();
		}
		if (noGamesFoundsText.activeSelf && joinGameSelections.Count > 0)
		{
			noGamesFoundsText.SetActive(value: false);
		}
		else if (!noGamesFoundsText.activeSelf && joinGameSelections.Count == 0)
		{
			noGamesFoundsText.SetActive(value: true);
		}
		OnGameListSelected(scrollRect);
	}

	private void RefreshGames()
	{
		joinBlocked = false;
		ClearGameList();
		GetFriendsInMultiplayer();
		PreventRefreshSpam();
	}

	public void Button_Refresh()
	{
		RefreshGames();
	}

	public void Button_OpenProfile()
	{
	}

	public void Button_SelectLoad(JoinGameConsole_Selection selection)
	{
		selectedGame = selection;
	}

	public void OnGameListSelected(ScrollRect scrollRect)
	{
		for (int i = 0; i < scrollRect.content.childCount; i++)
		{
			JoinGameConsole_Selection component = scrollRect.content.GetChild(i).GetComponent<JoinGameConsole_Selection>();
			if (selectedGame == component)
			{
				scrollRect.content.GetChild(i).GetComponent<Button>().Select();
			}
		}
	}

	public void Button_Select(JoinGameConsole_Selection selection)
	{
		int siblingIndex = selection.transform.GetSiblingIndex();
		float num = 1f - (float)siblingIndex / (float)scrollRect.content.transform.childCount;
		if ((double)num < 0.4)
		{
			float num2 = 1f / (float)scrollRect.content.transform.childCount;
			num -= num2;
		}
		scrollRect.verticalNormalizedPosition = num;
	}

	public void Button_Cancel()
	{
		Close();
	}

	public void Button_Join()
	{
		if (selectedGame != null)
		{
			if (selectedGame.passwordProtected)
			{
				joinPasswordBox.Open();
			}
			else
			{
				Join();
			}
		}
	}

	public void Join()
	{
		connectingBox.gameObject.SetActive(value: true);
		connectingBox.StartConnectTimeoutTimer(selectedGame.session.steamID);
		ComponentManager<Raft_Network>.Value.CleanupState();
		ComponentManager<Raft_Network>.Value.TryToJoinGame(selectedGame.session.steamID, (GameManager.Password == null) ? string.Empty : GameManager.Password);
		PreventJoinSpam();
	}

	public void Button_WorldCode()
	{
		worldCodeErrorText.text = string.Empty;
		worldCodeErrorText.gameObject.SetActive(value: false);
		joinCrossPlatform.SetActive(value: true);
		worldCodeField.Select();
	}
}
