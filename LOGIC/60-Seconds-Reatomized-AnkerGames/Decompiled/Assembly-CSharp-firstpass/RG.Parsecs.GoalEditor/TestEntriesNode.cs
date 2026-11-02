using System;
using System.Collections.Generic;
using NodeEditorFramework;
using RG.Parsecs.Common;
using RG.Parsecs.EventEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.GoalEditor;

[Node(false, "Game Stats/Tests/Test Entries Node", new Type[]
{
	typeof(Goal),
	typeof(ConditionEvent)
})]
public class TestEntriesNode : GoalNode
{
	private const string ID = "GE_TestEntriesNode";

	private List<TestStatsEntry> _result;

	private const int COUNT_OUTPUT_INDEX = 0;

	private const string OUTPUT_NAME = "Output";

	private const string NODE_NAME = "Test Entries";

	public override string GetID => "GE_TestEntriesNode";

	public override Node Create(Vector2 pos)
	{
		TestEntriesNode testEntriesNode = ScriptableObject.CreateInstance<TestEntriesNode>();
		testEntriesNode.rect = new Rect(pos.x, pos.y, 150f, 75f);
		testEntriesNode.name = "Test Entries";
		testEntriesNode.CreateOutput("Output", ListConnection.ID);
		return testEntriesNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		TestEntriesNode testEntriesNode = (TestEntriesNode)Create(rect.position + new Vector2(20f, 20f));
		testEntriesNode._result = _result;
		return testEntriesNode;
	}

	protected override void NodeGUI()
	{
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		_result = StatsManager.Instance.TestStatsEntries;
		if (output != 0)
		{
			throw new NotExistingOutputException(GetID, output);
		}
		return CastValue<T>(_result);
	}
}
