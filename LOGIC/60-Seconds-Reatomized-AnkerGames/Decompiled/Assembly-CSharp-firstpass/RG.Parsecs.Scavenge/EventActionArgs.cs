using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class EventActionArgs
{
	public GameObject Object;

	public IActorBrain Actor;

	public IActorBrain Target;

	public EventActionArgs()
	{
	}

	public EventActionArgs(GameObject o, IActorBrain a)
	{
		Object = o;
		Actor = a;
	}
}
