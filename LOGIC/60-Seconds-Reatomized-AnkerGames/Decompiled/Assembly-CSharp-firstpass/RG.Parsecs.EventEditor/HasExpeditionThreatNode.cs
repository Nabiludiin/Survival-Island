using System;
using NodeEditorFramework;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Expedition Nodes/Has Expedition Threat Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(ConditionEvent)
})]
public class HasExpeditionThreatNode : ExpeditionNode
{
	public const string ID = "EE_HasExpeditionThreatNode";

	private const string INPUT_DESTINATION_NAME = "Destination";

	private const string INPUT_THREAT_NAME = "Threat";

	private const string OUTPUT_HAS_THREAT_NAME = "Has Threat";

	private const int INPUT_DESTINATION_INDEX = 0;

	private const int INPUT_THREAT_INDEX = 1;

	private const int OUTPUT_HAS_THREAT_INDEX = 0;

	private const string NODE_NAME = "Has Expedition Threat";

	[SerializeField]
	private ExpeditionDestination _destination;

	[SerializeField]
	private ExpeditionThreat _threat;

	public override string GetID => "EE_HasExpeditionThreatNode";

	public override Node Create(Vector2 pos)
	{
		HasExpeditionThreatNode hasExpeditionThreatNode = ScriptableObject.CreateInstance<HasExpeditionThreatNode>();
		hasExpeditionThreatNode.rect = new Rect(pos.x, pos.y, 170f, 50f);
		hasExpeditionThreatNode.name = "Has Expedition Threat";
		hasExpeditionThreatNode.CreateInput("Destination", "ExpeditionDestination");
		hasExpeditionThreatNode.CreateInput("Threat", "ExpeditionThreat");
		hasExpeditionThreatNode.CreateOutput("Has Threat", "Bool");
		return hasExpeditionThreatNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		HasExpeditionThreatNode hasExpeditionThreatNode = (HasExpeditionThreatNode)Create(rect.position + new Vector2(20f, 20f));
		hasExpeditionThreatNode._destination = _destination;
		hasExpeditionThreatNode._threat = _threat;
		return hasExpeditionThreatNode;
	}

	protected override void NodeEnable()
	{
	}

	protected override void NodeGUI()
	{
	}

	protected override void OnNodeValidate()
	{
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output == 0)
		{
			GetInputValue(Inputs[0], ref _destination, canvas);
			GetInputValue(Inputs[1], ref _threat, canvas);
			return CastValue<T>(ExpeditionManager.Instance.HasExpeditionThreat(_destination, _threat));
		}
		throw new NotExistingOutputException(GetID, output);
	}
}
