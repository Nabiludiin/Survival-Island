using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public abstract class ActionOnEventMultiple<T1, T2, T3, T4> : OnEventHandlerMultiple<T1, T2, T3> where T1 : MonoBehaviour, IHaveEventHub<T1, T2, T3> where T2 : IEventHubArgument<T3> where T4 : IEqualityComparer<T3>
{
	protected Dictionary<T3, List<EventAction>> _dict;

	protected List<T3> _events;

	protected override List<T3> GetEvents()
	{
		return _events;
	}

	protected override void OnEvent(T3 e, T1 actor, T2 eargs)
	{
		List<EventAction> value = null;
		if (!_dict.TryGetValue(e, out value))
		{
			return;
		}
		for (int i = 0; i < value.Count; i++)
		{
			EventAction eventAction = value[i];
			if (eventAction == null)
			{
				Debug.LogErrorFormat("Action is null in ActionOnEventMultiple: {0}.", gameObject.name);
			}
			else
			{
				ActionInvoke(e, actor, eargs, eventAction);
			}
		}
	}

	protected virtual void ActionInvoke(T3 e, T1 actor, T2 eargs, EventAction action)
	{
		action.Invoke(new EventActionArgs
		{
			Object = actor.gameObject,
			Actor = actor,
			Target = eargs.OtherActor
		});
	}
}
