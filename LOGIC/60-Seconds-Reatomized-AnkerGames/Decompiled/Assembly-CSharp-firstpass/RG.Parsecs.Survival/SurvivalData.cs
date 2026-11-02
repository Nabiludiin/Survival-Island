using RG.Core.Base;
using RG.Core.SaveSystem;
using UnityEngine;

namespace RG.Parsecs.Survival;

[CreateAssetMenu(menuName = "60 Parsecs!/New Survival Data")]
public class SurvivalData : RGScriptableObject, ISaveable
{
	private const string SAVE_ID = "SO_survivalData";

	public int CurrentDay;

	public bool DailyEventResolved;

	public bool LandedOnPlanet;

	private int _daysToSkip;

	private bool _starlogOpened;

	[SerializeField]
	private SaveEvent _saveEvent;

	public int DisplayDay => CurrentDay + _daysToSkip;

	public int DaysToSkip
	{
		get
		{
			return _daysToSkip;
		}
		set
		{
			_daysToSkip = value;
		}
	}

	public bool StarlogOpenedOnFirstDay
	{
		get
		{
			if (!_starlogOpened)
			{
				return CurrentDay > 1;
			}
			return true;
		}
	}

	public bool StarlogOpened
	{
		get
		{
			return _starlogOpened;
		}
		set
		{
			_starlogOpened = value;
		}
	}

	public bool ShouldLandOnPlanet { get; set; }

	public string ID => Guid;

	private void OnEnable()
	{
		Register();
	}

	private void OnDestroy()
	{
		Unregister();
	}

	public string Serialize()
	{
		return JsonUtility.ToJson(new SurvivalDataWrapper
		{
			CurrentDay = CurrentDay,
			DailyEventResolved = DailyEventResolved,
			LandedOnPlanet = LandedOnPlanet
		});
	}

	public void Deserialize(string jsonData)
	{
		SurvivalDataWrapper survivalDataWrapper = JsonUtility.FromJson<SurvivalDataWrapper>(jsonData);
		CurrentDay = survivalDataWrapper.CurrentDay;
		LandedOnPlanet = survivalDataWrapper.LandedOnPlanet;
		DailyEventResolved = survivalDataWrapper.DailyEventResolved;
	}

	public void Register()
	{
		_saveEvent.RegisterListener(this);
	}

	public void Unregister()
	{
		_saveEvent.UnregisterListener(this);
	}

	public void ResetData()
	{
		CurrentDay = 1;
		DailyEventResolved = false;
		_starlogOpened = false;
		LandedOnPlanet = false;
		ShouldLandOnPlanet = false;
	}
}
