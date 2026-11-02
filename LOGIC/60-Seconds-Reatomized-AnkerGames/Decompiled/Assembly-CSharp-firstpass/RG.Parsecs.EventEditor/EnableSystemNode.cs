using System;
using NodeEditorFramework;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Ship System Nodes/Enable System Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent)
})]
public class EnableSystemNode : ResourceNode
{
	public const string ID = "EE_EnableSystemNode";

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

	public override string GetID => "EE_EnableSystemNode";

	public override Node Create(Vector2 pos)
	{
		EnableSystemNode enableSystemNode = ScriptableObject.CreateInstance<EnableSystemNode>();
		enableSystemNode.rect = new Rect(pos.x, pos.y, 180f, 130f);
		enableSystemNode.name = "Enable System";
		enableSystemNode.CreateMutliInput("In", "Flow");
		enableSystemNode.CreateInput("Ship System", "ShipSystem");
		enableSystemNode.CreateOutput("Out", "Flow");
		return enableSystemNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		EnableSystemNode enableSystemNode = (EnableSystemNode)Create(rect.position + new Vector2(20f, 20f));
		enableSystemNode._shipSystem = _shipSystem;
		return enableSystemNode;
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
		_shipSystem.IsAvailable = true;
		_shipSystem.SetDisabledReason("", "");
		CheckAreAllFlowOutputsConnected();
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
