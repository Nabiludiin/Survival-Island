using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class RGD_LandmarkItem_Container : RGD_LandmarkItem
{
	[SerializeField]
	private RGD_CarryNetwork[] i;

	public RGD_CarryNetwork[] RGDSpawnedItems
	{
		get
		{
			return i;
		}
		set
		{
			i = value;
		}
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<int>((this.i != null) ? this.i.Length : 0, default(FastBufferWriter.ForPrimitives));
		if (this.i != null)
		{
			RGD_CarryNetwork[] array = this.i;
			foreach (RGD_CarryNetwork rGD_CarryNetwork in array)
			{
				writer.WriteValueSafe<bool>(rGD_CarryNetwork is RGD_CarryNetworkInterestPoint, default(FastBufferWriter.ForPrimitives));
				rGD_CarryNetwork.SerializeFast(writer);
			}
		}
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		int value = 0;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		if (value <= 0)
		{
			return;
		}
		this.i = new RGD_CarryNetwork[value];
		for (int i = 0; i < value; i++)
		{
			reader.ReadValue(out bool value2, default(FastBufferWriter.ForPrimitives));
			if (value2)
			{
				this.i[i] = new RGD_CarryNetworkInterestPoint();
			}
			else
			{
				this.i[i] = new RGD_CarryNetwork();
			}
			this.i[i].DeserializeFast(reader);
		}
	}

	public RGD_LandmarkItem_Container()
	{
	}

	public RGD_LandmarkItem_Container(LandmarkItem landmarkItem, Container container)
		: base(landmarkItem)
	{
		if (container != null)
		{
			List<RGD_CarryNetwork> list = new List<RGD_CarryNetwork>();
			foreach (CarryNetworked spawnedItem in container.spawnedItems)
			{
				if (!(spawnedItem == null))
				{
					list.Add(spawnedItem.GetSaveData());
				}
			}
			RGDSpawnedItems = list.ToArray();
		}
		else
		{
			Debug.LogError("Can't save RGD_LandmarkItem_Container, Container script was null " + landmarkItem.name);
		}
	}

	public override void RestoreLandmarkItem(LandmarkItem landmarkItem)
	{
		base.RestoreLandmarkItem(landmarkItem);
		Container component = landmarkItem.GetComponent<Container>();
		if (!(component != null) || RGDSpawnedItems == null)
		{
			return;
		}
		RGD_CarryNetwork[] rGDSpawnedItems = RGDSpawnedItems;
		foreach (RGD_CarryNetwork rGD_CarryNetwork in rGDSpawnedItems)
		{
			if (rGD_CarryNetwork != null)
			{
				CarryNetworked item = component.SpawnObjectLocal(null, rGD_CarryNetwork.ItemObjectIndex);
				rGD_CarryNetwork.RestoreCarryNetworked(item);
			}
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("I", RGDSpawnedItems);
	}

	public RGD_LandmarkItem_Container(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "I")
			{
				RGDSpawnedItems = (RGD_CarryNetwork[])item.Value;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		RGDSpawnedItems = null;
	}
}
