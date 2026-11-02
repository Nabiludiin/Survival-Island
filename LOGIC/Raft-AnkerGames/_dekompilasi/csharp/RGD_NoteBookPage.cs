using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class RGD_NoteBookPage : SerializableBehaviour
{
	[SerializeField]
	private RGD_NoteBookNote[] rgd_notes;

	public RGD_NoteBookPage(NoteBookPage noteBookPage)
	{
		List<RGD_NoteBookNote> list = new List<RGD_NoteBookNote>();
		NoteBookNote[] notes = noteBookPage.GetNotes();
		for (int i = 0; i < notes.Length; i++)
		{
			RGD_NoteBookNote item = new RGD_NoteBookNote(notes[i]);
			list.Add(item);
		}
		rgd_notes = list.ToArray();
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("Notes", rgd_notes);
	}

	public RGD_NoteBookPage(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "Notes")
			{
				rgd_notes = Helper.ObjToValue<RGD_NoteBookNote[]>(item.Value);
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		rgd_notes = null;
	}

	public void RestoreNoteBookPage(NoteBookPage noteBookPage)
	{
		for (int i = 0; i < rgd_notes.Length; i++)
		{
			rgd_notes[i].RestoreNoteBookNote(noteBookPage.GetNoteFromNoteIndex(rgd_notes[i].noteIndex));
		}
	}
}
