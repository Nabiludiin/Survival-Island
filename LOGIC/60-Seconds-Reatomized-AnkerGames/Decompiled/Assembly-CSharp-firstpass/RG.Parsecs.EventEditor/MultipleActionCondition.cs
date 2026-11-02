using System.Collections.Generic;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[CreateAssetMenu(fileName = "New Multiple Action Condition", menuName = "60 Parsecs!/Event Editor/Action Conditions/New Multiple Action Condition")]
public class MultipleActionCondition : BaseActionCondition
{
	[SerializeField]
	private List<BaseActionCondition> _actionConditions;

	private readonly List<string> _resourceActionConditionsAmount = new List<string>();

	private readonly List<string> _consumableActionConditionsAmount = new List<string>();

	public override bool CanSelect()
	{
		if (_actionConditions == null)
		{
			return true;
		}
		bool flag = true;
		_resourceActionConditionsAmount.Clear();
		_consumableActionConditionsAmount.Clear();
		for (int i = 0; i < _actionConditions.Count; i++)
		{
			ResourceActionCondition resourceActionCondition = _actionConditions[i] as ResourceActionCondition;
			ConsumableActionCondition consumableActionCondition = _actionConditions[i] as ConsumableActionCondition;
			if (resourceActionCondition != null)
			{
				if (_resourceActionConditionsAmount.Contains(resourceActionCondition.Resource.Resource.Guid))
				{
					throw new UnityException($"{name} Action Condition has multiple usages of the same Resource type object. It's not allowed.");
				}
				_resourceActionConditionsAmount.Add(resourceActionCondition.Resource.Resource.Guid);
			}
			else if (consumableActionCondition != null)
			{
				List<ConsumableRemedium> consumables = consumableActionCondition.Consumables;
				for (int j = 0; j < consumableActionCondition.Consumables.Count; j++)
				{
					if (!_consumableActionConditionsAmount.Contains(consumables[j].Guid))
					{
						_consumableActionConditionsAmount.Add(consumables[j].Guid);
						continue;
					}
					throw new UnityException($"{name} Action Condition multiple usages of the same Consumable type object. It's not allowed.");
				}
			}
			flag &= _actionConditions[i].CanSelect();
		}
		return flag;
	}

	public override void OnSelect()
	{
		if (_actionConditions != null)
		{
			for (int i = 0; i < _actionConditions.Count; i++)
			{
				_actionConditions[i].OnSelect();
			}
		}
	}

	public override void OnDeselect()
	{
		if (_actionConditions != null)
		{
			for (int i = 0; i < _actionConditions.Count; i++)
			{
				_actionConditions[i].OnDeselect();
			}
		}
	}
}
