using System;
using System.Collections.Generic;
using NodeEditorFramework;
using RG.Parsecs.Common;
using RG.Parsecs.EventEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.GoalEditor;

[Node(false, "Game Stats/Crafting/Filter Ship system Operation Node", new Type[]
{
	typeof(Goal),
	typeof(ConditionEvent)
})]
public class FilterShipSystemOperationNode : GoalNode
{
	public const string ID = "GE_FilterShipSystemOperationNode";

	private const string NODE_NAME = "Filter Crafting Operation";

	private const string INPUT_CRAFTING_LIST = "Ship system stats entry";

	private const string OUTPUT_FILTER_LIST = "Filtered List";

	private const int INPUT_LIST = 0;

	private const int OUTPUT_LIST = 0;

	private List<ShipSystemUpgradeStatsEntry> _result;

	[SerializeField]
	private EPlannedCraftingAction _operation;

	public override string GetID => "GE_FilterShipSystemOperationNode";

	public override Node Create(Vector2 pos)
	{
		FilterShipSystemOperationNode filterShipSystemOperationNode = ScriptableObject.CreateInstance<FilterShipSystemOperationNode>();
		filterShipSystemOperationNode.rect = new Rect(pos.x, pos.y, 250f, 75f);
		filterShipSystemOperationNode.name = "Filter Crafting Operation";
		filterShipSystemOperationNode.CreateInput("Ship system stats entry", ListConnection.ID);
		filterShipSystemOperationNode.CreateOutput("Filtered List", ListConnection.ID);
		return filterShipSystemOperationNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		FilterShipSystemOperationNode filterShipSystemOperationNode = (FilterShipSystemOperationNode)Create(rect.position + new Vector2(20f, 20f));
		filterShipSystemOperationNode._operation = _operation;
		return filterShipSystemOperationNode;
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
		if (output != 0)
		{
			throw new NotExistingOutputException(GetID, output);
		}
		List<ShipSystemUpgradeStatsEntry> inputValue = GetInputValue<List<ShipSystemUpgradeStatsEntry>>(Inputs[0], canvas);
		_result = inputValue.FindAll((ShipSystemUpgradeStatsEntry entry) => entry.Action == _operation);
		return CastValue<T>(_result);
	}
}
