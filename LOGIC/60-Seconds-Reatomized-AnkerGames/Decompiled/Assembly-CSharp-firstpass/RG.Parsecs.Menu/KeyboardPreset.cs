using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Menu;

[CreateAssetMenu(fileName = "Settings", menuName = "RGCore/Save System/KeyboardPreset")]
public class KeyboardPreset : ScriptableObject
{
	public List<StringKeyValue> StringKeyValueStore = new List<StringKeyValue>();

	public void SetString(string key, string value)
	{
		for (int i = 0; i < StringKeyValueStore.Count; i++)
		{
			if (StringKeyValueStore[i].Key == key)
			{
				StringKeyValueStore[i].Value = value;
				return;
			}
		}
		StringKeyValueStore.Add(new StringKeyValue
		{
			Key = key,
			Value = value
		});
	}

	public bool HasKey(string key)
	{
		for (int i = 0; i < StringKeyValueStore.Count; i++)
		{
			if (StringKeyValueStore[i].Key == key)
			{
				return true;
			}
		}
		return false;
	}

	public string GetString(string key)
	{
		for (int i = 0; i < StringKeyValueStore.Count; i++)
		{
			if (StringKeyValueStore[i].Key == key)
			{
				return StringKeyValueStore[i].Value;
			}
		}
		return null;
	}
}
