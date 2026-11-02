using System.Collections.Generic;
using System.Text;
using RG.Utils;
using UnityEngine;

namespace RG.Core.SaveSystem;

[CreateAssetMenu(fileName = "SaveEventList", menuName = "RGCore/Save System/Create save event")]
public class SaveEvent : ScriptableObject
{
	internal struct SaveVersionWrapper
	{
		public int Version;
	}

	[SerializeField]
	private bool _addSaveFileVersion = true;

	[SerializeField]
	private bool _useEncryption = true;

	[SerializeField]
	private string _dataTag;

	[SerializeField]
	private int _saveVersion = 1;

	[SerializeField]
	private ObjectsReferenceList _objectsReferenceList;

	private const string SAVE_FILE_VERSION = "SAVE_VERSION";

	private int _actualSaveVersion = 4;

	private bool _saveLoaded;

	private Dictionary<string, ISaveable> _observators;

	private Dictionary<string, string> _loadedData;

	public int SaveVersion => _actualSaveVersion;

	public bool UseEncryption => _useEncryption;

	public string DataTag => _dataTag;

	public void RegisterListener(ISaveable observer)
	{
		if (_observators == null)
		{
			_observators = new Dictionary<string, ISaveable>();
		}
		observer.ResetData();
		if (!_observators.ContainsKey(observer.ID))
		{
			_observators.Add(observer.ID, observer);
			if (_loadedData != null && _loadedData.ContainsKey(observer.ID) && _saveLoaded)
			{
				observer.Deserialize(_loadedData[observer.ID]);
			}
		}
		else
		{
			Debug.LogError("There is an object with this ID: " + observer.ID);
		}
	}

	public void UnregisterListener(ISaveable observer)
	{
		_observators.Remove(observer.ID);
	}

	public void SetLoadedData(string jsonData)
	{
		_loadedData = JSONUtilities.SplitJSON(jsonData);
		if (_objectsReferenceList != null)
		{
			_objectsReferenceList.Build();
		}
	}

	public Object GetReferenceObjectByID(string id)
	{
		return _objectsReferenceList.GetReferenceObjectByGUID(id);
	}

	public string CollectData()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("{");
		foreach (KeyValuePair<string, ISaveable> observator in _observators)
		{
			stringBuilder.Append(observator.Key);
			stringBuilder.Append(":");
			stringBuilder.Append(observator.Value.Serialize());
			stringBuilder.Append(",");
		}
		if (_addSaveFileVersion)
		{
			string value = "SAVE_VERSION:{\"Version\":" + _saveVersion + "},";
			stringBuilder.Append(value);
		}
		stringBuilder.Remove(stringBuilder.Length - 1, 1);
		stringBuilder.Append("}");
		return stringBuilder.ToString();
	}

	public void SendData()
	{
		_saveLoaded = true;
		foreach (KeyValuePair<string, string> loadedDatum in _loadedData)
		{
			if (loadedDatum.Key == "SAVE_VERSION")
			{
				_actualSaveVersion = JsonUtility.FromJson<SaveVersionWrapper>(loadedDatum.Value).Version;
			}
			else if (_observators.ContainsKey(loadedDatum.Key))
			{
				_observators[loadedDatum.Key].Deserialize(loadedDatum.Value);
			}
		}
	}

	public void FinishLoad()
	{
		if (_objectsReferenceList != null)
		{
			_objectsReferenceList.Destroy();
		}
	}

	public void ResetGame()
	{
		_saveLoaded = false;
		foreach (KeyValuePair<string, ISaveable> observator in _observators)
		{
			observator.Value.ResetData();
		}
	}
}
