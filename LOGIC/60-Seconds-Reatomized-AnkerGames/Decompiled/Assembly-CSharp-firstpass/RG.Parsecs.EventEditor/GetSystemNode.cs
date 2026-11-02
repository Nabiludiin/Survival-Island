using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Ship System Nodes/Get System Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas),
	typeof(ConditionEvent)
})]
public class GetSystemNode : ResourceNode
{
	public const string ID = "EE_GetSystemNode";

	[SerializeField]
	private ShipSystem _shipSystem;

	private const int INPUT_SYSTEM_INDEX = 0;

	private const int OUTPUT_SYSTEM_INDEX = 0;

	private const string INPUT_SYSTEM_NAME = "Get System";

	private const string OUTPUT_SYSTEM_NAME = "Ship System";

	private const string NODE_NAME = "Get System";

	private const string NOT_SET_SYSTEM_MESSAGE = "Node has not set any system!";

	public override string GetID => "EE_GetSystemNode";

	public override Node Create(Vector2 pos)
	{
		GetSystemNode getSystemNode = ScriptableObject.CreateInstance<GetSystemNode>();
		getSystemNode.rect = new Rect(pos.x, pos.y, 180f, 120f);
		getSystemNode.name = "Get System";
		getSystemNode.CreateInput("Get System", "ShipSystem");
		getSystemNode.CreateOutput("Ship System", "ShipSystem");
		return getSystemNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		GetSystemNode getSystemNode = (GetSystemNode)Create(rect.position + new Vector2(20f, 20f));
		getSystemNode._shipSystem = _shipSystem;
		return getSystemNode;
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
		if (output != 0)
		{
			throw new NotExistingOutputException(GetID, output);
		}
		GetInputValue(Inputs[0], ref _shipSystem, canvas);
		return CastValue<T>(_shipSystem);
	}
}
