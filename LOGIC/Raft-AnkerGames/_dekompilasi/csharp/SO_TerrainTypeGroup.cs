using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_TerrainGroup", menuName = "Raft/Terrain/TerrainGroup")]
public class SO_TerrainTypeGroup : ScriptableObject
{
	public string groupName;

	public List<SO_TerrainType> terrainTypes;

	public SO_TerrainType GetTerrainType(int index)
	{
		if (terrainTypes == null)
		{
			return null;
		}
		if (terrainTypes.Count > index)
		{
			return terrainTypes[index];
		}
		return null;
	}
}
