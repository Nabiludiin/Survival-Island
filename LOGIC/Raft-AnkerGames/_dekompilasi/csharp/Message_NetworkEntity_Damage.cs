using System;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_NetworkEntity_Damage : Message_NetworkBehaviour_ID
{
	public EntityType damageInflictorEntityType;

	public uint entityObjectIndex;

	public int buffAssetIndex;

	public float damage;

	[SerializeField]
	private float posX;

	[SerializeField]
	private float posY;

	[SerializeField]
	private float posZ;

	[SerializeField]
	private float hitNormalX;

	[SerializeField]
	private float hitNormalY;

	[SerializeField]
	private float hitNormalZ;

	public Vector3 HitPosition
	{
		get
		{
			return new Vector3(posX, posY, posZ);
		}
		private set
		{
			posX = value.x;
			posY = value.y;
			posZ = value.z;
		}
	}

	public Vector3 HitNormal
	{
		get
		{
			return new Vector3(hitNormalX, hitNormalY, hitNormalZ);
		}
		private set
		{
			hitNormalX = value.x;
			hitNormalY = value.y;
			hitNormalZ = value.z;
		}
	}

	protected bool Equals(Message_NetworkEntity_Damage other)
	{
		if (Equals((Message_NetworkBehaviour_ID)other) && damageInflictorEntityType == other.damageInflictorEntityType && entityObjectIndex == other.entityObjectIndex && buffAssetIndex == other.buffAssetIndex && damage.Equals(other.damage) && posX.Equals(other.posX) && posY.Equals(other.posY) && posZ.Equals(other.posZ) && hitNormalX.Equals(other.hitNormalX) && hitNormalY.Equals(other.hitNormalY))
		{
			return hitNormalZ.Equals(other.hitNormalZ);
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
		return Equals((Message_NetworkEntity_Damage)obj);
	}

	public override int GetHashCode()
	{
		HashCode hashCode = default;
		hashCode.Add(base.GetHashCode());
		hashCode.Add((int)damageInflictorEntityType);
		hashCode.Add(entityObjectIndex);
		hashCode.Add(buffAssetIndex);
		hashCode.Add(damage);
		hashCode.Add(posX);
		hashCode.Add(posY);
		hashCode.Add(posZ);
		hashCode.Add(hitNormalX);
		hashCode.Add(hitNormalY);
		hashCode.Add(hitNormalZ);
		return hashCode.ToHashCode();
	}

	public static bool operator ==(Message_NetworkEntity_Damage left, Message_NetworkEntity_Damage right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_NetworkEntity_Damage left, Message_NetworkEntity_Damage right)
	{
		return !object.Equals(left, right);
	}

	public Message_NetworkEntity_Damage(Messages type, MonoBehaviour_Network behaviour, uint managerObjectIndex, uint entityObjectIndex, float damage, Vector3 hitPosition, Vector3 hitNormal, EntityType damageInflictorEntityType, SO_Buff buffAsset)
		: base(type, behaviour, managerObjectIndex)
	{
		this.entityObjectIndex = entityObjectIndex;
		this.damage = damage;
		HitPosition = hitPosition;
		HitNormal = hitNormal;
		this.damageInflictorEntityType = damageInflictorEntityType;
		buffAssetIndex = ((buffAsset != null) ? ((int)buffAsset.UniqueBuffIndex) : (-1));
	}

	public Message_NetworkEntity_Damage()
	{
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(40))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in damageInflictorEntityType, default(FastBufferWriter.ForEnums));
		writer.WriteValue(in entityObjectIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in buffAssetIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in damage, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in posX, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in posY, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in posZ, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in hitNormalX, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in hitNormalY, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in hitNormalZ, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out damageInflictorEntityType, default(FastBufferWriter.ForEnums));
		reader.ReadValue(out entityObjectIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out buffAssetIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out damage, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out posX, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out posY, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out posZ, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out hitNormalX, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out hitNormalY, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out hitNormalZ, default(FastBufferWriter.ForPrimitives));
	}
}
