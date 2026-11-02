using System;
using RG.Core.SaveSystem;
using Steamworks;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[CreateAssetMenu(fileName = "New int variable", menuName = "60 Parsecs!/Event Editor/Variables/Global/New int variable")]
public class GlobalIntVariable : Variable<int>, ISaveable
{
	private string _typeConnection = "Int";

	[SerializeField]
	private string _steamVariableId;

	private const string GLOBAL_INT_VAR_PREFIX = "GlobalIntVar";

	public override string TypeConnection => _typeConnection;

	public override Type GetVariableType => typeof(int);

	public string ID => Guid;

	public override string GuidPrefix => "GlobalIntVar";

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
		return JsonUtility.ToJson(new IntWrapper
		{
			Value = Value
		});
	}

	public void Deserialize(string jsonData)
	{
		Value = JsonUtility.FromJson<IntWrapper>(jsonData).Value;
		if (_steamVariableId == null)
		{
			return;
		}
		SteamUserStats.RequestCurrentStats();
		int pData = Value;
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
				Debug.Log($"Global int  variable {name} has not set Save Event");
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
