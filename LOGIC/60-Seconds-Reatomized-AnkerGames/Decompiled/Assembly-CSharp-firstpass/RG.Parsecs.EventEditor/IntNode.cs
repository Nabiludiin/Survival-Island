using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Type Nodes/Int Node", new Type[]
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
public class IntNode : CalculationNode
{
	public const string ID = "EE_intNode";

	[SerializeField]
	private int _value;

	private const string NODE_NAME = "Int Node";

	private const string OUTPUT_NAME = "Value";

	private const int OUTPUT_INDEX = 0;

	public override string GetID => "EE_intNode";

	public override Node Create(Vector2 pos)
	{
		IntNode intNode = ScriptableObject.CreateInstance<IntNode>();
		intNode.rect = new Rect(pos.x, pos.y, 180f, 100f);
		intNode.name = "Int Node";
		intNode.CreateOutput("Value", "Int");
		return intNode;
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
			throw new NodeOutputNotFoundException(output, "EE_intNode");
		}
		return CastValue<T>(_value);
	}
}
