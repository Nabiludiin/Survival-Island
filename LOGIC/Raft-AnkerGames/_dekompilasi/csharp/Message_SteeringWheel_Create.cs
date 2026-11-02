using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;

[Serializable]
public class Message_SteeringWheel_Create : Message_NetworkBehaviour
{
	public RGD_SteeringWheel[] rgdSteeringWheels;

	protected bool Equals(Message_SteeringWheel_Create other)
	{
		if (rgdSteeringWheels == null && other.rgdSteeringWheels != null)
		{
			return false;
		}
		if (rgdSteeringWheels != null && other.rgdSteeringWheels == null)
		{
			return false;
		}
		if (Equals((Message_NetworkBehaviour)other))
		{
			if (rgdSteeringWheels != null || other.rgdSteeringWheels != null)
			{
				return Enumerable.SequenceEqual(rgdSteeringWheels, other.rgdSteeringWheels);
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
		return Equals((Message_SteeringWheel_Create)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgdSteeringWheels);
	}

	public static bool operator ==(Message_SteeringWheel_Create left, Message_SteeringWheel_Create right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_SteeringWheel_Create left, Message_SteeringWheel_Create right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<int>((rgdSteeringWheels != null) ? rgdSteeringWheels.Length : 0, default(FastBufferWriter.ForPrimitives));
		if (rgdSteeringWheels != null)
		{
			RGD_SteeringWheel[] array = rgdSteeringWheels;
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
			rgdSteeringWheels = new RGD_SteeringWheel[value];
			for (int i = 0; i < value; i++)
			{
				rgdSteeringWheels[i] = new RGD_SteeringWheel();
				rgdSteeringWheels[i].DeserializeFast(reader);
			}
		}
	}

	public Message_SteeringWheel_Create()
	{
	}

	public Message_SteeringWheel_Create(Messages type, MonoBehaviour_Network behaviour, SteeringWheel[] steeringWheels)
		: base(type, behaviour)
	{
		if (steeringWheels == null)
		{
			return;
		}
		List<RGD_SteeringWheel> list = new List<RGD_SteeringWheel>();
		foreach (SteeringWheel steeringWheel in steeringWheels)
		{
			if (steeringWheel != null)
			{
				list.Add(new RGD_SteeringWheel(RGDType.Block_SteeringWheel, steeringWheel));
			}
		}
		rgdSteeringWheels = list.ToArray();
	}

	public void RestoreSteeringWheels(SteeringWheel[] steeringWheels)
	{
		if (steeringWheels == null || !steeringWheels.ContainsItems() || rgdSteeringWheels == null || !rgdSteeringWheels.ContainsItems())
		{
			return;
		}
		RGD_SteeringWheel[] array = rgdSteeringWheels;
		foreach (RGD_SteeringWheel rGD_SteeringWheel in array)
		{
			if (!(rGD_SteeringWheel == null))
			{
				GetSteeringWheelFromObjectIndex(steeringWheels, rGD_SteeringWheel.BlockObjectIndex).RestoreWheel(rGD_SteeringWheel);
			}
		}
	}

	private SteeringWheel GetSteeringWheelFromObjectIndex(SteeringWheel[] steeringWheels, uint objectIndex)
	{
		for (int i = 0; i < steeringWheels.Length; i++)
		{
			if (steeringWheels[i].ObjectIndex == objectIndex)
			{
				return steeringWheels[i];
			}
		}
		return null;
	}
}
