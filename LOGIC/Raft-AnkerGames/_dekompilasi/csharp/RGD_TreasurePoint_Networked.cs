using System;
using System.Runtime.Serialization;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class RGD_TreasurePoint_Networked : SerializableBehaviour
{
	public SerializableVector3 position;

	public int excevationProgress;

	public bool isRevealedViaDetector;

	public uint tpObjectIndex;

	public uint tpPickupObjectIndex;

	public int tpParentObjectIndex;

	public int uniqueTreasureIndex;

	protected bool Equals(RGD_TreasurePoint_Networked other)
	{
		if (object.Equals(position, other.position) && excevationProgress == other.excevationProgress && isRevealedViaDetector == other.isRevealedViaDetector && tpObjectIndex == other.tpObjectIndex && tpPickupObjectIndex == other.tpPickupObjectIndex && tpParentObjectIndex == other.tpParentObjectIndex)
		{
			return uniqueTreasureIndex == other.uniqueTreasureIndex;
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
		return Equals((RGD_TreasurePoint_Networked)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(position, excevationProgress, isRevealedViaDetector, tpObjectIndex, tpPickupObjectIndex, tpParentObjectIndex, uniqueTreasureIndex);
	}

	public static bool operator ==(RGD_TreasurePoint_Networked left, RGD_TreasurePoint_Networked right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_TreasurePoint_Networked left, RGD_TreasurePoint_Networked right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(33))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in position.x, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in position.y, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in position.z, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in excevationProgress, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in isRevealedViaDetector, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in tpObjectIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in tpPickupObjectIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in tpParentObjectIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in uniqueTreasureIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		position = new SerializableVector3();
		reader.ReadValue(out position.x, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out position.y, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out position.z, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out excevationProgress, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out isRevealedViaDetector, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out tpObjectIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out tpPickupObjectIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out tpParentObjectIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out uniqueTreasureIndex, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_TreasurePoint_Networked()
	{
	}

	public RGD_TreasurePoint_Networked(TreasurePoint treasurePoint)
	{
		position = treasurePoint.transform.localPosition;
		excevationProgress = treasurePoint.currentExcevationProgress;
		isRevealedViaDetector = treasurePoint.isRevealedViaDetector;
		tpObjectIndex = treasurePoint.ObjectIndex;
		tpPickupObjectIndex = treasurePoint.pickupNetworked.ObjectIndex;
		tpParentObjectIndex = treasurePoint.GetParentObjectIndex();
		uniqueTreasureIndex = treasurePoint.uniqueTreasureIndex;
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("Position", position);
		info.AddValue("ExcevationProgress", excevationProgress);
		info.AddValue("IsRevealedViaDetector", isRevealedViaDetector);
		info.AddValue("TpObjectIndex", tpObjectIndex);
		info.AddValue("TpPickupObjectIndex", tpPickupObjectIndex);
		info.AddValue("TpParentObjectIndex", tpParentObjectIndex);
		info.AddValue("UniqueTreasureIndex", uniqueTreasureIndex);
	}

	public virtual TreasurePoint RestoreTreasurePoint(TreasurePointManager treasurePointManager)
	{
		TreasurePoint treasurePoint = treasurePointManager.AddTreasurePoint(tpObjectIndex, tpPickupObjectIndex, null, position, uniqueTreasureIndex);
		RestoreTreasurePoint(treasurePoint);
		return treasurePoint;
	}

	public virtual TreasurePoint RestoreTreasurePoint(TreasurePoint tp)
	{
		if (tp != null)
		{
			tp.isRevealedViaDetector = isRevealedViaDetector;
			if (excevationProgress <= 0)
			{
				tp.SetIsDugUp(value: false);
			}
			else
			{
				tp.ProgressExcevation(excevationProgress, addAchievement: false);
			}
		}
		return tp;
	}

	public RGD_TreasurePoint_Networked(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "Position":
				position = Helper.ObjToValue<SerializableVector3>(item.Value);
				break;
			case "ExcevationProgress":
				excevationProgress = (int)item.Value;
				break;
			case "IsRevealedViaDetector":
				isRevealedViaDetector = (bool)item.Value;
				break;
			case "TpObjectIndex":
				tpObjectIndex = (uint)item.Value;
				break;
			case "TpPickupObjectIndex":
				tpPickupObjectIndex = (uint)item.Value;
				break;
			case "TpParentObjectIndex":
				tpParentObjectIndex = (int)item.Value;
				break;
			case "UniqueTreasureIndex":
				uniqueTreasureIndex = (int)item.Value;
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		position = Vector3.zero;
		excevationProgress = 0;
		isRevealedViaDetector = false;
		tpObjectIndex = (tpPickupObjectIndex = 0u);
		uniqueTreasureIndex = 0;
	}
}
