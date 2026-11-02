using System.Collections.Generic;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[CreateAssetMenu(fileName = "New Consumable Action Condition", menuName = "60 Parsecs!/Event Editor/Action Conditions/New Consumable Action Condition")]
public class ConsumableActionCondition : BaseActionCondition
{
	[SerializeField]
	private List<ConsumableRemedium> _consumables;

	[SerializeField]
	private float _amount;

	[SerializeField]
	private bool _multiplyByConsumption;

	public List<ConsumableRemedium> Consumables => _consumables;

	private float GetTotalAmount(ConsumableRemedium consumable)
	{
		if (_multiplyByConsumption)
		{
			return consumable.StaticData.ConsumeRatio * _amount;
		}
		return _amount;
	}

	public override bool CanSelect()
	{
		bool flag = true;
		for (int i = 0; i < _consumables.Count; i++)
		{
			for (int j = 0; j < i; j++)
			{
				if (_consumables[j].Equals(_consumables[i]))
				{
					throw new UnityException($"{_consumables[i]} is used more than once in {name} Consumable Action");
				}
			}
			flag &= _consumables[i].CanLock(GetTotalAmount(_consumables[i]));
		}
		return flag;
	}

	public override void OnSelect()
	{
		for (int i = 0; i < _consumables.Count; i++)
		{
			_consumables[i].Lock(GetTotalAmount(_consumables[i]));
		}
	}

	public override void OnDeselect()
	{
		for (int i = 0; i < _consumables.Count; i++)
		{
			_consumables[i].Unlock(GetTotalAmount(_consumables[i]));
		}
	}
}
