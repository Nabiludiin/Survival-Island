using System;
using System.Runtime.Serialization;

[Serializable]
public class RGD_NetworkBehaviour : RGD
{
	public int objectIndex;

	public RGD_NetworkBehaviour(RGDType type, MonoBehaviour_Network behaviour)
		: base(type, RGDSortingOrder.NetworkBehaviour)
	{
		objectIndex = (int)behaviour.ObjectIndex;
	}

	public RGD_NetworkBehaviour(RGDType type, MonoBehaviour_ID_Network IDbehaviour)
		: base(type, RGDSortingOrder.NetworkBehaviour)
	{
		objectIndex = (int)IDbehaviour.ObjectIndex;
	}

	public void RestoreIDBehaviour(MonoBehaviour_ID_Network IDbehaviour)
	{
		if (!(IDbehaviour == null))
		{
			if (objectIndex == -1)
			{
				IDbehaviour.ObjectIndex = SaveAndLoad.GetUniqueObjectIndex();
			}
			else
			{
				IDbehaviour.ObjectIndex = (uint)objectIndex;
			}
			NetworkIDManager.AddNetworkID(IDbehaviour);
		}
	}

	public void RestoreBehaviour(MonoBehaviour_Network behaviour)
	{
		if (!(behaviour == null))
		{
			if (objectIndex == -1)
			{
				behaviour.ObjectIndex = SaveAndLoad.GetUniqueObjectIndex();
			}
			else
			{
				behaviour.ObjectIndex = (uint)objectIndex;
			}
			behaviour.BehaviourIndex = NetworkUpdateManager.GetUniqueBehaviourIndex();
			NetworkUpdateManager.AddBehaviour(behaviour);
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("ObjectIndex", objectIndex);
	}

	public RGD_NetworkBehaviour(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "ObjectIndex")
			{
				objectIndex = GetIndex(item.Value);
			}
		}
	}

	private int GetIndex(object value)
	{
		Type type = value.GetType();
		if (type == typeof(uint))
		{
			return (int)(uint)value;
		}
		if (type == typeof(int))
		{
			return (int)value;
		}
		return -1;
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		objectIndex = -1;
	}
}
