using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;

[Serializable]
public class Message_PlantManagerCreate : Message_NetworkBehaviour
{
	public List<RGD_Cropplot> rgdCropplots = new List<RGD_Cropplot>();

	protected bool Equals(Message_PlantManagerCreate other)
	{
		if (rgdCropplots == null && other.rgdCropplots != null)
		{
			return false;
		}
		if (rgdCropplots != null && other.rgdCropplots == null)
		{
			return false;
		}
		if (Equals((Message_NetworkBehaviour)other))
		{
			if (rgdCropplots != null || other.rgdCropplots != null)
			{
				return rgdCropplots.SequenceEqual(other.rgdCropplots);
			}
			return true;
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
		return Equals((Message_PlantManagerCreate)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgdCropplots);
	}

	public static bool operator ==(Message_PlantManagerCreate left, Message_PlantManagerCreate right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_PlantManagerCreate left, Message_PlantManagerCreate right)
	{
		return !object.Equals(left, right);
	}

	public Message_PlantManagerCreate()
	{
	}

	public Message_PlantManagerCreate(Messages type, PlantManager plantManager)
		: base(type, plantManager)
	{
		for (int i = 0; i < PlantManager.allCropplots.Count; i++)
		{
			Cropplot cropplot = PlantManager.allCropplots[i];
			RGD_Cropplot item = new RGD_Cropplot(RGDType.Block_Cropplot, cropplot);
			rgdCropplots.Add(item);
		}
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<int>(rgdCropplots.Count, default(FastBufferWriter.ForPrimitives));
		foreach (RGD_Cropplot rgdCropplot in rgdCropplots)
		{
			rgdCropplot.SerializeFast(writer);
		}
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		int value = 0;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		rgdCropplots = new List<RGD_Cropplot>(value);
		for (int i = 0; i < value; i++)
		{
			RGD_Cropplot rGD_Cropplot = new RGD_Cropplot();
			rGD_Cropplot.DeserializeFast(reader);
			rgdCropplots.Add(rGD_Cropplot);
		}
	}
}
