using System;
using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Serializable]
[CreateAssetMenu(menuName = "60 Parsecs!/New SurvivalEvent Reference List", fileName = "New SurvivalEvent Reference List")]
public class SurvivalEventReferenceList : ObjectsReferenceList
{
	[SerializeField]
	private List<SurvivalEvent> _events = new List<SurvivalEvent>();

	private Dictionary<string, UnityEngine.Object> _objects;

	public List<SurvivalEvent> Events
	{
		get
		{
			return _events;
		}
		set
		{
			_events = value;
		}
	}

	private void OnEnable()
	{
		Build();
	}

	private void OnDestroy()
	{
		Destroy();
	}

	public override void Build()
	{
		_objects = new Dictionary<string, UnityEngine.Object>();
		for (int i = 0; i < _events.Count; i++)
		{
			if (_events[i] == null)
			{
				Debug.LogWarning("Null event in SurvivalEventReferenceList");
			}
			else if (_objects.ContainsKey(_events[i].ID))
			{
				Debug.LogError($"Duplicate ID. Object in dictionary: {_objects[_events[i].ID].name}.");
			}
			else
			{
				_objects.Add(_events[i].ID, _events[i]);
			}
		}
	}

	public override void Destroy()
	{
		if (_objects != null)
		{
			_objects.Clear();
		}
	}

	public override UnityEngine.Object GetReferenceObjectByGUID(string id)
	{
		if (_objects.ContainsKey(id))
		{
			return _objects[id];
		}
		throw new Exception($"There is no {id} in dictionary");
	}
}
