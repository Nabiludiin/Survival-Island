using System;
using System.Collections.Generic;
using RG.Core.SaveSystem;
using UnityEngine;

namespace RG.Parsecs.Menu;

[Serializable]
public class GameSettings
{
	public float musicVolume = 0.5f;

	public float soundVolume = 0.5f;

	public float masterVolume = 0.5f;

	public float gammaVal;

	public string currentLanguage = "";

	public string previousSteamLanguage = "";

	public string keyboardMap = "default";

	public string TWITTER_USER_ID;

	public string TWITTER_USER_SCREEN_NAME;

	public string TWITTER_USER_TOKEN;

	public string TWITTER_USER_TOKEN_SECRET;

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

	public void Save()
	{
		StorageDataManager.TheInstance.Save("settings", () =>
		{
			Debug.Log("done");
		}, null);
	}
}
