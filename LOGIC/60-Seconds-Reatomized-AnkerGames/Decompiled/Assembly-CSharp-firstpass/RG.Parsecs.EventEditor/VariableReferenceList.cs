using System;
using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Serializable]
[CreateAssetMenu(menuName = "60 Parsecs!/New Variable Reference List", fileName = "New Variable Reference List")]
public class VariableReferenceList : ObjectsReferenceList
{
	[SerializeField]
	private List<Variable> _variables = new List<Variable>();

	private Dictionary<string, UnityEngine.Object> _objects;

	public List<Variable> Variables
	{
		get
		{
			return _variables;
		}
		set
		{
			_variables = value;
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
		for (int i = 0; i < _variables.Count; i++)
		{
			if (_variables[i] == null)
			{
				Debug.LogWarning("Null Variable in " + name);
			}
			else if (_objects.ContainsKey(_variables[i].name))
			{
				Debug.LogError($"Duplicate ID. Object in dictionary: {_objects[_variables[i].name].name}.");
			}
			else
			{
				_objects.Add(_variables[i].name, _variables[i]);
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
