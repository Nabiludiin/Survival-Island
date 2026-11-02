using System;
using NodeEditorFramework;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Expedition Nodes/Get Current Destination Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(ConditionEvent)
})]
public class GetCurrentDestinationNode : ExpeditionNode
{
	public const string ID = "EE_GetCurrentDestinationNode";

	private const string OUTPUT_DESTINATION_NAME = "Destination";

	private const int OUTPUT_DESTINATION_INDEX = 0;

	private const string NODE_NAME = "Get Current Destination";

	public override string GetID => "EE_GetCurrentDestinationNode";

	public override Node Create(Vector2 pos)
	{
		GetCurrentDestinationNode getCurrentDestinationNode = ScriptableObject.CreateInstance<GetCurrentDestinationNode>();
		getCurrentDestinationNode.rect = new Rect(pos.x, pos.y, 170f, 50f);
		getCurrentDestinationNode.name = "Get Current Destination";
		getCurrentDestinationNode.CreateOutput("Destination", "ExpeditionDestination");
		return getCurrentDestinationNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return Create(rect.position + new Vector2(20f, 20f));
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output == 0)
		{
			return CastValue<T>(ExpeditionManager.Instance.GetCurrentDestination());
		}
		throw new NotExistingOutputException(GetID, output);
	}
}
