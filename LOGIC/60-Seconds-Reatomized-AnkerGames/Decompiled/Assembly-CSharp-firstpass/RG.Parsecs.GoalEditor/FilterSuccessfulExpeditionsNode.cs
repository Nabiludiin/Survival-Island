using System;
using System.Collections.Generic;
using NodeEditorFramework;
using RG.Parsecs.Common;
using RG.Parsecs.EventEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.GoalEditor;

[Node(false, "Game Stats/Expeditions/Filter Successful Expeditions Node", new Type[]
{
	typeof(Goal),
	typeof(SystemEvent),
	typeof(ConditionEvent)
})]
public class FilterSuccessfulExpeditionsNode : GoalNode
{
	public const string ID = "GE_FilterSuccessfulExpeditionsNode";

	private const string NODE_NAME = "Filter Successful Expeditions";

	private const string LIST_INPUT_NAME = "Expeditions Entries";

	private const string SUCCESS_INPUT_NAME = "Is Successfull";

	private const string OUTPUT_LIST_NAME = "Filtered List";

	private const int LIST_INPUT_INDEX = 0;

	private const int SUCCESS_INPUT_INDEX = 1;

	private const int FILTERED_OUTPUT_INDEX = 0;

	[SerializeField]
	private bool _isSuccessful;

	public override string GetID => "GE_FilterSuccessfulExpeditionsNode";

	public override Node Create(Vector2 pos)
	{
		FilterSuccessfulExpeditionsNode filterSuccessfulExpeditionsNode = ScriptableObject.CreateInstance<FilterSuccessfulExpeditionsNode>();
		filterSuccessfulExpeditionsNode.rect = new Rect(pos.x, pos.y, 150f, 100f);
		filterSuccessfulExpeditionsNode.name = "Filter Successful Expeditions";
		filterSuccessfulExpeditionsNode.CreateInput("Expeditions Entries", ListConnection.ID);
		filterSuccessfulExpeditionsNode.CreateInput("Is Successfull", "Bool");
		filterSuccessfulExpeditionsNode.CreateOutput("Filtered List", ListConnection.ID);
		return filterSuccessfulExpeditionsNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		FilterSuccessfulExpeditionsNode filterSuccessfulExpeditionsNode = (FilterSuccessfulExpeditionsNode)Create(rect.position + new Vector2(20f, 20f));
		filterSuccessfulExpeditionsNode._isSuccessful = _isSuccessful;
		return filterSuccessfulExpeditionsNode;
	}

	protected override void NodeEnable()
	{
	}

	protected override void NodeGUI()
	{
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 0)
		{
			throw new NotExistingOutputException(GetID, output);
		}
		List<ExpeditionStatsEntry> currentValue = null;
		GetInputValue(Inputs[0], ref currentValue, canvas);
		GetInputValue(Inputs[1], ref _isSuccessful, canvas);
		return CastValue<T>(currentValue.FindAll((ExpeditionStatsEntry entry) => entry.IsSuccessful == _isSuccessful));
	}
}
