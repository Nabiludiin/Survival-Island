using System;
using UnityEngine;

namespace Knife.DeferredDecals.Spawn;

[Serializable]
public class DecalSpawnController
{
	public SpawnType SpawnMode;

	public Decal DecalPrefab;

	public DecalSpawner Spawner;

	public float DestroyDelay = 15f;

	public Decal SpawnDecal()
	{
		Decal decal;
		if (SpawnMode == SpawnType.Instantiate)
		{
			decal = UnityEngine.Object.Instantiate(DecalPrefab);
			if (DestroyDelay > 0f)
			{
				UnityEngine.Object.Destroy(decal.gameObject, DestroyDelay);
			}
		}
		else
		{
			if (SpawnMode != SpawnType.Pool)
			{
				throw new Exception("Invalid SpawnType");
			}
			decal = Spawner.Spawn();
			if (DestroyDelay > 0f)
			{
				Spawner.DestroyDecal(decal, DestroyDelay);
			}
		}
		decal.gameObject.SetActive(value: true);
		return decal;
	}
}
