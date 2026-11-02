using System;
using Unity.Netcode;

[Serializable]
public class Message_HarvestPlant : Message_NetworkBehaviour
{
	public bool giveYield;

	public uint plantObjectIndex;

	public Message_HarvestPlant()
	{
	}

	public Message_HarvestPlant(Messages type, MonoBehaviour_Network behaviour, Plant plant, bool giveYield)
		: base(type, behaviour)
	{
		plantObjectIndex = plant.pickupComponent.networkID.ObjectIndex;
		this.giveYield = giveYield;
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in giveYield, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in plantObjectIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out giveYield, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out plantObjectIndex, default(FastBufferWriter.ForPrimitives));
	}
}
