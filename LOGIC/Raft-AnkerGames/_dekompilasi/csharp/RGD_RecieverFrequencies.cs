using System;
using System.Linq;
using System.Runtime.Serialization;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class RGD_RecieverFrequencies : SerializableBehaviour
{
	public int[][] generatedFrequencies;

	protected bool Equals(RGD_RecieverFrequencies other)
	{
		if (generatedFrequencies == null && other.generatedFrequencies != null)
		{
			return false;
		}
		if (generatedFrequencies != null && other.generatedFrequencies == null)
		{
			return false;
		}
		if (generatedFrequencies != null || other.generatedFrequencies != null)
		{
			return Enumerable.SequenceEqual(generatedFrequencies, other.generatedFrequencies);
		}
		return true;
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
		return Equals((RGD_RecieverFrequencies)obj);
	}

	public override int GetHashCode()
	{
		if (generatedFrequencies == null)
		{
			return 0;
		}
		return generatedFrequencies.GetHashCode();
	}

	public static bool operator ==(RGD_RecieverFrequencies left, RGD_RecieverFrequencies right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_RecieverFrequencies left, RGD_RecieverFrequencies right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<int>((generatedFrequencies != null) ? generatedFrequencies.Length : 0, default(FastBufferWriter.ForPrimitives));
		if (generatedFrequencies == null)
		{
			return;
		}
		int[][] array = generatedFrequencies;
		foreach (int[] array2 in array)
		{
			writer.WriteValueSafe<int>(array2.Length, default(FastBufferWriter.ForPrimitives));
			int[] array3 = array2;
			for (int j = 0; j < array3.Length; j++)
			{
				int value = array3[j];
				writer.WriteValueSafe(in value, default(FastBufferWriter.ForPrimitives));
			}
		}
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		int value = 0;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		if (value == 0)
		{
			return;
		}
		generatedFrequencies = new int[value][];
		for (int i = 0; i < value; i++)
		{
			int value2 = 0;
			reader.ReadValue(out value2, default(FastBufferWriter.ForPrimitives));
			generatedFrequencies[i] = new int[value2];
			for (int j = 0; j < value2; j++)
			{
				reader.ReadValue(out generatedFrequencies[i][j], default(FastBufferWriter.ForPrimitives));
			}
		}
	}

	public RGD_RecieverFrequencies(int tmp = 0)
	{
		if (RecieverFrequency.AllFrequencies != null)
		{
			generatedFrequencies = new int[RecieverFrequency.AllFrequencies.Length][];
			for (int i = 0; i < RecieverFrequency.AllFrequencies.Length; i++)
			{
				generatedFrequencies[i] = RecieverFrequency.AllFrequencies[i].numbers;
			}
		}
		else
		{
			Debug.Log("Reciever frequencies are null in Reciever.AllFrequencies");
		}
	}

	public void RestoreFrequencies()
	{
		if (generatedFrequencies == null)
		{
			return;
		}
		for (int i = 0; i < RecieverFrequency.AllFrequencies.Length; i++)
		{
			if (i < generatedFrequencies.Length)
			{
				RecieverFrequency.AllFrequencies[i].numbers = generatedFrequencies[i];
			}
		}
		RecieverFrequency.DoubleCheckDuplicateFrequencies();
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("GeneratedFrequencies", generatedFrequencies);
	}

	public RGD_RecieverFrequencies(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "GeneratedFrequencies")
			{
				generatedFrequencies = (int[][])item.Value;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		generatedFrequencies = null;
	}
}
