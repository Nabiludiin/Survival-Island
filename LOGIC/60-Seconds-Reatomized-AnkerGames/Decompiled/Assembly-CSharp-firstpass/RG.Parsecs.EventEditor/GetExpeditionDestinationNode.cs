using System;
using NodeEditorFramework;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Expedition Nodes/Get Destination Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(ConditionEvent)
})]
public class GetExpeditionDestinationNode : ExpeditionNode
{
	public const string ID = "EE_GetExpeditionDestinationNode";

	public const string NODE_NAME = "Get Destination";

	public const string OUTPUT_DESTINATION_NAME = "Destination";

	public const int OUTPUT_DESTINATION_INDEX = 0;

	[SerializeField]
	private ExpeditionDestination _destination;

	public override string GetID => "EE_GetExpeditionDestinationNode";

	public override Node Create(Vector2 pos)
	{
		GetExpeditionDestinationNode getExpeditionDestinationNode = ScriptableObject.CreateInstance<GetExpeditionDestinationNode>();
		getExpeditionDestinationNode.rect = new Rect(pos.x, pos.y, 180f, 90f);
		getExpeditionDestinationNode.name = "Get Destination";
		getExpeditionDestinationNode.CreateOutput("Destination", "ExpeditionDestination");
		return getExpeditionDestinationNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		GetExpeditionDestinationNode getExpeditionDestinationNode = (GetExpeditionDestinationNode)Create(rect.position + new Vector2(20f, 20f));
		getExpeditionDestinationNode._destination = _destination;
		return getExpeditionDestinationNode;
	}

	protected override void NodeEnable()
	{
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
		return CastValue<T>(_destination);
	}
}
