using System;
using UnityEngine;

public class WorldEvent_Tracker_AI : WorldEvent_Tracker
{
	public AI_StateMachine_Animal stateMachine;

	public AI_State eventEndState;

	public void Start()
	{
		if (!initialized)
		{
			InitTracker(WorldEventManager.GetWorldEventFromType(trackedEventType));
		}
	}

	public void OnDestroy()
	{
		if (Raft_Network.IsHost && stateMachine != null && worldEvent != null)
		{
			worldEvent.RemoveSpawnedEntity(stateMachine.networkBehaviour);
		}
	}

	public override void InitTracker(WorldEvent we)
	{
		if (initialized)
		{
			return;
		}
		base.InitTracker(we);
		we.OnEventStopped = (Action)Delegate.Combine(we.OnEventStopped, new Action(EventEnd));
		if (stateMachine == null)
		{
			stateMachine = gameObject.GetComponentInChildren<AI_StateMachine_Animal>();
			if (stateMachine != null && eventEndState == null)
			{
				eventEndState = stateMachine.damageTakenState;
			}
		}
	}

	protected override void EventEnd()
	{
		base.EventEnd();
		if (stateMachine != null && eventEndState != null)
		{
			stateMachine.ChangeState(eventEndState);
		}
		else
		{
			Debug.Log("was unable to change state via event end on worldeventtracker_AI");
		}
	}
}
