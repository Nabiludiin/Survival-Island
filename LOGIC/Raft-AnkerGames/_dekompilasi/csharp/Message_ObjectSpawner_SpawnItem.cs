using System;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_ObjectSpawner_SpawnItem : Message_NetworkBehaviour
{
	public uint uniqueSpawnerIndex;

	public uint spawnedObjectIndex;

	public uint spawnedBehaviourIndex;

	public uint uniqueSpawnableIndex;

	public int poolDuplicateIndex;

	[SerializeField]
	private float xRot;

	[SerializeField]
	private float yRot;

	[SerializeField]
	private float zRot;

	[SerializeField]
	private float xPos;

	[SerializeField]
	private float yPos;

	[SerializeField]
	private float zPos;

	public Vector3 SpawnRotation
	{
		get
		{
			return new Vector3(xRot, yRot, zRot);
		}
		set
		{
			xRot = value.x;
			yRot = value.y;
			zRot = value.z;
		}
	}

	public Vector3 SpawnPosition
	{
		get
		{
			return new Vector3(xPos, yPos, zPos);
		}
		set
		{
			xPos = value.x;
			yPos = value.y;
			zPos = value.z;
		}
	}

	protected bool Equals(Message_ObjectSpawner_SpawnItem other)
	{
		if (Equals((Message_NetworkBehaviour)other) && uniqueSpawnerIndex == other.uniqueSpawnerIndex && spawnedObjectIndex == other.spawnedObjectIndex && spawnedBehaviourIndex == other.spawnedBehaviourIndex && uniqueSpawnableIndex == other.uniqueSpawnableIndex && poolDuplicateIndex == other.poolDuplicateIndex && xRot.Equals(other.xRot) && yRot.Equals(other.yRot) && zRot.Equals(other.zRot) && xPos.Equals(other.xPos) && yPos.Equals(other.yPos))
		{
			return zPos.Equals(other.zPos);
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
		return Equals((Message_ObjectSpawner_SpawnItem)obj);
	}

	public override int GetHashCode()
	{
		HashCode hashCode = default;
		hashCode.Add(base.GetHashCode());
		hashCode.Add(uniqueSpawnerIndex);
		hashCode.Add(spawnedObjectIndex);
		hashCode.Add(spawnedBehaviourIndex);
		hashCode.Add(uniqueSpawnableIndex);
		hashCode.Add(poolDuplicateIndex);
		hashCode.Add(xRot);
		hashCode.Add(yRot);
		hashCode.Add(zRot);
		hashCode.Add(xPos);
		hashCode.Add(yPos);
		hashCode.Add(zPos);
		return hashCode.ToHashCode();
	}

	public static bool operator ==(Message_ObjectSpawner_SpawnItem left, Message_ObjectSpawner_SpawnItem right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_ObjectSpawner_SpawnItem left, Message_ObjectSpawner_SpawnItem right)
	{
		return !object.Equals(left, right);
	}

	public Message_ObjectSpawner_SpawnItem(Messages type, MonoBehaviour_Network behaviour, Vector3 spawnPosition, Vector3 spawnEuler, uint uniqueSpawnerIndex, uint spawnedObjectIndex, uint spawnedBehaviourIndex, uint uniqueSpawnableIndex, int poolDuplicateIndex)
		: base(type, behaviour)
	{
		SpawnPosition = spawnPosition;
		SpawnRotation = spawnEuler;
		this.uniqueSpawnerIndex = uniqueSpawnerIndex;
		this.uniqueSpawnableIndex = uniqueSpawnableIndex;
		this.spawnedObjectIndex = spawnedObjectIndex;
		this.spawnedBehaviourIndex = spawnedBehaviourIndex;
		this.poolDuplicateIndex = poolDuplicateIndex;
	}

	public Message_ObjectSpawner_SpawnItem()
	{
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(44))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in uniqueSpawnerIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in uniqueSpawnableIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in spawnedObjectIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in spawnedBehaviourIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in poolDuplicateIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in xRot, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in yRot, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in zRot, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in xPos, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in yPos, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in zPos, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out uniqueSpawnerIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out uniqueSpawnableIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out spawnedObjectIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out spawnedBehaviourIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out poolDuplicateIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out xRot, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out yRot, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out zRot, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out xPos, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out yPos, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out zPos, default(FastBufferWriter.ForPrimitives));
	}
}
