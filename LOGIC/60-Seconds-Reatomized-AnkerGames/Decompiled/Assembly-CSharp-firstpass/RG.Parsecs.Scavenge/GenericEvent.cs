using System;
using System.Collections.Generic;

namespace RG.Parsecs.Scavenge;

public class GenericEvent
{
	private List<Action> _subscribers = new List<Action>();

	private List<Action> _oneTimeSubscribers = new List<Action>();

	private bool _isInvoking;

	private List<Action> _subscribersDelayedAdd = new List<Action>();

	private List<Action> _subscribersDelayedRemove = new List<Action>();

	private List<Action> _oneTimeSubscribersDelayedAdd = new List<Action>();

	public void Add(Action handler)
	{
		if (_isInvoking)
		{
			_subscribersDelayedAdd.Add(handler);
		}
		else
		{
			_subscribers.Add(handler);
		}
	}

	public void AddOneTime(Action handler)
	{
		if (_isInvoking)
		{
			_oneTimeSubscribersDelayedAdd.Add(handler);
		}
		else
		{
			_oneTimeSubscribers.Add(handler);
		}
	}

	public bool Remove(Action handler)
	{
		if (_isInvoking)
		{
			_subscribersDelayedRemove.Add(handler);
			if (!_subscribers.Contains(handler))
			{
				return _oneTimeSubscribers.Contains(handler);
			}
			return true;
		}
		if (!_subscribers.Remove(handler))
		{
			return _oneTimeSubscribers.Remove(handler);
		}
		return true;
	}

	public void Invoke()
	{
		_isInvoking = true;
		int count = _subscribers.Count;
		for (int i = 0; i < count; i++)
		{
			_subscribers[i]();
		}
		count = _oneTimeSubscribers.Count;
		for (int j = 0; j < count; j++)
		{
			_oneTimeSubscribers[j]();
		}
		_oneTimeSubscribers.Clear();
		_isInvoking = false;
		count = _subscribersDelayedAdd.Count;
		for (int k = 0; k < count; k++)
		{
			_subscribers.Add(_subscribersDelayedAdd[k]);
		}
		_subscribersDelayedAdd.Clear();
		count = _oneTimeSubscribersDelayedAdd.Count;
		for (int l = 0; l < count; l++)
		{
			_oneTimeSubscribers.Add(_oneTimeSubscribersDelayedAdd[l]);
		}
		_oneTimeSubscribersDelayedAdd.Clear();
		count = _subscribersDelayedRemove.Count;
		for (int m = 0; m < count; m++)
		{
			_subscribers.Remove(_subscribersDelayedRemove[m]);
			_oneTimeSubscribers.Remove(_subscribersDelayedRemove[m]);
		}
		_subscribersDelayedRemove.Clear();
	}

	public void Clear()
	{
		_subscribers.Clear();
		_oneTimeSubscribers.Clear();
		_subscribersDelayedRemove.Clear();
		_subscribersDelayedAdd.Clear();
		_oneTimeSubscribersDelayedAdd.Clear();
	}
}
public class GenericEvent<T>
{
	private List<Action<T>> _subscribers = new List<Action<T>>();

	private List<Action<T>> _oneTimeSubscribers = new List<Action<T>>();

	private bool _isInvoking;

	private List<Action<T>> _subscribersDelayedAdd = new List<Action<T>>();

	private List<Action<T>> _subscribersDelayedRemove = new List<Action<T>>();

	private List<Action<T>> _oneTimeSubscribersDelayedAdd = new List<Action<T>>();

	public int SubscribersCount => _subscribers.Count + _oneTimeSubscribers.Count;

	public void Add(Action<T> handler)
	{
		if (_isInvoking)
		{
			_subscribersDelayedAdd.Add(handler);
		}
		else
		{
			_subscribers.Add(handler);
		}
	}

	public bool Contains(Action<T> handler)
	{
		if (!_subscribers.Contains(handler))
		{
			return _oneTimeSubscribers.Contains(handler);
		}
		return true;
	}

	public void AddOneTime(Action<T> handler)
	{
		if (_isInvoking)
		{
			_oneTimeSubscribersDelayedAdd.Add(handler);
		}
		else
		{
			_oneTimeSubscribers.Add(handler);
		}
	}

