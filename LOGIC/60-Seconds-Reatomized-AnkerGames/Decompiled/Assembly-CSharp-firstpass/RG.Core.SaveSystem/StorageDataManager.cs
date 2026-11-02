using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Steamworks;
using UnityEngine;

namespace RG.Core.SaveSystem;

public class StorageDataManager : MonoBehaviour
{
	public enum ESaveFileExists
	{
		Exists,
		NotExists,
		Damaged
	}

	[SerializeField]
	private bool _deleteBrokenSaves;

	public static bool SURVIVAL_SAVE_CORRUPTED = false;

	private static string strPassword = "M0n$t3rC0ucH32";

	private static string strSalt = "V@1H@11@N4";

	private const int Iterations = 555;

	private ConsoleUser _consoleUser;

	public List<SaveEvent> SaveEventList = new List<SaveEvent>();

	public static StorageDataManager TheInstance;

	public static string SETTINGS_FILENAME = "settings";

	public static string STATS_FILENAME = "GlobalGameData";

	private bool _initialized;

	public ConsoleUser ConsoleUser => _consoleUser;

	public bool Initialized => _initialized;

	private void Awake()
	{
		if (TheInstance == null)
		{
			TheInstance = this;
			UnityEngine.Object.DontDestroyOnLoad(this);
		}
		else
		{
			Debug.LogError("Other StorageDataManager on scene");
			UnityEngine.Object.Destroy(this);
		}
	}

	private void NotifySaveEvent(string tag, string content)
	{
		foreach (SaveEvent saveEvent in SaveEventList)
		{
			if (saveEvent.DataTag == tag)
			{
				if (saveEvent.UseEncryption)
				{
					content = Decrypt(content);
				}
				saveEvent.SetLoadedData(content);
				saveEvent.SendData();
				break;
			}
		}
	}

	private string CollectDataFromSaveEvent(string tag)
	{
		foreach (SaveEvent saveEvent in SaveEventList)
		{
			if (saveEvent.DataTag == tag)
			{
				string text = saveEvent.CollectData();
				if (saveEvent.UseEncryption)
				{
					text = Encrypt(text);
				}
				return text;
			}
		}
		Debug.LogError("Unable to find SaveEventList with name=" + tag);
		return "";
	}

	public void Initialize(ConsoleUser cu, Action result)
	{
		_consoleUser = cu;
		_initialized = true;
		Debug.Log("Storage data initialized with user id=" + cu.ConsolePlayerId);
		Load(SETTINGS_FILENAME, () =>
		{
			Load(STATS_FILENAME, result, useCloudSave: false);
		}, useCloudSave: false);
		CheckSurvivalSaveStatus();
	}

	public void Load(string fileName, Action result, bool useCloudSave, bool forceEvenWhenNotInitialized = false)
	{
		Load(fileName, fileName, result, useCloudSave, forceEvenWhenNotInitialized);
	}

	public void Load(string fileName, string saveEventTag, Action result, bool useCloudSave, bool forceEvenWhenNotInitialized = false)
	{
		ExistsAsync(fileName, (ESaveFileExists existsResult) =>
		{
			switch (existsResult)
			{
			case ESaveFileExists.Exists:
				LoadFromFileAsync(fileName, (byte[] bytes) =>
				{
					string text = "";
					if (bytes.Length != 0)
					{
						text = Encoding.ASCII.GetString(bytes);
						try
						{
							NotifySaveEvent(saveEventTag, text);
						}
						catch (Exception)
						{
							if (_deleteBrokenSaves)
							{
								DeleteBrokenSave(fileName);
							}
						}
					}
					result();
				}, useCloudSave, forceEvenWhenNotInitialized);
				break;
			case ESaveFileExists.NotExists:
				result();
				break;
			case ESaveFileExists.Damaged:
				throw new NotImplementedException();
			}
		}, useCloudSave, forceEvenWhenNotInitialized);
	}

	public void CheckSurvivalSaveStatus()
	{
		Load("survivalGameplay", () =>
		{
			Debug.Log("Checked survival save status");
		}, useCloudSave: false);
	}

	public void DeleteBrokenSave(string fileName)
	{
		if (fileName == "survivalGameplay")
		{
			SURVIVAL_SAVE_CORRUPTED = true;
		}
		DeleteAsync(fileName, () =>
		{
			Debug.Log("Deleted broken save " + fileName);
		});
	}

