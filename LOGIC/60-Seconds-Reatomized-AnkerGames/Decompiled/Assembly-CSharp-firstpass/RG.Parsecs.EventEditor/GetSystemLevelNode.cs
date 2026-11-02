using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Ship System Nodes/Get System Level Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas),
	typeof(Goal),
	typeof(ConditionEvent)
})]
public class GetSystemLevelNode : ResourceNode
{
	public const string ID = "EE_GetSystemLevelNode";

	[SerializeField]
	private ShipSystem _shipSystem;

	private const int INPUT_SYSTEM_INDEX = 0;

	private const int OUTPUT_LEVEL_INDEX = 0;

	private const string INPUT_SYSTEM_NAME = "Get System Level";

	private const string OUTPUT_LEVEL_NAME = "Level";

	private const string NODE_NAME = "Get System Level";

	private const string NOT_SET_SYSTEM_MESSAGE = "Node has not set any system!";

	public override string GetID => "EE_GetSystemLevelNode";

	public override Node Create(Vector2 pos)
	{
		GetSystemLevelNode getSystemLevelNode = ScriptableObject.CreateInstance<GetSystemLevelNode>();
		getSystemLevelNode.rect = new Rect(pos.x, pos.y, 180f, 120f);
		getSystemLevelNode.name = "Get System Level";
		getSystemLevelNode.CreateInput("Get System Level", "ShipSystem");
		getSystemLevelNode.CreateOutput("Level", "Int");
		return getSystemLevelNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		GetSystemLevelNode getSystemLevelNode = (GetSystemLevelNode)Create(rect.position + new Vector2(20f, 20f));
		getSystemLevelNode._shipSystem = _shipSystem;
		return getSystemLevelNode;
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
		return CastValue<T>(_shipSystem.SystemLevel);
	}
}