	public bool Remove(Action<T> handler)
	{
		if (_isInvoking)
		{
			_subscribersDelayedRemove.Add(handler);
			if (!_subscribers.Contains(handler))
			{
				return _oneTimeSubscribers.Contains(handler);
			}
			return true;
		}
		if (!_subscribers.Remove(handler))
		{
			return _oneTimeSubscribers.Remove(handler);
		}
		return true;
	}

	public int RemoveAllThatTarget(object target)
	{
		return _subscribers.RemoveAll((Action<T> handler) => handler.Target == target) + _oneTimeSubscribers.RemoveAll((Action<T> handler) => handler.Target == target);
	}

	public void Invoke(T arg)
	{
		_isInvoking = true;
		int count = _subscribers.Count;
		for (int i = 0; i < count; i++)
		{
			_subscribers[i](arg);
		}
		count = _oneTimeSubscribers.Count;
		for (int j = 0; j < count; j++)
		{
			_oneTimeSubscribers[j](arg);
		}
		_oneTimeSubscribers.Clear();
		_isInvoking = false;
		count = _subscribersDelayedAdd.Count;
		for (int k = 0; k < count; k++)
		{
			_subscribers.Add(_subscribersDelayedAdd[k]);
		}
		_subscribersDelayedAdd.Clear();
		count = _oneTimeSubscribersDelayedAdd.Count;
		for (int l = 0; l < count; l++)
		{
			_oneTimeSubscribers.Add(_oneTimeSubscribersDelayedAdd[l]);
		}
		_oneTimeSubscribersDelayedAdd.Clear();
		count = _subscribersDelayedRemove.Count;
		for (int m = 0; m < count; m++)
		{
			_subscribers.Remove(_subscribersDelayedRemove[m]);
			_oneTimeSubscribers.Remove(_subscribersDelayedRemove[m]);
		}
		_subscribersDelayedRemove.Clear();
	}

	public void Clear()
	{
		_subscribers.Clear();
		_oneTimeSubscribers.Clear();
		_subscribersDelayedRemove.Clear();
		_subscribersDelayedAdd.Clear();
		_oneTimeSubscribersDelayedAdd.Clear();
	}
}
public class GenericEvent<T1, T2>
{
	private List<Action<T1, T2>> _subscribers = new List<Action<T1, T2>>();

	private List<Action<T1, T2>> _oneTimeSubscribers = new List<Action<T1, T2>>();

	private bool _isInvoking;

	private List<Action<T1, T2>> _subscribersDelayedRemove = new List<Action<T1, T2>>();

	private List<Action<T1, T2>> _subscribersDelayedAdd = new List<Action<T1, T2>>();

	private List<Action<T1, T2>> _oneTimeSubscribersDelayedAdd = new List<Action<T1, T2>>();

	public void Add(Action<T1, T2> handler)
	{
		if (_isInvoking)
		{
			_subscribersDelayedAdd.Add(handler);
		}
		else
		{
			_subscribers.Add(handler);
		}
	}

	public void AddOneTime(Action<T1, T2> handler)
	{
		if (_isInvoking)
		{
			_oneTimeSubscribersDelayedAdd.Add(handler);
		}
		else
		{
			_oneTimeSubscribers.Add(handler);
		}
	}

	public bool Remove(Action<T1, T2> handler)
	{
		if (_isInvoking)
		{
			_subscribersDelayedRemove.Add(handler);
			if (!_subscribers.Contains(handler))
			{
				return _oneTimeSubscribers.Contains(handler);
			}
			return true;
		}
		if (!_subscribers.Remove(handler))
		{
			return _oneTimeSubscribers.Remove(handler);
		}
		return true;
	}

