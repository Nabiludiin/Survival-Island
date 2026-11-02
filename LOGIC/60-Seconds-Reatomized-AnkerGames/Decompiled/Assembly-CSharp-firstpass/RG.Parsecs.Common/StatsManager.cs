using System.Collections.Generic;
using RG.Core.SaveSystem;
using RG.Parsecs.EventEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.Common;

public class StatsManager : MonoBehaviour, ISaveable
{
	private static StatsManager _instance;

	[SerializeField]
	protected AchievementList _achievementList;

	[SerializeField]
	protected GoalsList _goalList;

	[SerializeField]
	protected GlobalStats _globalStats;

	[SerializeField]
	protected SaveEvent _saveEvent;

	[SerializeField]
	protected SaveEvent _globalSaveEvent;

	[SerializeField]
	protected EndGameData _endGameData;

	[SerializeField]
	protected GlobalBoolVariable _isTutorial;

	[SerializeField]
	protected SurvivalData _survivalData;

	[SerializeField]
	protected GlobalIntVariable _expeditionsMadeVariable;

	[SerializeField]
	protected GlobalIntVariable _expeditionsSuccessfulVariable;

	protected const int MAX_SYSTEM_LEVEL = 3;

	[SerializeField]
	private List<CharacterStatusChangeStatsEntry> _characterStatusChangeStatsEntries = new List<CharacterStatusChangeStatsEntry>();

	[SerializeField]
	private List<ExpeditionStatsEntry> _expeditionStatsEntries = new List<ExpeditionStatsEntry>();

	[SerializeField]
	private List<EventStatsEntry> _eventStatsEntries = new List<EventStatsEntry>();

	[SerializeField]
	private List<ShipSystemUpgradeStatsEntry> _systemUpgradeStatsEntries = new List<ShipSystemUpgradeStatsEntry>();

	[SerializeField]
	private List<CraftingStatsEntry> _craftingStatsEntries = new List<CraftingStatsEntry>();

	[SerializeField]
	private List<TestStatsEntry> _testStatsEntries = new List<TestStatsEntry>();

	[SerializeField]
	private List<NeedStatsEntry> _needStatsEntries = new List<NeedStatsEntry>();

	[SerializeField]
	private List<RationingStatsEntry> _rationingStatsEntries = new List<RationingStatsEntry>();

	[SerializeField]
	private List<ItemCollectedStatsEntry> _itemCollectedStatsEntries = new List<ItemCollectedStatsEntry>();

	[SerializeField]
	private List<AvailableDestinationsChangeStatsEntry> _availableDestinationsStatsEntries = new List<AvailableDestinationsChangeStatsEntry>();

	[SerializeField]
	private List<YesNoDecisionStatsEntry> _yesNoDecisionStatsEntries = new List<YesNoDecisionStatsEntry>();

	private const string STATS_MANAGER_ID = "MB_StatsManager_";

	public static StatsManager Instance
	{
		get
		{
			if (_instance == null)
			{
				FindInstanceInScene();
			}
			_ = _instance == null;
			return _instance;
		}
	}

	public List<ShipSystemUpgradeStatsEntry> SystemUpgradeEntries => _systemUpgradeStatsEntries;

	public List<NeedStatsEntry> NeedStatsEntries => _needStatsEntries;

	public List<RationingStatsEntry> RationingStatsEntries => _rationingStatsEntries;

	public List<ItemCollectedStatsEntry> ItemCollectedStatsEntries => _itemCollectedStatsEntries;

	public List<CharacterStatusChangeStatsEntry> CharacterStatusChangeStatsEntries => _characterStatusChangeStatsEntries;

	public List<ExpeditionStatsEntry> ExpeditionStatsEntries => _expeditionStatsEntries;

	public List<AvailableDestinationsChangeStatsEntry> AvailableDestinationsStatsEntries => _availableDestinationsStatsEntries;

