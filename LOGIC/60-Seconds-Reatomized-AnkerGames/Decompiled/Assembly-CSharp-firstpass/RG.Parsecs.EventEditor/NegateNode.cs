using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Condition Nodes/Negate Node", new Type[]
{
	typeof(Goal),
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas),
	typeof(ConditionEvent)
})]
public class NegateNode : ConditionNode
{
	public const string ID = "PE_NegateNode";

	[SerializeField]
	private bool _value;

	private const int INPUT_VALUE_INDEX = 0;

	private const int OUTPUT_RESULT_INDEX = 0;

	public override string GetID => "PE_NegateNode";

	public override Node Create(Vector2 pos)
	{
		NegateNode negateNode = ScriptableObject.CreateInstance<NegateNode>();
		negateNode.rect = new Rect(pos.x, pos.y, 100f, 70f);
		negateNode.name = "Negate";
		negateNode.CreateInput("Value", "Bool");
		negateNode.CreateOutput("Result", "Bool");
		return negateNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		NegateNode negateNode = (NegateNode)Create(rect.position + new Vector2(20f, 20f));
		negateNode._value = _value;
		return negateNode;
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
			throw new NotExistingOutputException(GetID, output);
		}
		GetInputValue(Inputs[0], ref _value, canvas);
		return CastValue<T>(!_value);
	}
}
