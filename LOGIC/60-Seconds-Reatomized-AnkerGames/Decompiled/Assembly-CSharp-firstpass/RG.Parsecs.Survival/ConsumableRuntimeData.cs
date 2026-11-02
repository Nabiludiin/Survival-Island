using System;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
public class ConsumableRuntimeData : IItemRuntimeData
{
	[SerializeField]
	private float _amount;

	[SerializeField]
	private float _plannedConsumption;

	public float Amount
	{
		get
		{
			return _amount;
		}
		set
		{
			_amount = value;
		}
	}

	public float PlannedConsumption
	{
		get
		{
			return _plannedConsumption;
		}
		set
		{
			if (value > _amount)
			{
				_plannedConsumption = _amount;
			}
			else if (value < 0f)
			{
				_plannedConsumption = 0f;
			}
			else
			{
				_plannedConsumption = value;
			}
		}
	}

	public override bool IsAvailable
	{
		get
		{
			return _amount > 0f;
		}
		set
		{
			throw new UnityException("Cannot use this setter in Consumable object");
		}
	}
}
