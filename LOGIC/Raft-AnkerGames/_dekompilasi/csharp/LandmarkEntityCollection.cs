using System.Collections.Generic;
using UnityEngine;

public class LandmarkEntityCollection : MonoBehaviour
{
	public LandmarkEntitySpawner[] spawners;

	[Range(0f, 100f)]
	public float spawnPercent = 100f;

	[SerializeField]
	private int minimumSpawnAmount;

	[SerializeField]
	private bool capSpawnAmount;

	[SerializeField]
	private int maximumSpawnAmount;

	private void Awake()
	{
		InitIfSpawnersShouldSpawn();
		AwakeSpawners();
	}

	private void OnEnable()
	{
		InitIfSpawnersShouldSpawn();
	}

	private void InspectorCode()
	{
		if (!Application.isEditor || Application.isPlaying)
		{
			return;
		}
		List<uint> list = new List<uint>();
		for (int i = 0; i < spawners.Length; i++)
		{
			LandmarkEntitySpawner landmarkEntitySpawner = spawners[i];
			if (landmarkEntitySpawner != null)
			{
				if (!list.Contains(landmarkEntitySpawner.spawnerIndex))
				{
					list.Add(landmarkEntitySpawner.spawnerIndex);
				}
				else
				{
					Debug.LogError("LandmarkEntitySpawner index duplicate ", landmarkEntitySpawner);
				}
			}
			else
			{
				Debug.Log("A spawner is null", gameObject);
			}
		}
	}

	private int CalculateSpawnAmount()
	{
		int num = 0;
		for (int i = 0; i < spawners.Length; i++)
		{
			if ((float)Random.Range(0, 100) < spawnPercent)
			{
				num++;
			}
		}
		if (num < minimumSpawnAmount)
		{
			num = minimumSpawnAmount;
		}
		else if (capSpawnAmount && num > maximumSpawnAmount)
		{
			num = maximumSpawnAmount;
		}
		return num;
	}

	private void InitIfSpawnersShouldSpawn()
	{
		List<int> list = ArrayRandomizer.GenerateRandom(CalculateSpawnAmount(), 0, spawners.Length);
		for (int i = 0; i < spawners.Length; i++)
		{
			spawners[i].ShouldSpawn = list.Contains(i);
		}
	}

	private void AwakeSpawners()
	{
		for (int i = 0; i < spawners.Length; i++)
		{
			spawners[i].AwakeCall();
		}
	}

	private void RandomizeSpawnerEntities()
	{
		for (int i = 0; i < spawners.Length; i++)
		{
			spawners[i].SetRandomEntityType();
		}
	}

	public List<LandmarkEntitySpawner> GetSpawnersWhichHaventSpawned()
	{
		List<LandmarkEntitySpawner> list = new List<LandmarkEntitySpawner>();
		LandmarkEntitySpawner[] array = spawners;
		foreach (LandmarkEntitySpawner landmarkEntitySpawner in array)
		{
			if (landmarkEntitySpawner.spawnedEntityBehaviour == null || landmarkEntitySpawner.spawnedEntityBehaviour.networkEntity.IsDead)
			{
				list.Add(landmarkEntitySpawner);
			}
		}
		return list;
	}
}
