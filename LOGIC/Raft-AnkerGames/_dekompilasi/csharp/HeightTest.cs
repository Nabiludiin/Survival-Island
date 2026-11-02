using UnityEngine;

[ExecuteAlways]
public class HeightTest : MonoBehaviour
{
	private void Update()
	{
		float y = Terrain.activeTerrain.SampleHeight(transform.position);
		Vector3 position = transform.position;
		position.y = y;
		transform.position = position;
	}
}
