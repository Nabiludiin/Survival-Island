using System;
using RG.Core.SaveSystem;
using RG.Parsecs.Common;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[CreateAssetMenu(fileName = "New Characer Variable", menuName = "60 Parsecs!/Event Editor/Variables/Global/New character variable")]
public class GlobalCharacterVariable : Variable<Character>, ISaveable
{
	private string _typeConnection = "Character";

	private const string GLOBAL_CHAR_VAR_PREFIX = "GlobalCharacterVar";

	public override string TypeConnection => _typeConnection;

	public override Type GetVariableType => typeof(Character);

	public string ID => Guid;

	public override string GuidPrefix => "GlobalCharacterVar";

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
		return JsonUtility.ToJson(new StringWrapper
		{
			Value = ((Value != null) ? Value.ID : string.Empty)
		});
	}

	public void Deserialize(string jsonData)
	{
		StringWrapper stringWrapper = JsonUtility.FromJson<StringWrapper>(jsonData);
		if (!string.IsNullOrEmpty(stringWrapper.Value))
		{
			Value = (Character)_saveEvent.GetReferenceObjectByID(stringWrapper.Value);
		}
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
				Debug.Log($"Global character variable {name} has not set Save Event");
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
