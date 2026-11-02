using System;
using NodeEditorFramework;
using RG.Parsecs.Common;
using RG.Parsecs.EventEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.GoalEditor;

[Node(false, "Game Stats/Expeditions/Expedition Entries Node", new Type[]
{
	typeof(Goal),
	typeof(SystemEvent),
	typeof(ConditionEvent)
})]
public class ExpeditionEntriesNode : GoalNode
{
	public const string ID = "GE_ExpeditionEntriesNode";

	private const string NODE_NAME = "Get Expedition Entries";

	private const string OUTPUT_NAME = "Entries";

	private const int OUTPUT_INDEX = 0;

	public override string GetID => "GE_ExpeditionEntriesNode";

	public override Node Create(Vector2 pos)
	{
		ExpeditionEntriesNode expeditionEntriesNode = ScriptableObject.CreateInstance<ExpeditionEntriesNode>();
		expeditionEntriesNode.rect = new Rect(pos.x, pos.y, 150f, 75f);
		expeditionEntriesNode.name = "Get Expedition Entries";
		expeditionEntriesNode.CreateOutput("Entries", ListConnection.ID);
		return expeditionEntriesNode;
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
		return CastValue<T>(StatsManager.Instance.ExpeditionStatsEntries);
	}
}
