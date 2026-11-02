using RG.Core.Base;
using TMPro;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class EndOfGameStatsViewManager : RGMonoBehaviour
{
	[SerializeField]
	private SurvivalData _survivalData;

	[SerializeField]
	private bool _useDayCountValueText = true;

	[Tooltip("This is the TextMeshProUGUI that will display the number of days the player played through.")]
	[SerializeField]
	private TextMeshProUGUI _dayCountValueText;

	[SerializeField]
	private TextMeshProUGUI[] _statNames = new TextMeshProUGUI[EndGameStatSet.STAT_COUNT];

	[SerializeField]
	private TextMeshProUGUI[] _statValues = new TextMeshProUGUI[EndGameStatSet.STAT_COUNT];

	protected override void CustomAwake()
	{
		if (_useDayCountValueText && _dayCountValueText != null)
		{
			_dayCountValueText.text = $"{_survivalData.CurrentDay}";
		}
		Mission currentMission = MissionManager.Instance.GetCurrentMission();
		for (int i = 0; i < EndGameStatSet.STAT_COUNT; i++)
		{
			if (_statNames[i] != null && _statValues[i] != null)
			{
				if (currentMission != null && currentMission.StatSet.EndGameStats[i] != null)
				{
					_statNames[i].SetText(currentMission.StatSet.EndGameStats[i].StatName);
					_statValues[i].SetText(currentMission.StatSet.EndGameStats[i].StatValue);
				}
				else
				{
					_statNames[i].SetText(string.Empty);
					_statValues[i].SetText(string.Empty);
				}
			}
		}
	}
}
