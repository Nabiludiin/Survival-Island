using System;
using System.Collections.Generic;

namespace RG.Parsecs.Scavenge;

public class EventHub<T1, T2, TEvent> where T2 : IEventHubArgument<TEvent>
{
	private GenericEvent<T1, T2> _all;

	public EventHub()
	{
		_all = new GenericEvent<T1, T2>();
	}

	public EventHub(IEqualityComparer<TEvent> comparer)
	{
		_all = new GenericEvent<T1, T2>();
	}

	public virtual void Add(Action<T1, T2> handler, TEvent e)
	{
		_all.Add(handler);
	}

	public virtual void Add(Action<T1, T2> handler)
	{
		_all.Add(handler);
	}

	public virtual void AddOneTime(Action<T1, T2> handler, TEvent e)
	{
		_all.AddOneTime(handler);
	}

	public virtual void AddOneTime(Action<T1, T2> handler)
	{
		_all.AddOneTime(handler);
	}

	public virtual bool Remove(Action<T1, T2> handler, TEvent e)
	{
		return _all.Remove(handler);
	}

	public virtual bool Remove(Action<T1, T2> handler)
	{
		return _all.Remove(handler);
	}

	public virtual void Invoke(T1 arg1, T2 arg2, TEvent e)
	{
		_all.Invoke(arg1, arg2);
	}

	public virtual void Invoke(T1 arg1, T2 arg2)
	{
		_all.Invoke(arg1, arg2);
	}

	public virtual void Clear()
	{
		_all.Clear();
	}
}
