using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoadGame_VersionSelection : MonoBehaviour
{
	public Image image_selection;

	public Image image_error;

	public Text text_LastPlayed;

	[HideInInspector]
	public GameToFolderConnection gameFolderCon;

	public static LoadGame_VersionSelection selectedVersionSelection;

	private static LoadGameVersionControlBox loadGameBox;

	private void Awake()
	{
		if (loadGameBox == null)
		{
			loadGameBox = GetComponentInParent<LoadGameVersionControlBox>();
		}
	}

	public void UpdateScrollView()
	{
		loadGameBox.Button_Select(this);
	}

	public void OnSelected()
	{
		if (selectedVersionSelection != null)
		{
			selectedVersionSelection.image_selection.gameObject.SetActiveSafe(value: false);
		}
		selectedVersionSelection = this;
		selectedVersionSelection.image_selection.gameObject.SetActiveSafe(value: true);
	}

	public void SetInfo(GameToFolderConnection gameFolderCon)
	{
		this.gameFolderCon = gameFolderCon;
		bool flag = gameFolderCon == null || gameFolderCon.rgdGame == null;
		if (!flag)
		{
			DateTime dateTime = new DateTime(gameFolderCon.rgdGame.lastPlayedDateTicks);
			text_LastPlayed.text = dateTime.ToString("yyyy/MM/dd H:mm");
		}
		else
		{
			text_LastPlayed.text = "Error";
			if (SaveAndLoad.ConvertBackupFolderNameToDateTime(gameFolderCon.gameDirectoryInfo.Name, out var dateTime2))
			{
				text_LastPlayed.text = dateTime2.ToString("yyyy/MM/dd H:mm") + " Error";
			}
		}
		image_error.gameObject.SetActiveSafe(flag);
	}

	public void OnCancel(BaseEventData eventData)
	{
		loadGameBox.Button_Cancel();
	}
}
