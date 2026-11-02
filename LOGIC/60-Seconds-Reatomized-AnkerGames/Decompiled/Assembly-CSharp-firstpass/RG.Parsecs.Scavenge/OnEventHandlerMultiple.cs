using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public abstract class OnEventHandlerMultiple<T1, T2, T3> : MonoBehaviour, IOnEventHandler<T1, T2, T3> where T1 : MonoBehaviour, IHaveEventHub<T1, T2, T3> where T2 : IEventHubArgument<T3>
{
	[SerializeField]
	protected T1 _actor;

	public bool SubsribeOnAwake;

	public bool SubscribeOnEnable;

	public bool ListenToAllActors;

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

	protected abstract List<T3> GetEvents();

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
			_actor.EventHub.Remove(_filter);
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
			Debug.LogErrorFormat("OnEventHandler.Subscribe has a null actor. Handler: {0}, Handler Type: {2}", text, ToString());
		}
		else
		{
			_filter = OnEventFilter;
			_actor.EventHub.Add(_filter);
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
		foreach (T3 @event in GetEvents())
		{
			if (EventsEqual(@event, args.Event) && (!(Filter != null) || Filter.IsMatch(actor, args)))
			{
				if (Delay.IsDelay())
				{
					StartCoroutine(DelayedOnEvent(@event, actor, args));
				}
				else
				{
					OnEvent(@event, actor, args);
				}
			}
		}
	}

	private IEnumerator DelayedOnEvent(T3 e, T1 actor, T2 args)
	{
		yield return new WaitForSeconds(Delay);
		OnEvent(e, actor, args);
	}

	protected virtual bool EventsEqual(T3 a, T3 b)
	{
		return a.Equals(b);
	}

	[ContextMenu("Test Event")]
	public void TestEvent()
	{
		OnEvent(default, null, default);
	}

	protected abstract void OnEvent(T3 e, T1 actor, T2 eargs);

	public virtual void OnDestroy()
	{
		if (_actor != null && _actor.EventHub != null)
		{
			_actor.EventHub.Remove(OnEventFilter);
		}
		_actor = null;
	}
}
