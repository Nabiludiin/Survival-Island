using System;
using NodeEditorFramework;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Supplies Nodes/Items/Is Crafting Ongoing Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(ConditionEvent)
})]
public class IsCraftingOngoingNode : ResourceNode
{
	public const string ID = "EE_IsCraftingOngoingNode";

	private const string OUTPUT_ONGOING_NAME = "Is Crafting Ongoing";

	private const int OUTPUT_ONGOING_INDEX = 0;

	private const string NODE_NAME = "Is Crafting Ongoing";

	public override string GetID => "EE_IsCraftingOngoingNode";

	public override Node Create(Vector2 pos)
	{
		IsCraftingOngoingNode isCraftingOngoingNode = ScriptableObject.CreateInstance<IsCraftingOngoingNode>();
		isCraftingOngoingNode.rect = new Rect(pos.x, pos.y, 170f, 50f);
		isCraftingOngoingNode.name = "Is Crafting Ongoing";
		isCraftingOngoingNode.CreateOutput("Is Crafting Ongoing", "Bool");
		return isCraftingOngoingNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return Create(rect.position + new Vector2(20f, 20f));
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output == 0)
		{
			return CastValue<T>(CraftingManager.IsCraftingOngoing());
		}
		throw new NotExistingOutputException(GetID, output);
	}
}
