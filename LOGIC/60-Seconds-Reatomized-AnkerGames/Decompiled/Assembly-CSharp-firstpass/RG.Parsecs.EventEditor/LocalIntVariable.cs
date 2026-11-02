using System;
using RG.Core.SaveSystem;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Serializable]
[CreateAssetMenu(fileName = "New Local Int Variable", menuName = "60 Parsecs!/Event Editor/Variables/Local/New int variable")]
public class LocalIntVariable : Variable<int>, ISaveable
{
	private string _typeConnection = "Int";

	[SerializeField]
	private ParsecsEvent _parsecsEvent;

	private const string LOCAL_INT_VAR_PREFIX = "LocalIntVar";

	public override string TypeConnection => _typeConnection;

	public override Type GetVariableType => typeof(int);

	public ParsecsEvent ParsecsEvent => _parsecsEvent;

	public string ID => Guid;

	public override string GuidPrefix => "LocalIntVar";

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
		return JsonUtility.ToJson(new IntWrapper
		{
			Value = Value
		});
	}

	public void Deserialize(string jsonData)
	{
		Value = JsonUtility.FromJson<IntWrapper>(jsonData).Value;
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
				Debug.Log($"Local int  variable {name} has not set Save Event");
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
