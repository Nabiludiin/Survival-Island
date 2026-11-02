using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Type Nodes/Float Node", new Type[]
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
public class FloatNode : CalculationNode
{
	public const string ID = "EE_floatNode";

	[SerializeField]
	private float _value;

	private const string NODE_NAME = "Float Node";

	private const string OUTPUT_NAME = "Value";

	private const int OUTPUT_INDEX = 0;

	public override string GetID => "EE_floatNode";

	public override Node Create(Vector2 pos)
	{
		FloatNode floatNode = ScriptableObject.CreateInstance<FloatNode>();
		floatNode.rect = new Rect(pos.x, pos.y, 180f, 100f);
		floatNode.name = "Float Node";
		floatNode.CreateOutput("Value", "Float");
		return floatNode;
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
			throw new NodeOutputNotFoundException(output, "EE_floatNode");
		}
		return CastValue<T>(_value);
	}
}
