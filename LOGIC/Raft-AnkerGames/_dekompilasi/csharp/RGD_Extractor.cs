using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_Extractor : SerializableBehaviour
{
	public uint objectIndex;

	public RGD_Tank processTank;

	public RGD_Tank outputTank;

	protected bool Equals(RGD_Extractor other)
	{
		if (objectIndex == other.objectIndex && object.Equals(processTank, other.processTank))
		{
			return object.Equals(outputTank, other.outputTank);
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
		return Equals((RGD_Extractor)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(objectIndex, processTank, outputTank);
	}

	public static bool operator ==(RGD_Extractor left, RGD_Extractor right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Extractor left, RGD_Extractor right)
	{
		return !object.Equals(left, right);
	}

	public RGD_Extractor()
	{
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in objectIndex, default(FastBufferWriter.ForPrimitives));
		processTank.SerializeFast(writer);
		outputTank.SerializeFast(writer);
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out objectIndex, default(FastBufferWriter.ForPrimitives));
		processTank = new RGD_Tank();
		processTank.DeserializeFast(reader);
		outputTank = new RGD_Tank();
		outputTank.DeserializeFast(reader);
	}

	public RGD_Extractor(Placeable_Extractor extractor)
	{
		processTank = new RGD_Tank(extractor.processingTank);
		outputTank = new RGD_Tank(extractor.outputTank);
		objectIndex = extractor.ObjectIndex;
	}

	public virtual void RestoreExtractor(Placeable_Extractor extractor)
	{
		processTank.RestoreTank(extractor.processingTank);
		outputTank.RestoreTank(extractor.outputTank);
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("CompostTank", processTank);
		info.AddValue("BioFuelOutputTank", outputTank);
		info.AddValue("BioFuelCreatorObjectIndex", objectIndex);
	}

	public RGD_Extractor(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "CompostTank":
				processTank = Helper.ObjToValue<RGD_Tank>(item.Value);
				break;
			case "BioFuelOutputTank":
				outputTank = Helper.ObjToValue<RGD_Tank>(item.Value);
				break;
			case "BioFuelCreatorObjectIndex":
				objectIndex = Helper.ObjToValue<uint>(item.Value);
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		processTank = null;
		outputTank = null;
		objectIndex = 0u;
	}
}
