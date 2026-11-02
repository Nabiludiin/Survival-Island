using System;
using NodeEditorFramework;
using RG.Parsecs.EventEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

[Node(false, "Ship System Nodes/Downgrade System Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent)
})]
public class DowngradeSystemNode : ResourceNode
{
	public const string ID = "EE_DowngradeSystemNode";

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

	private const string NODE_NAME = "Downgrade system";

	public override string GetID => "EE_DowngradeSystemNode";

	public override Node Create(Vector2 pos)
	{
		DowngradeSystemNode downgradeSystemNode = ScriptableObject.CreateInstance<DowngradeSystemNode>();
		downgradeSystemNode.name = "Downgrade system";
		downgradeSystemNode.rect = new Rect(pos.x, pos.y, 180f, 130f);
		downgradeSystemNode.CreateMutliInput("In", "Flow");
		downgradeSystemNode.CreateInput("Ship System", "ShipSystem");
		downgradeSystemNode.CreateOutput("Out", "Flow");
		return downgradeSystemNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		DowngradeSystemNode downgradeSystemNode = (DowngradeSystemNode)Create(rect.position + new Vector2(20f, 20f));
		downgradeSystemNode._shipSystem = _shipSystem;
		return downgradeSystemNode;
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
		if (_shipSystem.SystemLevel > 1)
		{
			_shipSystem.Downgrade();
		}
		CheckAreAllFlowOutputsConnected();
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
