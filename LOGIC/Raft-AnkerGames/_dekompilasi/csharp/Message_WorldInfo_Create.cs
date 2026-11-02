using System;
using Unity.Netcode;
using UnityEngine.AzureSky;

[Serializable]
public class Message_WorldInfo_Create : Message_NetworkBehaviour
{
	public float waterXpos;

	public float waterZpos;

	public float waterTime;

	public float timeOfDay;

	public int shiftCount;

	public int dayCounter;

	public float timePlayedSeconds;

	public RGD_ResearchTableWorld rgdResearchInfo;

	public RGD_RecieverFrequencies rgdRecieverFrequencies;

	protected bool Equals(Message_WorldInfo_Create other)
	{
		if (Equals((Message_NetworkBehaviour)other) && waterXpos.Equals(other.waterXpos) && waterZpos.Equals(other.waterZpos) && waterTime.Equals(other.waterTime) && timeOfDay.Equals(other.timeOfDay) && shiftCount == other.shiftCount && dayCounter == other.dayCounter && timePlayedSeconds.Equals(other.timePlayedSeconds) && object.Equals(rgdResearchInfo, other.rgdResearchInfo))
		{
			return object.Equals(rgdRecieverFrequencies, other.rgdRecieverFrequencies);
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
		return Equals((Message_WorldInfo_Create)obj);
	}

	public override int GetHashCode()
	{
		HashCode hashCode = default;
		hashCode.Add(base.GetHashCode());
		hashCode.Add(waterXpos);
		hashCode.Add(waterZpos);
		hashCode.Add(waterTime);
		hashCode.Add(timeOfDay);
		hashCode.Add(shiftCount);
		hashCode.Add(dayCounter);
		hashCode.Add(timePlayedSeconds);
		hashCode.Add(rgdResearchInfo);
		hashCode.Add(rgdRecieverFrequencies);
		return hashCode.ToHashCode();
	}

	public static bool operator ==(Message_WorldInfo_Create left, Message_WorldInfo_Create right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_WorldInfo_Create left, Message_WorldInfo_Create right)
	{
		return !object.Equals(left, right);
	}

	public Message_WorldInfo_Create()
	{
	}

	public Message_WorldInfo_Create(Messages type, MonoBehaviour_Network behaviour, Network_Water water, AzureSkyController skyController, int shiftCount)
		: base(type, behaviour)
	{
		waterXpos = water.transform.position.x;
		waterZpos = water.transform.position.z;
		waterTime = water.WaterTime;
		timeOfDay = skyController.timeOfDay.hour;
		this.shiftCount = shiftCount;
		dayCounter = WorldManager.DayCounter;
		timePlayedSeconds = GameManager.TimePlayed;
		rgdResearchInfo = new RGD_ResearchTableWorld(ComponentManager<Inventory_ResearchTable>.Value);
		rgdRecieverFrequencies = new RGD_RecieverFrequencies();
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(28))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in waterXpos, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in waterZpos, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in waterTime, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in timeOfDay, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in shiftCount, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in dayCounter, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in timePlayedSeconds, default(FastBufferWriter.ForPrimitives));
		rgdResearchInfo.SerializeFast(writer);
		rgdRecieverFrequencies.SerializeFast(writer);
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out waterXpos, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out waterZpos, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out waterTime, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out timeOfDay, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out shiftCount, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out dayCounter, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out timePlayedSeconds, default(FastBufferWriter.ForPrimitives));
		rgdResearchInfo = new RGD_ResearchTableWorld();
		rgdResearchInfo.DeserializeFast(reader);
		rgdRecieverFrequencies = new RGD_RecieverFrequencies();
		rgdRecieverFrequencies.DeserializeFast(reader);
	}
}
