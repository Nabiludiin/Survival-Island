using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour, SceneEventInterface
{
	public static Action<int> DayCounterChange;

	private static int dayCounter;

	public static List<Landmark> AllLandmarks = new List<Landmark>();

	private Network_Host network_host;

	private Raft_Network network;

	private float previousTime;

	public static int DayCounter
	{
		get
		{
			return dayCounter;
		}
		set
		{
			dayCounter = value;
			if (DayCounterChange != null)
			{
				DayCounterChange(dayCounter);
			}
		}
	}

	private void Start()
	{
		network = ComponentManager<Raft_Network>.Value;
		network_host = ComponentManager<Network_Host>.Value;
	}

	private void OnDestroy()
	{
		network = null;
		network_host = null;
	}

	void SceneEventInterface.OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent == SceneEvent.LeaveGame)
		{
			DayCounter = 0;
			DayCounterChange = null;
			AllLandmarks.Clear();
		}
	}

	private void Update()
	{
		float hour = network_host.skyController.timeOfDay.hour;
		if (previousTime > 23f && previousTime < 24f && hour < 1f && hour >= 0f)
		{
			DayCounter++;
		}
		previousTime = network_host.skyController.timeOfDay.hour;
	}

	public void RestoreLandmarks(Message_WorldManager_CreateLandmarks msg)
	{
		for (int i = 0; i < msg.rgdLandmarks.Length; i++)
		{
			RGD_Landmark rgdLandmark = msg.rgdLandmarks[i];
			Landmark landmark = AllLandmarks.Find((Landmark l) => l.uniqueLandmarkIndex == rgdLandmark.uniqueLandmarkIndex);
			if (landmark != null)
			{
				rgdLandmark.RestoreLandmark(landmark, Raft_Network.IsHost);
			}
		}
	}
}
