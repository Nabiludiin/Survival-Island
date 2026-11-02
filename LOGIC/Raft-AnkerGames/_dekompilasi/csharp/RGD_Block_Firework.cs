using System;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_Block_Firework : RGD_Block
{
	public RGD_RotationComponent rgdRotationComponent;

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		rgdRotationComponent.SerializeFast(writer);
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		rgdRotationComponent = new RGD_RotationComponent();
		rgdRotationComponent.DeserializeFast(reader);
	}

	public RGD_Block_Firework()
	{
	}

	public RGD_Block_Firework(RGDType type, Block_Firework fireworkBlock)
		: base(type, fireworkBlock)
	{
		if (fireworkBlock.rotationComponent != null)
		{
			rgdRotationComponent = new RGD_RotationComponent(fireworkBlock.rotationComponent);
		}
		else
		{
			rgdRotationComponent = null;
		}
	}

	public override void RestoreBlock(Block block)
	{
		base.RestoreBlock(block);
		if (rgdRotationComponent != null)
		{
			Block_Firework component = block.GetComponent<Block_Firework>();
			if (component != null)
			{
				rgdRotationComponent.RestoreRotational(component.rotationComponent);
			}
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("RotationComponent", rgdRotationComponent);
	}

	public RGD_Block_Firework(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "RotationComponent")
			{
				rgdRotationComponent = (RGD_RotationComponent)item.Value;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		rgdRotationComponent = null;
	}
}
