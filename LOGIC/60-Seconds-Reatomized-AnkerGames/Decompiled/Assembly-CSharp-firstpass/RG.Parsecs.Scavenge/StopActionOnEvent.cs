using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public abstract class StopActionOnEvent<T1, T2, T3> : OnEventHandler<T1, T2, T3> where T1 : MonoBehaviour, IHaveEventHub<T1, T2, T3> where T2 : IEventHubArgument<T3>
{
	public List<StopableEventAction> Actions;

	protected override void OnEvent(T1 actor, T2 eargs)
	{
		for (int i = 0; i < Actions.Count; i++)
		{
			StopableEventAction stopableEventAction = Actions[i];
			GameObject gameObject = ((actor != null) ? actor.gameObject : null);
			stopableEventAction.InvokeStop(new EventActionArgs
			{
				Object = gameObject
			});
		}
	}
}
