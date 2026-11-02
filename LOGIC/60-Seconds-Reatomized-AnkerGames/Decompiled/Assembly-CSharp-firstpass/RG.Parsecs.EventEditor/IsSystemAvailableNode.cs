using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Ship System Nodes/Is System Available Node", new Type[]
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
public class IsSystemAvailableNode : ResourceNode
{
	public const string ID = "EE_IsSystemAvailableNode";

	[SerializeField]
	private ShipSystem _shipSystem;

	private const int INPUT_SYSTEM_INDEX = 0;

	private const int OUTPUT_IS_AVAILABLE_INDEX = 0;

	private const string INPUT_SYSTEM_NAME = "Ship System";

	private const string OUTPUT_IS_AVAILABLE_NAME = "Is Available";

	private const string NODE_NAME = "Is System Available";

	private const string NOT_SET_SYSTEM_MESSAGE = "Node has not set any system!";

	public override string GetID => "EE_IsSystemAvailableNode";

	public override Node Create(Vector2 pos)
	{
		IsSystemAvailableNode isSystemAvailableNode = ScriptableObject.CreateInstance<IsSystemAvailableNode>();
		isSystemAvailableNode.rect = new Rect(pos.x, pos.y, 180f, 120f);
		isSystemAvailableNode.name = "Is System Available";
		isSystemAvailableNode.CreateInput("Ship System", "ShipSystem");
		isSystemAvailableNode.CreateOutput("Is Available", "Bool");
		return isSystemAvailableNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		IsSystemAvailableNode isSystemAvailableNode = (IsSystemAvailableNode)Create(rect.position + new Vector2(20f, 20f));
		isSystemAvailableNode._shipSystem = _shipSystem;
		return isSystemAvailableNode;
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
		return CastValue<T>(_shipSystem.IsAvailable);
	}
}
