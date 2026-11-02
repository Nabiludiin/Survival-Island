using System;
using NodeEditorFramework;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Expedition Nodes/Remove Expedition Threat Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent)
})]
public class RemoveExpeditionThreatNode : ExpeditionNode
{
	public const string ID = "EE_RemoveExpeditionThreatNode";

	private const string INPUT_DESTINATION_NAME = "Destination";

	private const string INPUT_THREAT_NAME = "Threat";

	private const string INPUT_IN_NAME = "In";

	private const string OUTPUT_OUT_NAME = "Out";

	private const int INPUT_IN_INDEX = 0;

	private const int INPUT_DESTINATION_INDEX = 1;

	private const int INPUT_THREAT_INDEX = 2;

	private const int OUTPUT_OUT_INDEX = 0;

	private const string NODE_NAME = "Remove Expedition Threat";

	[SerializeField]
	private ExpeditionDestination _destination;

	[SerializeField]
	private ExpeditionThreat _threat;

	public override string GetID => "EE_RemoveExpeditionThreatNode";

	public override Node Create(Vector2 pos)
	{
		RemoveExpeditionThreatNode removeExpeditionThreatNode = ScriptableObject.CreateInstance<RemoveExpeditionThreatNode>();
		removeExpeditionThreatNode.rect = new Rect(pos.x, pos.y, 170f, 50f);
		removeExpeditionThreatNode.name = "Remove Expedition Threat";
		removeExpeditionThreatNode.CreateMutliInput("In", "Flow");
		removeExpeditionThreatNode.CreateInput("Destination", "ExpeditionDestination");
		removeExpeditionThreatNode.CreateInput("Threat", "ExpeditionThreat");
		removeExpeditionThreatNode.CreateOutput("Out", "Flow");
		return removeExpeditionThreatNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		RemoveExpeditionThreatNode removeExpeditionThreatNode = (RemoveExpeditionThreatNode)Create(rect.position + new Vector2(20f, 20f));
		removeExpeditionThreatNode._destination = _destination;
		removeExpeditionThreatNode._threat = _threat;
		return removeExpeditionThreatNode;
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

	public override void Execute(NodeCanvas canvas)
	{
		GetInputValue(Inputs[1], ref _destination, canvas);
		GetInputValue(Inputs[2], ref _threat, canvas);
		ExpeditionManager.Instance.RemoveExpeditionThreat(_destination, _threat);
		CheckAreAllFlowOutputsConnected();
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
