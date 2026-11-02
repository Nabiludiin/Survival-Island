using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_BeeHive_Create : Message_NetworkBehaviour
{
	public RGD_BeeHive[] rgd_BeeHives;

	protected bool Equals(Message_BeeHive_Create other)
	{
		if (rgd_BeeHives == null && other.rgd_BeeHives != null)
		{
			return false;
		}
		if (rgd_BeeHives != null && other.rgd_BeeHives == null)
		{
			return false;
		}
		if (Equals((Message_NetworkBehaviour)other))
		{
			if (rgd_BeeHives != null || other.rgd_BeeHives != null)
			{
				return Enumerable.SequenceEqual(rgd_BeeHives, other.rgd_BeeHives);
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
		return Equals((Message_BeeHive_Create)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgd_BeeHives);
	}

	public static bool operator ==(Message_BeeHive_Create left, Message_BeeHive_Create right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_BeeHive_Create left, Message_BeeHive_Create right)
	{
		return !object.Equals(left, right);
	}

	public Message_BeeHive_Create()
	{
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<int>((rgd_BeeHives != null) ? rgd_BeeHives.Length : 0, default(FastBufferWriter.ForPrimitives));
		if (rgd_BeeHives != null)
		{
			RGD_BeeHive[] array = rgd_BeeHives;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SerializeFast(writer);
			}
		}
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		int value = 0;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		if (value > 0)
		{
			rgd_BeeHives = new RGD_BeeHive[value];
			for (int i = 0; i < value; i++)
			{
				rgd_BeeHives[i] = new RGD_BeeHive();
				rgd_BeeHives[i].DeserializeFast(reader);
			}
		}
	}

	public Message_BeeHive_Create(Messages type, MonoBehaviour_Network behaviour, BeeHive[] beeHives)
		: base(type, behaviour)
	{
		if (beeHives == null)
		{
			return;
		}
		List<RGD_BeeHive> list = new List<RGD_BeeHive>();
		foreach (BeeHive beeHive in beeHives)
		{
			if (beeHive != null)
			{
				list.Add(new RGD_BeeHive(beeHive));
			}
		}
		rgd_BeeHives = list.ToArray();
	}

	public void RestoreBeeHives(BeeHive[] beeHives)
	{
		if (beeHives == null || !beeHives.ContainsItems() || rgd_BeeHives == null || !rgd_BeeHives.ContainsItems())
		{
			return;
		}
		RGD_BeeHive[] array = rgd_BeeHives;
		foreach (RGD_BeeHive rGD_BeeHive in array)
		{
			if (!(rGD_BeeHive == null))
			{
				BeeHive beeHiveFromObjectIndex = GetBeeHiveFromObjectIndex(beeHives, rGD_BeeHive.BeeHiveObjectIndex);
				if (beeHiveFromObjectIndex != null)
				{
					rGD_BeeHive.RestoreBeeHive(beeHiveFromObjectIndex);
				}
				else
				{
					Debug.Log("attempted to restore null beehive");
				}
			}
		}
	}

	private BeeHive GetBeeHiveFromObjectIndex(BeeHive[] beeHives, uint objectIndex)
	{
		for (int i = 0; i < beeHives.Length; i++)
		{
			if (beeHives[i].ObjectIndex == objectIndex)
			{
				return beeHives[i];
			}
		}
		return null;
	}
}
