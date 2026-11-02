using System;
using UnityEngine;

public class WorldEvent_Tracker : MonoBehaviour
{
	public SO_WorldEvent.WorldEventType trackedEventType;

	protected WorldEvent worldEvent;

	protected bool initialized;

	public virtual void InitTracker(WorldEvent worldEvent)
	{
		this.worldEvent = worldEvent;
		trackedEventType = worldEvent.eventData.worldEventType;
		initialized = true;
	}

	protected virtual void EventEnd()
	{
		initialized = false;
		Untrack();
	}

	private void Untrack()
	{
		WorldEvent worldEvent = this.worldEvent;
		worldEvent.OnEventStopped = (Action)Delegate.Remove(worldEvent.OnEventStopped, new Action(EventEnd));
	}
}
