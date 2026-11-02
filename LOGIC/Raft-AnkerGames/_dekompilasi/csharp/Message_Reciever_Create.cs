using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_Reciever_Create : Message_NetworkBehaviour
{
	public RGD_Reciever[] rgdRecievers;

	protected bool Equals(Message_Reciever_Create other)
	{
		if (rgdRecievers == null && other.rgdRecievers != null)
		{
			return false;
		}
		if (rgdRecievers != null && other.rgdRecievers == null)
		{
			return false;
		}
		if (Equals((Message_NetworkBehaviour)other))
		{
			if (rgdRecievers != null || other.rgdRecievers != null)
			{
				return Enumerable.SequenceEqual(rgdRecievers, other.rgdRecievers);
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
		return Equals((Message_Reciever_Create)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgdRecievers);
	}

	public static bool operator ==(Message_Reciever_Create left, Message_Reciever_Create right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Reciever_Create left, Message_Reciever_Create right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<int>((rgdRecievers != null) ? rgdRecievers.Length : 0, default(FastBufferWriter.ForPrimitives));
		if (rgdRecievers != null)
		{
			RGD_Reciever[] array = rgdRecievers;
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
			rgdRecievers = new RGD_Reciever[value];
			for (int i = 0; i < value; i++)
			{
				rgdRecievers[i] = new RGD_Reciever();
				rgdRecievers[i].DeserializeFast(reader);
			}
		}
	}

	public Message_Reciever_Create()
	{
	}

	public Message_Reciever_Create(Messages type, MonoBehaviour_Network behaviour, Reciever[] recievers)
		: base(type, behaviour)
	{
		if (recievers == null)
		{
			return;
		}
		List<RGD_Reciever> list = new List<RGD_Reciever>();
		foreach (Reciever reciever in recievers)
		{
			if (reciever != null)
			{
				list.Add(new RGD_Reciever(RGDType.None, reciever, reciever.CurrentFrequency.numbers, reciever.FrequencyNumberIndex));
			}
		}
		rgdRecievers = list.ToArray();
	}

	public void RestoreRecievers(Reciever[] recievers, MonoBehaviour coroutineHolder)
	{
		if (recievers == null || !recievers.ContainsItems() || rgdRecievers == null || !rgdRecievers.ContainsItems())
		{
			return;
		}
		for (int i = 0; i < rgdRecievers.Length; i++)
		{
			RGD_Reciever rGD_Reciever = rgdRecievers[i];
			foreach (Reciever reciever in recievers)
			{
				if (rGD_Reciever.recieverObjectIndex == reciever.ObjectIndex)
				{
					coroutineHolder.StartCoroutine(rGD_Reciever.RestoreLate(1f, reciever));
					break;
				}
			}
		}
	}
}
