using System;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_CreateAINetworkBehaviour : Message_NetworkBehaviour_ID
{
	public AI_NetworkBehaviourType behaviourType;

	public SerializableVector3 Position;

	public uint behaviourObjectIndex;

	public uint behaviourEntityObjectIndex;

	public float entityHealth;

	public int uniqueSpawnerIndex;

	public int uniqueLandmarkIndex;

	public float scaledSize;

	protected bool Equals(Message_CreateAINetworkBehaviour other)
	{
		if (Equals((Message_NetworkBehaviour_ID)other) && behaviourType == other.behaviourType && object.Equals(Position, other.Position) && behaviourObjectIndex == other.behaviourObjectIndex && behaviourEntityObjectIndex == other.behaviourEntityObjectIndex && entityHealth.Equals(other.entityHealth) && uniqueSpawnerIndex == other.uniqueSpawnerIndex && uniqueLandmarkIndex == other.uniqueLandmarkIndex)
		{
			return scaledSize.Equals(other.scaledSize);
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
		return Equals((Message_CreateAINetworkBehaviour)obj);
	}

	public override int GetHashCode()
	{
		HashCode hashCode = default;
		hashCode.Add(base.GetHashCode());
		hashCode.Add((int)behaviourType);
		hashCode.Add(Position);
		hashCode.Add(behaviourObjectIndex);
		hashCode.Add(behaviourEntityObjectIndex);
		hashCode.Add(entityHealth);
		hashCode.Add(uniqueSpawnerIndex);
		hashCode.Add(uniqueLandmarkIndex);
		hashCode.Add(scaledSize);
		return hashCode.ToHashCode();
	}

	public static bool operator ==(Message_CreateAINetworkBehaviour left, Message_CreateAINetworkBehaviour right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_CreateAINetworkBehaviour left, Message_CreateAINetworkBehaviour right)
	{
		return !object.Equals(left, right);
	}

	public Message_CreateAINetworkBehaviour()
	{
	}

	public Message_CreateAINetworkBehaviour(Messages type, MonoBehaviour_Network behaviour, uint networkID_ObjectIndex, Vector3 position, AI_NetworkBehaviour networkBehaviour, LandmarkEntitySpawner spawner = null)
		: base(type, behaviour, networkID_ObjectIndex)
	{
		Position = new SerializableVector3(position);
		behaviourType = networkBehaviour.behaviourType;
		behaviourObjectIndex = networkBehaviour.ObjectIndex;
		behaviourEntityObjectIndex = networkBehaviour.networkEntity.ObjectIndex;
		entityHealth = networkBehaviour.networkEntity.stat_health.Value;
		scaledSize = networkBehaviour.scaledSize;
		if ((bool)spawner)
		{
			uniqueSpawnerIndex = (int)spawner.spawnerIndex;
			uniqueLandmarkIndex = (int)spawner.landmark.uniqueLandmarkIndex;
		}
		else
		{
			uniqueSpawnerIndex = (uniqueLandmarkIndex = -1);
		}
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(40))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in behaviourType, default(FastBufferWriter.ForEnums));
		writer.WriteValue(in Position.x, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in Position.y, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in Position.z, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in behaviourObjectIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in behaviourEntityObjectIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in entityHealth, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in uniqueSpawnerIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in uniqueLandmarkIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in scaledSize, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out behaviourType, default(FastBufferWriter.ForEnums));
		Position = new SerializableVector3();
		reader.ReadValue(out Position.x, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out Position.y, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out Position.z, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out behaviourObjectIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out behaviourEntityObjectIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out entityHealth, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out uniqueSpawnerIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out uniqueLandmarkIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out scaledSize, default(FastBufferWriter.ForPrimitives));
	}
}