	public List<EventStatsEntry> EventStatsEntries => _eventStatsEntries;

	public List<CraftingStatsEntry> CraftingStatsEntries => _craftingStatsEntries;

	public List<TestStatsEntry> TestStatsEntries => _testStatsEntries;

	public List<YesNoDecisionStatsEntry> YesNoDecisionStatsEntries => _yesNoDecisionStatsEntries;

	public string ID => "MB_StatsManager_" + name;

	private static void FindInstanceInScene()
	{
		_instance = Object.FindObjectOfType<StatsManager>();
		if (_instance == null)
		{
			Debug.LogError("No StatsManager in scene!");
		}
	}

	protected virtual bool CanAddToGlobalStat()
	{
		if (!_isTutorial.Value)
		{
			return !MissionManager.Instance.IsMissionOngoing();
		}
		return false;
	}

	public virtual int GetGlobalData(string id)
	{
		return _globalStats.GetData(id);
	}

	public virtual void AddGlobalData(string id, int data)
	{
		if (CanAddToGlobalStat())
		{
			_globalStats.AddData(id, data);
		}
	}

	public virtual void AddGlobalDataToList(string id, string data)
	{
		if (CanAddToGlobalStat())
		{
			_globalStats.AddDataToList(id, data);
		}
	}

	public void AddScavengeStats(ScavengeEntry entry)
	{
		if (!CanAddToGlobalStat())
		{
			return;
		}
		_globalStats.AddData("ScavengedSoups", entry.TotalSoups);
		_globalStats.AddData("ScavengedItems", entry.TotalItems);
		_globalStats.AddData("TotalTryEvacuations", 1);
		if (entry.IsSuccessful)
		{
			_globalStats.AddData("SuccessfulEvacuations", 1);
		}
		_globalStats.AddData("TotalCharactersDeadOnScavenge", entry.LeftForDead);
		if (entry.ItemsIds == null)
		{
			return;
		}
		foreach (string itemsId in entry.ItemsIds)
		{
			_globalStats.AddDataToList("ScavengedItemsIDs", itemsId);
		}
	}

	public void AddDayToEntry(StatsEntry entry)
	{
		entry.Day = EndOfDayManager.Instance.AcutalDay;
		entry.OnPlanet = ExpeditionManager.Instance.IsOnPlanet;
	}

	public void AddRationingStatsEntry(RationingStatsEntry entry)
	{
		AddDayToEntry(entry);
		RationingStatsEntries.Add(entry);
	}

	public void AddAvailableDestinationsChangeStatsEntry(AvailableDestinationsChangeStatsEntry entry)
	{
		AddDayToEntry(entry);
		AvailableDestinationsStatsEntries.Add(entry);
		if (entry.Planet != null)
		{
			string id = entry.Planet.name + "_dst_discovered";
			if (GetGlobalData(id) < entry.AvailableDestinations)
			{
				AddGlobalData(id, entry.AvailableDestinations - GetGlobalData(id));
			}
		}
	}

	public void AddNeedStatsEntry(NeedStatsEntry entry)
	{
		AddDayToEntry(entry);
		NeedStatsEntries.Add(entry);
	}

	public void AddSystemUpgradeStatsEntry(ShipSystemUpgradeStatsEntry entry)
	{
		AddDayToEntry(entry);
		SystemUpgradeEntries.Add(entry);
		if (CanAddToGlobalStat())
		{
			if (entry.SystemLevel == 3)
			{
				_globalStats.AddDataToList("UniqueUpgradedSystemList", entry.ShipSystemId);
			}
			_globalStats.AddData("UpgradedSystems", 1);
		}
	}

	public void AddCharacterStatusChangeStatsEntry(CharacterStatusChangeStatsEntry entry)
	{
		AddDayToEntry(entry);
		CharacterStatusChangeStatsEntries.Add(entry);
	}

