using System;
using System.Runtime.Serialization;

[Serializable]
public class RGD_NoteBookNote : SerializableBehaviour
{
	public int noteIndex;

	public bool isUnlocked;

	public RGD_NoteBookNote(NoteBookNote noteBook)
	{
		noteIndex = noteBook.noteIndex;
		isUnlocked = noteBook.isUnlocked;
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("NoteIndex", noteIndex);
		info.AddValue("IsUnlocked", isUnlocked);
	}

	public RGD_NoteBookNote(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			string name = item.Name;
			if (!(name == "NoteIndex"))
			{
				if (name == "IsUnlocked")
				{
					isUnlocked = Helper.ObjToValue<bool>(item.Value);
				}
			}
			else
			{
				noteIndex = Helper.ObjToValue<int>(item.Value);
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		noteIndex = 0;
		isUnlocked = false;
	}

	public void RestoreNoteBookNote(NoteBookNote noteBookNote)
	{
		noteBookNote.isUnlocked = isUnlocked;
	}
}
