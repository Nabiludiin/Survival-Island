using UnityEngine;

namespace Knife.DeferredDecals;

public struct TerrainData
{
	public int heightmapIndex;

	public Matrix4x4 world2Terrain;

	public Vector3 terrainSize;

	public static int Size => 0 + 4 + 64 + 12;
}
