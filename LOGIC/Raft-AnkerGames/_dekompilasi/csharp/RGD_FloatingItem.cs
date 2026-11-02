using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_FloatingItem : SerializableBehaviour
{
	public SerializableVector3 Position;

	public SerializableVector3 Rotation;

	public int spawnerIndex;

	public uint behaviourIndex;

	public uint uniqueSpawnableIndex;

	public uint floatingObjectIndex;

	public int poolDuplicateIndex;

	protected bool Equals(RGD_FloatingItem other)
	{
		if (object.Equals(Position, other.Position) && object.Equals(Rotation, other.Rotation) && spawnerIndex == other.spawnerIndex && behaviourIndex == other.behaviourIndex && uniqueSpawnableIndex == other.uniqueSpawnableIndex && floatingObjectIndex == other.floatingObjectIndex)
		{
			return poolDuplicateIndex == other.poolDuplicateIndex;
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
		return Equals((RGD_FloatingItem)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Position, Rotation, spawnerIndex, behaviourIndex, uniqueSpawnableIndex, floatingObjectIndex, poolDuplicateIndex);
	}

	public static bool operator ==(RGD_FloatingItem left, RGD_FloatingItem right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_FloatingItem left, RGD_FloatingItem right)
	{
		return !object.Equals(left, right);
	}

	public RGD_FloatingItem()
	{
	}

	public RGD_FloatingItem(PickupItem_Networked pickupNetworked)
	{
		if (pickupNetworked.spawner != null)
		{
			spawnerIndex = (int)pickupNetworked.spawner.UniqueIndex;
		}
		else
		{
			spawnerIndex = -1;
		}
		uniqueSpawnableIndex = pickupNetworked.uniqueSpawnableIndex;
		floatingObjectIndex = pickupNetworked.ObjectIndex;
		poolDuplicateIndex = pickupNetworked.poolDuplicateIndex;
		Position = new SerializableVector3(pickupNetworked.transform.position);
		Rotation = new SerializableVector3(pickupNetworked.transform.eulerAngles);
		if (pickupNetworked.networkBehaviour != null)
		{
			behaviourIndex = pickupNetworked.networkBehaviour.BehaviourIndex;
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("Position", Position);
		info.AddValue("Rotation", Rotation);
		info.AddValue("SpawnerIndex", spawnerIndex);
		info.AddValue("SpawnableIndex", uniqueSpawnableIndex);
		info.AddValue("FloatingObjectIndex", floatingObjectIndex);
		info.AddValue("PoolDuplicateIndex", poolDuplicateIndex);
		info.AddValue("BehaviourIndex", behaviourIndex);
	}

	public RGD_FloatingItem(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "Position":
				Position = (SerializableVector3)item.Value;
				break;
			case "Rotation":
				Rotation = (SerializableVector3)item.Value;
				break;
			case "SpawnerIndex":
				spawnerIndex = DeserializeSpawnerIndex(item.Value);
				break;
			case "SpawnableIndex":
				uniqueSpawnableIndex = (uint)item.Value;
				break;
			case "FloatingObjectIndex":
				floatingObjectIndex = (uint)item.Value;
				break;
			case "PoolDuplicateIndex":
				poolDuplicateIndex = (int)item.Value;
				break;
			case "BehaviourIndex":
				behaviourIndex = (uint)item.Value;
				break;
			}
		}
	}

	private int DeserializeSpawnerIndex(object e)
	{
		if (e.GetType() == typeof(uint))
		{
			return (int)Helper.ObjToValue<uint>(e);
		}
		return Helper.ObjToValue<int>(e);
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		Position = new SerializableVector3();
		Rotation = new SerializableVector3();
		spawnerIndex = -1;
		uniqueSpawnableIndex = 0u;
		floatingObjectIndex = 0u;
		poolDuplicateIndex = 0;
		behaviourIndex = 0u;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(44))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in Position.x, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in Position.y, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in Position.z, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in Rotation.x, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in Rotation.y, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in Rotation.z, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in spawnerIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in behaviourIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in uniqueSpawnableIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in floatingObjectIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in poolDuplicateIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		Position = new SerializableVector3();
		Rotation = new SerializableVector3();
		reader.ReadValue(out Position.x, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out Position.y, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out Position.z, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out Rotation.x, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out Rotation.y, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out Rotation.z, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out spawnerIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out behaviourIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out uniqueSpawnableIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out floatingObjectIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out poolDuplicateIndex, default(FastBufferWriter.ForPrimitives));
	}
}
