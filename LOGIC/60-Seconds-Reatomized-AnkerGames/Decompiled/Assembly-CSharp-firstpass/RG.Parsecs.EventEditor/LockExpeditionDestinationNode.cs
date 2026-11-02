using System;
using NodeEditorFramework;
using RG.Parsecs.Common;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Expedition Nodes/Lock Expedition Destination Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent)
})]
public class LockExpeditionDestinationNode : ExpeditionNode
{
	public const string ID = "EE_LockExpeditionDestinationNode";

	private const string INPUT_DESTINATION_NAME = "Destination";

	private const string INPUT_IN_NAME = "In";

	private const string OUTPUT_OUT_NAME = "Out";

	private const int INPUT_DESTINATION_INDEX = 1;

	private const int INPUT_IN_INDEX = 0;

	private const int OUTPUT_OUT_INDEX = 0;

	private const string NODE_NAME = "Lock Expedition Destination";

	[SerializeField]
	private ExpeditionDestination _destination;

	public override string GetID => "EE_LockExpeditionDestinationNode";

	public override Node Create(Vector2 pos)
	{
		LockExpeditionDestinationNode lockExpeditionDestinationNode = ScriptableObject.CreateInstance<LockExpeditionDestinationNode>();
		lockExpeditionDestinationNode.rect = new Rect(pos.x, pos.y, 170f, 50f);
		lockExpeditionDestinationNode.name = "Lock Expedition Destination";
		lockExpeditionDestinationNode.CreateMutliInput("In", "Flow");
		lockExpeditionDestinationNode.CreateInput("Destination", "ExpeditionDestination");
		lockExpeditionDestinationNode.CreateOutput("Out", "Flow");
		return lockExpeditionDestinationNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		LockExpeditionDestinationNode lockExpeditionDestinationNode = (LockExpeditionDestinationNode)Create(rect.position + new Vector2(20f, 20f));
		lockExpeditionDestinationNode._destination = _destination;
		return lockExpeditionDestinationNode;
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
		ExpeditionManager.Instance.LockExpeditionDestination(_destination);
		AvailableDestinationsChangeStatsEntry availableDestinationsChangeStatsEntry = new AvailableDestinationsChangeStatsEntry();
		availableDestinationsChangeStatsEntry.Planet = ExpeditionManager.Instance.GetPlanet();
		availableDestinationsChangeStatsEntry.AvailableDestinations = ExpeditionManager.Instance.AvailableDestinationsCount();
		StatsManager.Instance.AddAvailableDestinationsChangeStatsEntry(availableDestinationsChangeStatsEntry);
		CheckAreAllFlowOutputsConnected();
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
