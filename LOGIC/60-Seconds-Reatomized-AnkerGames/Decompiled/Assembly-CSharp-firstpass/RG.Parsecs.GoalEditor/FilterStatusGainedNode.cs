using System;
using System.Collections;
using System.Collections.Generic;
using NodeEditorFramework;
using RG.Parsecs.Common;
using RG.Parsecs.EventEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.GoalEditor;

[Node(false, "Game Stats/Character Status Change/Filter Status Gained Node", new Type[]
{
	typeof(Goal),
	typeof(ConditionEvent)
})]
public class FilterStatusGainedNode : GoalNode
{
	private const string ID = "GE_FilterStatusGainedNode";

	[SerializeField]
	private List<CharacterStatusChangeStatsEntry> _result;

	[SerializeField]
	private bool _filterForStatusGained;

	private const int OUTPUT_INDEX = 0;

	private const int INPUT_INDEX = 0;

	private const int INPUT_BOOL_INDEX = 1;

	private const string INPUT_STATUS_CHANGES_TEXT = "Status Changes List";

	private const string INPUT_FILTER_STATUS_TEXT = "Filter for status gained";

	private const string OUTPUT_FILTER_LIST_TEXT = "Filtered List";

	private const string NODE_NAME = "Filter Status Gained";

	public override string GetID => "GE_FilterStatusGainedNode";

	public override Node Create(Vector2 pos)
	{
		FilterStatusGainedNode filterStatusGainedNode = ScriptableObject.CreateInstance<FilterStatusGainedNode>();
		filterStatusGainedNode.rect = new Rect(pos.x, pos.y, 250f, 75f);
		filterStatusGainedNode.name = "Filter Status Gained";
		filterStatusGainedNode.CreateInput("Status Changes List", ListConnection.ID);
		filterStatusGainedNode.CreateInput("Filter for status gained", "Bool");
		filterStatusGainedNode.CreateOutput("Filtered List", ListConnection.ID);
		_filterForStatusGained = true;
		return filterStatusGainedNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		FilterStatusGainedNode filterStatusGainedNode = (FilterStatusGainedNode)Create(rect.position + new Vector2(20f, 20f));
		filterStatusGainedNode._result = _result;
		filterStatusGainedNode._filterForStatusGained = _filterForStatusGained;
		return filterStatusGainedNode;
	}

	protected override void NodeEnable()
	{
	}

	protected override void NodeGUI()
	{
	}

	private void UpdateResults(NodeCanvas canvas)
	{
		if (Inputs[1].isConnected)
		{
			GetInputValue(Inputs[1], ref _filterForStatusGained, canvas);
		}
		List<CharacterStatusChangeStatsEntry> list = new List<CharacterStatusChangeStatsEntry>();
		IList currentValue = null;
		GetInputValue(Inputs[0], ref currentValue, canvas);
		foreach (CharacterStatusChangeStatsEntry item in currentValue)
		{
			Debug.Log("Entry status change day " + item.Day + ", status gained: " + item.StatusGained + ", filter for status gained: " + _filterForStatusGained);
			if (item.StatusGained == _filterForStatusGained)
			{
				Debug.Log("Adding entry to filtered list");
				list.Add(item);
			}
		}
		Debug.Log("Filtered list count: " + list.Count);
		_result = list;
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		UpdateResults(canvas);
		if (output != 0)
		{
			throw new NotExistingOutputException(GetID, output);
		}
		return CastValue<T>(_result);
	}
}
