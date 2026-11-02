using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class ActionOnScavengeManagerEvent : ActionOnEvent<ScavengeManager, EventHubArgument<ScavengeManagerEvents>, ScavengeManagerEvents>
{
	protected override void OnEvent(ScavengeManager actor, EventHubArgument<ScavengeManagerEvents> eargs)
	{
		for (int i = 0; i < Actions.Count; i++)
		{
			EventAction eventAction = Actions[i];
			if (eventAction == null)
			{
				Debug.LogErrorFormat("Action is null in ActionOnActorEvent: {0}.", base.gameObject.name);
				continue;
			}
			GameObject gameObject = ((actor != null) ? actor.gameObject : null);
			eventAction.Invoke(new EventActionArgs
			{
				Actor = null,
				Object = gameObject
			});
		}
	}

	protected override bool EventsEqual(ScavengeManagerEvents a, ScavengeManagerEvents b)
	{
		return a == b;
	}
}
