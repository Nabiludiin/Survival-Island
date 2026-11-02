using System;
using System.Collections.Generic;
using NodeEditorFramework;
using RG.Parsecs.Common;
using RG.Parsecs.EventEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.GoalEditor;

[Node(false, "Game Stats/Crafting/Crafting Entries Node", new Type[]
{
	typeof(Goal),
	typeof(ConditionEvent)
})]
public class CraftingEntriesNode : GoalNode
{
	private const string ID = "GE_CraftingEntriesNode";

	private List<CraftingStatsEntry> _result;

	private const int OUTPUT_INDEX = 0;

	private const string OUTPUT_NAME = "Output";

	private const string NODE_NAME = "Crafting Entries";

	public override string GetID => "GE_CraftingEntriesNode";

	public override Node Create(Vector2 pos)
	{
		CraftingEntriesNode craftingEntriesNode = ScriptableObject.CreateInstance<CraftingEntriesNode>();
		craftingEntriesNode.rect = new Rect(pos.x, pos.y, 150f, 75f);
		craftingEntriesNode.name = "Crafting Entries";
		craftingEntriesNode.CreateOutput("Output", ListConnection.ID);
		return craftingEntriesNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		CraftingEntriesNode craftingEntriesNode = (CraftingEntriesNode)Create(rect.position + new Vector2(20f, 20f));
		craftingEntriesNode._result = _result;
		return craftingEntriesNode;
	}

	protected override void NodeGUI()
	{
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		_result = StatsManager.Instance.CraftingStatsEntries;
		if (output != 0)
		{
			throw new NotExistingOutputException(GetID, output);
		}
		return CastValue<T>(_result);
	}
}
