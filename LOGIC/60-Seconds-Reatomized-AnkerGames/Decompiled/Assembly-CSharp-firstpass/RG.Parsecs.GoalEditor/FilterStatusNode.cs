using System;
using System.Collections;
using System.Collections.Generic;
using NodeEditorFramework;
using RG.Parsecs.Common;
using RG.Parsecs.EventEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.GoalEditor;

[Node(false, "Game Stats/Character Status Change/Filter Status Node", new Type[]
{
	typeof(Goal),
	typeof(ConditionEvent)
})]
public class FilterStatusNode : GoalNode
{
	private const string ID = "GE_FilterStatusNode";

	[SerializeField]
	private List<CharacterStatusChangeStatsEntry> _result;

	[SerializeField]
	private CharacterStatus _filterForStatus;

	private const int OUTPUT_INDEX = 0;

	private const int INPUT_INDEX = 0;

	private const int INPUT_STATUS_INDEX = 1;

	private const string INPUT_STATUS_CHANGES_TEXT = "Status Changes List";

	private const string INPUT_FILTER_STATUS_TEXT = "Filter for status";

	private const string OUTPUT_FILTER_LIST_TEXT = "Filtered List";

	private const string NODE_NAME = "Filter Character Status";

	public override string GetID => "GE_FilterStatusNode";

	public override Node Create(Vector2 pos)
	{
		FilterStatusNode filterStatusNode = ScriptableObject.CreateInstance<FilterStatusNode>();
		filterStatusNode.rect = new Rect(pos.x, pos.y, 250f, 75f);
		filterStatusNode.name = "Filter Character Status";
		filterStatusNode.CreateInput("Status Changes List", ListConnection.ID);
		filterStatusNode.CreateInput("Filter for status", "CharacterStatusConnection");
		filterStatusNode.CreateOutput("Filtered List", ListConnection.ID);
		return filterStatusNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		FilterStatusNode filterStatusNode = (FilterStatusNode)Create(rect.position + new Vector2(20f, 20f));
		filterStatusNode._result = _result;
		filterStatusNode._filterForStatus = _filterForStatus;
		return filterStatusNode;
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
			GetInputValue(Inputs[1], ref _filterForStatus, canvas);
		}
		List<CharacterStatusChangeStatsEntry> list = new List<CharacterStatusChangeStatsEntry>();
		IList currentValue = null;
		GetInputValue(Inputs[0], ref currentValue, canvas);
		foreach (CharacterStatusChangeStatsEntry item in currentValue)
		{
			if (item.ChangedStatusId == _filterForStatus.Id)
			{
				list.Add(item);
			}
		}
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
