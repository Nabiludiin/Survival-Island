using System;
using RG.Core.SaveSystem;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Serializable]
[CreateAssetMenu(fileName = "New Local Float Variable", menuName = "60 Parsecs!/Event Editor/Variables/Local/New float variable")]
public class LocalFloatVariable : Variable<float>, ISaveable
{
	private string _typeConnection = "Float";

	[SerializeField]
	private ParsecsEvent _parsecsEvent;

	private const string LOCAL_FLOAT_VAR_PREFIX = "LocalFloatVar";

	public override string TypeConnection => _typeConnection;

	public override Type GetVariableType => typeof(float);

	public ParsecsEvent ParsecsEvent => _parsecsEvent;

	public string ID => Guid;

	public override string GuidPrefix => "LocalFloatVar";

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
		return JsonUtility.ToJson(new FloatWrapper
		{
			Value = Value
		});
	}

	public void Deserialize(string jsonData)
	{
		Value = JsonUtility.FromJson<FloatWrapper>(jsonData).Value;
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
				Debug.Log($"Local float variable {name} has not set Save Event");
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
