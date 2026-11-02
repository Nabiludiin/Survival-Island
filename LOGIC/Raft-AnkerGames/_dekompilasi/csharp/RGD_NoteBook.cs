using System;
using System.Linq;
using System.Runtime.Serialization;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class RGD_NoteBook : SerializableBehaviour
{
	public int[] unlockedIndexes;

	public uint latestIndex;

	protected bool Equals(RGD_NoteBook other)
	{
		if (unlockedIndexes == null && other.unlockedIndexes != null)
		{
			return false;
		}
		if (unlockedIndexes != null && other.unlockedIndexes == null)
		{
			return false;
		}
		if ((unlockedIndexes == null && other.unlockedIndexes == null) || Enumerable.SequenceEqual(unlockedIndexes, other.unlockedIndexes))
		{
			return latestIndex == other.latestIndex;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (this == obj)
		{
			return true;
		}
		if (obj.GetType() != GetType())
		{
			return false;
		}
		return Equals((RGD_NoteBook)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(unlockedIndexes, latestIndex);
	}

	public static bool operator ==(RGD_NoteBook left, RGD_NoteBook right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_NoteBook left, RGD_NoteBook right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<int>((unlockedIndexes != null) ? unlockedIndexes.Length : 0, default(FastBufferWriter.ForPrimitives));
		if (unlockedIndexes != null)
		{
			int[] array = unlockedIndexes;
			for (int i = 0; i < array.Length; i++)
			{
				int value = array[i];
				writer.WriteValueSafe(in value, default(FastBufferWriter.ForPrimitives));
			}
		}
		writer.WriteValueSafe(in latestIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		int value = 0;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		if (value > 0)
		{
			unlockedIndexes = new int[value];
			for (int i = 0; i < value; i++)
			{
				reader.ReadValue(out unlockedIndexes[i], default(FastBufferWriter.ForPrimitives));
			}
		}
		reader.ReadValue(out latestIndex, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_NoteBook()
	{
	}

	public RGD_NoteBook(NoteBook noteBook)
	{
		unlockedIndexes = NoteBook.unlockedNoteBookIndexes.ToArray();
		NoteBookUI noteBookUI = ComponentManager<Network_Player>.Value.NoteBookUI;
		if (noteBookUI != null)
		{
			latestIndex = noteBookUI.GetCurrentPageIndex();
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("Pages", unlockedIndexes);
		info.AddValue("LatestIndex", latestIndex);
	}

	public RGD_NoteBook(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			string name = item.Name;
			if (!(name == "Pages"))
			{
				if (name == "LatestIndex")
				{
					latestIndex = Helper.ObjToValue<uint>(item.Value);
				}
			}
			else
			{
				unlockedIndexes = Helper.ObjToValue<int[]>(item.Value);
			}
		}
	}

	public void RestoreNoteBook(NoteBook noteBook)
	{
		if (unlockedIndexes != null)
		{
			NoteBook.unlockedNoteBookIndexes.AddRange(unlockedIndexes);
		}
		Network_Player value = ComponentManager<Network_Player>.Value;
		if (value == null)
		{
			Debug.Log("Local player is null RGD_NoteBook.RestoreNoteBook()");
			return;
		}
		NoteBookUI noteBookUI = value.NoteBookUI;
		if (noteBookUI != null)
		{
			noteBookUI.SetPageIndex(latestIndex);
			noteBookUI.UpdateAllNotesVisibility();
			Array values = Enum.GetValues(typeof(ChunkPointType));
			if (values == null)
			{
				return;
			}
			{
				foreach (ChunkPointType item in values)
				{
					AchievementHandler.EvalueateNoteBookAchievement(item, noteBookUI);
				}
				return;
			}
		}
		Debug.Log("Notebook UI null");
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		unlockedIndexes = null;
		latestIndex = 0u;
	}
}
