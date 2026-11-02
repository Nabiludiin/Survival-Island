using System;
using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
public class RemediumStaticData : IItemStaticData
{
	[SerializeField]
	private List<InfluencesList> _influencesLevel;

	[SerializeField]
	private List<CharacterStatus> _addStatuses;

	[SerializeField]
	private List<CharacterStatus> _removeStatuses;

	public List<InfluencesList> InfluencesLevel => _influencesLevel;

	public List<CharacterStatus> AddStatuses => _addStatuses;

	public List<CharacterStatus> RemoveStatuses => _removeStatuses;
}
