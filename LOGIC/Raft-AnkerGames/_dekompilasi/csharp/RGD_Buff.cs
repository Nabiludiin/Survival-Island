using System;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class RGD_Buff : SerializableBehaviour
{
	[SerializeField]
	private int u;

	[SerializeField]
	private float n;

	public int UniqueBuffIndex
	{
		get
		{
			return u;
		}
		set
		{
			u = value;
		}
	}

	public float NormalizedTimer
	{
		get
		{
			return n;
		}
		set
		{
			n = value;
		}
	}

	public RGD_Buff(Buff buff)
	{
		UniqueBuffIndex = (int)buff.Asset.UniqueBuffIndex;
		NormalizedTimer = buff.NormalizedTimer;
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("UniqueBuffIndex", UniqueBuffIndex);
		info.AddValue("NormalizedTimer", NormalizedTimer);
	}

	public RGD_Buff(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			string name = item.Name;
			if (!(name == "UniqueBuffIndex"))
			{
				if (name == "NormalizedTimer")
				{
					NormalizedTimer = Helper.ObjToValue<float>(item.Value);
				}
			}
			else
			{
				UniqueBuffIndex = Helper.ObjToValue<int>(item.Value);
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		UniqueBuffIndex = -1;
		NormalizedTimer = -1f;
	}
}
