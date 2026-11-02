using I2.Loc;
using RG.Core.Base;
using RG.Core.SaveSystem;
using RG.Parsecs.EventEditor;
using UnityEngine;

namespace RG.Parsecs.Common;

[CreateAssetMenu(fileName = "New Achievement", menuName = "60 Parsecs!/Goal Editor/New Achievement")]
public class Achievement : RGScriptableObject, ISaveable
{
	[SerializeField]
	private SaveEvent _saveEvent;

	[SerializeField]
	private string _steamID;

	[SerializeField]
	private int _playstationID;

	[SerializeField]
	private int _xboxID;

	[SerializeField]
	private string _macAppStoreID;

	public LocalizedString title;

	public LocalizedString description;

	public Sprite icon;

	[SerializeField]
	private bool _isAchieved;

	private const string ACHIEVEMENT_PREFIX = "achievement";

	public bool IsAchieved => _isAchieved;

	public string SteamId => _steamID;

	public string MacAppStoreID => _macAppStoreID;

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

	public string ID => Guid;

	public override string GuidPrefix => "achievement";

	public void SetAsAchieved()
	{
		if (!_isAchieved)
		{
			_isAchieved = true;
		}
	}

	public void SetAsNotAchieved()
	{
		if (_isAchieved)
		{
			_isAchieved = false;
		}
	}

	private void OnDestroy()
	{
		Unregister();
	}

	private void OnEnable()
	{
		Register();
	}

	public string Serialize()
	{
		return JsonUtility.ToJson(new BoolWrapper
		{
			Value = _isAchieved
		});
	}

	public void Deserialize(string jsonData)
	{
		_isAchieved = JsonUtility.FromJson<BoolWrapper>(jsonData).Value;
	}

	public void Register()
	{
		if ((bool)_saveEvent)
		{
			_saveEvent.RegisterListener(this);
		}
		else
		{
			Debug.Log($"Achievement {name} has not set Save Event");
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
		_isAchieved = false;
	}
}
