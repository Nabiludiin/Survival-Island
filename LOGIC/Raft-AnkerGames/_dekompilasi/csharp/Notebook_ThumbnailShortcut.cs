using UnityEngine;

public class Notebook_ThumbnailShortcut : MonoBehaviour
{
	public NoteBookPage targetPage;

	private NoteBookUI notebookUI;

	private void Awake()
	{
		notebookUI = GetComponentInParent<NoteBookUI>();
	}

	public void OnThumbnailButtonPress()
	{
		if (!NoteBookNote_Enhancer.IsEnhacedOnNote)
		{
			notebookUI.FlipToPageNetwork(targetPage.pageIndex);
		}
	}
}
