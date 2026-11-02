using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.AzureSky;

[Serializable]
public class RGD_World : SerializableBehaviour
{
	public float raftXPos;

	public float raftZPos;

	public float raftYRot;

	public string worldGuid;

	public float timeOfDay;

	public int weatherIndex;

	public float weatherTimer;

	public int dayCounter;

	public bool fjuttLandmarkRemoved;

	public bool radioTowerReached;

	public RGD_ChunkPoint[] rgdChunkPoints;

	public int[] chunkManagerSpawnerIndexes;

	public RGD_BiomeZone[] rgdBiomeZones;

	public RGD_RecieverFrequencies rgdRecieverFrequencies;

	public RGD_ZiplinePath[] ziplinePaths;

	public RGD_MeshPath[] meshPaths;

	public bool engineControlsAnchorDown;

	public RGD_World(AzureSkyController skyController, Raft raft, RaftCollisionManager raftCollisionManager, WeatherManager weatherManager)
	{
		raftXPos = raftCollisionManager.transform.position.x;
		raftZPos = raftCollisionManager.transform.position.z;
		raftYRot = raftCollisionManager.transform.eulerAngles.y;
		timeOfDay = skyController.timeOfDay.hour;
		worldGuid = SaveAndLoad.WorldGuid.ToString();
		weatherIndex = (int)weatherManager.GetCurrentWeatherType();
		weatherTimer = weatherManager.WeatherTimer;
		dayCounter = WorldManager.DayCounter;
		radioTowerReached = QuestProgressTracker.HasFinishedQuest(QuestType.RadioTowerReached);
		List<RGD_BiomeZone> list = new List<RGD_BiomeZone>();
		for (int i = 0; i < BiomeZoneManager.biomeZones.Count; i++)
		{
			BiomeZone biomeZone = BiomeZoneManager.biomeZones[i];
			if (biomeZone != null)
			{
				list.Add(new RGD_BiomeZone(biomeZone));
			}
		}
		rgdBiomeZones = list.ToArray();
		List<RGD_MeshPath> list2 = new List<RGD_MeshPath>();
		for (int j = 0; j < MeshPath.PathConnections.Count; j++)
		{
			MeshPathConnection meshPathConnection = MeshPath.PathConnections[j];
			if (meshPathConnection != null && meshPathConnection.IsValid())
			{
				list2.Add(new RGD_MeshPath(meshPathConnection));
			}
		}
		if (list2.ContainsItems())
		{
			meshPaths = list2.ToArray();
		}
		engineControlsAnchorDown = EngineControls.AnchorsAreDown;
	}

	public void RestoreWorld(AzureSkyController skyController, Raft raft, RaftCollisionManager raftCollisionManager, WeatherManager weatherManager)
	{
		raftCollisionManager.transform.eulerAngles = new Vector3(0f, raftYRot, 0f);
		Transform transform = raft.transform;
		Vector3 position = (raftCollisionManager.transform.position = new Vector3(raftXPos, 0f, raftZPos));
		transform.position = position;
		SaveAndLoad.WorldGuid = new Guid(worldGuid);
		skyController.timeOfDay.GotoTime(timeOfDay);
		weatherManager.SetWeather((UniqueWeatherType)weatherIndex, instant: true);
		weatherManager.WeatherTimer = weatherTimer;
		WorldManager.DayCounter = dayCounter;
		if (rgdBiomeZones != null)
		{
			for (int i = 0; i < rgdBiomeZones.Length; i++)
			{
				RGD_BiomeZone rGD_BiomeZone = rgdBiomeZones[i];
				if (rGD_BiomeZone != null)
				{
					BiomeZoneManager.AddBiomeZone(new BiomeZone(rGD_BiomeZone.biomeZoneID, rGD_BiomeZone.worldPosition.Value, rGD_BiomeZone.radius));
				}
			}
		}
		if (ziplinePaths.ContainsItems())
		{
			RGD_ZiplinePath[] array = ziplinePaths;
			for (int j = 0; j < array.Length; j++)
			{
				array[j]?.Restore(playSound: false);
			}
		}
		if (meshPaths.ContainsItems())
		{
			RGD_MeshPath[] array2 = meshPaths;
			foreach (RGD_MeshPath rGD_MeshPath in array2)
			{
				if (rGD_MeshPath != null)
				{
					rGD_MeshPath.Restore(replicating: true);
				}
			}
		}
		EngineControls.AnchorsAreDown = engineControlsAnchorDown;
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("RaftPosX", raftXPos);
		info.AddValue("RaftPosZ", raftZPos);
		info.AddValue("RaftRotY", raftYRot);
		info.AddValue("WorldGuid", worldGuid);
		info.AddValue("TimeOfDay", timeOfDay);
		info.AddValue("WeatherTypeIndex", weatherIndex);
		info.AddValue("WeatherTimer", weatherTimer);
		info.AddValue("DayCounter", dayCounter);
		info.AddValue("FjuttLandmark", fjuttLandmarkRemoved);
		info.AddValue("RadioTowerReached", radioTowerReached);
		info.AddValue("RGDChunkPoints", rgdChunkPoints);
		info.AddValue("ChunkManagerSpawnerIndexes", chunkManagerSpawnerIndexes);
		info.AddValue("BiomeZones", rgdBiomeZones);
		info.AddValue("RecieverFrequencies", rgdRecieverFrequencies);
		info.AddValue("ZiplinePaths", ziplinePaths);
		info.AddValue("MeshPaths", meshPaths);
		info.AddValue("EngineControlsAnchorDown", engineControlsAnchorDown);
	}

