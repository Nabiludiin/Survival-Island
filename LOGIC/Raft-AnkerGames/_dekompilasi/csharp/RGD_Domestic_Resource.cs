using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_Domestic_Resource : RGD_Domestic
{
	public float regenCount;

	protected bool Equals(RGD_Domestic_Resource other)
	{
		if (Equals((RGD_Domestic)other))
		{
			return regenCount.Equals(other.regenCount);
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
		return Equals((RGD_Domestic_Resource)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), regenCount);
	}

	public static bool operator ==(RGD_Domestic_Resource left, RGD_Domestic_Resource right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Domestic_Resource left, RGD_Domestic_Resource right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in regenCount, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out regenCount, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_Domestic_Resource()
	{
	}

	public RGD_Domestic_Resource(AI_NetworkBehaviour_Domestic domesticBehaviour, ResourceRegenerative resource)
		: base(domesticBehaviour)
	{
		regenCount = resource.GetRegenCount();
	}

	public void RestoreDomesticResource(ResourceRegenerative resource)
	{
		if (isTamed && regenCount != -1f)
		{
			resource.SetRegenCount(regenCount);
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("RegenCount", regenCount);
	}

	public RGD_Domestic_Resource(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "RegenCount")
			{
				regenCount = Helper.ObjToValue<float>(item.Value);
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		regenCount = -1f;
	}
}
