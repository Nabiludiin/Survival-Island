using System;
using System.Runtime.Serialization;

[Serializable]
public class RGD_BiomeZone : SerializableBehaviour
{
	public SerializableVector3 worldPosition;

	public BiomeZone.BiomeZoneIdentifier biomeZoneID;

	public float radius;

	public RGD_BiomeZone(BiomeZone biomeZone)
	{
		worldPosition = new SerializableVector3(biomeZone.position);
		biomeZoneID = biomeZone.zoneID;
		radius = biomeZone.radius;
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("WorldPosition", worldPosition);
		info.AddValue("BiomeZoneID", biomeZoneID);
		info.AddValue("Radius", radius);
	}

	public RGD_BiomeZone(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "WorldPosition":
				worldPosition = Helper.ObjToValue<SerializableVector3>(item.Value);
				break;
			case "BiomeZoneID":
				biomeZoneID = Helper.ObjToValue<BiomeZone.BiomeZoneIdentifier>(item.Value);
				break;
			case "Radius":
				radius = Helper.ObjToValue<float>(item.Value);
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		worldPosition = null;
		biomeZoneID = BiomeZone.BiomeZoneIdentifier.BIOME_NONE;
		radius = 0f;
	}
}
