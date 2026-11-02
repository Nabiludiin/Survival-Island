using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_RadioPlayer : SerializableBehaviour
{
	public bool onState;

	public int cassetteIndex;

	public int timelinePosition;

	public uint objectIndex;

	protected bool Equals(RGD_RadioPlayer other)
	{
		if (onState == other.onState && cassetteIndex == other.cassetteIndex && timelinePosition == other.timelinePosition)
		{
			return objectIndex == other.objectIndex;
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
		return Equals((RGD_RadioPlayer)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(onState, cassetteIndex, timelinePosition, objectIndex);
	}

	public static bool operator ==(RGD_RadioPlayer left, RGD_RadioPlayer right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_RadioPlayer left, RGD_RadioPlayer right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(13))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in onState, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in cassetteIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in timelinePosition, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in objectIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out onState, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out cassetteIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out timelinePosition, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out objectIndex, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_RadioPlayer()
	{
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("OnState", onState);
		info.AddValue("ObjectIndex", objectIndex);
		info.AddValue("CassetteIndex", cassetteIndex);
		info.AddValue("TimelinePosition", timelinePosition);
	}

	public RGD_RadioPlayer(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "OnState":
				onState = (bool)item.Value;
				break;
			case "ObjectIndex":
				objectIndex = (uint)item.Value;
				break;
			case "CassetteIndex":
				cassetteIndex = (int)item.Value;
				break;
			case "TimelinePosition":
				timelinePosition = (int)item.Value;
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		onState = false;
		cassetteIndex = 0;
		timelinePosition = 0;
	}
}
