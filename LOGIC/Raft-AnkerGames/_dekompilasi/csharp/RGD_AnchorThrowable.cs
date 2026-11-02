using System;
using System.Runtime.Serialization;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class RGD_AnchorThrowable : RGD_Block
{
	public SerializableVector3 AnchorPosition;

	public SerializableVector3 AnchorRotation;

	public bool hasBeenThrown;

	public bool fullyAnchored;

	protected bool Equals(RGD_AnchorThrowable other)
	{
		if (Equals((RGD_Block)other) && object.Equals(AnchorPosition, other.AnchorPosition) && object.Equals(AnchorRotation, other.AnchorRotation) && hasBeenThrown == other.hasBeenThrown)
		{
			return fullyAnchored == other.fullyAnchored;
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
		return Equals((RGD_AnchorThrowable)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), AnchorPosition, AnchorRotation, hasBeenThrown, fullyAnchored);
	}

	public static bool operator ==(RGD_AnchorThrowable left, RGD_AnchorThrowable right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_AnchorThrowable left, RGD_AnchorThrowable right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(26))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in AnchorPosition.x, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in AnchorPosition.y, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in AnchorPosition.z, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in AnchorRotation.x, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in AnchorRotation.y, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in AnchorRotation.z, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in hasBeenThrown, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in fullyAnchored, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		AnchorPosition = new SerializableVector3();
		AnchorRotation = new SerializableVector3();
		reader.ReadValue(out AnchorPosition.x, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out AnchorPosition.y, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out AnchorPosition.z, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out AnchorRotation.x, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out AnchorRotation.y, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out AnchorRotation.z, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out hasBeenThrown, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out fullyAnchored, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_AnchorThrowable()
	{
	}

	public RGD_AnchorThrowable(RGDType type, Block block, Anchor_Throwable_Stand anchorStand)
		: base(type, block)
	{
		hasBeenThrown = anchorStand.HasThrownAnchor;
		fullyAnchored = anchorStand.FullyAnchored;
		if (hasBeenThrown || fullyAnchored)
		{
			Transform thrownAnchor = anchorStand.GetThrownAnchor();
			AnchorPosition = new SerializableVector3(thrownAnchor.transform.position);
			AnchorRotation = new SerializableVector3(thrownAnchor.transform.eulerAngles);
		}
	}

	public void RestoreAnchorThrowable(Block block, Anchor_Throwable_Stand anchorStand)
	{
		if (!hasBeenThrown)
		{
			return;
		}
		GameObject pickupModel = anchorStand.PickupModel;
		pickupModel.SetActive(value: true);
		pickupModel.transform.SetParent(null);
		pickupModel.transform.eulerAngles = AnchorRotation.Value;
		pickupModel.transform.position = AnchorPosition.Value;
		if (!fullyAnchored)
		{
			if (Physics.Raycast(pickupModel.transform.position, Vector3.down, out var hitInfo, float.MaxValue, LayerMasks.MASK_Obstruction))
			{
				pickupModel.transform.position = hitInfo.point;
			}
			else
			{
				pickupModel.transform.position = new Vector3(pickupModel.transform.position.x, GameManager.OceanBottom, pickupModel.transform.position.z);
			}
		}
		anchorStand.FullyAnchored = true;
		anchorStand.block = block;
		anchorStand.block.canBeRemoved = false;
		anchorStand.HasThrownAnchor = true;
		anchorStand.Rope.SetPosition(0, anchorStand.RopeConnectionTransform);
		anchorStand.Rope.SetPosition(1, pickupModel.transform);
		ComponentManager<Raft>.Value.AddAnchor(playSound: false);
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("AnchorPosition", AnchorPosition);
		info.AddValue("AnchorRotation", AnchorRotation);
		info.AddValue("HasBeenThrown", hasBeenThrown);
		info.AddValue("FullyAnchored", fullyAnchored);
	}

	public RGD_AnchorThrowable(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "AnchorPosition":
				AnchorPosition = (SerializableVector3)item.Value;
				break;
			case "AnchorRotation":
				AnchorRotation = (SerializableVector3)item.Value;
				break;
			case "HasBeenThrown":
				hasBeenThrown = (bool)item.Value;
				break;
			case "MaxHealth":
				fullyAnchored = (bool)item.Value;
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		AnchorPosition = new SerializableVector3();
		AnchorRotation = new SerializableVector3();
		hasBeenThrown = false;
		fullyAnchored = false;
	}
}