	public RGD_World(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "RaftPosX":
				raftXPos = (float)item.Value;
				if (float.IsNaN(raftXPos))
				{
					raftXPos = 0f;
				}
				break;
			case "RaftPosZ":
				raftZPos = (float)item.Value;
				if (float.IsNaN(raftZPos))
				{
					raftZPos = 0f;
				}
				break;
			case "RaftRotY":
				raftYRot = (float)item.Value;
				if (float.IsNaN(raftYRot))
				{
					raftYRot = 0f;
				}
				break;
			case "WorldGuid":
				worldGuid = (string)item.Value;
				break;
			case "TimeOfDay":
				timeOfDay = (float)item.Value;
				break;
			case "WeatherTypeIndex":
				weatherIndex = (int)item.Value;
				break;
			case "WeatherTimer":
				weatherTimer = (float)item.Value;
				break;
			case "DayCounter":
				dayCounter = (int)item.Value;
				break;
			case "FjuttLandmark":
				fjuttLandmarkRemoved = (bool)item.Value;
				break;
			case "RadioTowerReached":
				radioTowerReached = (bool)item.Value;
				break;
			case "RGDChunkPoints":
				rgdChunkPoints = (RGD_ChunkPoint[])item.Value;
				break;
			case "ChunkManagerSpawnerIndexes":
				chunkManagerSpawnerIndexes = (int[])item.Value;
				break;
			case "BiomeZones":
				rgdBiomeZones = (RGD_BiomeZone[])item.Value;
				break;
			case "RecieverFrequencies":
				rgdRecieverFrequencies = Helper.ObjToValue<RGD_RecieverFrequencies>(item.Value);
				break;
			case "ZiplinePaths":
				ziplinePaths = (RGD_ZiplinePath[])item.Value;
				break;
			case "MeshPaths":
				meshPaths = (RGD_MeshPath[])item.Value;
				break;
			case "EngineControlsAnchorDown":
				engineControlsAnchorDown = (bool)item.Value;
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		raftXPos = 0f;
		raftZPos = 0f;
		raftYRot = 0f;
		worldGuid = "Default WorldGuid";
		timeOfDay = 0f;
		weatherIndex = 0;
		weatherTimer = 60f;
		dayCounter = 0;
		fjuttLandmarkRemoved = false;
		radioTowerReached = false;
		rgdChunkPoints = null;
		chunkManagerSpawnerIndexes = null;
		rgdBiomeZones = null;
		rgdRecieverFrequencies = null;
		ziplinePaths = null;
		meshPaths = null;
		engineControlsAnchorDown = false;
	}
}
