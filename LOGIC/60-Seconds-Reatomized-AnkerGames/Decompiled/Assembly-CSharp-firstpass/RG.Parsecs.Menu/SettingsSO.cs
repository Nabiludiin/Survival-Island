using System;
using RG.Core.Base;
using RG.Core.SaveSystem;
using UnityEngine;

namespace RG.Parsecs.Menu;

[CreateAssetMenu(fileName = "Settings", menuName = "RGCore/Save System/Settings")]
public class SettingsSO : RGScriptableObject, ISaveable
{
	public SaveEvent SaveEvent;

	[SerializeField]
	private string _id;

	public GameSettings staticData;

	public GameSettings runtimeData;

	public string ID => Guid;

	public void ResetData()
	{
		runtimeData = staticData;
	}

	private void OnEnable()
	{
		Register();
	}

	public void DefaultData()
	{
		throw new NotImplementedException();
	}

	public void Deserialize(string jsonData)
	{
		GameSettings objectToOverwrite = new GameSettings();
		JsonUtility.FromJsonOverwrite(jsonData, objectToOverwrite);
		runtimeData = objectToOverwrite;
	}

	public void Register()
	{
		SaveEvent.RegisterListener(this);
	}

	public string Serialize()
	{
		return JsonUtility.ToJson(runtimeData);
	}

	public void Unregister()
	{
		SaveEvent.UnregisterListener(this);
	}
}
