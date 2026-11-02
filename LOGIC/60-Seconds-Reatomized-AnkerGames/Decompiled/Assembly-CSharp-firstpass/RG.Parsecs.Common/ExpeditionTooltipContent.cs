using System.Collections.Generic;
using I2.Loc;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.Common;

public class ExpeditionTooltipContent : TooltipContent
{
	[Tooltip("Reference to expedition data scriptable object.")]
	[SerializeField]
	private ExpeditionData _expeditionData;

	[Tooltip("List of names depending on Expedition system level.")]
	[SerializeField]
	private List<LocalizedString> _expeditionNamesWithLevel;

	[Tooltip("Term containing phrase - Status.")]
	[SerializeField]
	private LocalizedString _statusName;

	[Tooltip("Term containing phrase - Unavailable.")]
	[SerializeField]
	private LocalizedString _unavailableStatus;

	[Tooltip("Term containing phrase - Damaged.")]
	[SerializeField]
	private LocalizedString _damagedStatus;

	[Tooltip("Term containing phrase - Busy.")]
	[SerializeField]
	private LocalizedString _busyStatus;

	[Tooltip("Additional term (optional).")]
	[SerializeField]
	private TooltipTerm _additionalTooltipContent;

	[Tooltip("Reference to suit data scriptable object.")]
	[SerializeField]
	private SuitData _suitData;

	[Tooltip("Additional term (optional).")]
	[SerializeField]
	private TooltipTerm _additionalSuitTooltipTerm;

	[Tooltip("List of names depending on Suit system level.")]
	[SerializeField]
	private List<LocalizedString> _suitLevelName;

	public SuitData SuitData => _suitData;

	public TooltipTerm SuitTooltipTerm => _additionalSuitTooltipTerm;

	public List<LocalizedString> SuitLevelName => _suitLevelName;

	public ExpeditionData Data => _expeditionData;

	public List<LocalizedString> ExpeditionNamesWithLevel => _expeditionNamesWithLevel;

	public LocalizedString Status => _statusName;

	public LocalizedString Damaged => _damagedStatus;

	public LocalizedString Unavailable => _unavailableStatus;

	public LocalizedString Busy => _busyStatus;

	public TooltipTerm ExpeditionTooltipTerm => _additionalTooltipContent;

	public override bool IsValid()
	{
		if (_expeditionNamesWithLevel == null)
		{
			return false;
		}
		for (int i = 0; i < _expeditionNamesWithLevel.Count; i++)
		{
			if ((string)_expeditionNamesWithLevel[i] == null || _expeditionNamesWithLevel[i].IsNullOrEmpty())
			{
				return false;
			}
		}
		if (_suitLevelName == null)
		{
			return false;
		}
		for (int j = 0; j < _suitLevelName.Count; j++)
		{
			if ((string)_suitLevelName[j] == null || _suitLevelName[j].IsNullOrEmpty())
			{
				return false;
			}
		}
		return true;
	}

	private void OnValidate()
	{
		if (_expeditionData.StaticData.MaxLevel != _expeditionNamesWithLevel.Count)
		{
			Debug.LogWarning("Inappropriate number of set term names for expedition. " + name);
		}
		if (_suitData.StaticData.MaxLevel != _suitLevelName.Count)
		{
			Debug.LogWarning("Inappropriate number of set term names for suit. " + name);
		}
	}
}
