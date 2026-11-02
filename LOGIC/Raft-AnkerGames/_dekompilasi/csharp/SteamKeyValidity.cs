using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SteamKeyValidity : MonoBehaviour
{
	private SteamKeyRGD data;

	private string path;

	private void Awake()
	{
		path = Application.streamingAssetsPath + "/Data";
		LoadKeys();
	}

	public SteamKeyResult CheckKey(string inputKey)
	{
		if (data == null || data.allKeys == null || data.allKeys.Count == 0)
		{
			LoadKeys();
			return SteamKeyResult.NoKeysAvailable;
		}
		if (inputKey.Length != 17)
		{
			return SteamKeyResult.WrongFormat;
		}
		string[] array = inputKey.Split('-');
		if (!LooksLikeSteamKey(array))
		{
			return SteamKeyResult.WrongFormat;
		}
		foreach (string allKey in data.allKeys)
		{
			if (allKey.Contains(array[0], StringComparison.OrdinalIgnoreCase) && allKey.Contains(array[2], StringComparison.OrdinalIgnoreCase))
			{
				return SteamKeyResult.Valid;
			}
		}
		return SteamKeyResult.NoMatch;
	}

	private bool LooksLikeSteamKey(string[] keySplit)
	{
		if (keySplit.Length != 3)
		{
			return false;
		}
		for (int i = 0; i < keySplit.Length; i++)
		{
			if (keySplit[i].Length != 5)
			{
				return false;
			}
		}
		return true;
	}

	private void CreateKeyRGD()
	{
		data = new SteamKeyRGD();
		StreamReader streamReader = new StreamReader(path + ".txt");
		_ = string.Empty;
		while (!streamReader.EndOfStream)
		{
			string[] array = streamReader.ReadLine().Split('-');
			if (LooksLikeSteamKey(array))
			{
				data.allKeys.Add(array[0] + "_" + array[2]);
			}
		}
		streamReader.Close();
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream fileStream = File.Create(path + ".rgd");
		binaryFormatter.Serialize(fileStream, data);
		fileStream.Close();
	}

	private void LoadKeyRGD()
	{
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream fileStream = File.Open(path + ".rgd", FileMode.Open);
		try
		{
			data = (SteamKeyRGD)binaryFormatter.Deserialize(fileStream);
		}
		catch (Exception message)
		{
			Debug.Log(message);
			fileStream.Close();
		}
		fileStream.Close();
	}

	private void LoadKeys()
	{
		bool flag = File.Exists(path + ".txt");
		bool flag2 = File.Exists(path + ".rgd");
		if (flag && !flag2)
		{
			CreateKeyRGD();
			File.Delete(path + ".txt");
			flag2 = File.Exists(path + ".rgd");
		}
		if (flag2)
		{
			LoadKeyRGD();
		}
	}
}
