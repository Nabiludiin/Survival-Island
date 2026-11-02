using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_LandmarkSpawner : SerializableBehaviour
{
	public RGD_AI_NetworkBehaviour spawnedEntityData;

	public uint spawnerIndex;

	protected bool Equals(RGD_LandmarkSpawner other)
	{
		if (object.Equals(spawnedEntityData, other.spawnedEntityData))
		{
			return spawnerIndex == other.spawnerIndex;
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
		return Equals((RGD_LandmarkSpawner)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(spawnedEntityData, spawnerIndex);
	}

	public static bool operator ==(RGD_LandmarkSpawner left, RGD_LandmarkSpawner right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_LandmarkSpawner left, RGD_LandmarkSpawner right)
	{
		return !object.Equals(left, right);
	}

	public RGD_LandmarkSpawner()
	{
	}

	public RGD_LandmarkSpawner(LandmarkEntitySpawner spawner)
	{
		spawnerIndex = spawner.spawnerIndex;
		if (spawner.spawnedEntityBehaviour != null)
		{
			spawnedEntityData = spawner.spawnedEntityBehaviour.Create_SaveData();
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("SpawnerIndex", spawnerIndex);
		info.AddValue("SpawnedEntityData", spawnedEntityData);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (spawnedEntityData != null)
		{
			RGD_AI_NetworkBehaviour rGD_AI_NetworkBehaviour = spawnedEntityData;
			if (!(rGD_AI_NetworkBehaviour is RGD_Boar))
			{
				if (!(rGD_AI_NetworkBehaviour is RGD_Stonebird))
				{
					if (!(rGD_AI_NetworkBehaviour is RGD_Domestic_Chicken))
					{
						if (!(rGD_AI_NetworkBehaviour is RGD_Domestic_Resource))
						{
							if (rGD_AI_NetworkBehaviour is RGD_Domestic)
							{
								writer.WriteValueSafe<AiType>(AiType.Domestic, default(FastBufferWriter.ForEnums));
							}
							else
							{
								writer.WriteValueSafe<AiType>(AiType.Nomal, default(FastBufferWriter.ForEnums));
							}
						}
						else
						{
							writer.WriteValueSafe<AiType>(AiType.Resource, default(FastBufferWriter.ForEnums));
						}
					}
					else
					{
						writer.WriteValueSafe<AiType>(AiType.Chicken, default(FastBufferWriter.ForEnums));
					}
				}
				else
				{
					writer.WriteValueSafe<AiType>(AiType.Stonebird, default(FastBufferWriter.ForEnums));
				}
			}
			else
			{
				writer.WriteValueSafe<AiType>(AiType.Boar, default(FastBufferWriter.ForEnums));
			}
			spawnedEntityData.SerializeFast(writer);
		}
		else
		{
			writer.WriteValueSafe<AiType>(AiType.None, default(FastBufferWriter.ForEnums));
		}
		writer.WriteValueSafe(in spawnerIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out AiType value, default(FastBufferWriter.ForEnums));
		switch (value)
		{
		case AiType.Boar:
			spawnedEntityData = new RGD_Boar();
			goto IL_0094;
		case AiType.Domestic:
			spawnedEntityData = new RGD_Domestic();
			goto IL_0094;
		case AiType.Stonebird:
			spawnedEntityData = new RGD_Stonebird();
			goto IL_0094;
		case AiType.Chicken:
			spawnedEntityData = new RGD_Domestic_Chicken();
			goto IL_0094;
		case AiType.Resource:
			spawnedEntityData = new RGD_Domestic_Resource();
			goto IL_0094;
		case AiType.Nomal:
			spawnedEntityData = new RGD_AI_NetworkBehaviour();
			goto IL_0094;
		default:
			throw new ArgumentOutOfRangeException();
		case AiType.None:
			break;
			IL_0094:
			spawnedEntityData.DeserializeFast(reader);
			break;
		}
		reader.ReadValue(out spawnerIndex, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_LandmarkSpawner(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			string name = item.Name;
			if (!(name == "SpawnerIndex"))
			{
				if (name == "SpawnedEntityData")
				{
					spawnedEntityData = item.Value as RGD_AI_NetworkBehaviour;
				}
			}
			else
			{
				spawnerIndex = Helper.ObjToValue<uint>(item.Value);
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		spawnerIndex = 0u;
		spawnedEntityData = null;
	}
}