	public void Invoke(T1 arg1, T2 arg2)
	{
		_isInvoking = true;
		int count = _subscribers.Count;
		for (int i = 0; i < count; i++)
		{
			_subscribers[i](arg1, arg2);
		}
		count = _oneTimeSubscribers.Count;
		for (int j = 0; j < count; j++)
		{
			_oneTimeSubscribers[j](arg1, arg2);
		}
		_oneTimeSubscribers.Clear();
		_isInvoking = false;
		count = _subscribersDelayedAdd.Count;
		for (int k = 0; k < count; k++)
		{
			_subscribers.Add(_subscribersDelayedAdd[k]);
		}
		_subscribersDelayedAdd.Clear();
		count = _oneTimeSubscribersDelayedAdd.Count;
		for (int l = 0; l < count; l++)
		{
			_oneTimeSubscribers.Add(_oneTimeSubscribersDelayedAdd[l]);
		}
		_oneTimeSubscribersDelayedAdd.Clear();
		count = _subscribersDelayedRemove.Count;
		for (int m = 0; m < count; m++)
		{
			_subscribers.Remove(_subscribersDelayedRemove[m]);
			_oneTimeSubscribers.Remove(_subscribersDelayedRemove[m]);
		}
		_subscribersDelayedRemove.Clear();
	}

	public void Clear()
	{
		_subscribers.Clear();
		_oneTimeSubscribers.Clear();
		_subscribersDelayedRemove.Clear();
		_subscribersDelayedAdd.Clear();
		_oneTimeSubscribersDelayedAdd.Clear();
	}
}
public class GenericEvent<T1, T2, T3>
{
	private List<Action<T1, T2, T3>> _subscribers = new List<Action<T1, T2, T3>>();

	private List<Action<T1, T2, T3>> _oneTimeSubscribers = new List<Action<T1, T2, T3>>();

	private bool _isInvoking;

	private List<Action<T1, T2, T3>> _subscribersDelayedRemove = new List<Action<T1, T2, T3>>();

	private List<Action<T1, T2, T3>> _subscribersDelayedAdd = new List<Action<T1, T2, T3>>();

	private List<Action<T1, T2, T3>> _oneTimeSubscribersDelayedAdd = new List<Action<T1, T2, T3>>();

	public void Add(Action<T1, T2, T3> handler)
	{
		if (_isInvoking)
		{
			_subscribersDelayedAdd.Add(handler);
		}
		else
		{
			_subscribers.Add(handler);
		}
	}

	public void AddOneTime(Action<T1, T2, T3> handler)
	{
		if (_isInvoking)
		{
			_oneTimeSubscribersDelayedAdd.Add(handler);
		}
		else
		{
			_oneTimeSubscribers.Add(handler);
		}
	}

	public bool Remove(Action<T1, T2, T3> handler)
	{
		if (_isInvoking)
		{
			_subscribersDelayedRemove.Add(handler);
			if (!_subscribers.Contains(handler))
			{
				return _oneTimeSubscribers.Contains(handler);
			}
			return true;
		}
		if (!_subscribers.Remove(handler))
		{
			return _oneTimeSubscribers.Remove(handler);
		}
		return true;
	}

	public void Invoke(T1 arg1, T2 arg2, T3 arg3)
	{
		_isInvoking = true;
		int count = _subscribers.Count;
		for (int i = 0; i < count; i++)
		{
			_subscribers[i](arg1, arg2, arg3);
		}
		count = _oneTimeSubscribers.Count;
		for (int j = 0; j < count; j++)
		{
			_oneTimeSubscribers[j](arg1, arg2, arg3);
		}
		_oneTimeSubscribers.Clear();
		_isInvoking = false;
		count = _subscribersDelayedAdd.Count;
		for (int k = 0; k < count; k++)
		{
			_subscribers.Add(_subscribersDelayedAdd[k]);
		}
		_subscribersDelayedAdd.Clear();
		count = _oneTimeSubscribersDelayedAdd.Count;
		for (int l = 0; l < count; l++)
		{
			_oneTimeSubscribers.Add(_oneTimeSubscribersDelayedAdd[l]);
		}
		_oneTimeSubscribersDelayedAdd.Clear();
		count = _subscribersDelayedRemove.Count;
		for (int m = 0; m < count; m++)
		{
			_subscribers.Remove(_subscribersDelayedRemove[m]);
			_oneTimeSubscribers.Remove(_subscribersDelayedRemove[m]);
		}
		_subscribersDelayedRemove.Clear();
	}

	public void Clear()
	{
		_subscribers.Clear();
		_oneTimeSubscribers.Clear();
		_subscribersDelayedRemove.Clear();
		_subscribersDelayedAdd.Clear();
		_oneTimeSubscribersDelayedAdd.Clear();
	}
}
