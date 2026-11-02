using System;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
public class OngoingCraftingActionData
{
	[SerializeField]
	private EPlannedCraftingAction _plannedCraftingAction;

	[SerializeField]
	private IItem _item;

	[SerializeField]
	private ShipSystem _system;

	public EPlannedCraftingAction PlannedCraftingAction
	{
		get
		{
			return _plannedCraftingAction;
		}
		set
		{
			_plannedCraftingAction = value;
		}
	}

	public IItem Item
	{
		get
		{
			return _item;
		}
		set
		{
			_item = value;
		}
	}

	public ShipSystem System
	{
		get
		{
			return _system;
		}
		set
		{
			_system = value;
		}
	}

	public OngoingCraftingActionData Clone()
	{
		return new OngoingCraftingActionData
		{
			Item = Item,
			System = System,
			PlannedCraftingAction = PlannedCraftingAction
		};
	}
}
