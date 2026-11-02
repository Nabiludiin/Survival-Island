using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;

[Serializable]
public class Message_Sprinkler_Create : Message_NetworkBehaviour
{
	public RGD_Sprinkler[] rgdSprinklers;

	protected bool Equals(Message_Sprinkler_Create other)
	{
		if (rgdSprinklers == null && other.rgdSprinklers != null)
		{
			return false;
		}
		if (rgdSprinklers != null && other.rgdSprinklers == null)
		{
			return false;
		}
		if (Equals((Message_NetworkBehaviour)other))
		{
			if (rgdSprinklers != null || other.rgdSprinklers != null)
			{
				return Enumerable.SequenceEqual(rgdSprinklers, other.rgdSprinklers);
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
		return Equals((Message_Sprinkler_Create)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgdSprinklers);
	}

	public static bool operator ==(Message_Sprinkler_Create left, Message_Sprinkler_Create right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Sprinkler_Create left, Message_Sprinkler_Create right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<int>((rgdSprinklers != null) ? rgdSprinklers.Length : 0, default(FastBufferWriter.ForPrimitives));
		if (rgdSprinklers != null)
		{
			RGD_Sprinkler[] array = rgdSprinklers;
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
			rgdSprinklers = new RGD_Sprinkler[value];
			for (int i = 0; i < value; i++)
			{
				rgdSprinklers[i] = new RGD_Sprinkler();
				rgdSprinklers[i].DeserializeFast(reader);
			}
		}
	}

	public Message_Sprinkler_Create()
	{
	}

	public Message_Sprinkler_Create(Messages type, MonoBehaviour_Network behaviour, Sprinkler[] sprinklers)
		: base(type, behaviour)
	{
		if (sprinklers == null)
		{
			return;
		}
		List<RGD_Sprinkler> list = new List<RGD_Sprinkler>();
		for (int i = 0; i < sprinklers.Length; i++)
		{
			Sprinkler sprinkler = sprinklers[i];
			if (sprinkler != null && (sprinkler.waterTank.CurrentTankAmount > 0f || !sprinkler.battery.BatterySlotIsEmpty))
			{
				list.Add(new RGD_Sprinkler(sprinklers[i]));
			}
		}
		rgdSprinklers = list.ToArray();
	}

	public void RestoreSprinklers(Sprinkler[] sprinklers)
	{
		if (sprinklers == null || !sprinklers.ContainsItems() || rgdSprinklers == null || !rgdSprinklers.ContainsItems())
		{
			return;
		}
		for (int i = 0; i < rgdSprinklers.Length; i++)
		{
			RGD_Sprinkler rGD_Sprinkler = rgdSprinklers[i];
			foreach (Sprinkler sprinkler in sprinklers)
			{
				if (rGD_Sprinkler.sprinklerObjectIndex == sprinkler.ObjectIndex)
				{
					rGD_Sprinkler.RestoreSprinkler(sprinkler);
					break;
				}
			}
		}
	}
}
