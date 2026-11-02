using TMPro;
using UnityEngine;

public class NoteBookPage : MonoBehaviour
{
	public uint pageIndex;

	[SerializeField]
	private string pageTitleString = string.Empty;

	[SerializeField]
	private TextMeshProUGUI pageTitle;

	[SerializeField]
	private TextMeshProUGUI pageNumber;

	private NoteBookNote[] notes;

	private void Start()
	{
		FixPageTitle();
		FixPageNumber();
	}

	public void FindNotes()
	{
		notes = GetComponentsInChildren<NoteBookNote>(includeInactive: true);
	}

	public virtual void SetPageActive(bool value)
	{
		if (notes == null)
		{
			FindNotes();
		}
		gameObject.SetActiveSafe(value);
		if (value)
		{
			pageTitle.gameObject.SetActiveSafe(HasNotesUnlockedOnPage());
		}
	}

	public bool HasNotesUnlockedOnPage()
	{
		if (notes == null)
		{
			FindNotes();
		}
		for (int i = 0; i < notes.Length; i++)
		{
			if (notes[i].isUnlocked)
			{
				return true;
			}
		}
		return false;
	}

	public NoteBookNote[] GetNotes()
	{
		if (notes == null || notes.Length == 0)
		{
			FindNotes();
		}
		return notes;
	}

	public NoteBookNote GetNoteFromNoteIndex(int noteIndex)
	{
		NoteBookNote[] array = notes;
		foreach (NoteBookNote noteBookNote in array)
		{
			if (noteBookNote.noteIndex == noteIndex)
			{
				return noteBookNote;
			}
		}
		return null;
	}

	public void SetNoteLockState(int p_noteIndex, bool value)
	{
		if (notes[p_noteIndex] == null)
		{
			Debug.Log("note " + p_noteIndex + " at page " + pageIndex + "doesnt exist");
		}
		else
		{
			notes[p_noteIndex].isUnlocked = value;
		}
	}

	private void FixPageTitle()
	{
		if (pageTitle != null)
		{
			pageTitle.text = pageTitleString;
		}
	}

	private void FixPageNumber()
	{
		if (pageNumber != null)
		{
			pageNumber.text = (pageIndex + 1).ToString();
		}
	}

	private void FixName()
	{
		name = "NoteBookPage" + ((pageIndex % 2 == 0) ? "L" : "R") + "_" + pageTitleString + "_PageIndex " + pageIndex;
		FixPageTitle();
		FixPageNumber();
	}
}