	public void AddExpeditionStatsEntry(ExpeditionStatsEntry entry)
	{
		AddDayToEntry(entry);
		ExpeditionStatsEntries.Add(entry);
		if (!CanAddToGlobalStat())
		{
			return;
		}
		_globalStats.AddData("ExpeditionsMade", 1);
		if (entry.IsSuccessful)
		{
			_globalStats.AddData("ExpeditionsSuccessful", 1);
		}
		if (CanAddExpeditionEntryToVariables())
		{
			_expeditionsMadeVariable.SetValue(_expeditionsMadeVariable.Value + 1);
			if (entry.IsSuccessful)
			{
				_expeditionsSuccessfulVariable.SetValue(_expeditionsSuccessfulVariable.Value + 1);
			}
		}
		_globalStats.AddData("ItemsCollectedOnExpeditions", entry.IdsOfItemsTaken.Count);
	}

	protected virtual bool CanAddExpeditionEntryToVariables()
	{
		return false;
	}

	public void AddEventStatsEntry(EventStatsEntry entry)
	{
		AddDayToEntry(entry);
		EventStatsEntries.Add(entry);
	}

	public void AddCraftingStatsEntry(CraftingStatsEntry entry)
	{
		AddDayToEntry(entry);
		CraftingStatsEntries.Add(entry);
		if (CanAddToGlobalStat())
		{
			if (entry.CraftedItemId == "item_sock" && entry.Action == EPlannedCraftingAction.CRAFT)
			{
				_globalStats.AddData("CraftedSockPuppets", 1);
			}
			if (entry.Action == EPlannedCraftingAction.CRAFT)
			{
				_globalStats.AddData("CraftedItems", 1);
			}
			if (entry.Action == EPlannedCraftingAction.RECYCLE)
			{
				_globalStats.AddData("RecycledItems", 1);
			}
			if (entry.Action == EPlannedCraftingAction.REPAIR)
			{
				_globalStats.AddData("RepairedItems", 1);
			}
			if (entry.Action == EPlannedCraftingAction.UPGRADE)
			{
				_globalStats.AddData("UpgradedItems", 1);
				_globalStats.AddDataToList("UniqueUpgradedItemList", entry.CraftedItemId);
			}
		}
	}

	public void AddItemCollectedStatsEntry(ItemCollectedStatsEntry entry)
	{
		AddDayToEntry(entry);
		if (CanAddToGlobalStat())
		{
			if (entry.FromExpedition)
			{
				_globalStats.AddData("ItemsCollectedOnExpeditions", 1);
			}
			else
			{
				_globalStats.AddData("ItemsCollectedInScavengeMode", 1);
			}
		}
		ItemCollectedStatsEntries.Add(entry);
	}

	public void AddYesNoDecisionStatsEntry(YesNoDecisionStatsEntry entry)
	{
		AddDayToEntry(entry);
		YesNoDecisionStatsEntries.Add(entry);
	}

	public void AddTestStatsEntry(TestStatsEntry entry)
	{
		AddDayToEntry(entry);
		TestStatsEntries.Add(entry);
		if (!CanAddToGlobalStat())
		{
			return;
		}
		_globalStats.AddData("TestsMade", 1);
		if (entry.IsPassed)
		{
			_globalStats.AddData("TestsPassed", 1);
		}
		if (entry.IsCrewTest)
		{
			_globalStats.AddData("TestsCrewMade", 1);
			if (entry.IsPassed)
			{
				_globalStats.AddData("TestsCrewPassed", 1);
			}
		}
		if (entry.TestedAttributeId == "attr_agl")
		{
			_globalStats.AddData("TestsAgilityMade", 1);
			if (entry.IsPassed)
			{
				_globalStats.AddData("TestsAgilityPassed", 1);
			}
		}
		if (entry.TestedAttributeId == "attr_int")
		{
			_globalStats.AddData("TestsWitsMade", 1);
			if (entry.IsPassed)
			{
				_globalStats.AddData("TestsWitsPassed", 1);
			}
		}
		if (entry.TestedAttributeId == "attr_str")
		{
			_globalStats.AddData("TestsStrengthMade", 1);
			if (entry.IsPassed)
			{
				_globalStats.AddData("TestsStrengthPassed", 1);
			}
		}
	}

