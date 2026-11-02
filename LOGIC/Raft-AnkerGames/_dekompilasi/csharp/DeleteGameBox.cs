using UnityEngine;
using UnityEngine.UI;

public class DeleteGameBox : MenuBox
{
	[SerializeField]
	private LoadGameBox loadGameBox;

	[SerializeField]
	private Text text_title;

	public override void Open()
	{
		base.Open();
		text_title.text = Helper.GetTerm("Menu/DeleteWorldButton") + " " + loadGameBox.selectedGame.text_GameName.text + " " + Helper.GetTerm("Menu/Permanently") + "?";
		loadGameBox.Close();
	}

	public void YesButton()
	{
		loadGameBox.DeleteSave();
		loadGameBox.Open();
		Close();
	}

	public void NoButton()
	{
		loadGameBox.Open();
		Close();
	}
}
