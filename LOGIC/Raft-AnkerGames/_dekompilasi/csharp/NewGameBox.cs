using System.IO;
using I2.Loc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NewGameBox : MenuBox
{
	[Header("Components")]
	[SerializeField]
	private InputField inputfield_GameName;

	[SerializeField]
	private Dropdown authSettingDropdown;

	[SerializeField]
	private Toggle toggle_FriendlyFire;

	[SerializeField]
	private Toggle toggle_Crossplay;

	[SerializeField]
	private Button createGameButton;

	[SerializeField]
	private Text text_nameFeedback;

	[SerializeField]
	private InputField inputfield_Password;

	[SerializeField]
	private TabGroup tabGroup;

	[SerializeField]
	private GameObject closeButton;

	[SerializeField]
	private GameObject lb;

	[SerializeField]
	private GameObject rb;

	[SerializeField]
	private GameObject gamepadLayout;

	[SerializeField]
	private GameObject createGameText;

	[SerializeField]
	private Text offlineText;

	private Raft_Network network;

	private bool multiplayerAllowed;

	public void Start()
	{
		network = ComponentManager<Raft_Network>.Value;
	}

	public void GameNameEndEdit(string str)
	{
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	public override void Update()
	{
		base.Update();
		string text = inputfield_GameName.text.ToUpper();
		if (string.IsNullOrEmpty(text))
		{
			text_nameFeedback.text = LocalizationManager.GetTranslation("Menu/EmptyName");
			createGameButton.interactable = false;
		}
		else
		{
			bool flag = false;
			string text2 = SaveAndLoad.SanitizeFileName(text);
			string text3 = (string.IsNullOrEmpty(text2) ? text : text2);
			flag = File.Exists(SaveAndLoad.WorldPath + text3 + ".rgd") || Directory.Exists(SaveAndLoad.WorldPath + text3);
			bool flag2 = true;
			for (int i = 0; i < text.Length; i++)
			{
				if (text[i] != '.')
				{
					flag2 = false;
					break;
				}
			}
			if (text != text2)
			{
				flag2 = true;
			}
			if (flag2 || text.ContainsInvalidWindowsCharacter())
			{
				text_nameFeedback.text = LocalizationManager.GetTranslation("Menu/InvalidName");
				createGameButton.interactable = false;
			}
			else if (flag)
			{
				text_nameFeedback.text = LocalizationManager.GetTranslation("Menu/NameExists");
				createGameButton.interactable = false;
			}
			else
			{
				text_nameFeedback.text = string.Empty;
				createGameButton.interactable = true;
			}
		}
		bool flag3 = CheckAuthSettingFromDropdown() != RequestJoinAuthSetting.ALLOW_NONE;
		bool flag4 = flag3;
		multiplayerAllowed = Raft_Network.SignedIntoPlayfab;
		toggle_FriendlyFire.gameObject.SetActive(flag3 && multiplayerAllowed);
		toggle_Crossplay.gameObject.SetActive(flag4 && multiplayerAllowed);
		inputfield_Password.gameObject.SetActive(flag3 && multiplayerAllowed && CheckAuthSettingFromDropdown() != RequestJoinAuthSetting.INVITE_ONLY);
		if (!flag3)
		{
			Navigation navigation = authSettingDropdown.navigation;
			navigation.selectOnDown = (createGameButton.interactable ? createGameButton : null);
			authSettingDropdown.navigation = navigation;
			Navigation navigation2 = createGameButton.navigation;
			navigation2.selectOnUp = authSettingDropdown;
			createGameButton.navigation = navigation2;
			offlineText.gameObject.SetActive(value: false);
		}
		else if (multiplayerAllowed)
		{
			offlineText.gameObject.SetActive(value: false);
			if (flag4)
			{
				Navigation navigation3 = toggle_FriendlyFire.navigation;
				navigation3.selectOnDown = toggle_Crossplay;
				toggle_FriendlyFire.navigation = navigation3;
				if (CheckAuthSettingFromDropdown() == RequestJoinAuthSetting.INVITE_ONLY)
				{
					Navigation navigation4 = toggle_Crossplay.navigation;
					navigation4.selectOnDown = (createGameButton.interactable ? createGameButton : null);
					toggle_Crossplay.navigation = navigation4;
					Navigation navigation5 = createGameButton.navigation;
					navigation5.selectOnUp = toggle_Crossplay;
					createGameButton.navigation = navigation5;
				}
				else
				{
					Navigation navigation6 = toggle_Crossplay.navigation;
					navigation6.selectOnDown = inputfield_Password;
					toggle_Crossplay.navigation = navigation6;
					Navigation navigation7 = createGameButton.navigation;
					navigation7.selectOnUp = inputfield_Password;
					createGameButton.navigation = navigation7;
				}
			}
			else
			{
				Navigation navigation8 = toggle_FriendlyFire.navigation;
				navigation8.selectOnDown = (createGameButton.interactable ? createGameButton : null);
				toggle_FriendlyFire.navigation = navigation8;
				Navigation navigation9 = createGameButton.navigation;
				navigation9.selectOnUp = toggle_FriendlyFire;
				createGameButton.navigation = navigation9;
			}
			Navigation navigation10 = authSettingDropdown.navigation;
			navigation10.selectOnDown = toggle_FriendlyFire;
			authSettingDropdown.navigation = navigation10;
		}
		else
		{
			Navigation navigation11 = authSettingDropdown.navigation;
			navigation11.selectOnDown = null;
			authSettingDropdown.navigation = navigation11;
			createGameButton.interactable = false;
			offlineText.gameObject.SetActive(value: true);
			if (EventSystem.current.currentSelectedGameObject == toggle_FriendlyFire.gameObject || EventSystem.current.currentSelectedGameObject == toggle_Crossplay.gameObject)
			{
				authSettingDropdown.Select();
			}
		}
		createGameText.SetActive(createGameButton.interactable);
	}

	public override void Open()
	{
		base.Open();
		if (tabGroup == null)
		{
			Debug.LogError("TabGroup is null, add it in the newgame box");
		}
		if (tabGroup.SelectedTabButton is TabButtonGameMode)
		{
			GameModeValueManager.SelectCurrentGameMode(((TabButtonGameMode)tabGroup.SelectedTabButton).ButtonGameMode);
		}
	}

	public void Button_Close()
	{
		Close();
	}

	public void Button_CreateNewGame()
	{
		if (!inputfield_GameName.text.IsNullOrEmpty())
		{
			authSettingDropdown.Hide();
			if (tabGroup.SelectedTabButton is TabButtonGameMode)
			{
				GameModeValueManager.SelectCurrentGameMode(((TabButtonGameMode)tabGroup.SelectedTabButton).ButtonGameMode);
			}
			GameManager.IsInNewGame = true;
			GameManager.FriendlyFire = toggle_FriendlyFire.isOn;
			GameManager.Crossplay = toggle_Crossplay.isOn;
			SaveAndLoad.CurrentGameFileName = inputfield_GameName.text.TrimEnd();
			RequestJoinAuthSetting joinAuthSetting = CheckAuthSettingFromDropdown();
			network.HostGame(joinAuthSetting, inputfield_Password.text);
		}
	}

	public void OnAllowFriendsToggle()
	{
	}

	public RequestJoinAuthSetting CheckAuthSettingFromDropdown()
	{
		RequestJoinAuthSetting result = RequestJoinAuthSetting.ALLOW_NONE;
		switch (authSettingDropdown.value)
		{
		case 0:
			result = RequestJoinAuthSetting.ALLOW_FRIENDS;
			break;
		case 1:
			result = RequestJoinAuthSetting.ALLOW_NONE;
			break;
		case 2:
			result = RequestJoinAuthSetting.INVITE_ONLY;
			break;
		}
		return result;
	}
}
