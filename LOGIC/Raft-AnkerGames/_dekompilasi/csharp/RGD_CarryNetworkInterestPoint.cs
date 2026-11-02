using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_CarryNetworkInterestPoint : RGD_CarryNetwork
{
	public ObjectOfInterestType interestTypeParent;

	public RGD_CarryNetworkInterestPoint()
	{
	}

	public RGD_CarryNetworkInterestPoint(CarryNetworked_InterestPoint carried)
		: base(carried)
	{
		interestTypeParent = carried.currentInterestType;
	}

	public override void RestoreCarryNetworked(CarryNetworked item)
	{
		base.RestoreCarryNetworked(item);
		if (interestTypeParent != ObjectOfInterestType.None)
		{
			CarryNetworked_InterestPoint carryNetworked_InterestPoint = item as CarryNetworked_InterestPoint;
			if (carryNetworked_InterestPoint != null)
			{
				carryNetworked_InterestPoint.AttachToInterest(interestTypeParent);
			}
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("InterestType", interestTypeParent);
	}

	public RGD_CarryNetworkInterestPoint(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "InterestType")
			{
				interestTypeParent = Helper.ObjToValue<ObjectOfInterestType>(item.Value);
			}
		}
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in interestTypeParent, default(FastBufferWriter.ForEnums));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out interestTypeParent, default(FastBufferWriter.ForEnums));
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		interestTypeParent = ObjectOfInterestType.None;
	}
}
