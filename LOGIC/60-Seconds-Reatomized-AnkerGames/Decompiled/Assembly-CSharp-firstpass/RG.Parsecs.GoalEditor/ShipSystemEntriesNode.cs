using System;
using NodeEditorFramework;
using RG.Parsecs.Common;
using RG.Parsecs.EventEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.GoalEditor;

[Node(false, "Game Stats/Crafting/Ship system Entries Node", new Type[]
{
	typeof(Goal),
	typeof(ConditionEvent)
})]
public class ShipSystemEntriesNode : GoalNode
{
	private const string ID = "GE_ShipSystemEntriesNode";

	private const int OUTPUT_INDEX = 0;

	private const string OUTPUT_NAME = "Output";

	private const string NODE_NAME = "Ship system Entries";

	public override string GetID => "GE_ShipSystemEntriesNode";

	public override Node Create(Vector2 pos)
	{
		ShipSystemEntriesNode shipSystemEntriesNode = ScriptableObject.CreateInstance<ShipSystemEntriesNode>();
		shipSystemEntriesNode.rect = new Rect(pos.x, pos.y, 150f, 75f);
		shipSystemEntriesNode.name = "Ship system Entries";
		shipSystemEntriesNode.CreateOutput("Output", ListConnection.ID);
		return shipSystemEntriesNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return Create(rect.position + new Vector2(20f, 20f));
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
		return CastValue<T>(StatsManager.Instance.SystemUpgradeEntries);
	}
}
