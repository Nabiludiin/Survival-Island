using System;
using RG.Core.SaveSystem;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Serializable]
[CreateAssetMenu(fileName = "New Local Bool Variable", menuName = "60 Parsecs!/Event Editor/Variables/Local/New bool variable")]
public class LocalBoolVariable : Variable<bool>, ISaveable
{
	private string _typeConnection = "Bool";

	[SerializeField]
	private ParsecsEvent _parsecsEvent;

	private const string LOCAL_BOOL_VAR_PREFIX = "LocalBoolVar";

	public override string TypeConnection => _typeConnection;

	public override Type GetVariableType => typeof(bool);

	public ParsecsEvent ParsecsEvent => _parsecsEvent;

	public string ID => Guid;

	public override string GuidPrefix => "LocalBoolVar";

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
		return JsonUtility.ToJson(new BoolWrapper
		{
			Value = Value
		});
	}

	public void Deserialize(string jsonData)
	{
		Value = JsonUtility.FromJson<BoolWrapper>(jsonData).Value;
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
				Debug.Log($"Local bool variable {name} has not set Save Event");
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
