using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadGameVersionControlBox : MenuBox
{
	[SerializeField]
	private Text text_saveName;

	[SerializeField]
	private Button button_loadgame;

	[SerializeField]
	private Button button_deleteVersion;

	[SerializeField]
	private DeleteGameVersionBox deleteGameVersionBox;

	[SerializeField]
	private LoadGameBox loadGameBox;

	[SerializeField]
	private ScrollRect scrollRect;

	[HideInInspector]
	public LoadGame_Selection selectedGame;

	private LoadGame_VersionSelection[] versionSelectionPrefabs;

	private List<GameToFolderConnection> backupRGDGames = new List<GameToFolderConnection>();

	public static int latestWrittenSaveIndex;

	public override void Update()
	{
		base.Update();
		LoadGame_VersionSelection selectedVersionSelection = LoadGame_VersionSelection.selectedVersionSelection;
		button_loadgame.interactable = selectedGame != null && selectedVersionSelection != null && selectedVersionSelection.gameFolderCon != null && selectedVersionSelection.gameFolderCon.rgdGame != null;
		if (button_deleteVersion != null)
		{
			button_deleteVersion.interactable = selectedGame != null && selectedVersionSelection != null && selectedVersionSelection.gameFolderCon != null;
		}
	}

	private async void LoadInBackupSaves()
	{
		text_saveName.text = ((selectedGame != null) ? selectedGame.text_GameName.text : string.Empty);
		if (versionSelectionPrefabs == null)
		{
			versionSelectionPrefabs = GetComponentsInChildren<LoadGame_VersionSelection>();
			if (8 > versionSelectionPrefabs.Length)
			{
				Debug.LogError("There is not enough 'LoadGameVersionSelections' in menu compared to how many Raft can save as backups");
			}
		}
		for (int i = 0; i < versionSelectionPrefabs.Length; i++)
		{
			versionSelectionPrefabs[i].gameObject.SetActiveSafe(value: false);
		}
		if (backupRGDGames == null)
		{
			backupRGDGames = new List<GameToFolderConnection>();
		}
		else
		{
			backupRGDGames.Clear();
		}
		SaveAndLoad saveandload = ComponentManager<SaveAndLoad>.Value;
		if (saveandload != null && selectedGame != null)
		{
			DirectoryInfo[] directories = selectedGame.directoryInfo.GetDirectories();
			DirectoryInfo[] array = directories;
			foreach (DirectoryInfo versionFolder in array)
			{
				RGD_Game[] array2 = await saveandload.GetRGDGamesFromFolderAsync(versionFolder.FullName + "/");
				if (array2.ContainsItems())
				{
					backupRGDGames.Add(new GameToFolderConnection
					{
						rgdGame = array2[0],
						directoryInfo = versionFolder.Parent,
						gameDirectoryInfo = versionFolder
					});
				}
				else
				{
					backupRGDGames.Add(new GameToFolderConnection
					{
						rgdGame = null,
						directoryInfo = versionFolder.Parent,
						gameDirectoryInfo = versionFolder
					});
				}
			}
		}
		if (backupRGDGames.ContainsItems())
		{
			backupRGDGames.Reverse();
		}
		if (versionSelectionPrefabs.ContainsItems())
		{
			LoadGame_VersionSelection loadGame_VersionSelection = null;
			for (int k = 0; k < versionSelectionPrefabs.Length; k++)
			{
				LoadGame_VersionSelection loadGame_VersionSelection2 = versionSelectionPrefabs[k];
				loadGame_VersionSelection2.gameObject.SetActiveSafe(k < backupRGDGames.Count);
				if (loadGame_VersionSelection2.gameObject.activeSelf)
				{
					GameToFolderConnection info = backupRGDGames[k];
					loadGame_VersionSelection2.SetInfo(info);
					if (loadGame_VersionSelection == null)
					{
						loadGame_VersionSelection = loadGame_VersionSelection2;
					}
				}
			}
			if (loadGame_VersionSelection != null)
			{
				loadGame_VersionSelection.OnSelected();
			}
		}
		for (int l = 0; l < scrollRect.content.childCount - 1; l++)
		{
			Button component = scrollRect.content.GetChild(l).GetComponent<Button>();
			Button component2 = scrollRect.content.GetChild(l + 1).GetComponent<Button>();
			Navigation navigation = component.navigation;
			navigation.mode = Navigation.Mode.Explicit;
			navigation.selectOnDown = component2;
			component.navigation = navigation;
			Navigation navigation2 = component2.navigation;
			navigation2.mode = Navigation.Mode.Explicit;
			navigation2.selectOnUp = component;
			component2.navigation = navigation2;
		}
		OnSaveListSelected(scrollRect);
	}

	public override void Open()
	{
		base.Open();
		LoadInBackupSaves();
	}

	public override void Close()
	{
		base.Close();
		if (LoadGame_VersionSelection.selectedVersionSelection != null)
		{
			LoadGame_VersionSelection.selectedVersionSelection.image_selection.gameObject.SetActiveSafe(value: false);
		}
		LoadGame_VersionSelection.selectedVersionSelection = null;
		loadGameBox.CloseBackupList();
	}

	public void Button_DeleteVersion()
	{
		if (LoadGame_VersionSelection.selectedVersionSelection != null)
		{
			deleteGameVersionBox.Open();
		}
	}

	public void Button_Close()
	{
		Close();
	}

	public void DeleteSelectedVersion()
	{
		if (LoadGame_VersionSelection.selectedVersionSelection != null)
		{
			string fullName = LoadGame_VersionSelection.selectedVersionSelection.gameFolderCon.gameDirectoryInfo.FullName;
			if (Directory.Exists(fullName))
			{
				Directory.Delete(fullName, recursive: true);
				LoadGame_VersionSelection.selectedVersionSelection.gameObject.SetActiveSafe(value: false);
				LoadGame_VersionSelection.selectedVersionSelection = null;
			}
		}
	}

	public void OnSaveListSelected(ScrollRect scrollRect)
	{
		for (int i = 0; i < scrollRect.content.childCount; i++)
		{
			LoadGame_VersionSelection component = scrollRect.content.GetChild(i).GetComponent<LoadGame_VersionSelection>();
			if (LoadGame_VersionSelection.selectedVersionSelection == component)
			{
				scrollRect.content.GetChild(i).GetComponent<Button>().Select();
			}
		}
	}

	public void Button_Select(LoadGame_VersionSelection selection)
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
}
