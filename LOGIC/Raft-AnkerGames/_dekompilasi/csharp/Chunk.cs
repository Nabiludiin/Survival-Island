using UnityEngine;

public class Chunk
{
	public Vector3 worldPosition = Vector3.zero;

	public uint size = 1000u;

	public Vector3 GetRandomPoint()
	{
		Vector3 result = worldPosition;
		result.x += Random.Range(0f, (float)size / 2f) * (float)Helper.RandomOneMinus();
		result.z += Random.Range(0f, (float)size / 2f) * (float)Helper.RandomOneMinus();
		return result;
	}
}
