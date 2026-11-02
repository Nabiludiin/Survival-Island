using System;
using RG.Core.SaveSystem;
using RG.Parsecs.Common;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[CreateAssetMenu(fileName = "New Item Variable", menuName = "60 Parsecs!/Event Editor/Variables/Global/New item variable")]
public class GlobalItemVariable : Variable<IItem>, ISaveable
{
	private string _typeConnection = "Item";

	private const string GLOBAL_ITEM_VAR_PREFIX = "GlobalItemVar";

	public override string TypeConnection => _typeConnection;

	public override Type GetVariableType => typeof(IItem);

	public string ID => Guid;

	public override string GuidPrefix => "GlobalItemVar";

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
				Debug.Log($"Global item variable {name} has not set Save Event");
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
