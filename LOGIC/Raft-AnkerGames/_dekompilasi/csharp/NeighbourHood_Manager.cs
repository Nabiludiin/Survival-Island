using System;
using System.Collections.Generic;
using UnityEngine;

public class NeighbourHood_Manager : MonoBehaviour
{
	private List<TankTracker> tankTrackers = new List<TankTracker>();

	public float tankTrackUpdateRate = 15f;

	public void Awake()
	{
		PipeGroupManager.OnPipeGroupAdded = (Action<PipeGroup>)Delegate.Combine(PipeGroupManager.OnPipeGroupAdded, new Action<PipeGroup>(OnPipeGroupAdd));
		PipeGroupManager.OnPipeGroupRemoved = (Action<PipeGroup>)Delegate.Combine(PipeGroupManager.OnPipeGroupRemoved, new Action<PipeGroup>(OnPipeGroupRemove));
	}

	public void OnDestroy()
	{
		PipeGroupManager.OnPipeGroupAdded = (Action<PipeGroup>)Delegate.Remove(PipeGroupManager.OnPipeGroupAdded, new Action<PipeGroup>(OnPipeGroupAdd));
		PipeGroupManager.OnPipeGroupRemoved = (Action<PipeGroup>)Delegate.Remove(PipeGroupManager.OnPipeGroupRemoved, new Action<PipeGroup>(OnPipeGroupRemove));
	}

	public void AddTankTracker(PipeGroup pipeGroup)
	{
		tankTrackers.Add(new TankTracker(pipeGroup, 15f));
	}

	public void Update()
	{
		if (tankTrackers.Count > 0)
		{
			for (int i = 0; i < tankTrackers.Count; i++)
			{
				tankTrackers[i].UpdateTracker(Time.deltaTime);
			}
		}
	}

	public void OnPipeGroupAdd(PipeGroup pipeGroup)
	{
		tankTrackers.Add(new TankTracker(pipeGroup, tankTrackUpdateRate));
	}

	public void OnPipeGroupRemove(PipeGroup pipeGroup)
	{
		int count = tankTrackers.Count;
		while (count-- > 0)
		{
			if (tankTrackers[count].pipeGroup == pipeGroup)
			{
				tankTrackers[count].RemoveTrackerAdditions();
				tankTrackers.RemoveAt(count);
			}
		}
	}
}
