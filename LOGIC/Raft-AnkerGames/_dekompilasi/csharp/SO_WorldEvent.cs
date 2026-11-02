using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldEvent", menuName = "Raft/WorldEvent")]
public class SO_WorldEvent : ScriptableObject
{
	public enum WorldEventType
	{
		Undefined,
		Dolphins,
		Whale,
		Birdswarm,
		Turtle,
		Stingray,
		Count
	}

	public WorldEventType worldEventType;

	public bool handledByClient;

	public bool handledByServer;

	public List<Restriction> eventStartRestrictions;

	public List<Restriction> eventCancellationRestrictions;

	private WorldEvent eventHandle;

	public void InitHandle(WorldEvent we)
	{
		eventHandle = we;
	}

	public bool IsEventStartable()
	{
		foreach (Restriction eventStartRestriction in eventStartRestrictions)
		{
			if (eventStartRestriction.IsRestricted(eventHandle))
			{
				return false;
			}
		}
		return true;
	}

	public bool CanEventProlong()
	{
		foreach (Restriction eventCancellationRestriction in eventCancellationRestrictions)
		{
			if (eventCancellationRestriction.IsRestricted(eventHandle))
			{
				return false;
			}
		}
		return true;
	}
}
