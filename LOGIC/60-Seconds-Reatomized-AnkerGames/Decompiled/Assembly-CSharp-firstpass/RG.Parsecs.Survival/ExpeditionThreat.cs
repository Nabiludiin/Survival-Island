using RG.Core.Base;
using RG.Core.SaveSystem;
using RG.Parsecs.EventEditor;
using UnityEngine;

namespace RG.Parsecs.Survival;

[CreateAssetMenu(menuName = "60 Parsecs!/Event Editor/Planets/New Threat")]
public class ExpeditionThreat : RGScriptableObject, ISaveable
{
	[Tooltip("Event to which data will be save and load")]
	[SerializeField]
	private SaveEvent _saveEvent;

	[SerializeField]
	[Tooltip("Static, design time data.")]
	private ExpeditionThreatStaticData _staticData;

	[SerializeField]
	[Tooltip("Runtime data that will be modified in game.")]
	private ExpeditionThreatRuntimeData _runtimeData;

	public SaveEvent SaveEvent
	{
		get
		{
			return _saveEvent;
		}
		set
		{
			_saveEvent = value;
		}
	}

	public ExpeditionThreatStaticData StaticData => _staticData;

	public ExpeditionThreatRuntimeData RuntimeData => _runtimeData;

	public string ID => Guid;

	private void OnEnable()
	{
		Register();
	}

	public string Serialize()
	{
		return JsonUtility.ToJson(new BoolWrapper
		{
			Value = _runtimeData.Enabled
		});
	}

	public void Deserialize(string jsonData)
	{
		BoolWrapper boolWrapper = JsonUtility.FromJson<BoolWrapper>(jsonData);
		_runtimeData.Enabled = boolWrapper.Value;
	}

	public void Register()
	{
		if ((bool)_saveEvent)
		{
			_saveEvent.RegisterListener(this);
		}
		else
		{
			Debug.Log($"Expedition Threat {name} has not set Save Event");
		}
	}

	public void Unregister()
	{
		if ((bool)_saveEvent)
		{
			_saveEvent.UnregisterListener(this);
		}
	}

	public void ResetData()
	{
		_runtimeData = new ExpeditionThreatRuntimeData
		{
			Enabled = _staticData.Enabled
		};
	}
}
