using System;
using System.Collections;
using System.Collections.Generic;
using NodeEditorFramework;
using RG.Parsecs.Common;
using RG.Parsecs.EventEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.GoalEditor;

[Node(false, "Game Stats/Filters/Filter Test Successful Node", new Type[]
{
	typeof(Goal),
	typeof(ConditionEvent)
})]
public class FilterTestSuccessfulNode : GoalNode
{
	private const string ID = "GE_FilterTestSuccessfulNode";

	[SerializeField]
	private List<TestStatsEntry> _result;

	[SerializeField]
	private bool _filterForPassed;

	private const string INPUT_TEST_LIST_TEXT = "Test List";

	private const string INPUT_FILTER_STAT_TEXT = "Filter for passed";

	private const string OUTPUT_FILTER_LIST_TEXT = "Filtered List";

	private const int OUTPUT_INDEX = 0;

	private const int INPUT_INDEX = 0;

	private const int INPUT_BOOL_INDEX = 1;

	private const string NODE_NAME = "Filter Test Successful";

	public override string GetID => "GE_FilterTestSuccessfulNode";

	public override Node Create(Vector2 pos)
	{
		FilterTestSuccessfulNode filterTestSuccessfulNode = ScriptableObject.CreateInstance<FilterTestSuccessfulNode>();
		filterTestSuccessfulNode.rect = new Rect(pos.x, pos.y, 250f, 75f);
		filterTestSuccessfulNode.name = "Filter Test Successful";
		filterTestSuccessfulNode.CreateInput("Test List", ListConnection.ID);
		filterTestSuccessfulNode.CreateInput("Filter for passed", "Bool");
		filterTestSuccessfulNode.CreateOutput("Filtered List", ListConnection.ID);
		_filterForPassed = true;
		return filterTestSuccessfulNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		FilterTestSuccessfulNode filterTestSuccessfulNode = (FilterTestSuccessfulNode)Create(rect.position + new Vector2(20f, 20f));
		filterTestSuccessfulNode._result = _result;
		filterTestSuccessfulNode._filterForPassed = _filterForPassed;
		return filterTestSuccessfulNode;
	}

	protected override void NodeEnable()
	{
	}

	protected override void NodeGUI()
	{
	}

	private void UpdateResults(NodeCanvas canvas)
	{
		GetInputValue(Inputs[1], ref _filterForPassed, canvas);
		List<TestStatsEntry> list = new List<TestStatsEntry>();
		IList currentValue = null;
		GetInputValue(Inputs[0], ref currentValue, canvas);
		foreach (TestStatsEntry item in currentValue)
		{
			if (item.IsPassed == _filterForPassed)
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
