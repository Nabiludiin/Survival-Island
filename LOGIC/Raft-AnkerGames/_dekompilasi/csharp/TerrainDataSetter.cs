using UnityEngine;

public class TerrainDataSetter : MonoBehaviour
{
	public TerrainData data;

	public Terrain terrain;

	public void ApplyData()
	{
		if (data == null)
		{
			Debug.Log("Can't apply data because data reference is not set");
			return;
		}
		if (terrain == null)
		{
			Debug.Log("Can't apply data because terrain reference is not set");
			return;
		}
		terrain.terrainData = data;
		TerrainCollider component = terrain.GetComponent<TerrainCollider>();
		if (component != null)
		{
			component.terrainData = data;
		}
	}

	private void OnEnable()
	{
		Terrain component = GetComponent<Terrain>();
		if (terrain == null && component != null)
		{
			terrain = component;
		}
	}
}
