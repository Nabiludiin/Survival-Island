using System;
using NodeEditorFramework;
using RG.Parsecs.EventEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

[Node(false, "Ship System Nodes/Set System Level Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent)
})]
public class SetSystemLevelNode : ResourceNode
{
	public const string ID = "EE_SetSystemLevelNode";

	private const string INPUT_IN_NAME = "In";

	private const string INPUT_SYSTEM_NAME = "Ship System";

	private const string INPUT_SYSTEM_LEVEL_NAME = "Level";

	private const string OUTPUT_OUT_NAME = "Out";

	private const int INPUT_IN_INDEX = 0;

	private const int INPUT_SYSTEM_INDEX = 1;

	private const int INPUT_SYSTEM_LEVEL_INDEX = 2;

	private const int OUTPUT_OUT_INDEX = 0;

	private const string OUTPUT_NOT_CONNECTED_MESSAGE = "Output is not connected";

	private const string NOT_SET_SYSTEM_MESSAGE = "Node has not set any ship system!";

	private const string NODE_NAME = "Set system level";

	private const int MINIMAL_SYSTEM_LEVEL = 1;

	[SerializeField]
	private ShipSystem _shipSystem;

	[SerializeField]
	private int _level = 1;

	public override string GetID => "EE_SetSystemLevelNode";

	public override Node Create(Vector2 pos)
	{
		SetSystemLevelNode setSystemLevelNode = ScriptableObject.CreateInstance<SetSystemLevelNode>();
		setSystemLevelNode.name = "Set system level";
		setSystemLevelNode.rect = new Rect(pos.x, pos.y, 180f, 130f);
		setSystemLevelNode.CreateMutliInput("In", "Flow");
		setSystemLevelNode.CreateInput("Ship System", "ShipSystem");
		setSystemLevelNode.CreateInput("Level", "Int");
		setSystemLevelNode.CreateOutput("Out", "Flow");
		return setSystemLevelNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		SetSystemLevelNode setSystemLevelNode = (SetSystemLevelNode)Create(rect.position + new Vector2(20f, 20f));
		setSystemLevelNode._shipSystem = _shipSystem;
		setSystemLevelNode._level = _level;
		return setSystemLevelNode;
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
		GetInputValue(Inputs[2], ref _level, canvas);
		_shipSystem.SystemLevel = Mathf.Clamp(_level, 1, _shipSystem.MaxLevel());
		CheckAreAllFlowOutputsConnected();
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
