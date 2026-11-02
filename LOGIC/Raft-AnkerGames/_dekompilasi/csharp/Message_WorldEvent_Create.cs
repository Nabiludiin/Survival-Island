using System;
using System.Linq;
using Unity.Netcode;

[Serializable]
public class Message_WorldEvent_Create : Message_NetworkBehaviour
{
	public RGD_WorldEvent[] rgdWorldEvents;

	protected bool Equals(Message_WorldEvent_Create other)
	{
		if (rgdWorldEvents == null && other.rgdWorldEvents != null)
		{
			return false;
		}
		if (rgdWorldEvents != null && other.rgdWorldEvents == null)
		{
			return false;
		}
		if (Equals((Message_NetworkBehaviour)other))
		{
			if (rgdWorldEvents != null || other.rgdWorldEvents != null)
			{
				return Enumerable.SequenceEqual(rgdWorldEvents, other.rgdWorldEvents);
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
		return Equals((Message_WorldEvent_Create)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgdWorldEvents);
	}

	public static bool operator ==(Message_WorldEvent_Create left, Message_WorldEvent_Create right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_WorldEvent_Create left, Message_WorldEvent_Create right)
	{
		return !object.Equals(left, right);
	}

	public Message_WorldEvent_Create()
	{
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<int>((rgdWorldEvents != null) ? rgdWorldEvents.Length : 0, default(FastBufferWriter.ForPrimitives));
		if (rgdWorldEvents != null)
		{
			RGD_WorldEvent[] array = rgdWorldEvents;
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
			rgdWorldEvents = new RGD_WorldEvent[value];
			for (int i = 0; i < value; i++)
			{
				rgdWorldEvents[i] = new RGD_WorldEvent();
				rgdWorldEvents[i].DeserializeFast(reader);
			}
		}
	}

	public Message_WorldEvent_Create(Messages type, MonoBehaviour_Network behaviour)
		: base(type, behaviour)
	{
		rgdWorldEvents = WorldEventManager.GetSaveDataForEvents();
	}

	public void RestoreWorldEvents()
	{
		if (rgdWorldEvents == null || rgdWorldEvents.Length == 0)
		{
			return;
		}
		for (int i = 0; i < rgdWorldEvents.Length; i++)
		{
			RGD_WorldEvent rGD_WorldEvent = rgdWorldEvents[i];
			if (!(rGD_WorldEvent == null))
			{
				WorldEventManager.GetWorldEventFromType(rGD_WorldEvent.eventType)?.RestoreWorldEvent(rGD_WorldEvent);
			}
		}
	}
}
