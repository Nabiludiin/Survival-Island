using System;
using System.Collections.Generic;

namespace RG.Parsecs.Scavenge;

public class DictionaryEventHub<T1, T2, TEvent> : EventHub<T1, T2, TEvent> where T2 : IEventHubArgument<TEvent>
{
	private GenericEvent<T1, T2> _all;

	private Dictionary<TEvent, GenericEvent<T1, T2>> _hub;

	public DictionaryEventHub()
	{
		_hub = new Dictionary<TEvent, GenericEvent<T1, T2>>();
		_all = new GenericEvent<T1, T2>();
	}

	public DictionaryEventHub(IEqualityComparer<TEvent> comparer)
	{
		_hub = new Dictionary<TEvent, GenericEvent<T1, T2>>(comparer);
		_all = new GenericEvent<T1, T2>();
	}

	private GenericEvent<T1, T2> GetGenericEvent(TEvent e, bool create = true)
	{
		GenericEvent<T1, T2> value = null;
		if (!_hub.TryGetValue(e, out value) && create)
		{
			value = new GenericEvent<T1, T2>();
			_hub.Add(e, value);
		}
		return value;
	}

	public override void Add(Action<T1, T2> handler, TEvent e)
	{
		GetGenericEvent(e).Add(handler);
	}

	public override void Add(Action<T1, T2> handler)
	{
		_all.Add(handler);
	}

	public override void AddOneTime(Action<T1, T2> handler, TEvent e)
	{
		GetGenericEvent(e).AddOneTime(handler);
	}

	public override void AddOneTime(Action<T1, T2> handler)
	{
		_all.AddOneTime(handler);
	}

	public override bool Remove(Action<T1, T2> handler, TEvent e)
	{
		GenericEvent<T1, T2> genericEvent = GetGenericEvent(e);
		_all.Remove(handler);
		return genericEvent.Remove(handler);
	}

	public override bool Remove(Action<T1, T2> handler)
	{
		foreach (KeyValuePair<TEvent, GenericEvent<T1, T2>> item in _hub)
		{
			item.Value.Remove(handler);
		}
		return _all.Remove(handler);
	}

	public override void Invoke(T1 arg1, T2 arg2, TEvent e)
	{
		GenericEvent<T1, T2> genericEvent = GetGenericEvent(e, create: false);
		_all.Invoke(arg1, arg2);
		genericEvent?.Invoke(arg1, arg2);
	}

	public override void Invoke(T1 arg1, T2 arg2)
	{
		GenericEvent<T1, T2> genericEvent = GetGenericEvent(arg2.Event, create: false);
		_all.Invoke(arg1, arg2);
		genericEvent?.Invoke(arg1, arg2);
	}

	public override void Clear()
	{
		_all.Clear();
		foreach (KeyValuePair<TEvent, GenericEvent<T1, T2>> item in _hub)
		{
			item.Value.Clear();
		}
		_hub.Clear();
	}
}
