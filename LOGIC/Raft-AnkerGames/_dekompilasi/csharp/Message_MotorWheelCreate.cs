using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;

[Serializable]
public class Message_MotorWheelCreate : Message_NetworkBehaviour
{
	public RGD_MotorWheel[] rgdMotors;

	protected bool Equals(Message_MotorWheelCreate other)
	{
		if (rgdMotors == null && other.rgdMotors != null)
		{
			return false;
		}
		if (rgdMotors != null && other.rgdMotors == null)
		{
			return false;
		}
		if (Equals((Message_NetworkBehaviour)other))
		{
			if (rgdMotors != null || other.rgdMotors != null)
			{
				return Enumerable.SequenceEqual(rgdMotors, other.rgdMotors);
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
		return Equals((Message_MotorWheelCreate)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgdMotors);
	}

	public static bool operator ==(Message_MotorWheelCreate left, Message_MotorWheelCreate right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_MotorWheelCreate left, Message_MotorWheelCreate right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<int>((rgdMotors != null) ? rgdMotors.Length : 0, default(FastBufferWriter.ForPrimitives));
		if (rgdMotors != null)
		{
			RGD_MotorWheel[] array = rgdMotors;
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
			rgdMotors = new RGD_MotorWheel[value];
			for (int i = 0; i < value; i++)
			{
				rgdMotors[i] = new RGD_MotorWheel();
				rgdMotors[i].DeserializeFast(reader);
			}
		}
	}

	public Message_MotorWheelCreate()
	{
	}

	public Message_MotorWheelCreate(Messages type, MonoBehaviour_Network behaviour, MotorWheel[] motors)
		: base(type, behaviour)
	{
		if (motors == null)
		{
			return;
		}
		List<RGD_MotorWheel> list = new List<RGD_MotorWheel>();
		for (int i = 0; i < motors.Length; i++)
		{
			if (motors[i] != null)
			{
				RGD_MotorWheel item = new RGD_MotorWheel(RGDType.Block_Motor_Wheel, motors[i]);
				list.Add(item);
			}
		}
		rgdMotors = list.ToArray();
	}

	public void RestoreMotorWheels(MotorWheel[] motorWheels)
	{
		if (motorWheels == null || !motorWheels.ContainsItems() || rgdMotors == null || !rgdMotors.ContainsItems())
		{
			return;
		}
		RGD_MotorWheel[] array = rgdMotors;
		foreach (RGD_MotorWheel rGD_MotorWheel in array)
		{
			if (!(rGD_MotorWheel == null))
			{
				GetMotorWheelFromObjectIndex(motorWheels, rGD_MotorWheel.BlockObjectIndex).RestoreMotor(rGD_MotorWheel);
			}
		}
	}

	private MotorWheel GetMotorWheelFromObjectIndex(MotorWheel[] motorWheels, uint objectIndex)
	{
		for (int i = 0; i < motorWheels.Length; i++)
		{
			if (motorWheels[i].ObjectIndex == objectIndex)
			{
				return motorWheels[i];
			}
		}
		return null;
	}
}
