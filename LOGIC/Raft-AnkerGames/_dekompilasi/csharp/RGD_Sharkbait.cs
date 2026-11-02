using System;
using System.Runtime.Serialization;

[Serializable]
public class RGD_Sharkbait : RGD_Block
{
	public RGD_Sharkbait()
	{
	}

	public RGD_Sharkbait(RGDType type, SharkBait sharkbait)
		: base(type, sharkbait)
	{
	}

	public void RestoreSharbait(SharkBait sharkbait)
	{
		if (!(sharkbait == null))
		{
			sharkbait.OnHitWater();
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
	}

	public RGD_Sharkbait(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
	}
}
