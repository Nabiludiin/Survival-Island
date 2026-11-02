using System;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_ChickenEggs : Message_NetworkBehaviour
{
	public RGD_Chicken_Eggs rgdChickenEggs;

	protected bool Equals(Message_ChickenEggs other)
	{
		if (Equals((Message_NetworkBehaviour)other))
		{
			return object.Equals(rgdChickenEggs, other.rgdChickenEggs);
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
		return Equals((Message_ChickenEggs)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgdChickenEggs);
	}

	public static bool operator ==(Message_ChickenEggs left, Message_ChickenEggs right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_ChickenEggs left, Message_ChickenEggs right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		rgdChickenEggs.SerializeFast(writer);
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		rgdChickenEggs = new RGD_Chicken_Eggs();
		rgdChickenEggs.DeserializeFast(reader);
	}

	public Message_ChickenEggs()
	{
	}

	public Message_ChickenEggs(Messages type, MonoBehaviour_Network behaviour, PickupItem_Networked[] eggs)
		: base(type, behaviour)
	{
		rgdChickenEggs = new RGD_Chicken_Eggs(eggs);
	}

	public void RestoreEggs()
	{
		ObjectManager value = ComponentManager<ObjectManager>.Value;
		if (value == null)
		{
			Debug.LogError("Can't restore eggs because object manager is null");
		}
		else
		{
			if (!rgdChickenEggs.rgdEggs.ContainsItems())
			{
				return;
			}
			RGD_Egg[] rgdEggs = rgdChickenEggs.rgdEggs;
			foreach (RGD_Egg rGD_Egg in rgdEggs)
			{
				if (!(rGD_Egg == null))
				{
					value.Object_Chicken.LayEgg(rGD_Egg.localRaftPosition.Value, rGD_Egg.eggObjectIndex);
				}
			}
		}
	}
}
