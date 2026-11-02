using UnityEngine;
using UnityEngine.UI;

public class DeleteGameVersionBox : MenuBox
{
	[SerializeField]
	private LoadGameVersionControlBox loadGameVersionBox;

	[SerializeField]
	private Text text_title;

	public override void Open()
	{
		base.Open();
		text_title.text = Helper.GetTerm("Menu/DeleteWorldButton") + " " + LoadGame_VersionSelection.selectedVersionSelection.text_LastPlayed.text + " " + Helper.GetTerm("Menu/Permanently") + "?";
	}

	public void YesButton()
	{
		loadGameVersionBox.DeleteSelectedVersion();
		Close();
	}

	public void NoButton()
	{
		Close();
	}
}
