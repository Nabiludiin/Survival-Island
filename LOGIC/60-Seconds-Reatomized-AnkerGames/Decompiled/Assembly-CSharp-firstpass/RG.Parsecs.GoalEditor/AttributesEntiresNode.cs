using System;
using NodeEditorFramework;
using RG.Parsecs.Common;
using RG.Parsecs.EventEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.GoalEditor;

[Node(false, "Game Stats/Stat Attributes Node", new Type[]
{
	typeof(Goal),
	typeof(ConditionEvent)
})]
public class AttributesEntiresNode : GoalNode
{
	public const string ID = "PE_AttributesEntriesNode";

	private const string NODE_NAME = "Attributes Entries Node";

	private const string OUTPUT_NAME = "List";

	private const int OUTPUT_INDEX = 0;

	public override string GetID => "PE_AttributesEntriesNode";

	public override Node Create(Vector2 pos)
	{
		AttributesEntiresNode attributesEntiresNode = ScriptableObject.CreateInstance<AttributesEntiresNode>();
		attributesEntiresNode.rect = new Rect(pos.x, pos.y, 150f, 75f);
		attributesEntiresNode.name = "Attributes Entries Node";
		attributesEntiresNode.CreateOutput("List", ListConnection.ID);
		return attributesEntiresNode;
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
		return CastValue<T>(StatsManager.Instance.TestStatsEntries);
	}
}
