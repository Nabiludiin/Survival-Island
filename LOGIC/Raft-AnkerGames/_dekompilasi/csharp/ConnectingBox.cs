using System.Collections;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

public class ConnectingBox : MonoBehaviour
{
	public GameObject button_ok;

	public GameObject button_cancel;

	public Text connectingText;

	[SerializeField]
	private JoinGameBoxConsole joinGameBox;

	private Raft_Network network;

	private CSteamID connectingTo;

	private void OnEnable()
	{
		connectingText.text = Helper.GetTerm("Menu/Connecting") + "...";
		button_cancel.SetActive(value: true);
		button_cancel.GetComponent<Selectable>().Select();
		button_ok.SetActive(value: false);
	}

	private void Awake()
	{
		Raft_Network.OnJoinResult += OnJoinResult;
	}

	private void Start()
	{
		network = ComponentManager<Raft_Network>.Value;
	}

	private void OnDestroy()
	{
		Raft_Network.OnJoinResult -= OnJoinResult;
	}

	public void OnJoinResult(CSteamID remoteID, InitiateResult result)
	{
		if (this == null)
		{
			Debug.Log("CONNECTIN BOX IS NULL, ABORTING JOIN RESULT");
			return;
		}
		StopAllCoroutines();
		button_cancel.SetActive(value: false);
		button_ok.SetActive(value: true);
		button_ok.GetComponent<Selectable>().Select();
		switch (result)
		{
		case InitiateResult.Fail_AllowNoFriends:
			connectingText.text = Helper.GetTerm("Menu/NoFriends");
			break;
		case InitiateResult.Fail:
			connectingText.text = Helper.GetTerm("Menu/ConnectFail");
			break;
		case InitiateResult.Fail_MissmatchAppBuildID:
			connectingText.text = Helper.GetTerm("Menu/ConnectFail_MissmatchID");
			break;
		case InitiateResult.Fail_WrongPassword:
			connectingText.text = Helper.GetTerm("Menu/ConnectFail_Password");
			break;
		case InitiateResult.Fail_NotFriendWithHost:
			connectingText.text = Helper.GetTerm("Menu/ConnectFail_NotFriendWithHost");
			break;
		}
		if (joinGameBox != null)
		{
			joinGameBox.Button_Refresh();
		}
	}

	public void StartConnectTimeoutTimer(CSteamID steamId)
	{
		connectingTo = steamId;
		StartCoroutine(StartConnectTimer());
	}

	private IEnumerator StartConnectTimer()
	{
		if (network == null)
		{
			network = ComponentManager<Raft_Network>.Value;
		}
		float timer = network.connectTimeoutTime;
		while (timer > 0f)
		{
			timer -= Time.deltaTime;
			connectingText.text = Helper.GetTerm("Menu/Connecting") + "... " + Mathf.RoundToInt(timer);
			yield return null;
		}
		network.CancelTryingToConnect(connectingTo);
		connectingText.text = Helper.GetTerm("Menu/ConnectTimeout");
		button_cancel.SetActive(value: false);
		button_ok.SetActive(value: true);
		button_ok.GetComponent<Selectable>().Select();
	}
}
