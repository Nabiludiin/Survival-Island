using System;
using System.Collections;
using PlayFab.Party;
using Steamworks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Lobby_Selection : MonoBehaviour
{
	public enum Context
	{
		Lobby,
		Friends,
		Blocked
	}

	[SerializeField]
	private Image avatarImage;

	[SerializeField]
	private Image platformImage;

	[SerializeField]
	private Image muteImage;

	[SerializeField]
	private Text onlineId;

	[SerializeField]
	private Text realName;

	[SerializeField]
	private Text pressence;

	[SerializeField]
	private GameObject image_selection;

	[SerializeField]
	private Sprite ps5Sprite;

	[SerializeField]
	private Sprite xboxSprite;

	[SerializeField]
	private Sprite pcSprite;

	[SerializeField]
	private Sprite otherGamepadSprite;

	private Lobby lobby;

	private Context context;

	private PlayFabPlayer player;

	private Network_Player networkPlayer;

	private string platformId;

	private bool isCrossPlatformPlayer;

	[HideInInspector]
	public bool inviteBlocked;

	public PlayFabPlayer Player => player;

	public bool IsCrossPlatformPlayer => isCrossPlatformPlayer;

	public string OnlineId => onlineId.text;

	public string PlatformId => platformId;

	private void OnDestroy()
	{
		lobby = null;
	}

	public void Update()
	{
		if (player != null)
		{
			muteImage.enabled = player.IsMuted;
		}
	}

	private void UpdateName(string name)
	{
		onlineId.text = name;
	}

	public void SetBlocked(Lobby _lobby, string _onlineId, string titleId)
	{
		lobby = _lobby;
		context = Context.Blocked;
		onlineId.text = _onlineId;
		platformId = titleId;
		platformImage.enabled = false;
	}

	public void SetFriend(Lobby _lobby, string userName, string _realName, string _platformId)
	{
		lobby = _lobby;
		context = Context.Friends;
		platformId = _platformId;
		onlineId.text = userName;
		if (_realName.Length > 4)
		{
			realName.gameObject.SetActive(value: true);
			realName.text = _realName;
		}
		else
		{
			realName.gameObject.SetActive(value: false);
		}
		platformImage.sprite = pcSprite;
		platformImage.enabled = true;
		Color color = avatarImage.color;
		color.a = 0f;
		avatarImage.color = color;
	}

	public IEnumerator GetAvatarRoutine(string avatarUrl)
	{
		avatarImage.sprite = Helper.GetMediumSteamAvatarFromID(new CSteamID(ulong.Parse(avatarUrl)));
		return null;
	}

	private IEnumerator WaitForSelectionToBeActive(string avatarUrl)
	{
		while (!gameObject.activeInHierarchy)
		{
			yield return null;
		}
		avatarImage.sprite = Helper.GetMediumSteamAvatarFromID(new CSteamID(ulong.Parse(avatarUrl)));
		Color color = avatarImage.color;
		color.a = 1f;
		avatarImage.color = color;
	}

	public void DownloadAvatarWhenActive(string avatarUrl)
	{
		StartCoroutine(WaitForSelectionToBeActive(avatarUrl));
	}

	private IEnumerator WaitBeforeInviteIsAvailable()
	{
		yield return new WaitForSeconds(30f);
		inviteBlocked = false;
	}

	public void PreventInviteSpam()
	{
		inviteBlocked = true;
		StartCoroutine(WaitBeforeInviteIsAvailable());
	}

	public void SetLobbyMember(Lobby _lobby, Network_Player _networkPlayer, PlayFabPlayer _player, string _platformId)
	{
		lobby = _lobby;
		context = Context.Lobby;
		player = _player;
		networkPlayer = _networkPlayer;
		platformId = _platformId;
		onlineId.text = networkPlayer.visualName;
		if (networkPlayer.realName.Length > 4)
		{
			realName.gameObject.SetActive(value: true);
			realName.text = networkPlayer.realName;
		}
		else
		{
			realName.gameObject.SetActive(value: false);
		}
		if (networkPlayer.characterSettings.Platform == 0)
		{
			Sprite smallSteamProfile = networkPlayer.GetSmallSteamProfile();
			if (smallSteamProfile != null)
			{
				avatarImage.sprite = smallSteamProfile;
			}
			platformImage.sprite = pcSprite;
			platformImage.enabled = true;
		}
		else
		{
			isCrossPlatformPlayer = true;
			platformImage.sprite = otherGamepadSprite;
			platformImage.enabled = true;
		}
	}

	public void OnSelected()
	{
		switch (context)
		{
		case Context.Lobby:
			if ((bool)lobby.selectedLobbyMember)
			{
				lobby.selectedLobbyMember.image_selection.SetActive(value: false);
			}
			break;
		case Context.Friends:
			if ((bool)lobby.selectedFriendMember)
			{
				lobby.selectedFriendMember.image_selection.SetActive(value: false);
			}
			break;
		case Context.Blocked:
			if ((bool)lobby.selectedBlockedMember)
			{
				lobby.selectedBlockedMember.image_selection.SetActive(value: false);
			}
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		lobby.Button_SelectMember(this, context);
		image_selection.SetActive(value: true);
	}

	public void UpdateScrollView()
	{
		ScrollRect scrollRect = context switch
		{
			Context.Lobby => lobby.lobbyScrollRect, 
			Context.Friends => lobby.friendsScrollRect, 
			Context.Blocked => lobby.blockedScrollRect, 
			_ => throw new ArgumentOutOfRangeException(), 
		};
		int siblingIndex = transform.GetSiblingIndex();
		float num = 1f - (float)siblingIndex / (float)scrollRect.content.transform.childCount;
		if ((double)num < 0.4)
		{
			float num2 = 1f / (float)scrollRect.content.transform.childCount;
			num -= num2;
		}
		scrollRect.verticalNormalizedPosition = num;
	}

	public void OnCancel(BaseEventData eventData)
	{
		switch (context)
		{
		case Context.Lobby:
			lobby.lobbyScrollRect.GetComponent<SelectableScrollList>().Select();
			break;
		case Context.Friends:
			lobby.Close();
			break;
		case Context.Blocked:
			lobby.Close();
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	public void EnableTriggers(bool enabled)
	{
		EventTrigger[] components = GetComponents<EventTrigger>();
		for (int i = 0; i < components.Length; i++)
		{
			components[i].enabled = enabled;
		}
	}
}
