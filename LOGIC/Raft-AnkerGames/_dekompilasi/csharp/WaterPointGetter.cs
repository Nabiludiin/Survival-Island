using UltimateWater;
using UnityEngine;

public class WaterPointGetter : MonoBehaviour, SceneEventInterface
{
	public float offset;

	private static Water water;

	private void Start()
	{
		if (water == null)
		{
			water = Object.FindObjectOfType<Water>();
		}
	}

	void SceneEventInterface.OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent == SceneEvent.LeaveGame)
		{
			water = null;
		}
	}

	public float GetWaterPoint(Vector3 origin)
	{
		if (water == null)
		{
			water = Object.FindObjectOfType<Water>();
		}
		float num = water.GetHeightAt(origin.x, origin.z, water.Time) + offset;
		if (float.IsNaN(num))
		{
			return 0f;
		}
		return num;
	}
}
