using Steamworks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoinPasswordBox : MenuBox
{
	[HideInInspector]
	public CSteamID openedViaFriendJoinArgument;

	[SerializeField]
	private ConnectingBox connectingBox;

	[SerializeField]
	private JoinGameBoxConsole joinGameBox;

	[SerializeField]
	private InputField inputfield_Password;

	public override void Open()
	{
		base.Open();
		inputfield_Password.Select();
	}

	protected override void OnControlChanged(bool gamepad)
	{
		base.OnControlChanged(gamepad);
		if (gamepad && EventSystem.current.currentSelectedGameObject != inputfield_Password)
		{
			inputfield_Password.Select();
		}
	}

	public void JoinButton()
	{
		GameManager.Password = inputfield_Password.text;
		if (!openedViaFriendJoinArgument.IsValid())
		{
			joinGameBox.Join();
		}
		else
		{
			Object.FindObjectOfType<Raft_Network>().TryToJoinGame(openedViaFriendJoinArgument, GameManager.Password);
			connectingBox.gameObject.SetActive(value: true);
			connectingBox.StartConnectTimeoutTimer(openedViaFriendJoinArgument);
		}
		Close();
	}

	public override void Close()
	{
		base.Close();
		if (!openedViaFriendJoinArgument.IsValid())
		{
			joinGameBox.ClosePasswordBox();
		}
		else
		{
			Object.FindObjectOfType<StartMenuScreen>().OnPasswordBoxClosed();
		}
		openedViaFriendJoinArgument.Clear();
	}
}
