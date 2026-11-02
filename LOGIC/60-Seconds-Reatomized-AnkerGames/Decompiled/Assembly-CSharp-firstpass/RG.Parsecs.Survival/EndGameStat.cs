using I2.Loc;
using RG.Core.Base;
using RG.Parsecs.Common;
using RG.Parsecs.EventEditor;
using UnityEngine;

namespace RG.Parsecs.Survival;

[CreateAssetMenu(menuName = "60 Parsecs!/Missions/New End Game Stat")]
public class EndGameStat : RGScriptableObject
{
	[SerializeField]
	private LocalizedString _statName;

	[SerializeField]
	private EndGameStatValueType _valueType;

	[SerializeField]
	private GlobalIntVariable _firstValueVariable;

	[SerializeField]
	private bool _usesSecondValueVariable;

	[SerializeField]
	private GlobalIntVariable _secondValueVariable;

	public LocalizedString StatName => _statName;

	public GlobalIntVariable FirstValueVariable => _firstValueVariable;

	public GlobalIntVariable SecondValueVariable => _secondValueVariable;

	public bool UsesSecondValueVariable => _usesSecondValueVariable;

	public EndGameStatValueType ValueType => _valueType;

	public string StatValue
	{
		get
		{
			switch (_valueType)
			{
			case EndGameStatValueType.Variable:
				if (UsesSecondValueVariable)
				{
					return $"{((FirstValueVariable != null) ? _firstValueVariable.Value : 0)}/{((_secondValueVariable != null) ? _secondValueVariable.Value : 0)}";
				}
				return ((FirstValueVariable != null) ? _firstValueVariable.Value : 0).ToString();
			case EndGameStatValueType.SuccessfulExpeditions:
				return $"{StatsManager.Instance.GetSuccessfulExpeditionCountStat()}/{StatsManager.Instance.GetExpeditionCountStat()}";
			case EndGameStatValueType.SoupConsumed:
				return StatsManager.Instance.GetSoupConsumedStat().ToString();
			case EndGameStatValueType.WaterConsumed:
				return StatsManager.Instance.GetWaterConsumedStat().ToString();
			case EndGameStatValueType.MedkitsUsed:
				return StatsManager.Instance.GetMedkitsUsedStat().ToString();
			case EndGameStatValueType.SockPuppetsUsed:
				return StatsManager.Instance.GetSockPuppetsUsedStat().ToString();
			case EndGameStatValueType.ItemsCollectedOnExpeditions:
				return StatsManager.Instance.GetItemsCollectedOnExpeditionsCountStat().ToString();
			case EndGameStatValueType.ItemsCrafted:
				return StatsManager.Instance.GetItemsCraftedCountStat().ToString();
			case EndGameStatValueType.ItemsUpgraded:
				return StatsManager.Instance.GetItemsUpgradedCountStat().ToString();
			case EndGameStatValueType.ItemsRecycled:
				return StatsManager.Instance.GetItemsRecycledCountStat().ToString();
			case EndGameStatValueType.LocationsDiscovered:
			{
				int num = ((!(ExpeditionManager.Instance.GetPlanet() == null)) ? ExpeditionManager.Instance.GetPlanet().Destinations.Count : 0);
				int num2 = ((StatsManager.Instance.AvailableDestinationsStatsEntries.Count > 0) ? StatsManager.Instance.AvailableDestinationsStatsEntries[StatsManager.Instance.AvailableDestinationsStatsEntries.Count - 1].AvailableDestinations : 0);
				return $"{num2}/{num}";
			}
			case EndGameStatValueType.DaysSurvived:
				return StatsManager.Instance.GetDaysSurvivedStat().ToString();
			case EndGameStatValueType.YesDecisions:
				return StatsManager.Instance.GetYesDecisionCountStat().ToString();
			case EndGameStatValueType.NoDecisions:
				return StatsManager.Instance.GetNoDecisionCountStat().ToString();
			default:
				return string.Empty;
			}
		}
	}
}
