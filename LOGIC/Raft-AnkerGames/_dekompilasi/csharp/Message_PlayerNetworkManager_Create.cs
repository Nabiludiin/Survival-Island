using System;
using System.Linq;
using Unity.Netcode;

[Serializable]
public class Message_PlayerNetworkManager_Create : Message_NetworkBehaviour
{
	public RGD_Brick[] rgdBricks;

	protected bool Equals(Message_PlayerNetworkManager_Create other)
	{
		if (rgdBricks == null && other.rgdBricks != null)
		{
			return false;
		}
		if (rgdBricks != null && other.rgdBricks == null)
		{
			return false;
		}
		if (Equals((Message_NetworkBehaviour)other))
		{
			if (rgdBricks != null || other.rgdBricks != null)
			{
				return Enumerable.SequenceEqual(rgdBricks, other.rgdBricks);
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
		return Equals((Message_PlayerNetworkManager_Create)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgdBricks);
	}

	public static bool operator ==(Message_PlayerNetworkManager_Create left, Message_PlayerNetworkManager_Create right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_PlayerNetworkManager_Create left, Message_PlayerNetworkManager_Create right)
	{
		return !object.Equals(left, right);
	}

	public Message_PlayerNetworkManager_Create()
	{
	}

	public Message_PlayerNetworkManager_Create(Messages type, MonoBehaviour_Network behaviour)
		: base(type, behaviour)
	{
		AddBricks();
	}

	public void Restore(PlayerNetworkManager manager, Message_PlayerNetworkManager_Create msgCreate)
	{
		RestoreBricks(manager, msgCreate);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<int>(rgdBricks.Length, default(FastBufferWriter.ForPrimitives));
		RGD_Brick[] array = rgdBricks;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SerializeFast(writer);
		}
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		int value = 0;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		rgdBricks = new RGD_Brick[value];
		for (int i = 0; i < value; i++)
		{
			rgdBricks[i] = new RGD_Brick();
			rgdBricks[i].DeserializeFast(reader);
		}
	}

	public void AddBricks()
	{
		rgdBricks = new RGD_Brick[PlayerNetworkManager.allBricks.Count];
		for (int i = 0; i < PlayerNetworkManager.allBricks.Count; i++)
		{
			rgdBricks[i] = new RGD_Brick(RGDType.Block_Brick, PlayerNetworkManager.allBricks[i]);
		}
	}

	public void RestoreBricks(PlayerNetworkManager manager, Message_PlayerNetworkManager_Create msg)
	{
		RGD_Brick[] array = rgdBricks;
		foreach (RGD_Brick rGD_Brick in array)
		{
			rGD_Brick.RestoreBrick(manager.GetBrickByObjectIndex(rGD_Brick.brickObjectIndex));
		}
	}
}
