using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_ChunkPoint : SerializableBehaviour
{
	public SerializableVector3 worldPosition;

	public ChunkPointType chunkPointType;

	public int spawnerIndex = -1;

	public int spawnableUniqueIndex = -1;

	public int spawnedObjectIndex = -1;

	public bool hasSpawnedObject;

	public bool isAddedByForce;

	public bool isCorrupt;

	public bool visible;

	protected bool Equals(RGD_ChunkPoint other)
	{
		if (object.Equals(worldPosition, other.worldPosition) && chunkPointType == other.chunkPointType && spawnerIndex == other.spawnerIndex && spawnableUniqueIndex == other.spawnableUniqueIndex && spawnedObjectIndex == other.spawnedObjectIndex && hasSpawnedObject == other.hasSpawnedObject && isAddedByForce == other.isAddedByForce && isCorrupt == other.isCorrupt)
		{
			return visible == other.visible;
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
		return Equals((RGD_ChunkPoint)obj);
	}

	public override int GetHashCode()
	{
		HashCode hashCode = default;
		hashCode.Add(worldPosition);
		hashCode.Add((int)chunkPointType);
		hashCode.Add(spawnerIndex);
		hashCode.Add(spawnableUniqueIndex);
		hashCode.Add(spawnedObjectIndex);
		hashCode.Add(hasSpawnedObject);
		hashCode.Add(isAddedByForce);
		hashCode.Add(isCorrupt);
		hashCode.Add(visible);
		return hashCode.ToHashCode();
	}

	public static bool operator ==(RGD_ChunkPoint left, RGD_ChunkPoint right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_ChunkPoint left, RGD_ChunkPoint right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(32))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in worldPosition.x, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in worldPosition.y, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in worldPosition.z, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in chunkPointType, default(FastBufferWriter.ForEnums));
		writer.WriteValue(in spawnerIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in spawnableUniqueIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in spawnedObjectIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in hasSpawnedObject, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in isAddedByForce, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in isCorrupt, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in visible, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		worldPosition = new SerializableVector3();
		reader.ReadValue(out worldPosition.x, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out worldPosition.y, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out worldPosition.z, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out chunkPointType, default(FastBufferWriter.ForEnums));
		reader.ReadValue(out spawnerIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out spawnableUniqueIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out spawnedObjectIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out hasSpawnedObject, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out isAddedByForce, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out isCorrupt, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out visible, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_ChunkPoint()
	{
	}

	public RGD_ChunkPoint(ChunkPoint point)
	{
		worldPosition = new SerializableVector3(point.worldPosition);
		chunkPointType = point.rule.ChunkPointType;
		isAddedByForce = point.IsAddedByForce;
		isCorrupt = point.IsCorrupt;
		hasSpawnedObject = point.HasSpawnedObject;
		spawnerIndex = ((point.spawner == null) ? (-1) : ((int)point.spawner.UniqueIndex));
		spawnedObjectIndex = (point.HasSpawnedObject ? ((int)point.spawnedObject.ObjectIndex) : (-1));
		spawnableUniqueIndex = ((point.spawnableFloatingObject == null) ? (-1) : ((int)point.spawnableFloatingObject.uniqueIndex));
		visible = point.HasSpawnedObject && point.spawnedObject.gameObject.activeSelf;
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("WorldPosition", worldPosition);
		info.AddValue("ChunkPointType", chunkPointType);
		info.AddValue("FloatingObjectIndex", spawnableUniqueIndex);
		info.AddValue("SpawnedObjectIndex", spawnedObjectIndex);
		info.AddValue("HasSpawnedObject", hasSpawnedObject);
		info.AddValue("IsAddedByForce", isAddedByForce);
		info.AddValue("SpawnerIndex", spawnerIndex);
		info.AddValue("IsCorrupt", isCorrupt);
		info.AddValue("Visible", visible);
	}

	public RGD_ChunkPoint(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "WorldPosition":
				worldPosition = (SerializableVector3)item.Value;
				break;
			case "ChunkPointType":
				chunkPointType = (ChunkPointType)item.Value;
				break;
			case "FloatingObjectIndex":
				spawnableUniqueIndex = Helper.ObjToValue<int>(item.Value);
				break;
			case "SpawnedObjectIndex":
				spawnedObjectIndex = Helper.ObjToValue<int>(item.Value);
				break;
			case "HasSpawnedObject":
				hasSpawnedObject = Helper.ObjToValue<bool>(item.Value);
				break;
			case "IsAddedByForce":
				isAddedByForce = Helper.ObjToValue<bool>(item.Value);
				break;
			case "SpawnerIndex":
				spawnerIndex = Helper.ObjToValue<int>(item.Value);
				break;
			case "IsCorrupt":
				isCorrupt = Helper.ObjToValue<bool>(item.Value);
				break;
			case "Visible":
				visible = Helper.ObjToValue<bool>(item.Value);
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		worldPosition = null;
		chunkPointType = ChunkPointType.None;
		spawnableUniqueIndex = -1;
		spawnerIndex = -1;
		spawnedObjectIndex = -1;
		hasSpawnedObject = false;
		isAddedByForce = false;
		isCorrupt = false;
		visible = false;
	}
}
