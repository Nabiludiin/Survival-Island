using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_AcidPool : SerializableBehaviour
{
	public SerializableVector3 position;

	public float startScale;

	public float waitTimer;

	public float shrinkTimer;

	protected bool Equals(RGD_AcidPool other)
	{
		if (object.Equals(position, other.position) && startScale.Equals(other.startScale) && waitTimer.Equals(other.waitTimer))
		{
			return shrinkTimer.Equals(other.shrinkTimer);
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
		return Equals((RGD_AcidPool)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(position, startScale, waitTimer, shrinkTimer);
	}

	public static bool operator ==(RGD_AcidPool left, RGD_AcidPool right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_AcidPool left, RGD_AcidPool right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(24))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in position.x, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in position.y, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in position.z, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in startScale, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in waitTimer, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in shrinkTimer, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		position = new SerializableVector3();
		reader.ReadValue(out position.x, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out position.y, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out position.z, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out startScale, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out waitTimer, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out shrinkTimer, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_AcidPool()
	{
	}

	public RGD_AcidPool(AcidPool pool)
	{
		position = new SerializableVector3(pool.transform.position);
		startScale = pool.damageAreaShrink.startScale.x;
		waitTimer = pool.damageAreaShrink.waitTimer;
		shrinkTimer = pool.damageAreaShrink.shrinkTimer;
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("Position", position);
		info.AddValue("StartScale", startScale);
		info.AddValue("WaitTimer", waitTimer);
		info.AddValue("ShrinkTimer", shrinkTimer);
	}

	public RGD_AcidPool(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "Position":
				position = (SerializableVector3)item.Value;
				break;
			case "StartScale":
				startScale = Helper.ObjToValue<float>(item.Value);
				break;
			case "WaitTimer":
				waitTimer = Helper.ObjToValue<float>(item.Value);
				break;
			case "ShrinkTimer":
				shrinkTimer = Helper.ObjToValue<float>(item.Value);
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		startScale = 0f;
		waitTimer = 0f;
		shrinkTimer = 0f;
	}
}