	public float GetSoupConsumedStat()
	{
		int num = 0;
		for (int i = 0; i < RationingStatsEntries.Count; i++)
		{
			if (RationingStatsEntries[i].ItemId.Equals("item_food"))
			{
				num++;
			}
		}
		float consumeRatio = ((ConsumableRemedium)Singleton<ItemManager>.Instance.GetItem("item_food")).StaticData.ConsumeRatio;
		return (float)num * consumeRatio;
	}

	public float GetWaterConsumedStat()
	{
		int num = 0;
		for (int i = 0; i < RationingStatsEntries.Count; i++)
		{
			if (RationingStatsEntries[i].ItemId.Equals("item_water"))
			{
				num++;
			}
		}
		float consumeRatio = ((ConsumableRemedium)Singleton<ItemManager>.Instance.GetItem("item_water")).StaticData.ConsumeRatio;
		return (float)num * consumeRatio;
	}

	public int GetMedkitsUsedStat()
	{
		int num = 0;
		for (int i = 0; i < RationingStatsEntries.Count; i++)
		{
			if (RationingStatsEntries[i].ItemId.Equals("item_medkit"))
			{
				num++;
			}
		}
		return num;
	}

	public int GetSockPuppetsUsedStat()
	{
		int num = 0;
		for (int i = 0; i < RationingStatsEntries.Count; i++)
		{
			if (RationingStatsEntries[i].ItemId.Equals("item_sock"))
			{
				num++;
			}
		}
		return num;
	}

	public int GetSuccessfulExpeditionCountStat()
	{
		int num = 0;
		for (int i = 0; i < ExpeditionStatsEntries.Count; i++)
		{
			if (ExpeditionStatsEntries[i].IsSuccessful)
			{
				num++;
			}
		}
		return num;
	}

	public int GetExpeditionCountStat()
	{
		return ExpeditionStatsEntries.Count;
	}

	public int GetSuccessfulExpeditionsWithUniqueDestinationsCountStat()
	{
		List<string> list = new List<string>();
		for (int i = 0; i < _expeditionStatsEntries.Count; i++)
		{
			if (_expeditionStatsEntries[i].IsSuccessful && !list.Contains(_expeditionStatsEntries[i].DestinationId))
			{
				list.Add(_expeditionStatsEntries[i].DestinationId);
			}
		}
		return list.Count;
	}

	public int GetItemsCollectedOnExpeditionsCountStat()
	{
		int num = 0;
		for (int i = 0; i < ExpeditionStatsEntries.Count; i++)
		{
			num += ExpeditionStatsEntries[i].IdsOfItemsTaken.Count;
		}
		return num;
	}

	public int GetItemsCraftedCountStat()
	{
		int num = 0;
		for (int i = 0; i < CraftingStatsEntries.Count; i++)
		{
			if (CraftingStatsEntries[i].Action == EPlannedCraftingAction.CRAFT)
			{
				num++;
			}
		}
		return num;
	}

	public int GetItemsUpgradedCountStat()
	{
		int num = 0;
		for (int i = 0; i < CraftingStatsEntries.Count; i++)
		{
			if (CraftingStatsEntries[i].Action == EPlannedCraftingAction.UPGRADE)
			{
				num++;
			}
		}
		return num;
	}

	public int GetItemsRecycledCountStat()
	{
		int num = 0;
		for (int i = 0; i < CraftingStatsEntries.Count; i++)
		{
			if (CraftingStatsEntries[i].Action == EPlannedCraftingAction.RECYCLE)
			{
				num++;
			}
		}
		return num;
	}

