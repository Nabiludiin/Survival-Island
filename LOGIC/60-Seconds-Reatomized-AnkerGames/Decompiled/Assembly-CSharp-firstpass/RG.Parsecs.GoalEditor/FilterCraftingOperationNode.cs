using System;
using System.Collections.Generic;
using NodeEditorFramework;
using RG.Parsecs.Common;
using RG.Parsecs.EventEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.GoalEditor;

[Node(false, "Game Stats/Crafting/Filter Crafting Operation Node", new Type[]
{
	typeof(Goal),
	typeof(ConditionEvent)
})]
public class FilterCraftingOperationNode : GoalNode
{
	public const string ID = "GE_FilterCraftingOperationNode";

	private const string NODE_NAME = "Filter Crafting Operation";

	private const string INPUT_CRAFTING_LIST = "Crafing stats entry";

	private const string OUTPUT_FILTER_LIST = "Filtered List";

	private const int INPUT_LIST = 0;

	private const int OUTPUT_LIST = 0;

	private List<CraftingStatsEntry> _result;

	[SerializeField]
	private EPlannedCraftingAction _operation;

	public override string GetID => "GE_FilterCraftingOperationNode";

	public override Node Create(Vector2 pos)
	{
		FilterCraftingOperationNode filterCraftingOperationNode = ScriptableObject.CreateInstance<FilterCraftingOperationNode>();
		filterCraftingOperationNode.rect = new Rect(pos.x, pos.y, 250f, 75f);
		filterCraftingOperationNode.name = "Filter Crafting Operation";
		filterCraftingOperationNode.CreateInput("Crafing stats entry", ListConnection.ID);
		filterCraftingOperationNode.CreateOutput("Filtered List", ListConnection.ID);
		return filterCraftingOperationNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		FilterCraftingOperationNode filterCraftingOperationNode = (FilterCraftingOperationNode)Create(rect.position + new Vector2(20f, 20f));
		filterCraftingOperationNode._operation = _operation;
		return filterCraftingOperationNode;
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
		List<CraftingStatsEntry> inputValue = GetInputValue<List<CraftingStatsEntry>>(Inputs[0], canvas);
		_result = inputValue.FindAll((CraftingStatsEntry entry) => entry.Action == _operation);
		return CastValue<T>(_result);
	}
}
