using System;
using System.Collections.Generic;
using System.Linq;
using NodeEditorFramework;
using RG.Parsecs.Common;
using RG.Parsecs.EventEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.GoalEditor;

[Node(false, "Game Stats/Expeditions/Filter Destination Expedition Node", new Type[]
{
	typeof(Goal),
	typeof(ConditionEvent)
})]
public class FilterDestinationExpeditionNode : GoalNode
{
	private const string ID = "GE_FilterDestinationExpeditionNode";

	private const string NODE_NAME = "Filter Destination Expedition";

	private const string INTPUT_LIST_NAME = "Expeditions";

	private const string INPUT_UNIQUE_NAME = "Uniques";

	private const string INPUT_DESTINATION_NAME = "Destination";

	private const string OUTPUT_FILTERED_LIST_NAME = "Filtered List";

	private const int INTPUT_LIST_INDEX = 0;

	private const int INPUT_UNIQUE_INDEX = 1;

	private const int INPUT_DESTINATION_INDEX = 2;

	private const int OUTPUT_FILTERED_LIST_INDEX = 0;

	[SerializeField]
	private bool _uniques;

	[SerializeField]
	private ExpeditionDestination _destination;

	public override string GetID => "GE_FilterDestinationExpeditionNode";

	public override Node Create(Vector2 pos)
	{
		FilterDestinationExpeditionNode filterDestinationExpeditionNode = ScriptableObject.CreateInstance<FilterDestinationExpeditionNode>();
		filterDestinationExpeditionNode.rect = new Rect(pos.x, pos.y, 150f, 150f);
		filterDestinationExpeditionNode.name = "Filter Destination Expedition";
		filterDestinationExpeditionNode.CreateInput("Expeditions", ListConnection.ID);
		filterDestinationExpeditionNode.CreateInput("Uniques", "Bool");
		filterDestinationExpeditionNode.CreateInput("Destination", "ExpeditionDestination");
		filterDestinationExpeditionNode.CreateOutput("Filtered List", ListConnection.ID);
		return filterDestinationExpeditionNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		FilterDestinationExpeditionNode filterDestinationExpeditionNode = (FilterDestinationExpeditionNode)Create(rect.position + new Vector2(20f, 20f));
		filterDestinationExpeditionNode._destination = _destination;
		filterDestinationExpeditionNode._uniques = _uniques;
		return filterDestinationExpeditionNode;
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
		List<ExpeditionStatsEntry> list = null;
		GetInputValue(Inputs[1], ref _uniques, canvas);
		GetInputValue(Inputs[2], ref _destination, canvas);
		List<ExpeditionStatsEntry> inputValue = GetInputValue<List<ExpeditionStatsEntry>>(Inputs[0], canvas);
		list = ((!_uniques) ? inputValue.FindAll((ExpeditionStatsEntry e) => e.DestinationId == _destination.StaticData.Id) : inputValue.Distinct(new ExpeditionDestinationComparer()).ToList());
		return CastValue<T>(list);
	}
}
