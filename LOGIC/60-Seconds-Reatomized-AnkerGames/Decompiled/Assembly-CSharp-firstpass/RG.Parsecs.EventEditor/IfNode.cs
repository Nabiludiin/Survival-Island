using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Flow Nodes/If Node", new Type[]
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
public class IfNode : FlowNode
{
	public const string ID = "GE_IfNode";

	private const int INPUT_FLOW_INDEX = 0;

	private const int INPUT_CONDITION_INDEX = 1;

	private const int OUTPUT_TRUE_INDEX = 0;

	private const int OUTPUT_FALSE_INDEX = 1;

	private const int OUTPUT_RESULT_INDEX = 2;

	[SerializeField]
	private bool _condition;

	public override string GetID => "GE_IfNode";

	public override Node Create(Vector2 pos)
	{
		IfNode ifNode = ScriptableObject.CreateInstance<IfNode>();
		ifNode.rect = new Rect(pos.x, pos.y, 150f, 125f);
		ifNode.name = "If";
		ifNode.CreateMutliInput("In", "Flow");
		ifNode.CreateInput("Condition", "Bool");
		ifNode.CreateOutput("True", "Flow");
		ifNode.CreateOutput("False", "Flow");
		ifNode.CreateOutput("Result", "Bool");
		return ifNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		IfNode ifNode = (IfNode)Create(rect.position + new Vector2(20f, 20f));
		ifNode._condition = _condition;
		return ifNode;
	}

	protected override void NodeEnable()
	{
	}

	protected override void NodeGUI()
	{
	}

	public override void Execute(NodeCanvas canvas)
	{
		CheckAreAllFlowOutputsConnected();
		GetInputValue(Inputs[1], ref _condition, canvas);
		if (_condition)
		{
			Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
		}
		else
		{
			Outputs[1].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
		}
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 2)
		{
			throw new NotExistingOutputException(GetID, output);
		}
		GetInputValue(Inputs[1], ref _condition, canvas);
		return CastValue<T>(_condition);
	}
}
