using System;
using UnityEngine;

[Serializable]
public class RGD_Egg
{
	public SerializableVector3 localRaftPosition;

	public uint eggObjectIndex;

	protected bool Equals(RGD_Egg other)
	{
		if (object.Equals(localRaftPosition, other.localRaftPosition))
		{
			return eggObjectIndex == other.eggObjectIndex;
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
		return Equals((RGD_Egg)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(localRaftPosition, eggObjectIndex);
	}

	public static bool operator ==(RGD_Egg left, RGD_Egg right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_Egg left, RGD_Egg right)
	{
		return !object.Equals(left, right);
	}

	public RGD_Egg()
	{
	}

	public RGD_Egg(Vector3 localRaftPosition, uint eggObjectIndex)
	{
		this.localRaftPosition = new SerializableVector3(localRaftPosition);
		this.eggObjectIndex = eggObjectIndex;
	}
}
