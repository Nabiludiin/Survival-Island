using TMPro;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class StatsPanelController : StarlogPanelController
{
	[SerializeField]
	private TextMeshProUGUI[] _statNames = new TextMeshProUGUI[EndGameStatSet.STAT_COUNT];

	[SerializeField]
	private TextMeshProUGUI[] _statValues = new TextMeshProUGUI[EndGameStatSet.STAT_COUNT];

	[SerializeField]
	private TextMeshProUGUI[] _statSections = new TextMeshProUGUI[EndGameStatSet.SECTION_COUNT];

	public void Start()
	{
		UpdateStatsDisplay();
	}

	public void UpdateStatsDisplay()
	{
		Mission currentMission = MissionManager.Instance.GetCurrentMission();
		for (int i = 0; i < EndGameStatSet.SECTION_COUNT; i++)
		{
			if (_statSections[i] != null)
			{
				if (currentMission != null && (string)currentMission.StatSet.SectionNames[i] != null && !currentMission.StatSet.SectionNames[i].IsNullOrEmpty())
				{
					_statSections[i].SetText(currentMission.StatSet.SectionNames[i]);
				}
				else
				{
					_statSections[i].SetText(string.Empty);
				}
			}
		}
		for (int j = 0; j < EndGameStatSet.STAT_COUNT; j++)
		{
			if (_statNames[j] != null && _statValues[j] != null)
			{
				if (currentMission != null && currentMission.StatSet.EndGameStats[j] != null)
				{
					_statNames[j].SetText(currentMission.StatSet.EndGameStats[j].StatName);
					_statValues[j].SetText(currentMission.StatSet.EndGameStats[j].StatValue);
				}
				else
				{
					_statNames[j].SetText(string.Empty);
					_statValues[j].SetText(string.Empty);
				}
			}
		}
	}
}
