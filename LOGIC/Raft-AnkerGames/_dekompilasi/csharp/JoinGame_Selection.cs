using System.Collections;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

public class JoinGame_Selection : MonoBehaviour
{
	[HideInInspector]
	public CSteamID steamID;

	[SerializeField]
	private Text text_GameName;

	[SerializeField]
	private Image image_Avatar;

	[SerializeField]
	private Image image_passwordLock;

	public GameObject image_selection;

	public bool passwordProtected;

	private static JoinGame_Selection selectedSelection;

	private static JoinGameBox joinGameBox;

	private void Awake()
	{
		joinGameBox = GetComponentInParent<JoinGameBox>();
	}

	public void OnSelected()
	{
		joinGameBox.Button_SelectGame(this);
		if (selectedSelection != null)
		{
			selectedSelection.image_selection.SetActive(value: false);
		}
		selectedSelection = this;
		image_selection.SetActive(value: true);
	}

	public void Set(CSteamID steamID, bool passwordProtected)
	{
		this.steamID = steamID;
		this.passwordProtected = passwordProtected;
		text_GameName.text = SteamFriends.GetFriendPersonaName(steamID);
		image_passwordLock.gameObject.SetActive(passwordProtected);
		StartCoroutine(SetAvatar(image_Avatar, steamID));
	}

	private IEnumerator SetAvatar(Image target, CSteamID steamid)
	{
		int FriendAvatar = SteamFriends.GetMediumFriendAvatar(steamid);
		while (FriendAvatar == -1)
		{
			yield return null;
		}
		if (FriendAvatar > 0 && SteamUtils.GetImageSize(FriendAvatar, out var pnWidth, out var pnHeight) && pnWidth != 0 && pnHeight != 0)
		{
			byte[] array = new byte[pnWidth * pnHeight * 4];
			SteamUtils.GetImageRGBA(FriendAvatar, array, (int)(pnWidth * pnHeight * 4));
			Texture2D texture2D = new Texture2D((int)pnWidth, (int)pnHeight, TextureFormat.RGBA32, mipChain: false, linear: true);
			texture2D.LoadRawTextureData(array);
			texture2D.Apply();
			target.sprite = Sprite.Create(texture2D, new Rect(0f, 0f, pnWidth, pnHeight), new Vector2(0.5f, 0.5f));
		}
	}
}