	public int GetDaysSurvivedStat()
	{
		if (_survivalData != null)
		{
			return _survivalData.CurrentDay;
		}
		Debug.LogWarning("No Survival Data set up in StatsManager");
		return 0;
	}

	public int GetYesDecisionCountStat()
	{
		int num = 0;
		for (int i = 0; i < YesNoDecisionStatsEntries.Count; i++)
		{
			if (YesNoDecisionStatsEntries[i].YesDecision)
			{
				num++;
			}
		}
		return num;
	}

	public int GetNoDecisionCountStat()
	{
		int num = 0;
		for (int i = 0; i < YesNoDecisionStatsEntries.Count; i++)
		{
			if (!YesNoDecisionStatsEntries[i].YesDecision)
			{
				num++;
			}
		}
		return num;
	}

	private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
		}
		if (_instance == this)
		{
			ResetData();
		}
	}

	public void OnEnable()
	{
		RegisterDevToolsCommands();
		Register();
	}

	public void OnDisable()
	{
		UnregisterDevToolsCommands();
	}

	private void OnDestroy()
	{
		Unregister();
	}

	private void RegisterDevToolsCommands()
	{
	}

	private void UnregisterDevToolsCommands()
	{
	}

	public string Serialize()
	{
		return JsonUtility.ToJson(new StatsManagerWrapper
		{
			CharacterStatusChangeStatsEntries = _characterStatusChangeStatsEntries,
			ExpeditionStatsEntries = _expeditionStatsEntries,
			EventStatsEntries = _eventStatsEntries,
			SystemUpgradeStatsEntries = _systemUpgradeStatsEntries,
			CraftingStatsEntries = _craftingStatsEntries,
			TestStatsEntries = _testStatsEntries,
			NeedStatsEntries = _needStatsEntries,
			RationingStatsEntries = _rationingStatsEntries,
			ItemCollectedStatsEntries = _itemCollectedStatsEntries,
			AvailableDestinationsChangeStatsEntries = _availableDestinationsStatsEntries,
			YesNoDecisionStatsEntries = _yesNoDecisionStatsEntries
		});
	}

	public void Deserialize(string jsonData)
	{
		StatsManagerWrapper statsManagerWrapper = JsonUtility.FromJson<StatsManagerWrapper>(jsonData);
		_characterStatusChangeStatsEntries = statsManagerWrapper.CharacterStatusChangeStatsEntries;
		_expeditionStatsEntries = statsManagerWrapper.ExpeditionStatsEntries;
		_eventStatsEntries = statsManagerWrapper.EventStatsEntries;
		_systemUpgradeStatsEntries = statsManagerWrapper.SystemUpgradeStatsEntries;
		_craftingStatsEntries = statsManagerWrapper.CraftingStatsEntries;
		_testStatsEntries = statsManagerWrapper.TestStatsEntries;
		_needStatsEntries = statsManagerWrapper.NeedStatsEntries;
		_rationingStatsEntries = statsManagerWrapper.RationingStatsEntries;
		_itemCollectedStatsEntries = statsManagerWrapper.ItemCollectedStatsEntries;
		_availableDestinationsStatsEntries = statsManagerWrapper.AvailableDestinationsChangeStatsEntries;
		_yesNoDecisionStatsEntries = statsManagerWrapper.YesNoDecisionStatsEntries;
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
		_characterStatusChangeStatsEntries.Clear();
		_expeditionStatsEntries.Clear();
		_eventStatsEntries.Clear();
		_systemUpgradeStatsEntries.Clear();
		_craftingStatsEntries.Clear();
		_testStatsEntries.Clear();
		_needStatsEntries.Clear();
		_rationingStatsEntries.Clear();
		_itemCollectedStatsEntries.Clear();
		_availableDestinationsStatsEntries.Clear();
		_yesNoDecisionStatsEntries.Clear();
	}
}
