using System.Collections.Generic;
using UnityEngine;

public class TerrainNGroundManager : MonoBehaviour
{
	public static List<SO_TerrainType> s_terrainTypes;

	[SerializeField]
	private List<SO_TerrainType> terrainTypes;

	private void Awake()
	{
		s_terrainTypes = terrainTypes;
	}

	public static SO_TerrainType GetTerrainType(int hashCode)
	{
		foreach (SO_TerrainType s_terrainType in s_terrainTypes)
		{
			if (s_terrainType.HasHashCodeIdentifyer(hashCode))
			{
				return s_terrainType;
			}
		}
		return null;
	}
}
