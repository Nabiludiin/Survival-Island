using System;
using NodeEditorFramework;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Expedition Nodes/Is Expedition Planned Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(ConditionEvent)
})]
public class IsExpeditionPlannedNode : ExpeditionNode
{
	public const string ID = "EE_IsExpeditionPlannedNode";

	private const string OUTPUT_ONGOING_NAME = "Is Expedition Planned";

	private const int OUTPUT_ONGOING_INDEX = 0;

	private const string NODE_NAME = "Is Expedition Planned";

	public override string GetID => "EE_IsExpeditionPlannedNode";

	public override Node Create(Vector2 pos)
	{
		IsExpeditionPlannedNode isExpeditionPlannedNode = ScriptableObject.CreateInstance<IsExpeditionPlannedNode>();
		isExpeditionPlannedNode.rect = new Rect(pos.x, pos.y, 170f, 50f);
		isExpeditionPlannedNode.name = "Is Expedition Planned";
		isExpeditionPlannedNode.CreateOutput("Is Expedition Planned", "Bool");
		return isExpeditionPlannedNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return Create(rect.position + new Vector2(20f, 20f));
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output == 0)
		{
			return CastValue<T>(ExpeditionManager.Instance.IsExpeditionPlanned());
		}
		throw new NotExistingOutputException(GetID, output);
	}
}
