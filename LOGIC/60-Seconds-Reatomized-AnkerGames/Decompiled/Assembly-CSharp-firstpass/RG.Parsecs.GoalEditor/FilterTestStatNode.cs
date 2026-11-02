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

[Node(false, "Game Stats/Filters/Filter Test Stat Node", new Type[]
{
	typeof(Goal),
	typeof(ConditionEvent)
})]
public class FilterTestStatNode : GoalNode
{
	public const string ID = "GE_FilterTestStatNode";

	[SerializeField]
	private List<TestStatsEntry> _result;

	[SerializeField]
	private CharacterAttribute _characterAttribute;

	private const string INPUT_TEST_LIST_TEXT = "Test List";

	private const string INPUT_FILTER_STAT_TEXT = "Filter for stat";

	private const string OUTPUT_FILTER_LIST_TEXT = "Filtered List";

	private const int OUTPUT_INDEX = 0;

	private const int INPUT_INDEX = 0;

	private const int INPUT_ATTRIBUTE_INDEX = 1;

	private const string NODE_NAME = "Filter Test By Stat";

	public override string GetID => "GE_FilterTestStatNode";

	public override Node Create(Vector2 pos)
	{
		FilterTestStatNode filterTestStatNode = ScriptableObject.CreateInstance<FilterTestStatNode>();
		filterTestStatNode.rect = new Rect(pos.x, pos.y, 250f, 75f);
		filterTestStatNode.name = "Filter Test By Stat";
		filterTestStatNode.CreateInput("Test List", ListConnection.ID);
		filterTestStatNode.CreateInput("Filter for stat", "Character");
		filterTestStatNode.CreateOutput("Filtered List", ListConnection.ID);
		return filterTestStatNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		FilterTestStatNode filterTestStatNode = (FilterTestStatNode)Create(rect.position + new Vector2(20f, 20f));
		filterTestStatNode._result = _result;
		filterTestStatNode._characterAttribute = _characterAttribute;
		return filterTestStatNode;
	}

	protected override void NodeEnable()
	{
	}

	protected override void NodeGUI()
	{
	}

	private void UpdateResults(NodeCanvas canvas)
	{
		GetInputValue(Inputs[1], ref _characterAttribute, canvas);
		List<TestStatsEntry> list = new List<TestStatsEntry>();
		IList currentValue = null;
		GetInputValue(Inputs[0], ref currentValue, canvas);
		foreach (TestStatsEntry item in currentValue)
		{
			if (item.TestedAttributeId == _characterAttribute.Id)
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
