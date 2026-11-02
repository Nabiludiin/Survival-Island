using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_Block_Recycler : RGD_Block
{
	public RGD_Recycler rgdRecycler;

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		rgdRecycler.SerializeFast(writer);
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		rgdRecycler = new RGD_Recycler();
		rgdRecycler.DeserializeFast(reader);
	}

	public RGD_Block_Recycler()
	{
	}

	public RGD_Block_Recycler(Placeable_RecyclerExtractor recycler)
		: base(RGDType.Block_Recycler, recycler.GetComponent<Block>())
	{
		rgdRecycler = new RGD_Recycler(recycler);
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("Recycler", rgdRecycler);
	}

	public RGD_Block_Recycler(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "Recycler")
			{
				rgdRecycler = Helper.ObjToValue<RGD_Recycler>(item.Value);
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		rgdRecycler = null;
	}
}
