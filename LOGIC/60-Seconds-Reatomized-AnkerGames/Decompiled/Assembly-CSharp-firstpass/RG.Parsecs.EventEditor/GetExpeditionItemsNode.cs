using System;
using NodeEditorFramework;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Expedition Nodes/Get Expedition Items Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(ConditionEvent)
})]
public class GetExpeditionItemsNode : ExpeditionNode
{
	public const string ID = "EE_GetExpeditionItemsNode";

	private const string OUTPUT_ITEMS_NAME = "Items";

	private const int OUTPUT_ITEMS_INDEX = 0;

	private const string NODE_NAME = "Get Expedition Items";

	public override string GetID => "EE_GetExpeditionItemsNode";

	public override Node Create(Vector2 pos)
	{
		GetExpeditionItemsNode getExpeditionItemsNode = ScriptableObject.CreateInstance<GetExpeditionItemsNode>();
		getExpeditionItemsNode.rect = new Rect(pos.x, pos.y, 170f, 50f);
		getExpeditionItemsNode.name = "Get Expedition Items";
		getExpeditionItemsNode.CreateOutput("Items", "ItemList");
		return getExpeditionItemsNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return Create(rect.position + new Vector2(20f, 20f));
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output == 0)
		{
			return CastValue<T>(ExpeditionManager.Instance.GetExpeditionItems());
		}
		throw new NotExistingOutputException(GetID, output);
	}
}
