using System;
using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
public class ConsumableStaticData : IItemStaticData
{
	[SerializeField]
	private float _amount;

	[SerializeField]
	private float _consumeRatio = 1f;

	[SerializeField]
	private List<InfluencesList> _influencesLevel = new List<InfluencesList>();

	[SerializeField]
	private bool _useMaxAmount;

	[SerializeField]
	private float _maxAmount;

	[SerializeField]
	private List<CharacterStatus> _addStatuses;

	[SerializeField]
	private List<CharacterStatus> _removeStatuses;

	public List<InfluencesList> InfluencesLevel => _influencesLevel;

	public float Amount => _amount;

	public float ConsumeRatio => _consumeRatio;

	public bool UseMaxAmount => _useMaxAmount;

	public float MaxAmount => _maxAmount;

	public List<CharacterStatus> AddStatuses => _addStatuses;

	public List<CharacterStatus> RemoveStatuses => _removeStatuses;
}
