using System.Collections.Generic;
using RG.Parsecs.Common;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class ExpeditionManager : MonoBehaviour
{
	protected const int REVOKE_EXPEDITION_LISTENER_LIST_PRIORITY = 10;

	protected const int LAND_ON_PLANET_LISTENER_LIST_PRIORITY = 25;

	[SerializeField]
	[Header("Data")]
	[Tooltip("Expedition Data scriptable object holding informations about expedition system.")]
	protected ExpeditionData _expeditionData;

	[Tooltip("Character List scriptable object holding informations about all available characters.")]
	[SerializeField]
	private CharacterList _characterList;

	[Tooltip("Item List scriptable object holding informations about all available items.")]
	[SerializeField]
	private ItemList _itemList;

	[Tooltip("Threats List scriptable object holding informations about all available items.")]
	[SerializeField]
	private ThreatsList _threatList;

	[Tooltip("Destinations List scriptable object holding informations about all available destinations.")]
	[SerializeField]
	private DestinationsList _destinationsList;

	[SerializeField]
	[Tooltip("Reference to Survival Data.")]
	private SurvivalData _survivalData;

	[SerializeField]
	[Tooltip("Reference to Suit System Data")]
	private SuitData _suitData;

	[SerializeField]
	[Tooltip("End of day listener list scriptable object reference.")]
	protected EndOfDayListenerList _endOfDayListenerList;

	[SerializeField]
	[Header("References")]
	[Tooltip("Reference to Expedition View on scene.")]
	private ExpeditionView _expeditionView;

	protected StatsManager _statsManager;

	public static ExpeditionManager Instance { get; protected set; }

	public bool IsOnPlanet => _expeditionData.RuntimeData.CurrentPlanetDeck != null;

	protected virtual void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		_endOfDayListenerList.RegisterOnEndOfDay(SendExpeditionIfPlannedAndResetView, "PreEventSystems", 4, this);
		_endOfDayListenerList.RegisterOnEndOfDay(RevokeExpeditionAndUpdateExpeditionTime, "Update", 10, this);
		_endOfDayListenerList.RegisterOnEndOfDay(LandOnPlanet, "SystemEvents", 25, this);
		_statsManager = StatsManager.Instance;
	}

	protected virtual void OnDestroy()
	{
		_endOfDayListenerList.UnregisterOnEndOfDay(SendExpeditionIfPlannedAndResetView, "PreEventSystems");
		_endOfDayListenerList.UnregisterOnEndOfDay(RevokeExpeditionAndUpdateExpeditionTime, "Update");
		_endOfDayListenerList.UnregisterOnEndOfDay(LandOnPlanet, "SystemEvents");
	}

	public void SendExpeditionIfPlannedAndResetView()
	{
		if (_expeditionData.RuntimeData.IsPlanned)
		{
			SendExpedition();
		}
		else
		{
			_expeditionData.RuntimeData.PlannedExpeditionData.ExpeditionCharacter = null;
			_expeditionData.RuntimeData.PlannedExpeditionData.ChosenDestination = null;
			_expeditionData.RuntimeData.PlannedExpeditionData.ExpeditionItems.Clear();
		}
		if (_expeditionView != null)
		{
			_expeditionView.ResetView();
		}
	}

	public void RevokeExpeditionAndUpdateExpeditionTime()
	{
		if (_expeditionData.RuntimeData.IsOngoing)
		{
			if (_expeditionData.RuntimeData.ElapsedExpeditionTime >= _expeditionData.RuntimeData.OngoingExpeditionData.ChosenDestination.Event.ExpeditionLength)
			{
				RevokeExpedition();
			}
			if (_expeditionData.RuntimeData.IsOngoing)
			{
				IncrementeExpeditionTime();
			}
		}
	}

	public void LandOnPlanet()
	{
		if (_expeditionData.RuntimeData.CurrentPlanetDeck != null && !_survivalData.LandedOnPlanet)
		{
			_survivalData.ShouldLandOnPlanet = true;
			_survivalData.LandedOnPlanet = true;
			AvailableDestinationsChangeStatsEntry availableDestinationsChangeStatsEntry = new AvailableDestinationsChangeStatsEntry();
			availableDestinationsChangeStatsEntry.Planet = Instance.GetPlanet();
			availableDestinationsChangeStatsEntry.AvailableDestinations = Instance.AvailableDestinationsCount();
			StatsManager.Instance.AddAvailableDestinationsChangeStatsEntry(availableDestinationsChangeStatsEntry);
		}
	}

	private void SendExpedition()
	{
		_expeditionData.RuntimeData.IsOngoing = true;
		_expeditionData.RuntimeData.ElapsedExpeditionTime = 0;
		_suitData.RuntimeData.IsActive = false;
		int num = Random.Range(_expeditionData.RuntimeData.PlannedExpeditionData.ChosenDestination.Event.MinimumDaysAway, _expeditionData.RuntimeData.PlannedExpeditionData.ChosenDestination.Event.MaximumDaysAway + 1);
		if (DemoManager.IS_DEMO_VERSION && _survivalData.CurrentDay <= 9 && _survivalData.CurrentDay + num > 9)
		{
			num = 9 - _survivalData.CurrentDay;
		}
		_expeditionData.RuntimeData.PlannedExpeditionData.ChosenDestination.Event.ExpeditionLength = num;
		_expeditionData.SetOngoingExpeditionRuntimeData();
		for (int i = 0; i < _expeditionData.RuntimeData.OngoingExpeditionData.ExpeditionItems.Count; i++)
		{
			_expeditionData.RuntimeData.OngoingExpeditionData.ExpeditionItems[i].BaseRuntimeData.IsOnExpedition = true;
			_expeditionData.RuntimeData.OngoingExpeditionData.ExpeditionItems[i].Unlock();
			ConsumableRemedium consumableRemedium = _expeditionData.RuntimeData.OngoingExpeditionData.ExpeditionItems[i] as ConsumableRemedium;
			if (consumableRemedium != null)
			{
				consumableRemedium.Remove(1f);
			}
			else
			{
				_expeditionData.RuntimeData.OngoingExpeditionData.ExpeditionItems[i].BaseRuntimeData.IsAvailable = false;
			}
		}
		_expeditionData.RuntimeData.OngoingExpeditionData.ExpeditionCharacter.RuntimeData.AddStatus(_expeditionData.StaticData.AwayOnExpeditionStatus);
		if (num == 0)
		{
			SchedulerManager.Instance.Schedule(_expeditionData.RuntimeData.OngoingExpeditionData.ChosenDestination.Event, 0);
		}
		else
		{
			SchedulerManager.Instance.Schedule(_expeditionData.RuntimeData.PlannedExpeditionData.ChosenDestination.Event, 0);
			SchedulerManager.Instance.Schedule(_expeditionData.RuntimeData.PlannedExpeditionData.ChosenDestination.Event, num);
		}
		_expeditionData.RuntimeData.IsPlanned = false;
	}

	private void RevokeExpedition()
	{
		_expeditionData.RuntimeData.IsOngoing = false;
		_suitData.RuntimeData.IsActive = true;
		_expeditionData.RuntimeData.ElapsedExpeditionTime = 0;
		_expeditionData.RuntimeData.OngoingExpeditionData.ChosenDestination.DynamicData.Visited = true;
		ExpeditionStatsEntry expeditionStatsEntry = new ExpeditionStatsEntry();
		expeditionStatsEntry.DestinationId = _expeditionData.RuntimeData.OngoingExpeditionData.ChosenDestination.StaticData.Id;
		expeditionStatsEntry.CharacterId = _expeditionData.RuntimeData.OngoingExpeditionData.ExpeditionCharacter.ID;
		expeditionStatsEntry.IdsOfItemsTaken = new List<string>();
		for (int i = 0; i < _expeditionData.RuntimeData.OngoingExpeditionData.ExpeditionItems.Count; i++)
		{
			expeditionStatsEntry.IdsOfItemsTaken.Add(_expeditionData.RuntimeData.OngoingExpeditionData.ExpeditionItems[i].BaseStaticData.ItemId);
		}
		if (_expeditionData.RuntimeData.OngoingExpeditionData.ExpeditionCharacter.RuntimeData.HasStatusPreventingReturnFromExpedition())
		{
			_expeditionData.RuntimeData.OngoingExpeditionData.ExpeditionCharacter.RuntimeData.AddStatus(_expeditionData.StaticData.MissingOnExpeditionStatus);
			for (int j = 0; j < _expeditionData.RuntimeData.OngoingExpeditionData.ExpeditionItems.Count; j++)
			{
				IItem item = _expeditionData.RuntimeData.OngoingExpeditionData.ExpeditionItems[j];
				_expeditionData.RuntimeData.OngoingExpeditionData.ExpeditionItems[j].BaseRuntimeData.IsOnExpedition = false;
				if (!(item is ConsumableRemedium))
				{
					_expeditionData.RuntimeData.OngoingExpeditionData.ExpeditionItems[j].BaseRuntimeData.IsAvailable = false;
				}
			}
			expeditionStatsEntry.IsSuccessful = false;
		}
		else
		{
			for (int k = 0; k < _expeditionData.RuntimeData.OngoingExpeditionData.ExpeditionItems.Count; k++)
			{
				if (_expeditionData.RuntimeData.OngoingExpeditionData.ExpeditionItems[k].BaseRuntimeData.IsOnExpedition)
				{
					_expeditionData.RuntimeData.OngoingExpeditionData.ExpeditionItems[k].BaseRuntimeData.IsOnExpedition = false;
					ConsumableRemedium consumableRemedium = _expeditionData.RuntimeData.OngoingExpeditionData.ExpeditionItems[k] as ConsumableRemedium;
					if (consumableRemedium != null)
					{
						consumableRemedium.Add(1f);
					}
					else
					{
						_expeditionData.RuntimeData.OngoingExpeditionData.ExpeditionItems[k].BaseRuntimeData.IsAvailable = true;
					}
				}
			}
			expeditionStatsEntry.IsSuccessful = true;
			if (_expeditionData.RuntimeData.CurrentPlanetDeck != null)
			{
				string id = _expeditionData.RuntimeData.CurrentPlanetDeck.name + "_dst";
				_statsManager.AddGlobalDataToList(id, _expeditionData.RuntimeData.OngoingExpeditionData.ChosenDestination.name);
			}
		}
		StatsManager.Instance.AddExpeditionStatsEntry(expeditionStatsEntry);
		_expeditionData.RuntimeData.OngoingExpeditionData.ExpeditionCharacter.RuntimeData.RemoveStatus(_expeditionData.StaticData.AwayOnExpeditionStatus);
		if (_expeditionView != null)
		{
			_expeditionView.HideExpeditionPaper();
		}
		_expeditionData.RuntimeData.PlannedExpeditionData.ExpeditionItems.Clear();
	}

	private void IncrementeExpeditionTime()
	{
		_expeditionData.RuntimeData.ElapsedExpeditionTime++;
	}

	public int GetElapsedExpeditionTime()
	{
		return _expeditionData.RuntimeData.ElapsedExpeditionTime;
	}

	public void SkipExpeditionDays(int days)
	{
		_expeditionData.RuntimeData.ElapsedExpeditionTime += days;
	}

	public Character GetExpeditionCharacter()
	{
		return _expeditionData.RuntimeData.OngoingExpeditionData.ExpeditionCharacter;
	}

	public bool IsExpeditionOngoing()
	{
		return _expeditionData.RuntimeData.IsOngoing;
	}

	public bool IsExpeditionPlanned()
	{
		return _expeditionData.RuntimeData.IsPlanned;
	}

	public List<IItem> GetExpeditionItems()
	{
		List<IItem> list = new List<IItem>();
		for (int i = 0; i < _expeditionData.RuntimeData.OngoingExpeditionData.ExpeditionItems.Count; i++)
		{
			if (_expeditionData.RuntimeData.OngoingExpeditionData.ExpeditionItems[i].BaseRuntimeData.IsOnExpedition)
			{
				list.Add(_expeditionData.RuntimeData.OngoingExpeditionData.ExpeditionItems[i]);
			}
		}
		return list;
	}

	public void RemoveExpeditionItem(IItem item)
	{
		item.BaseRuntimeData.IsOnExpedition = false;
	}

	public void LockExpeditionDestination(ExpeditionDestination destination)
	{
		destination.SetEnabled(enabled: false);
	}

	public void UnlockExpeditionDestination(ExpeditionDestination destination)
	{
		destination.SetEnabled(enabled: true);
	}

	public bool IsExpeditionDestinationLocked(ExpeditionDestination destination)
	{
		return !destination.Enabled;
	}

	public void AddExpeditionThreat(ExpeditionDestination destination, ExpeditionThreat threat)
	{
		if (!destination.Threats.Contains(threat))
		{
			destination.Threats.Add(threat);
		}
	}

	public void AddExpeditionItemToFind(ExpeditionDestination destination, IItem item)
	{
		if (!destination.DynamicData.Items.Contains(item))
		{
			destination.DynamicData.Items.Add(item);
		}
	}

	public void RemoveExpeditionThreat(ExpeditionDestination destination, ExpeditionThreat threat)
	{
		destination.Threats.Remove(threat);
	}

	public void RemoveExpeditionItemToFind(ExpeditionDestination destination, IItem item)
	{
		destination.DynamicData.Items.Remove(item);
	}

	public bool HasExpeditionThreat(ExpeditionDestination destination, ExpeditionThreat threat)
	{
		return destination.Threats.Contains(threat);
	}

	public ExpeditionDestination GetCurrentDestination()
	{
		return _expeditionData.RuntimeData.OngoingExpeditionData.ChosenDestination;
	}

	public void BlockThreat(ExpeditionThreat threat)
	{
		threat.RuntimeData.Enabled = false;
	}

	public void UnblockThreat(ExpeditionThreat threat)
	{
		threat.RuntimeData.Enabled = true;
	}

	public bool IsThreatBlocked(ExpeditionThreat threat)
	{
		return !threat.RuntimeData.Enabled;
	}

	public PlanetDeck GetPlanet()
	{
		return _expeditionData.RuntimeData.CurrentPlanetDeck;
	}

	public int AvailableDestinationsCount()
	{
		if (_expeditionData.RuntimeData.CurrentPlanetDeck != null && _expeditionData.RuntimeData.ExpeditionDestinations != null)
		{
			int num = 0;
			for (int i = 0; i < _expeditionData.RuntimeData.ExpeditionDestinations.Count; i++)
			{
				if (_expeditionData.RuntimeData.ExpeditionDestinations[i].Enabled && _expeditionData.RuntimeData.ExpeditionDestinations[i].RequiredSystemLevel <= _expeditionData.RuntimeData.ExpeditionSystemLevel)
				{
					num++;
				}
			}
			return num;
		}
		return 0;
	}

	public int DestinationsCount()
	{
		if (_expeditionData.RuntimeData.CurrentPlanetDeck != null)
		{
			return _expeditionData.RuntimeData.ExpeditionDestinations.Count;
		}
		return 0;
	}
}
