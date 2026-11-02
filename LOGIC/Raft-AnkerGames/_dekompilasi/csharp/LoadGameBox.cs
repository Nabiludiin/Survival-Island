using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LoadGameBox : MenuBox
{
	[SerializeField]
	private Dropdown authSettingDropdown;

	[SerializeField]
	private Toggle allowFriendlyFireToggle;

	[SerializeField]
	private Toggle allowCrossplay;

	[SerializeField]
	private LoadGame_Selection loadGameSelectionPrefab;

	[SerializeField]
	private ScrollRect scrollRect;

	[SerializeField]
	private Button deleteButton;

	[SerializeField]
	private Button loadButton;

	[SerializeField]
	private Button versionControlButton;

	[SerializeField]
	private DeleteGameBox deleteGameBox;

	[SerializeField]
	private InputField inputfield_Password;

	[SerializeField]
	private Text saveFileVersioningText;

	[SerializeField]
	private LoadGameVersionControlBox versionControlBox;

	[SerializeField]
	private GameObject noGamesFoundsText;

	[SerializeField]
	private GameObject saveFolder;

	[SerializeField]
	private GameObject versionControl;

	[SerializeField]
	private GameObject loadGameText;

	[SerializeField]
	private GameObject backupsText;

	[SerializeField]
	private GameObject deleteText;

	[HideInInspector]
	public LoadGame_Selection selectedGame;

	private List<LoadGame_Selection> loadGameSelections = new List<LoadGame_Selection>();

	[SerializeField]
	private InputActionReference inputDelete;

	[SerializeField]
	private GameObject closeButton;

	[SerializeField]
	private Text offlineText;

	private Selectable remeberSelectable;

	private bool multiplayerAllowed;

	public void Start()
	{
	}

	protected override void OnControlChanged(bool gamepad)
	{
		base.OnControlChanged(gamepad);
		if (gamepad)
		{
			authSettingDropdown.Hide();
			OnGameListSelected(scrollRect);
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	public override void Update()
	{
		base.Update();
		if (selectedGame != null)
		{
			saveFileVersioningText.text = selectedGame.GetSaveFileStatusStringFromSave();
			if (inputDelete.action.WasPressedThisFrame())
			{
				Button_DeleteGame();
			}
		}
		bool flag = selectedGame != null && selectedGame.IsSelectionLoadable;
		bool flag2 = selectedGame != null && !versionControlBox.IsOpen;
		bool flag3 = selectedGame != null && !versionControlBox.IsOpen;
		versionControlButton.interactable = flag3;
		deleteButton.interactable = flag2;
		loadButton.interactable = flag;
		loadGameText.SetActive(flag);
		deleteText.SetActive(flag2);
		backupsText.SetActive(flag3);
		bool flag4 = CheckAuthSettingFromDropdown() != RequestJoinAuthSetting.ALLOW_NONE;
		bool flag5 = flag4;
		multiplayerAllowed = Raft_Network.SignedIntoPlayfab;
		allowFriendlyFireToggle.gameObject.SetActive(flag4 && multiplayerAllowed);
		allowCrossplay.gameObject.SetActive(flag5 && multiplayerAllowed);
		inputfield_Password.gameObject.SetActive(flag4 && multiplayerAllowed && CheckAuthSettingFromDropdown() != RequestJoinAuthSetting.INVITE_ONLY);
		if (!flag4)
		{
			Navigation navigation = authSettingDropdown.navigation;
			navigation.selectOnDown = (flag ? loadButton : null);
			authSettingDropdown.navigation = navigation;
			Navigation navigation2 = loadButton.navigation;
			navigation2.selectOnUp = authSettingDropdown;
			loadButton.navigation = navigation2;
			offlineText.gameObject.SetActive(value: false);
		}
		else if (multiplayerAllowed)
		{
			offlineText.gameObject.SetActive(value: false);
			if (flag5)
			{
				Navigation navigation3 = allowFriendlyFireToggle.navigation;
				navigation3.selectOnDown = allowCrossplay;
				allowFriendlyFireToggle.navigation = navigation3;
				if (CheckAuthSettingFromDropdown() == RequestJoinAuthSetting.INVITE_ONLY)
				{
					Navigation navigation4 = allowCrossplay.navigation;
					navigation4.selectOnDown = (flag ? loadButton : null);
					allowCrossplay.navigation = navigation4;
					Navigation navigation5 = loadButton.navigation;
					navigation5.selectOnUp = allowCrossplay;
					loadButton.navigation = navigation5;
				}
				else
				{
					Navigation navigation6 = allowCrossplay.navigation;
					navigation6.selectOnDown = inputfield_Password;
					allowCrossplay.navigation = navigation6;
					Navigation navigation7 = loadButton.navigation;
					navigation7.selectOnUp = inputfield_Password;
					loadButton.navigation = navigation7;
				}
			}
			else
			{
				Navigation navigation8 = allowFriendlyFireToggle.navigation;
				navigation8.selectOnDown = (flag ? loadButton : null);
				allowFriendlyFireToggle.navigation = navigation8;
				Navigation navigation9 = loadButton.navigation;
				navigation9.selectOnUp = allowFriendlyFireToggle;
				loadButton.navigation = navigation9;
			}
			Navigation navigation10 = authSettingDropdown.navigation;
			navigation10.selectOnDown = allowFriendlyFireToggle;
			authSettingDropdown.navigation = navigation10;
		}
		else
		{
			Navigation navigation11 = authSettingDropdown.navigation;
			navigation11.selectOnDown = null;
			authSettingDropdown.navigation = navigation11;
			loadButton.interactable = false;
			loadGameText.SetActive(value: false);
			offlineText.gameObject.SetActive(value: true);
			if (EventSystem.current.currentSelectedGameObject == allowFriendlyFireToggle.gameObject || EventSystem.current.currentSelectedGameObject == allowCrossplay.gameObject)
			{
				authSettingDropdown.Select();
			}
		}
	}

	public override void Open()
	{
		base.Open();
		LoadInSavedGames();
	}

	public void CloseBackupList()
	{
		remeberSelectable?.Select();
		remeberSelectable = null;
	}

	public async void LoadInSavedGames()
	{
		ClearLoadGameSelections();
		SaveAndLoad saveandload = ComponentManager<SaveAndLoad>.Value;
		saveandload.CreateNecessaryLoadDirectories();
		saveandload.ConvertAllOldSavesToNewFormat();
		DirectoryInfo[] directories = new DirectoryInfo(SaveAndLoad.WorldPath).GetDirectories();
		List<GameToFolderConnection> gameConnections = new List<GameToFolderConnection>();
		DirectoryInfo[] array = directories;
		foreach (DirectoryInfo worldDirectory in array)
		{
			if (!(worldDirectory.Name == SaveAndLoad.BackupFolderName))
			{
				try
				{
					SaveAndLoad.LatestSaveInfo latestSaveInfo = await saveandload.GetLatestRGDGameFromBackupFoldersAsync(worldDirectory);
					gameConnections.Add(new GameToFolderConnection
					{
						rgdGame = latestSaveInfo.rgdGame,
						directoryInfo = worldDirectory,
						gameDirectoryInfo = latestSaveInfo.rgdGameDirInfo
					});
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogWarning("Failed to load world '" + worldDirectory.Name + "': " + ex.Message);
					gameConnections.Add(new GameToFolderConnection
					{
						rgdGame = null,
						directoryInfo = worldDirectory,
						gameDirectoryInfo = null
					});
				}
			}
		}
		foreach (GameToFolderConnection item in gameConnections.OrderByDescending((GameToFolderConnection con) => con.InGameTicks))
		{
			LoadGame_Selection loadGame_Selection = UnityEngine.Object.Instantiate(loadGameSelectionPrefab, scrollRect.content);
			loadGame_Selection.SetInfo(item);
			loadGameSelections.Add(loadGame_Selection);
		}
		if (gameObject.activeInHierarchy)
		{
			if (scrollRect.content.childCount > 0)
			{
				scrollRect.content.GetChild(0).GetComponent<LoadGame_Selection>().OnSelected();
			}
			for (int num = 0; num < scrollRect.content.childCount - 1; num++)
			{
				Button component = scrollRect.content.GetChild(num).GetComponent<Button>();
				Button component2 = scrollRect.content.GetChild(num + 1).GetComponent<Button>();
				Navigation navigation = component.navigation;
				navigation.mode = Navigation.Mode.Explicit;
				navigation.selectOnDown = component2;
				component.navigation = navigation;
				Navigation navigation2 = component2.navigation;
				navigation2.mode = Navigation.Mode.Explicit;
				navigation2.selectOnUp = component;
				component2.navigation = navigation2;
			}
			if (noGamesFoundsText.activeSelf && loadGameSelections.Count > 0)
			{
				noGamesFoundsText.SetActive(value: false);
			}
			else if (!noGamesFoundsText.activeSelf && loadGameSelections.Count == 0)
			{
				noGamesFoundsText.SetActive(value: true);
			}
		}
	}

	private void ClearLoadGameSelections()
	{
		for (int i = 0; i < loadGameSelections.Count; i++)
		{
			UnityEngine.Object.Destroy(loadGameSelections[i].gameObject);
		}
		loadGameSelections.Clear();
		scrollRect.content.DetachChildren();
	}

	public void Button_Select(LoadGame_Selection selection)
	{
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
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
	}

	public void Button_Cancel()
	{
		scrollRect.GetComponent<SelectableScrollList>().Select();
	}

	public void OnGameListSelected(ScrollRect scrollRect)
	{
		for (int i = 0; i < scrollRect.content.childCount; i++)
		{
			LoadGame_Selection component = scrollRect.content.GetChild(i).GetComponent<LoadGame_Selection>();
			if (LoadGame_Selection.selectedLoadGame == component)
			{
				scrollRect.content.GetChild(i).GetComponent<Button>().Select();
			}
		}
	}

	public void Button_SelectLoad(LoadGame_Selection selection)
	{
		selectedGame = selection;
		SaveAndLoad.WorldToLoad = selection.rgdGame;
		if (SaveAndLoad.WorldToLoad != null)
		{
			GameModeValueManager.SelectCurrentGameMode(SaveAndLoad.WorldToLoad.mode);
		}
		if (versionControlBox.IsOpen)
		{
			Button_VersionControl();
		}
	}

	public void Button_VersionControl()
	{
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
		{
			remeberSelectable = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>();
		}
		versionControlBox.selectedGame = selectedGame;
		versionControlBox.Open();
	}

	public void Button_Close()
	{
		Close();
		if (versionControlBox.IsOpen)
		{
			versionControlBox.Close();
		}
	}

	public void Button_LoadGame()
	{
		authSettingDropdown.Hide();
		if (versionControlBox.IsOpen && LoadGame_VersionSelection.selectedVersionSelection != null)
		{
			SaveAndLoad.WorldToLoad = LoadGame_VersionSelection.selectedVersionSelection.gameFolderCon.rgdGame;
			if (SaveAndLoad.WorldToLoad != null)
			{
				GameModeValueManager.SelectCurrentGameMode(SaveAndLoad.WorldToLoad.mode);
			}
		}
		if (!(SaveAndLoad.WorldToLoad == null))
		{
			GameManager.IsInNewGame = false;
			GameManager.FriendlyFire = allowFriendlyFireToggle.isOn;
			GameManager.Crossplay = allowCrossplay.isOn;
			RequestJoinAuthSetting joinAuthSetting = CheckAuthSettingFromDropdown();
			ComponentManager<Raft_Network>.Value.HostGame(joinAuthSetting, inputfield_Password.text);
		}
	}

	public void Button_DeleteGame()
	{
		deleteGameBox.Open();
		deleteGameBox.GetComponentInChildren<Selectable>().Select();
	}

	public void Button_OpenSaveFolderPath()
	{
		try
		{
			string worldPath = SaveAndLoad.WorldPath;
			worldPath = worldPath.Replace("/", "\\");
			Process.Start("explorer.exe", "/select," + worldPath);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log("Button_OpenSaveFolderPath() - " + ex);
		}
	}

	public void DeleteSave()
	{
		if (!(selectedGame != null))
		{
			return;
		}
		string empty = string.Empty;
		empty = Path.Combine(SaveAndLoad.WorldPath, selectedGame.text_GameName.text);
		if (Directory.Exists(empty))
		{
			try
			{
				Directory.Delete(string.Format(empty), recursive: true);
				loadGameSelections.Remove(selectedGame);
				UnityEngine.Object.Destroy(selectedGame.gameObject);
				selectedGame = null;
			}
			catch (Exception e)
			{
				FileManager.HandleSystemError(e);
			}
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

	public void OnScroll(PointerEventData eventData)
	{
		scrollRect.OnScroll(eventData);
	}
}
