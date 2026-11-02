using UnityEngine.Events;

namespace RG.Parsecs.Scavenge;

public class UnityEventAction : EventAction
{
	public UnityEvent action;

	protected override void Act(EventActionArgs args = null)
	{
		action.Invoke();
	}
}
