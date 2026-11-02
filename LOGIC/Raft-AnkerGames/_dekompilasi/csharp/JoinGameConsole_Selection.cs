using System.Collections;
using Steamworks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoinGameConsole_Selection : MonoBehaviour
{
	[SerializeField]
	private Image avatarImage;

	[SerializeField]
	private Text onlineId;

	[SerializeField]
	private Text gameMode;

	[SerializeField]
	private Text playerCount;

	[SerializeField]
	private Text crossPlay;

	public GameObject image_selection;

	public JoinableSession session;

	public SessionDetails details;

	private bool detailsUpdated;

	private string avatarUrl;

	private bool avatarUrlUpdated;

	private static JoinGameConsole_Selection selectedSelection;

	private static JoinGameBoxConsole joinGameBox;

	public bool passwordProtected;

	private void Awake()
	{
		if (joinGameBox == null)
		{
			joinGameBox = GetComponentInParent<JoinGameBoxConsole>();
		}
	}

	private IEnumerator GetAvatarRoutine()
	{
		avatarImage.sprite = Helper.GetMediumSteamAvatarFromID(new CSteamID(ulong.Parse(avatarUrl)));
		avatarImage.enabled = true;
		return null;
	}

	public void Update()
	{
		if (detailsUpdated)
		{
			gameMode.text = details.gameInfo.gameMode.ToString();
			crossPlay.text = (details.gameInfo.isCrossplay ? "Yes" : "No");
			detailsUpdated = false;
		}
		if (avatarUrlUpdated)
		{
			StartCoroutine(GetAvatarRoutine());
			avatarUrlUpdated = false;
		}
	}

	public void SetInfo(JoinableSession _session, bool _passwordProtected)
	{
		session = _session;
		onlineId.text = _session.onlineId;
		passwordProtected = _passwordProtected;
		avatarImage.sprite = Helper.GetMediumSteamAvatarFromID(_session.steamID);
		avatarImage.enabled = true;
	}

	public void SetSessionDetails(SessionDetails _details)
	{
		details = _details;
		detailsUpdated = true;
	}

	public void SetAvatarUrl(string _avatarUrl)
	{
		avatarUrl = _avatarUrl;
		avatarUrlUpdated = true;
	}

	public void OnSelected()
	{
		joinGameBox.Button_SelectLoad(this);
		if (selectedSelection != null)
		{
			selectedSelection.image_selection.SetActive(value: false);
		}
		selectedSelection = this;
		image_selection.SetActive(value: true);
	}

	public void UpdateScrollView()
	{
		joinGameBox.Button_Select(this);
	}

	public void OnCancel(BaseEventData eventData)
	{
		joinGameBox.Button_Cancel();
	}
}
