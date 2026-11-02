using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(true, "Supplies Nodes/Items/Break Item Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas),
	typeof(Goal)
})]
public class BreakItemNodeLegacy : ResourceNode
{
	public const string ID = "EE_breakItemNodeLegacy";

	private const int INDEX_ITEM_INPUT = 0;

	private const int INDEX_OUTPUT_NAME = 0;

	private const int INDEX_OUTPUT_IS_AVAILABLE = 1;

	private const int INDEX_OUTPUT_IS_ON_EXPEDITION = 2;

	private const int INDEX_OUTPUT_ACTUAL_LEVEL = 3;

	private const int INDEX_OUTPUT_IS_MAX_LEVEL = 4;

	private const int INDEX_OUTPUT_DURABILITY = 5;

	private const int INDEX_OUTPUT_IS_DAMAGE = 6;

	private const int INDEX_OUTPUT_AMOUNT = 7;

	[SerializeField]
	private IItem _item;

	public override string GetID => "EE_breakItemNodeLegacy";

	public override Node Create(Vector2 pos)
	{
		BreakItemNodeLegacy breakItemNodeLegacy = ScriptableObject.CreateInstance<BreakItemNodeLegacy>();
		breakItemNodeLegacy.rect = new Rect(pos.x, pos.y, 180f, 155f);
		breakItemNodeLegacy.name = "Break Item";
		breakItemNodeLegacy.CreateInput("Item", "Item");
		breakItemNodeLegacy.CreateOutput("Name", "LocalizedString");
		breakItemNodeLegacy.CreateOutput("Is Available", "Bool");
		breakItemNodeLegacy.CreateOutput("Is On Expedition", "Bool");
		breakItemNodeLegacy.CreateOutput("Actual Level", "Int");
		breakItemNodeLegacy.CreateOutput("Is max level", "Bool");
		breakItemNodeLegacy.CreateOutput("Durability", "Int");
		breakItemNodeLegacy.CreateOutput("Is damage", "Bool");
		breakItemNodeLegacy.CreateOutput("Amount", "Int");
		return breakItemNodeLegacy;
	}

	public override Node Duplicate(Vector2 pos)
	{
		BreakItemNodeLegacy breakItemNodeLegacy = (BreakItemNodeLegacy)Create(rect.position + new Vector2(20f, 20f));
		breakItemNodeLegacy._item = _item;
		return breakItemNodeLegacy;
	}

	protected override void NodeEnable()
	{
	}

	protected override void NodeGUI()
	{
	}

	protected override void OnNodeValidate()
	{
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		GetInputValue(Inputs[0], ref _item, canvas);
		switch (output)
		{
		case 0:
			return CastValue<T>(_item.BaseStaticData.Name);
		case 1:
			return CastValue<T>(_item.BaseRuntimeData.IsAvailable);
		case 2:
			return CastValue<T>(_item.BaseRuntimeData.IsOnExpedition);
		case 3:
			return CastValue<T>(_item.BaseRuntimeData.Level);
		case 4:
			return CastValue<T>(_item.IsMaxLevel());
		case 5:
			if (_item is Item)
			{
				return CastValue<T>(((Item)_item).RuntimeData.Durability);
			}
			return CastValue<T>(1);
		case 6:
			if (_item is Item)
			{
				return CastValue<T>(((Item)_item).RuntimeData.IsDamaged);
			}
			return CastValue<T>(false);
		case 7:
			if (_item is ConsumableRemedium)
			{
				ConsumableRemedium consumableRemedium = (ConsumableRemedium)_item;
				return CastValue<T>(consumableRemedium.RuntimeData.Amount - consumableRemedium.RuntimeData.PlannedConsumption);
			}
			return CastValue<T>(1);
		default:
			throw new NotExistingOutputException("EE_breakItemNodeLegacy", output);
		}
	}
}
