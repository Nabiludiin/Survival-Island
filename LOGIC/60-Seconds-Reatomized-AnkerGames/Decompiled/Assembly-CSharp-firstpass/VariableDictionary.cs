using System;
using UnityEngine;

[Serializable]
public class VariableDictionary
{
	[SerializeField]
	private StringStringDictionary _stringVars = new StringStringDictionary();

	[SerializeField]
	private StringIntDictionary _intVars = new StringIntDictionary();

	[SerializeField]
	private StringFloatDictionary _floatVars = new StringFloatDictionary();

	public void Clear()
	{
		_stringVars.Clear();
		_intVars.Clear();
		_floatVars.Clear();
	}

	public void SetString(string key, string val)
	{
		if (_stringVars.ContainsKey(key))
		{
			_stringVars[key] = val;
		}
		else
		{
			_stringVars.Add(key, val);
		}
	}

	public void SetFloat(string key, float val)
	{
		if (_floatVars.ContainsKey(key))
		{
			_floatVars[key] = val;
		}
		else
		{
			_floatVars.Add(key, val);
		}
	}

	public void SetInt(string key, int val)
	{
		if (_intVars.ContainsKey(key))
		{
			_intVars[key] = val;
		}
		else
		{
			_intVars.Add(key, val);
		}
	}

	public bool GetString(string key, out string val)
	{
		if (_stringVars.ContainsKey(key))
		{
			val = _stringVars[key];
			return true;
		}
		val = null;
		return false;
	}

	public bool GetInt(string key, out int val)
	{
		if (_intVars.ContainsKey(key))
		{
			val = _intVars[key];
			return true;
		}
		val = 0;
		return false;
	}

	public bool GetFloat(string key, out float val)
	{
		if (_floatVars.ContainsKey(key))
		{
			val = _floatVars[key];
			return true;
		}
		val = 0f;
		return false;
	}

	public string GetString(string key)
	{
		if (_stringVars.ContainsKey(key))
		{
			return _stringVars[key];
		}
		return null;
	}

	public float GetFloat(string key)
	{
		if (_floatVars.ContainsKey(key))
		{
			return _floatVars[key];
		}
		return 0f;
	}

	public int GetInt(string key)
	{
		if (_intVars.ContainsKey(key))
		{
			return _intVars[key];
		}
		return 0;
	}
}
