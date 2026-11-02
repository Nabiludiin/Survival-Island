using System;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoadGame_Selection : MonoBehaviour, IScrollHandler, IEventSystemHandler
{
	public enum SaveFileComparedToAppStatus
	{
		Same,
		Older,
		Newer,
		Unknown
	}

	public Text text_GameName;

	[SerializeField]
	private Text text_LastPlayed;

	[SerializeField]
	private Text text_GameMode;

	public Image image_selection;

	public Image image_error;

	[HideInInspector]
	public RGD_Game rgdGame;

	[HideInInspector]
	public DirectoryInfo directoryInfo;

	[HideInInspector]
	public DateTime lastPlayedDate;

	[SerializeField]
	private Image nameBg;

	[HideInInspector]
	public GameMode gameMode;

	public static LoadGame_Selection selectedLoadGame;

	private static LoadGameBox loadGameBox;

	private SaveFileComparedToAppStatus saveFileComparedToAppStatus;

	public bool IsSelectionLoadable
	{
		get
		{
			if (saveFileComparedToAppStatus != SaveFileComparedToAppStatus.Newer)
			{
				return !IsCorrupt;
			}
			return false;
		}
	}

	public bool IsCorrupt => rgdGame == null;

	public string GetContainerName()
	{
		return $"RaftSave@{text_GameName.text}@{(int)gameMode}";
	}

	private void Awake()
	{
		if (loadGameBox == null)
		{
			loadGameBox = GetComponentInParent<LoadGameBox>();
		}
	}

	private void Update()
	{
		Color color = nameBg.color;
		color.a = (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad ? 1 : 0);
		nameBg.color = color;
	}

	public void UpdateScrollView()
	{
		loadGameBox.Button_Select(this);
	}

	public void OnSelected()
	{
		loadGameBox.Button_SelectLoad(this);
		if (selectedLoadGame != null)
		{
			selectedLoadGame.image_selection.gameObject.SetActive(value: false);
		}
		selectedLoadGame = this;
		image_selection.gameObject.SetActive(value: true);
	}

	public void OnClick()
	{
		loadGameBox.Button_LoadGame();
	}

	public void SetInfo(GameToFolderConnection con)
	{
		bool flag = con.rgdGame == null;
		if (!flag)
		{
			rgdGame.name = con.directoryInfo.Name;
			saveFileComparedToAppStatus = GetSaveFileAppStatusFromSave(con.rgdGame);
			text_GameMode.text = Helper.GetTerm("Difficulty/" + con.rgdGame.mode);
			lastPlayedDate = new DateTime(con.rgdGame.lastPlayedDateTicks);
			text_LastPlayed.text = lastPlayedDate.ToString("yyyy/MM/dd H:mm");
		}
		else
		{
			saveFileComparedToAppStatus = SaveFileComparedToAppStatus.Unknown;
			text_GameMode.text = "-";
			if (con.gameDirectoryInfo != null && !SaveAndLoad.ConvertBackupFolderNameToDateTime(con.gameDirectoryInfo.Name, out lastPlayedDate))
			{
				lastPlayedDate = DateTime.MinValue;
				text_LastPlayed.text = "-";
			}
			else
			{
				text_LastPlayed.text = "-";
			}
		}
		image_error.gameObject.SetActiveSafe(flag);
		directoryInfo = con.directoryInfo;
		rgdGame = con.rgdGame;
		text_GameName.text = con.directoryInfo.Name;
	}

	public static bool IsKnownBuildID(uint versionNumber)
	{
		uint[] array = new uint[13]
		{
			20653906u, 19836385u, 18884971u, 18434136u, 18283430u, 17481012u, 16761548u, 16655021u, 16626967u, 16611938u,
			16249207u, 16033837u, 10199006u
		};
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == versionNumber)
			{
				return true;
			}
		}
		return false;
	}

	public SaveFileComparedToAppStatus GetSaveFileAppStatusFromSave(RGD_Game rgdGame)
	{
		if (Settings.SaveVersion == (int)rgdGame.versionNumber || IsKnownBuildID(rgdGame.versionNumber))
		{
			return SaveFileComparedToAppStatus.Same;
		}
		if ((uint)Settings.SaveVersion < rgdGame.versionNumber)
		{
			return SaveFileComparedToAppStatus.Newer;
		}
		return SaveFileComparedToAppStatus.Older;
	}

	public string GetSaveFileStatusStringFromSave()
	{
		return saveFileComparedToAppStatus switch
		{
			SaveFileComparedToAppStatus.Same => "", 
			SaveFileComparedToAppStatus.Newer => Helper.GetTerm("Game/VersionOutdated"), 
			SaveFileComparedToAppStatus.Older => "", 
			_ => "", 
		};
	}

	public void OnCancel(BaseEventData eventData)
	{
		loadGameBox.Button_Cancel();
	}

	public void OnScroll(PointerEventData eventData)
	{
		loadGameBox.OnScroll(eventData);
	}
}