	public void Save(string fileName, Action success, Action failed, bool syncToCloud = true, bool forceEvenWhenNotInitialized = false)
	{
		Save(fileName, fileName, success, failed, syncToCloud, forceEvenWhenNotInitialized);
	}

	public void Save(string fileName, string saveEventTag, Action success, Action failed, bool syncToCloud = true, bool forceEvenWhenNotInitialized = false)
	{
		string s = CollectDataFromSaveEvent(saveEventTag);
		byte[] bytes = Encoding.ASCII.GetBytes(s);
		WriteToFileAsync(bytes, fileName, success, failed, syncToCloud, forceEvenWhenNotInitialized);
	}

	private void WriteToFileAsync(byte[] content, string fileName, Action success, Action failed, bool syncToCloud = true, bool forceEvenWhenNotInitialized = false)
	{
		if (!_initialized && !forceEvenWhenNotInitialized)
		{
			Debug.LogError("Storage data not initialized");
			return;
		}
		if (!SteamManager.Initialized)
		{
			Debug.LogError("Steamworks not initialized");
			return;
		}
		if (syncToCloud)
		{
			SteamRemoteStorage.FileWrite(fileName, content, content.Length);
			success?.Invoke();
			return;
		}
		try
		{
			File.WriteAllBytes(Application.persistentDataPath + "//" + fileName, content);
			success();
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.Message);
			failed();
		}
	}

	private void LoadFromFileAsync(string fileName, Action<byte[]> result, bool useCloudSave, bool forceEvenWhenNotInitialized = false)
	{
		if (!_initialized && !forceEvenWhenNotInitialized)
		{
			Debug.LogError("Storage data not initialized");
			return;
		}
		if (!SteamManager.Initialized)
		{
			Debug.LogError("Steamworks not initialized");
			return;
		}
		int fileSize = SteamRemoteStorage.GetFileSize(fileName);
		byte[] array = new byte[fileSize];
		SteamRemoteStorage.FileRead(fileName, array, fileSize);
		result?.Invoke(array);
	}

	public void ExistsAsync(string fileName, Action<ESaveFileExists> result, bool checkInCloud, bool forceEvenWhenNotInitialized = false)
	{
		if (!_initialized && !forceEvenWhenNotInitialized)
		{
			Debug.LogError("Storage data not initialized");
		}
		else if (!SteamManager.Initialized)
		{
			Debug.LogError("Steamworks not initialized");
		}
		else if (SteamRemoteStorage.FileExists(fileName))
		{
			result(ESaveFileExists.Exists);
		}
		else
		{
			result(ESaveFileExists.NotExists);
		}
	}

	public void DeleteAsync(string fileName, Action result)
	{
		if (!_initialized)
		{
			Debug.LogError("Storage data not initialized");
			return;
		}
		if (!SteamManager.Initialized)
		{
			Debug.LogError("Steamworks not initialized");
			return;
		}
		SteamRemoteStorage.FileDelete(fileName);
		result?.Invoke();
	}

	private static string Encrypt(string strPlain)
	{
		try
		{
			DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
			byte[] bytes = new Rfc2898DeriveBytes(strPassword, GetIV(), 555).GetBytes(8);
			using MemoryStream memoryStream = new MemoryStream();
			using CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateEncryptor(bytes, GetIV()), CryptoStreamMode.Write);
			memoryStream.Write(GetIV(), 0, GetIV().Length);
			byte[] bytes2 = Encoding.UTF8.GetBytes(strPlain);
			cryptoStream.Write(bytes2, 0, bytes2.Length);
			cryptoStream.FlushFinalBlock();
			return Convert.ToBase64String(memoryStream.ToArray());
		}
		catch (Exception ex)
		{
			Debug.LogWarning("Encrypt Exception: " + ex);
			return strPlain;
		}
	}

	private static byte[] GetIV()
	{
		return Encoding.UTF8.GetBytes(strSalt);
	}

	private static string Decrypt(string strEncript)
	{
		try
		{
			using MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(strEncript));
			DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
			byte[] iV = GetIV();
			memoryStream.Read(iV, 0, iV.Length);
			byte[] bytes = new Rfc2898DeriveBytes(strPassword, iV, 555).GetBytes(8);
			using CryptoStream stream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateDecryptor(bytes, iV), CryptoStreamMode.Read);
			using StreamReader streamReader = new StreamReader(stream);
			return streamReader.ReadToEnd();
		}
		catch (Exception ex)
		{
			Debug.LogWarning("Decrypt Exception: " + ex);
			return strEncript;
		}
	}
}
