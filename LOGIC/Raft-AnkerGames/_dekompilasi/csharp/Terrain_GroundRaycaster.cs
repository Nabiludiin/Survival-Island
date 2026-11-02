using UnityEngine;

public class Terrain_GroundRaycaster : GroundRaycaster
{
	public TerrainDetector terrainDetector;

	public SO_TerrainType CurrentTerrainType => terrainDetector.CurrentTerrainType;

	public TerrainID CurrentTerrainID
	{
		get
		{
			if (!terrainDetector.HasTerrainIdentifier)
			{
				return TerrainID.NONE;
			}
			return terrainDetector.CurrentTerrainType.terrainID;
		}
	}

	protected override void RayHitGround(RaycastHit hit)
	{
		base.RayHitGround(hit);
		Terrain component = hit.collider.GetComponent<Terrain>();
		terrainDetector.SetNewTerrain(component, carrier.transform.position);
		if (!(component == null))
		{
			return;
		}
		string text = hit.transform.gameObject.tag;
		if (text.Contains("TerrainID"))
		{
			SO_TerrainType terrainType = TerrainNGroundManager.GetTerrainType(text.GetHashCode());
			if (terrainType != null)
			{
				terrainDetector.SetNewCurrentTerrainType(terrainType);
			}
		}
	}

	public override void Reset()
	{
		base.Reset();
		terrainDetector.SetNewTerrain(null, carrier.transform.position);
	}

	public bool IsCurrentTerrainIDUsingCustomFriction()
	{
		if (!(terrainDetector.CurrentTerrainType != null))
		{
			return false;
		}
		return terrainDetector.CurrentTerrainType.hasCustomFriction;
	}
}
