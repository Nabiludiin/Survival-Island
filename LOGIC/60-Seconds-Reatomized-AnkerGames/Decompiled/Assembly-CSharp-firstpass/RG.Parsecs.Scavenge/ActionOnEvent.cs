using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public abstract class ActionOnEvent<T1, T2, T3> : OnEventHandler<T1, T2, T3> where T1 : MonoBehaviour, IActorBrain, IHaveEventHub<T1, T2, T3> where T2 : IEventHubArgument<T3>
{
	public List<EventAction> Actions;

	protected override void OnEvent(T1 actor, T2 eargs)
	{
		for (int i = 0; i < Actions.Count; i++)
		{
			EventAction eventAction = Actions[i];
			if (eventAction == null)
			{
				Debug.LogErrorFormat("Action is null in ActionOnEvent: {0}.", gameObject.name);
				continue;
			}
			eventAction.Invoke(new EventActionArgs
			{
				Object = actor.gameObject,
				Actor = actor,
				Target = eargs.OtherActor
			});
		}
	}
}
