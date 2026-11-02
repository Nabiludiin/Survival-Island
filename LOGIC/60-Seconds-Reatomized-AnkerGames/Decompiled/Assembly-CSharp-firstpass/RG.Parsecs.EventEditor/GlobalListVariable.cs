using System;
using System.Collections;
using RG.Core.SaveSystem;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[CreateAssetMenu(fileName = "New List Variable", menuName = "60 Parsecs!/Event Editor/Variables/Global/New list variable")]
public class GlobalListVariable : Variable<IList>, ISaveable
{
	private string _typeConnection = ListConnection.ID;

	private const string GLOBAL_LIST_VAR_PREFIX = "GlobalListVar";

	public override string TypeConnection => _typeConnection;

	public override Type GetVariableType => typeof(IList);

	public string ID => Guid;

	public override string GuidPrefix => "GlobalListVar";

	public override object GetValue()
	{
		return Value;
	}

	private void OnEnable()
	{
		Register();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		Unregister();
	}

	public string Serialize()
	{
		return JsonUtility.ToJson(default(ListWrapper));
	}

	public void Deserialize(string jsonData)
	{
		Value = JsonUtility.FromJson<ListWrapper>(jsonData).Value;
	}

	public void Register()
	{
		if (!_isStatic)
		{
			if ((bool)_saveEvent)
			{
				_saveEvent.RegisterListener(this);
			}
			else
			{
				Debug.Log($"Global List variable {name} has not set Save Event");
			}
		}
	}

	public void Unregister()
	{
		if (!_isStatic && (bool)_saveEvent)
		{
			_saveEvent.UnregisterListener(this);
		}
	}

	public void ResetData()
	{
		Value = _initialValue;
	}
}
