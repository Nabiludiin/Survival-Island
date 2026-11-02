using System;
using NodeEditorFramework;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Expedition Nodes/Add Expedition Threat Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent)
})]
public class AddExpeditionThreatNode : ExpeditionNode
{
	public const string ID = "EE_AddExpeditionThreatNode";

	private const string INPUT_DESTINATION_NAME = "Destination";

	private const string INPUT_THREAT_NAME = "Threat";

	private const string INPUT_IN_NAME = "In";

	private const string OUTPUT_OUT_NAME = "Out";

	private const int INPUT_IN_INDEX = 0;

	private const int INPUT_DESTINATION_INDEX = 1;

	private const int INPUT_THREAT_INDEX = 2;

	private const int OUTPUT_OUT_INDEX = 0;

	private const string NODE_NAME = "Add Expedition Threat";

	[SerializeField]
	private ExpeditionDestination _destination;

	[SerializeField]
	private ExpeditionThreat _threat;

	public override string GetID => "EE_AddExpeditionThreatNode";

	public override Node Create(Vector2 pos)
	{
		AddExpeditionThreatNode addExpeditionThreatNode = ScriptableObject.CreateInstance<AddExpeditionThreatNode>();
		addExpeditionThreatNode.rect = new Rect(pos.x, pos.y, 170f, 50f);
		addExpeditionThreatNode.name = "Add Expedition Threat";
		addExpeditionThreatNode.CreateMutliInput("In", "Flow");
		addExpeditionThreatNode.CreateInput("Destination", "ExpeditionDestination");
		addExpeditionThreatNode.CreateInput("Threat", "ExpeditionThreat");
		addExpeditionThreatNode.CreateOutput("Out", "Flow");
		return addExpeditionThreatNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		AddExpeditionThreatNode addExpeditionThreatNode = (AddExpeditionThreatNode)Create(rect.position + new Vector2(20f, 20f));
		addExpeditionThreatNode._destination = _destination;
		addExpeditionThreatNode._threat = _threat;
		return addExpeditionThreatNode;
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
		ExpeditionManager.Instance.AddExpeditionThreat(_destination, _threat);
		CheckAreAllFlowOutputsConnected();
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
