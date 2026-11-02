using UnityEngine;

public class BiomeZone
{
	public enum BiomeZoneIdentifier
	{
		BIOME_NONE,
		BIOME_Vasagatan,
		BIOME_Balboa,
		BIOME_Caravan,
		BIOME_TEMPERANCE
	}

	public BiomeZoneIdentifier zoneID;

	public Vector3 position;

	public float radius;

	public BiomeZone(BiomeZoneIdentifier zoneID, Vector3 position, float radius)
	{
		this.zoneID = zoneID;
		this.position = position;
		this.radius = radius;
	}
}
