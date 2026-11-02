using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

public class JoinGameBox : MenuBox
{
	[SerializeField]
	private ScrollRect scrollRect;

	[SerializeField]
	private JoinGame_Selection gameSelectionPrefab;

	[SerializeField]
	private Button joinGameButton;

	[SerializeField]
	private GameObject noGamesFoundsText;

	[SerializeField]
	private GameObject cheat_joinGameSteamIDBox;

	[SerializeField]
	private InputField cheat_joinGameSteamID;

	[SerializeField]
	private InputField cheat_joinGamePassword;

	public ConnectingBox connectingBox;

	public JoinPasswordBox joinPasswordBox;

	private Raft_Network network;

	private JoinGame_Selection selectedGame;

	private List<JoinGame_Selection> joinGameSelections = new List<JoinGame_Selection>();

	private void Start()
	{
		network = ComponentManager<Raft_Network>.Value;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	public override void Update()
	{
		base.Update();
		joinGameButton.interactable = selectedGame != null && selectedGame.steamID.IsValid();
		Input.GetKeyDown(KeyCode.F8);
	}

	public override void Open()
	{
		base.Open();
		RefreshGames();
	}

	public void JoinMe()
	{
		if (selectedGame.steamID.IsValid())
		{
			if (selectedGame.passwordProtected)
			{
				joinPasswordBox.Open();
			}
			else
			{
				JoinSelectedGame();
			}
		}
		connectingBox.gameObject.SetActive(value: true);
		connectingBox.StartConnectTimeoutTimer(CSteamID.Nil);
	}

	public void JoinSelectedGame()
	{
		if (selectedGame != null)
		{
			connectingBox.gameObject.SetActive(value: true);
			connectingBox.StartConnectTimeoutTimer(CSteamID.Nil);
		}
	}

	public void JoinGameSteamIDBOX()
	{
		connectingBox.gameObject.SetActive(value: true);
		connectingBox.StartConnectTimeoutTimer(CSteamID.Nil);
	}

	private void RefreshGames()
	{
	}

	private void OnFriendInfo(CSteamID steamID)
	{
		_ = gameObject.activeSelf;
	}

	private void HandleFriendInfo(CSteamID steamID, string status)
	{
	}

	private IEnumerator SetIDToSelection(CSteamID steamID, bool passwordProtected, JoinGame_Selection selection)
	{
		while (selection != null && !selection.gameObject.activeInHierarchy)
		{
			yield return null;
		}
		if (selection != null)
		{
			selection.Set(steamID, passwordProtected);
		}
	}

	public void Button_SelectGame(JoinGame_Selection gameSelection)
	{
		selectedGame = gameSelection;
	}

	public void Button_Close()
	{
		Close();
	}

	public void Button_Join()
	{
		if (selectedGame.steamID.IsValid())
		{
			if (selectedGame.passwordProtected)
			{
				joinPasswordBox.Open();
			}
			else
			{
				JoinSelectedGame();
			}
		}
	}

	public void Button_Refresh()
	{
		RefreshGames();
	}
}
