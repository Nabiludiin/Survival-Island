using System;
using RG.Core.SaveSystem;
using Steamworks;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[CreateAssetMenu(fileName = "New Float Variable", menuName = "60 Parsecs!/Event Editor/Variables/Global/New float variable")]
public class GlobalFloatVariable : Variable<float>, ISaveable
{
	[SerializeField]
	private string _steamVariableId;

	private string _typeConnection = "Float";

	private const string GLOBAL_FLOAT_VAR_PREFIX = "GlobalFloatVar";

	public override string TypeConnection => _typeConnection;

	public override Type GetVariableType => typeof(float);

	public string ID => Guid;

	public override string GuidPrefix => "GlobalFloatVar";

	public override object GetValue()
	{
		return Value;
	}

	public override void SetValue(object o)
	{
		if (_steamVariableId != null)
		{
			SteamUserStats.SetStat(_steamVariableId, Value);
			SteamUserStats.StoreStats();
		}
		base.SetValue(o);
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
		if (string.IsNullOrEmpty(_steamVariableId))
		{
			return;
		}
		SteamUserStats.RequestCurrentStats();
		float pData = Value;
		SteamUserStats.GetStat(_steamVariableId, out pData);
		if (!Mathf.Approximately(pData, Value))
		{
			if (pData > Value)
			{
				Value = pData;
				return;
			}
			SteamUserStats.SetStat(_steamVariableId, Value);
			SteamUserStats.StoreStats();
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
				Debug.Log($"Global float variable {name} has not set Save Event");
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
