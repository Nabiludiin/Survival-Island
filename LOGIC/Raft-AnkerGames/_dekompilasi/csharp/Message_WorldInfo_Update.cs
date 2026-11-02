using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine.AzureSky;

[Serializable]
public class Message_WorldInfo_Update : Message_NetworkBehaviour
{
	public int w;

	public int ti;

	public int[] k;

	public List<float> kWaterWeightsCache = new List<float>();

	public float WaterTime
	{
		get
		{
			return (float)w / 100f;
		}
		set
		{
			w = (int)(value * 100f);
		}
	}

	public float TimeOfDay
	{
		get
		{
			return (float)ti / 100f;
		}
		set
		{
			ti = (int)(value * 100f);
		}
	}

	protected bool Equals(Message_WorldInfo_Update other)
	{
		if (k == null && other.k != null)
		{
			return false;
		}
		if (k != null && other.k == null)
		{
			return false;
		}
		if (kWaterWeightsCache == null && other.kWaterWeightsCache != null)
		{
			return false;
		}
		if (kWaterWeightsCache != null && other.kWaterWeightsCache == null)
		{
			return false;
		}
		if (Equals((Message_NetworkBehaviour)other) && w == other.w && ti == other.ti && ((k == null && other.k == null) || Enumerable.SequenceEqual(k, other.k)))
		{
			if (kWaterWeightsCache != null || other.kWaterWeightsCache != null)
			{
				return kWaterWeightsCache.SequenceEqual(other.kWaterWeightsCache);
			}
			return true;
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
		return Equals((Message_WorldInfo_Update)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), w, ti, k, kWaterWeightsCache);
	}

	public static bool operator ==(Message_WorldInfo_Update left, Message_WorldInfo_Update right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_WorldInfo_Update left, Message_WorldInfo_Update right)
	{
		return !object.Equals(left, right);
	}

	public Message_WorldInfo_Update()
	{
	}

	public Message_WorldInfo_Update(Messages type, MonoBehaviour_Network behaviour, Network_Water water, AzureSkyController skyController, WaterRoughnessManager waterRougnessManager)
		: base(type, behaviour)
	{
		WaterTime = water.WaterTime;
		TimeOfDay = skyController.timeOfDay.hour;
		SetWaterWeights(waterRougnessManager.WaterProfileWeights.ToArray());
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in w, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in ti, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(k, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe<int>(kWaterWeightsCache.Count, default(FastBufferWriter.ForPrimitives));
		foreach (float item in kWaterWeightsCache)
		{
			writer.WriteValueSafe<float>(item, default(FastBufferWriter.ForPrimitives));
		}
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out w, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out ti, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out k, default(FastBufferWriter.ForPrimitives));
		int value = 0;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		kWaterWeightsCache = new List<float>(value);
		for (int i = 0; i < value; i++)
		{
			reader.ReadValue(out float value2, default(FastBufferWriter.ForPrimitives));
			kWaterWeightsCache.Add(value2);
		}
	}

	public float[] GetWaterWeight()
	{
		if (k.ContainsItems())
		{
			float[] array = new float[k.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = (float)k[i] / 100f;
			}
			return array;
		}
		return null;
	}

	public List<float> GetWaterWeightList()
	{
		if (k.ContainsItems())
		{
			kWaterWeightsCache.Clear();
			for (int i = 0; i < k.Length; i++)
			{
				kWaterWeightsCache.Add((float)k[i] / 100f);
			}
			return kWaterWeightsCache;
		}
		return null;
	}

	public void SetWaterWeights(float[] weights)
	{
		k = new int[weights.Length];
		for (int i = 0; i < weights.Length; i++)
		{
			k[i] = (int)(weights[i] * 100f);
		}
	}
}
