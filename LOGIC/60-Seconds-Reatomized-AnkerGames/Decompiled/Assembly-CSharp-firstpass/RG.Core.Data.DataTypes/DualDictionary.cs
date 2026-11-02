using System;
using RG.Core.Data.Abstractions;
using UnityEngine;

namespace RG.Core.Data.DataTypes;

[Serializable]
public class DualDictionary : ISerializable
{
	[SerializeField]
	private VariableDictionary _values;

	[SerializeField]
	private VariableDictionary _constants;

	public void Reset()
	{
		_values.Clear();
		_constants.Clear();
	}

	public string GetConstString(string key)
	{
		return _constants.GetString(key);
	}

	public float GetConstFloat(string key)
	{
		return _constants.GetFloat(key);
	}

	public int GetConstInt(string key)
	{
		return _constants.GetInt(key);
	}

	public string GetVarString(string key)
	{
		return _values.GetString(key);
	}

	public float GetVarFloat(string key)
	{
		return _values.GetFloat(key);
	}

	public int GetVarInt(string key)
	{
		return _values.GetInt(key);
	}

	public void SetVarString(string key, string value)
	{
		_values.SetString(key, value);
	}

	public void SetVarFloat(string key, float value)
	{
		_values.SetFloat(key, value);
	}

	public void SetVarInt(string key, int value)
	{
		_values.SetInt(key, value);
	}

	public void Load()
	{
	}

	public void Save()
	{
	}
}
