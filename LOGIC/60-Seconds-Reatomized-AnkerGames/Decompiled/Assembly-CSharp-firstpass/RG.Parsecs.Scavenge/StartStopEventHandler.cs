using UnityEngine;

namespace RG.Parsecs.Scavenge;

public abstract class StartStopEventHandler<T1, T2, T3> : MonoBehaviour where T1 : MonoBehaviour, IHaveEventHub<T1, T2, T3> where T2 : IEventHubArgument<T3>
{
	public T3 StartEvent;

	public T3 StopEvent;

	private T1 _actor;

	public void Start()
	{
		_actor = GetComponentInParent<T1>();
		_actor.EventHub.Add(OnActorEvent);
	}

	private void OnActorEvent(T1 actor, T2 e)
	{
		if (EventsEqual(StartEvent, e.Event))
		{
			OnStart(actor, e);
		}
		else if (EventsEqual(StopEvent, e.Event))
		{
			OnStop(actor, e);
		}
	}

	protected virtual bool EventsEqual(T3 a, T3 b)
	{
		return a.Equals(b);
	}

	protected abstract void OnStart(T1 actor, T2 e);

	protected abstract void OnStop(T1 actor, T2 e);

	public void OnDestroy()
	{
		if (_actor != null && _actor.EventHub != null)
		{
			_actor.EventHub.Remove(OnActorEvent);
		}
		_actor = null;
	}
}
