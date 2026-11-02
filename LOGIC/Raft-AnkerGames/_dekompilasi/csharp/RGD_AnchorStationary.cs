using System;
using System.Runtime.Serialization;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class RGD_AnchorStationary : RGD_Block
{
	public SerializableVector3 AnchorPosition;

	public SerializableVector3 AnchorRotation;

	public bool anchorAtTop;

	public bool atBottom;

	public float jointDistance;

	protected bool Equals(RGD_AnchorStationary other)
	{
		if (Equals((RGD_Block)other) && object.Equals(AnchorPosition, other.AnchorPosition) && object.Equals(AnchorRotation, other.AnchorRotation) && anchorAtTop == other.anchorAtTop && atBottom == other.atBottom)
		{
			return jointDistance.Equals(other.jointDistance);
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
		return Equals((RGD_AnchorStationary)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), AnchorPosition, AnchorRotation, anchorAtTop, atBottom, jointDistance);
	}

	public static bool operator ==(RGD_AnchorStationary left, RGD_AnchorStationary right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_AnchorStationary left, RGD_AnchorStationary right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(30))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in AnchorPosition.x, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in AnchorPosition.y, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in AnchorPosition.z, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in AnchorRotation.x, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in AnchorRotation.y, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in AnchorRotation.z, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in anchorAtTop, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in atBottom, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in jointDistance, default(FastBufferWriter.ForPrimitives));
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
		reader.ReadValue(out anchorAtTop, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out atBottom, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out jointDistance, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_AnchorStationary()
	{
	}

	public RGD_AnchorStationary(RGDType type, Block block, Anchor_Stationary anchor)
		: base(type, block)
	{
		AnchorPosition = new SerializableVector3(anchor.GetAnchorTransform().position);
		AnchorRotation = new SerializableVector3(anchor.GetAnchorTransform().eulerAngles);
		anchorAtTop = !anchor.AtBottom && !anchor.DroppingAnchor;
		atBottom = anchor.AtBottom;
		jointDistance = anchor.JointDistance;
	}

	public void RestoreAnchor(Anchor_Stationary anchor)
	{
		if (anchorAtTop)
		{
			return;
		}
		anchor.AtBottom = true;
		anchor.CanUse = true;
		anchor.DroppingAnchor = false;
		Rigidbody anchorBody = anchor.GetAnchorBody();
		anchorBody.isKinematic = true;
		anchorBody.transform.SetParent(null);
		anchorBody.transform.eulerAngles = AnchorRotation.Value;
		anchorBody.transform.position = AnchorPosition.Value;
		if (!atBottom)
		{
			if (Physics.Raycast(anchorBody.transform.position, Vector3.down, out var hitInfo, float.MaxValue, LayerMasks.MASK_Obstruction))
			{
				anchorBody.transform.position = hitInfo.point;
			}
			else
			{
				anchorBody.transform.position = new Vector3(anchorBody.transform.position.x, GameManager.OceanBottom, anchorBody.transform.position.z);
			}
		}
		anchor.Rope.SetPosition(0, anchorBody.transform);
		anchor.Rope.SetPosition(1, anchor.RopeConnectionPoint);
		anchor.JointDistance = jointDistance;
		if (anchor.remoteControlled)
		{
			anchor.anim.SetBool("Up", value: false);
		}
		ComponentManager<Raft>.Value.AddAnchor(playSound: false);
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("AnchorPosition", AnchorPosition);
		info.AddValue("AnchorRotation", AnchorRotation);
		info.AddValue("AnchorAtTop", anchorAtTop);
		info.AddValue("AtBottom", atBottom);
		info.AddValue("JointDistance", jointDistance);
	}

	public RGD_AnchorStationary(SerializationInfo info, StreamingContext sc)
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
			case "AnchorAtTop":
				anchorAtTop = (bool)item.Value;
				break;
			case "AtBottom":
				atBottom = (bool)item.Value;
				break;
			case "JointDistance":
				jointDistance = (float)item.Value;
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		AnchorPosition = new SerializableVector3();
		AnchorRotation = new SerializableVector3();
		anchorAtTop = true;
		atBottom = false;
		jointDistance = 0f;
	}
}
