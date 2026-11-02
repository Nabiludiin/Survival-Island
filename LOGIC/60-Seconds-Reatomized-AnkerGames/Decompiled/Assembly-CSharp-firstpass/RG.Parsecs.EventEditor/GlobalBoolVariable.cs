using System;
using RG.Core.SaveSystem;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[CreateAssetMenu(fileName = "New Bool Variable", menuName = "60 Parsecs!/Event Editor/Variables/Global/New bool variable")]
public class GlobalBoolVariable : Variable<bool>, ISaveable
{
	private string _typeConnection = "Bool";

	private const string GLOBAL_BOOL_VAR_PREFIX = "GlobalBoolVar";

	public override string TypeConnection => _typeConnection;

	public override Type GetVariableType => typeof(bool);

	public string ID => Guid;

	public override string GuidPrefix => "GlobalBoolVar";

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
				Debug.Log($"Global bool variable {name} has not set Save Event");
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
