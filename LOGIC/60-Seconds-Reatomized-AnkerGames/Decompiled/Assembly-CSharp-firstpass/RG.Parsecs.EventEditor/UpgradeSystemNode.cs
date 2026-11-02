using System;
using NodeEditorFramework;
using RG.Parsecs.Common;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Ship System Nodes/Upgrade System Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent)
})]
public class UpgradeSystemNode : ResourceNode
{
	public const string ID = "EE_UpgradeSystemNode";

	[SerializeField]
	private ShipSystem _shipSystem;

	private const string INPUT_IN_NAME = "In";

	private const string INPUT_SYSTEM_NAME = "Ship System";

	private const string OUTPUT_OUT_NAME = "Out";

	private const int INPUT_IN_INDEX = 0;

	private const int INPUT_SYSTEM_INDEX = 1;

	private const int OUTPUT_OUT_INDEX = 0;

	private const string OUTPUT_NOT_CONNECTED_MESSAGE = "Output is not connected";

	private const string NOT_SET_SYSTEM_MESSAGE = "Node has not set any ship system!";

	public override string GetID => "EE_UpgradeSystemNode";

	public override Node Create(Vector2 pos)
	{
		UpgradeSystemNode upgradeSystemNode = ScriptableObject.CreateInstance<UpgradeSystemNode>();
		upgradeSystemNode.rect = new Rect(pos.x, pos.y, 180f, 130f);
		upgradeSystemNode.name = "Upgrade System";
		upgradeSystemNode.CreateMutliInput("In", "Flow");
		upgradeSystemNode.CreateInput("Ship System", "ShipSystem");
		upgradeSystemNode.CreateOutput("Out", "Flow");
		return upgradeSystemNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		UpgradeSystemNode upgradeSystemNode = (UpgradeSystemNode)Create(rect.position + new Vector2(20f, 20f));
		upgradeSystemNode._shipSystem = _shipSystem;
		return upgradeSystemNode;
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
		GetInputValue(Inputs[1], ref _shipSystem, canvas);
		if (_shipSystem.IsUpgradable)
		{
			_shipSystem.Upgrade();
		}
		if (_shipSystem.Id.Equals("system_expedition"))
		{
			AvailableDestinationsChangeStatsEntry availableDestinationsChangeStatsEntry = new AvailableDestinationsChangeStatsEntry();
			availableDestinationsChangeStatsEntry.Planet = ExpeditionManager.Instance.GetPlanet();
			availableDestinationsChangeStatsEntry.AvailableDestinations = ExpeditionManager.Instance.AvailableDestinationsCount();
			StatsManager.Instance.AddAvailableDestinationsChangeStatsEntry(availableDestinationsChangeStatsEntry);
		}
		CheckAreAllFlowOutputsConnected();
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
