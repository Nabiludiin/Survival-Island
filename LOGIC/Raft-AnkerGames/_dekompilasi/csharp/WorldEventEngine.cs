using System;
using System.Linq;
using UnityEngine;

public class WorldEventEngine : MonoBehaviour
{
	public TimerEventer timerEventUpdater;

	public float startEventCooldownMin = 300f;

	public float startEventCooldownMax = 1200f;

	public float startEventProgress;

	private float startEventCooldown;

	private WorldEventManager worldEventHandler;

	private void Start()
	{
		if (Raft_Network.IsHost)
		{
			TimerEventer timerEventer = timerEventUpdater;
			timerEventer.OnTimerReached = (Action)Delegate.Combine(timerEventer.OnTimerReached, new Action(HandleEvents));
			startEventCooldown = UnityEngine.Random.Range(startEventCooldownMin, startEventCooldownMax);
		}
		worldEventHandler = ComponentManager<WorldEventManager>.Value;
	}

	public void Update()
	{
		if (Raft_Network.IsHost)
		{
			startEventProgress += Time.deltaTime;
			timerEventUpdater.DoUpdate(Time.deltaTime, autoResetTimer: true);
		}
	}

	private void HandleEvents()
	{
		HandleEventStart();
		HandleEventEnd();
	}

	private void HandleEventStart()
	{
		if (!WorldEventManager.CanAddActiveEvent() || !(startEventProgress >= startEventCooldown))
		{
			return;
		}
		foreach (WorldEvent item in (from a in WorldEventManager.worldEvents.ToList()
			orderby Guid.NewGuid()
			select a).ToList())
		{
			if (!item.isActive && item.eventData.IsEventStartable())
			{
				worldEventHandler.StartEvent(item);
				startEventProgress = 0f;
				startEventCooldown = UnityEngine.Random.Range(startEventCooldownMin, startEventCooldownMax);
				break;
			}
		}
	}

	private void HandleEventEnd()
	{
		WorldEvent[] worldEvents = WorldEventManager.worldEvents;
		foreach (WorldEvent worldEvent in worldEvents)
		{
			if (worldEvent.isActive && !worldEvent.eventData.CanEventProlong())
			{
				worldEventHandler.StopEvent(worldEvent);
			}
		}
	}
}
