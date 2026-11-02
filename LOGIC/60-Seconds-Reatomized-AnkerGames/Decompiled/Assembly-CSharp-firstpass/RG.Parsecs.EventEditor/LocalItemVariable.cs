using System;
using RG.Core.SaveSystem;
using RG.Parsecs.Common;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Serializable]
[CreateAssetMenu(fileName = "New Local Item Variable", menuName = "60 Parsecs!/Event Editor/Variables/Local/New item variable")]
public class LocalItemVariable : Variable<IItem>, ISaveable
{
	private string _typeConnection = "Item";

	[SerializeField]
	private ParsecsEvent _parsecsEvent;

	private const string LOCAL_ITEM_VAR_PREFIX = "LocalItemVar";

	public override string TypeConnection => _typeConnection;

	public override Type GetVariableType => typeof(IItem);

	public ParsecsEvent ParsecsEvent => _parsecsEvent;

	public string ID => Guid;

	public override string GuidPrefix => "LocalItemVar";

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

	public void ResetData()
	{
		Value = _initialValue;
	}

	public string Serialize()
	{
		return JsonUtility.ToJson(new StringWrapper
		{
			Value = ((Value != null) ? Value.BaseStaticData.ItemId : string.Empty)
		});
	}

	public void Deserialize(string jsonData)
	{
		StringWrapper stringWrapper = JsonUtility.FromJson<StringWrapper>(jsonData);
		if (!string.IsNullOrEmpty(stringWrapper.Value))
		{
			Value = (IItem)_saveEvent.GetReferenceObjectByID(stringWrapper.Value);
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
				Debug.Log($"Local item variable {name} has not set Save Event");
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
}
