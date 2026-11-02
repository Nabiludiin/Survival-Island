using System;
using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
public class EventContentList
{
	[SerializeField]
	private List<EventContent> _eventContents;

	[SerializeField]
	private int _day;

	[SerializeField]
	private Transform _holder;

	public int Day => _day;

	public Transform Holder => _holder;

	public List<EventContent> EventContents => _eventContents;

	public EventContentList(int day, Transform holder)
	{
		_eventContents = new List<EventContent>();
		_day = day;
		_holder = holder;
	}

	public void Set(int index, EventContent content)
	{
		_eventContents[index] = content;
	}

	public void Add(EventContent content)
	{
		_eventContents.Add(content);
	}

	public EventContent GetLastContent()
	{
		return _eventContents[_eventContents.Count - 1];
	}

	public EventContent GetSecondContentFromEnd()
	{
		return _eventContents[_eventContents.Count - 2];
	}
}
