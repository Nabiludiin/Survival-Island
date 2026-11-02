using UnityEngine;

public class TerrainDetector : MonoBehaviour
{
	[HideInInspector]
	public Terrain currentTerrain;

	[HideInInspector]
	public TerrainIdentifier currentTerrainIdentifier;

	private SO_TerrainType _currentTerrainType;

	private TerrainData terrainData;

	private int alphamapWidth;

	private int alphamapHeight;

	private float[,,] splatmapData;

	private int numTextures;

	public bool HasTerrainIdentifier => currentTerrainIdentifier != null;

	public SO_TerrainType CurrentTerrainType => _currentTerrainType;

	public void SetNewTerrain(Terrain terrain, Vector3 fromPosition)
	{
		SetNewTerrain(terrain);
		if (terrain == null)
		{
			_currentTerrainType = null;
		}
		else
		{
			_currentTerrainType = currentTerrainIdentifier.terrainTypeGroup.GetTerrainType(GetActiveTerrainTextureIdx(fromPosition));
		}
	}

	public void SetNewCurrentTerrainType(SO_TerrainType terrainType)
	{
		if (_currentTerrainType != terrainType)
		{
			_currentTerrainType = terrainType;
		}
	}

	public int GetActiveTerrainTextureIdx(Vector3 position)
	{
		Vector3 vector = ConvertToSplatMapCoordinate(position);
		int result = 0;
		float num = 0f;
		for (int i = 0; i < numTextures; i++)
		{
			if (num < splatmapData[(int)vector.z, (int)vector.x, i])
			{
				result = i;
				num = splatmapData[(int)vector.z, (int)vector.x, i];
			}
		}
		return result;
	}

	private bool SetNewTerrain(Terrain terrain)
	{
		if (terrain != currentTerrain)
		{
			currentTerrainIdentifier = null;
			currentTerrain = terrain;
			if (terrain != null)
			{
				currentTerrainIdentifier = terrain.GetComponent<TerrainIdentifier>();
				if (currentTerrainIdentifier == null)
				{
					Debug.Log("Unable to find terrain identifier on " + terrain.name);
				}
				terrainData = terrain.terrainData;
				alphamapWidth = terrainData.alphamapWidth;
				alphamapHeight = terrainData.alphamapHeight;
				splatmapData = terrainData.GetAlphamaps(0, 0, alphamapWidth, alphamapHeight);
				numTextures = splatmapData.Length / (alphamapWidth * alphamapHeight);
			}
			return true;
		}
		return false;
	}

	private Vector3 ConvertToSplatMapCoordinate(Vector3 worldPosition)
	{
		Vector3 result = default;
		Vector3 position = currentTerrain.transform.position;
		result.x = (worldPosition.x - position.x) / currentTerrain.terrainData.size.x * (float)currentTerrain.terrainData.alphamapWidth;
		result.z = (worldPosition.z - position.z) / currentTerrain.terrainData.size.z * (float)currentTerrain.terrainData.alphamapHeight;
		return result;
	}
}
