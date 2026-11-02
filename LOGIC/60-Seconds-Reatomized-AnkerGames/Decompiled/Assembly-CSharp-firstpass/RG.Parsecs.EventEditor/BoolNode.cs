using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Type Nodes/Bool Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(Goal),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas),
	typeof(ConditionEvent)
})]
public class BoolNode : CalculationNode
{
	public const string ID = "EE_boolNode";

	[SerializeField]
	private bool _value = true;

	private const string NODE_NAME = "Bool Node";

	private const string OUTPUT_NAME = "Value";

	private const int OUTPUT_INDEX = 0;

	public override string GetID => "EE_boolNode";

	public override Node Create(Vector2 pos)
	{
		BoolNode boolNode = ScriptableObject.CreateInstance<BoolNode>();
		boolNode.rect = new Rect(pos.x, pos.y, 180f, 100f);
		boolNode.name = "Bool Node";
		boolNode.CreateOutput("Value", "Bool");
		return boolNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return Create(rect.position + new Vector2(20f, 20f));
	}

	protected override void NodeEnable()
	{
	}

	protected override void NodeGUI()
	{
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 0)
		{
			throw new NodeOutputNotFoundException(output, "EE_boolNode");
		}
		return CastValue<T>(_value);
	}
}
