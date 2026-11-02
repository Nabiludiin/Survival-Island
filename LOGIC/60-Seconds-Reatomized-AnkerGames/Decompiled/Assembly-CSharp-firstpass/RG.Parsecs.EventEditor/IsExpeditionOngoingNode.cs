using System;
using NodeEditorFramework;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Expedition Nodes/Is Expedition Ongoing Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(ConditionEvent)
})]
public class IsExpeditionOngoingNode : ExpeditionNode
{
	public const string ID = "EE_IsExpeditionOngoingNode";

	private const string OUTPUT_ONGOING_NAME = "Is Expedition Ongoing";

	private const int OUTPUT_ONGOING_INDEX = 0;

	private const string NODE_NAME = "Is Expedition Ongoing";

	public override string GetID => "EE_IsExpeditionOngoingNode";

	public override Node Create(Vector2 pos)
	{
		IsExpeditionOngoingNode isExpeditionOngoingNode = ScriptableObject.CreateInstance<IsExpeditionOngoingNode>();
		isExpeditionOngoingNode.rect = new Rect(pos.x, pos.y, 170f, 50f);
		isExpeditionOngoingNode.name = "Is Expedition Ongoing";
		isExpeditionOngoingNode.CreateOutput("Is Expedition Ongoing", "Bool");
		return isExpeditionOngoingNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return Create(rect.position + new Vector2(20f, 20f));
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output == 0)
		{
			return CastValue<T>(ExpeditionManager.Instance.IsExpeditionOngoing());
		}
		throw new NotExistingOutputException(GetID, output);
	}
}
