using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_Brick : RGD_Block
{
	public uint brickObjectIndex;

	public float dryTimer;

	protected bool Equals(RGD_Brick other)
	{
		if (Equals((RGD_Block)other) && brickObjectIndex == other.brickObjectIndex)
		{
			return dryTimer.Equals(other.dryTimer);
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
		return Equals((RGD_Brick)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), brickObjectIndex, dryTimer);
	}

	public static bool operator ==(RGD_Brick left, RGD_Brick right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Brick left, RGD_Brick right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in brickObjectIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in dryTimer, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out brickObjectIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out dryTimer, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_Brick()
	{
	}

	public RGD_Brick(RGDType type, Brick_Wet brick)
		: base(type, brick)
	{
		brickObjectIndex = brick.ObjectIndex;
		dryTimer = brick.Timer;
	}

	public void RestoreBrick(Brick_Wet brick)
	{
		if (dryTimer != -1f)
		{
			if (dryTimer >= brick.dryTimeSeconds)
			{
				brick.OnFinishedDrying();
			}
			else
			{
				brick.Timer = dryTimer;
			}
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("BrickObjectIndex", brickObjectIndex);
		info.AddValue("DryTimer", dryTimer);
	}

	public RGD_Brick(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			string name = item.Name;
			if (!(name == "BrickObjectIndex"))
			{
				if (name == "DryTimer")
				{
					dryTimer = (float)item.Value;
				}
			}
			else
			{
				brickObjectIndex = (uint)item.Value;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		brickObjectIndex = 0u;
		dryTimer = -1f;
	}
}
