using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_StateMachine_HyenaBoss : RGD_StateMachine
{
	private RGD_AcidPool[] rgdAcidPools;

	protected bool Equals(RGD_StateMachine_HyenaBoss other)
	{
		if (rgdAcidPools == null && other.rgdAcidPools != null)
		{
			return false;
		}
		if (rgdAcidPools != null && other.rgdAcidPools == null)
		{
			return false;
		}
		if (Equals((RGD_StateMachine)other))
		{
			if (rgdAcidPools != null || other.rgdAcidPools != null)
			{
				return Enumerable.SequenceEqual(rgdAcidPools, other.rgdAcidPools);
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
		return Equals((RGD_StateMachine_HyenaBoss)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgdAcidPools);
	}

	public static bool operator ==(RGD_StateMachine_HyenaBoss left, RGD_StateMachine_HyenaBoss right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_StateMachine_HyenaBoss left, RGD_StateMachine_HyenaBoss right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<int>((rgdAcidPools != null) ? rgdAcidPools.Length : 0, default(FastBufferWriter.ForPrimitives));
		if (rgdAcidPools != null)
		{
			RGD_AcidPool[] array = rgdAcidPools;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SerializeFast(writer);
			}
		}
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		int value = 0;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		if (value > 0)
		{
			rgdAcidPools = new RGD_AcidPool[value];
			for (int i = 0; i < value; i++)
			{
				RGD_AcidPool rGD_AcidPool = new RGD_AcidPool();
				rGD_AcidPool.DeserializeFast(reader);
				rgdAcidPools[i] = rGD_AcidPool;
			}
		}
	}

	public RGD_StateMachine_HyenaBoss()
	{
	}

	public RGD_StateMachine_HyenaBoss(AI_StateMachine_HyenaBoss stateMachineBoss)
		: base(stateMachineBoss)
	{
		List<RGD_AcidPool> list = new List<RGD_AcidPool>();
		for (int i = 0; i < stateMachineBoss.spawnedPools.Count; i++)
		{
			AcidPool acidPool = stateMachineBoss.spawnedPools[i];
			if (!(acidPool == null))
			{
				list.Add(new RGD_AcidPool(acidPool));
			}
		}
		if (list.Count > 0)
		{
			rgdAcidPools = list.ToArray();
		}
		else
		{
			rgdAcidPools = null;
		}
	}

	public override void Restore(AI_StateMachine stateMachine)
	{
		base.Restore(stateMachine);
		if (rgdAcidPools == null || rgdAcidPools.Length == 0)
		{
			return;
		}
		AI_StateMachine_HyenaBoss aI_StateMachine_HyenaBoss = stateMachine as AI_StateMachine_HyenaBoss;
		for (int i = 0; i < rgdAcidPools.Length; i++)
		{
			RGD_AcidPool rGD_AcidPool = rgdAcidPools[i];
			if (!(rGD_AcidPool == null))
			{
				AcidPool acidPool = aI_StateMachine_HyenaBoss.CreateAcidPool(rGD_AcidPool.position, rGD_AcidPool.startScale);
				acidPool.damageAreaShrink.waitTimer = rGD_AcidPool.waitTimer;
				acidPool.damageAreaShrink.shrinkTimer = rGD_AcidPool.shrinkTimer;
			}
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("RGDAcidPools", rgdAcidPools);
	}

	public RGD_StateMachine_HyenaBoss(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "RGDAcidPools")
			{
				rgdAcidPools = (RGD_AcidPool[])item.Value;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		rgdAcidPools = null;
	}
}
