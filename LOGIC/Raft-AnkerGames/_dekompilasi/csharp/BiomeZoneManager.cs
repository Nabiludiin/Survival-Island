using System;
using System.Collections.Generic;
using UnityEngine;

public class BiomeZoneManager : MonoBehaviour, SceneEventInterface
{
	public static Action<BiomeZone> OnSpawnZone;

	public static Action<BiomeZone> OnZoneDespawn;

	public static Action<BiomeZone> OnEnterZone;

	public static Action<BiomeZone> OnExitZone;

	private Transform _raftTransform;

	public static List<BiomeZone> biomeZones = new List<BiomeZone>();

	private static BiomeZone currentBiome;

	private Network_Player networkPlayer;

	public Transform RaftTransform
	{
		get
		{
			if (_raftTransform == null)
			{
				_raftTransform = SingletonGeneric<GameManager>.Singleton.raftTransform;
			}
			return _raftTransform;
		}
	}

	private void Start()
	{
		networkPlayer = ComponentManager<Network_Player>.Value;
		WorldShiftManager.OnWorldShift = (Action<Vector3>)Delegate.Combine(WorldShiftManager.OnWorldShift, new Action<Vector3>(OnWorldShift));
	}

	private void Update()
	{
		if (!(networkPlayer == null) && networkPlayer.IsLocalPlayer)
		{
			CheckIfPlayersAreInBounds();
		}
	}

	private void OnDrawGizmos()
	{
	}

	private void OnDestroy()
	{
		CleanUp();
	}

	private void CleanUp()
	{
		currentBiome = null;
		OnSpawnZone = (OnZoneDespawn = (OnEnterZone = (OnExitZone = null)));
		if (biomeZones != null)
		{
			biomeZones.Clear();
		}
		WorldShiftManager.OnWorldShift = (Action<Vector3>)Delegate.Remove(WorldShiftManager.OnWorldShift, new Action<Vector3>(OnWorldShift));
	}

	void SceneEventInterface.OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent == SceneEvent.LeaveGame)
		{
			CleanUp();
		}
	}

	private void OnWorldShift(Vector3 shift)
	{
		ChunkManager value = ComponentManager<ChunkManager>.Value;
		for (int i = 0; i < biomeZones.Count; i++)
		{
			BiomeZone biomeZone = biomeZones[i];
			biomeZone.position -= shift;
			if (value != null && biomeZone.position.DistanceXZ(Vector3.zero) > (float)value.TotalChunkWidth + biomeZone.radius)
			{
				RemoveBiomeZone(biomeZone);
				i--;
			}
		}
	}

	public static void SendOnSpawnZone(BiomeZone biome)
	{
		OnSpawnZone?.Invoke(biome);
	}

	public static void SendOnZoneDespawn(BiomeZone biome)
	{
		OnZoneDespawn?.Invoke(biome);
	}

	public static void SendOnEnterZone(BiomeZone biome, bool changeToBiome = false)
	{
		if (changeToBiome)
		{
			currentBiome = biome;
		}
		OnEnterZone?.Invoke(biome);
	}

	public static void SendOnExitZone(BiomeZone biome, bool removeCurrentBiome = false)
	{
		if (removeCurrentBiome)
		{
			currentBiome = null;
		}
		OnExitZone?.Invoke(biome);
	}

	public bool IsPlayerInRangeOfBiome(BiomeZone zone)
	{
		if (networkPlayer.transform.position.DistanceXZ(zone.position) <= zone.radius)
		{
			return true;
		}
		return false;
	}

	public static void AddBiomeZone(BiomeZone zone)
	{
		if (!biomeZones.Contains(zone))
		{
			biomeZones.Add(zone);
			SendOnSpawnZone(zone);
		}
	}

	public static void RemoveBiomeZone(BiomeZone zone)
	{
		if (zone != null && biomeZones != null && biomeZones.Contains(zone))
		{
			biomeZones.Remove(zone);
			SendOnZoneDespawn(zone);
		}
	}

	public static BiomeZone GetBiomeAtPoint(Vector3 point)
	{
		BiomeZone result = null;
		float num = 99999f;
		for (int i = 0; i < biomeZones.Count; i++)
		{
			float num2 = biomeZones[i].position.DistanceXZ(point);
			if (num2 < biomeZones[i].radius && num2 < num)
			{
				result = biomeZones[i];
				num = num2;
			}
		}
		return result;
	}

	public static BiomeZone GetClosestBiomeTo(Vector3 point)
	{
		BiomeZone result = null;
		float num = 99999f;
		for (int i = 0; i < biomeZones.Count; i++)
		{
			float num2 = biomeZones[i].position.DistanceXZ(point);
			if (num2 < num)
			{
				result = biomeZones[i];
				num = num2;
			}
		}
		return result;
	}

	public static BiomeZone GetCurrentBiome()
	{
		return currentBiome;
	}

	private void CheckIfPlayersAreInBounds()
	{
		if (biomeZones.Count == 0)
		{
			return;
		}
		BiomeZone closestBiomeTo = GetClosestBiomeTo(networkPlayer.transform.position);
		if (currentBiome == null)
		{
			if (closestBiomeTo != null && IsPlayerInRangeOfBiome(closestBiomeTo))
			{
				SendOnEnterZone(closestBiomeTo, changeToBiome: true);
			}
		}
		else if (currentBiome != closestBiomeTo)
		{
			SendOnExitZone(currentBiome);
			SendOnEnterZone(closestBiomeTo, changeToBiome: true);
		}
		else if (!IsPlayerInRangeOfBiome(currentBiome))
		{
			SendOnExitZone(currentBiome, removeCurrentBiome: true);
		}
	}
}
