using System;
using System.Collections;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public abstract class OnEventHandler<T1, T2, T3> : MonoBehaviour, IOnEventHandler<T1, T2, T3> where T1 : MonoBehaviour, IHaveEventHub<T1, T2, T3> where T2 : IEventHubArgument<T3>
{
	[SerializeField]
	protected T1 _actor;

	public bool SubsribeOnAwake;

	public bool SubscribeOnEnable;

	public bool ListenToAllActors;

	public T3 Event;

	public FloatOptionalRange Delay;

	private Action<T1, T2> _filter;

	public EventFilters<T1, T2, T3> Filter { get; set; }

	protected virtual void Awake()
	{
		if (SubsribeOnAwake)
		{
			Subscribe();
		}
	}

	protected virtual void OnEnable()
	{
		if (SubscribeOnEnable)
		{
			Subscribe();
		}
	}

	private void Subscribe()
	{
		if (_filter != null)
		{
			_actor.EventHub.Remove(_filter, Event);
			_filter = null;
		}
		if (_actor == null)
		{
			if (ListenToAllActors)
			{
				_actor = GlobalEventHubResolver.I.GetEventHub<T1>();
			}
			else
			{
				_actor = GetComponentInParent<T1>();
			}
		}
		if (_actor == null)
		{
			string text = "";
			Debug.LogErrorFormat("OnEventHandler.Subscribe has a null actor. Handler: {0}, Event: {1}, Handler Type: {2}", text, Event.ToString(), ToString());
		}
		else
		{
			_filter = OnEventFilter;
			_actor.EventHub.Add(_filter, Event);
		}
	}

	protected virtual void Start()
	{
		if (!SubsribeOnAwake)
		{
			Subscribe();
		}
	}

	protected void OnEventFilter(T1 actor, T2 args)
	{
		if (EventsEqual(Event, args.Event) && (!(Filter != null) || Filter.IsMatch(actor, args)))
		{
			if (Delay.IsDelay())
			{
				StartCoroutine(DelayedOnEvent(actor, args));
			}
			else
			{
				OnEvent(actor, args);
			}
		}
	}

	private IEnumerator DelayedOnEvent(T1 actor, T2 args)
	{
		yield return new WaitForSeconds(Delay);
		OnEvent(actor, args);
	}

	protected virtual bool EventsEqual(T3 a, T3 b)
	{
		return a.Equals(b);
	}

	[ContextMenu("Test Event")]
	public void TestEvent()
	{
		OnEvent(null, default);
	}

	protected abstract void OnEvent(T1 actor, T2 eargs);

	public virtual void OnDestroy()
	{
		if (_actor != null && _actor.EventHub != null)
		{
			_actor.EventHub.Remove(OnEventFilter);
		}
		_actor = null;
	}
}
