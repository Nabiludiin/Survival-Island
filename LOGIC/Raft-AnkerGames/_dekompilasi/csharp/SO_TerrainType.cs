using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_TerrainType", menuName = "Raft/Terrain/TerrainType")]
public class SO_TerrainType : ScriptableObject
{
	public enum BootlegTagIDs
	{
		TerrainID_GRASS,
		TerrainID_DIRT,
		TerrainID_ROCK,
		TerrainID_SAND,
		TerrainID_GRAVEL,
		TerrainID_ALPHAMAP,
		TerrainID_SANDDIRT,
		TerrainID_STONE,
		TerrainID_SNOW,
		TerrainID_SHALLOWSNOW,
		TerrainID_ICE
	}

	public List<BootlegTagIDs> idsToConvert = new List<BootlegTagIDs>();

	public List<int> tagIdentifiers = new List<int>();

	public TerrainID terrainID;

	public bool hasCustomFriction;

	public int terrainWalkSoundID;

	[Range(0f, 1f)]
	public float normalMovementControll = 0.8f;

	public float externalVelocityAcceleration = 3.5f;

	public float externalVelocityGroundedDamping = 1f;

	public float maxExternalVelocity = 3f;

	public void Convert()
	{
		tagIdentifiers.Clear();
		foreach (BootlegTagIDs item in idsToConvert)
		{
			int hashCode = item.ToString().GetHashCode();
			tagIdentifiers.Add(hashCode);
		}
	}

	public bool HasHashCodeIdentifyer(int hashCode)
	{
		if (tagIdentifiers.Contains(hashCode))
		{
			return true;
		}
		return false;
	}
}
