using System;
using RG.Core.Data.Abstractions;
using RG.Core.Data.DataTypes;
using UnityEngine;

namespace RG.Core.Data.Specialisations;

[Serializable]
[CreateAssetMenu(fileName = "SettingsData", menuName = "RG/Data/Core/SettingsData", order = 1)]
public class SettingsData : DataObject, ISerializable
{
	[SerializeField]
	private readonly int _versionId;

	[SerializeField]
	private readonly int _saveGameId;

	[SerializeField]
	private readonly string _versionInfo = string.Empty;

	[SerializeField]
	private DualDictionary _entries = new DualDictionary();

	public int VersionId => _versionId;

	public string VersionInfo => _versionInfo;

	public int SaveGameId => _saveGameId;

	public override void ResetData()
	{
		_entries.Reset();
	}

	public void Load()
	{
		_entries.Load();
	}

	public void Save()
	{
		_entries.Save();
	}

	public string GetConstString(string key)
	{
		return _entries.GetConstString(key);
	}

	public int GetConstInt(string key)
	{
		return _entries.GetConstInt(key);
	}

	public float GetConstFloat(string key)
	{
		return _entries.GetConstFloat(key);
	}

	public string GetVarString(string key)
	{
		return _entries.GetVarString(key);
	}

	public float GetVarFloat(string key)
	{
		return _entries.GetVarFloat(key);
	}

	public int GetVarInt(string key)
	{
		return _entries.GetVarInt(key);
	}

	public void SetVarString(string key, string value)
	{
		_entries.SetVarString(key, value);
	}

	public void SetVarFloat(string key, float value)
	{
		_entries.SetVarFloat(key, value);
	}

	public void SetVarInt(string key, int value)
	{
		_entries.SetVarInt(key, value);
	}
}
