using System;
using NodeEditorFramework;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Function Nodes/Execute Function Only Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(Goal),
	typeof(ConditionEvent)
})]
public class ExecuteFunctionOnlyNode : EventNode
{
	public const string ID = "EE_ExecuteFunctionOnlyNode";

	private const string NODE_NAME = "Execute Function Node";

	private const string INPUT_IN_NAME = "In";

	private const string INPUT_FUNCTION_NAME = "Function";

	private const string OUTPUT_OUT_NAME = "Out";

	[SerializeField]
	private NodeFunction _nodeFunction;

	public override string GetID => "EE_ExecuteFunctionOnlyNode";

	public NodeFunction Function => _nodeFunction;

	public override Node Create(Vector2 pos)
	{
		ExecuteFunctionOnlyNode executeFunctionOnlyNode = ScriptableObject.CreateInstance<ExecuteFunctionOnlyNode>();
		executeFunctionOnlyNode.rect = new Rect(pos.x, pos.y, 300f, 200f);
		executeFunctionOnlyNode.name = "Execute Function Node";
		executeFunctionOnlyNode.CreateMutliInput("In", "Flow");
		executeFunctionOnlyNode.CreateInput("Function", "NodeFunction");
		executeFunctionOnlyNode.CreateOutput("Out", "Flow");
		return executeFunctionOnlyNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return Create(rect.position + new Vector2(20f, 20f));
	}

	public override void Execute(NodeCanvas canvas)
	{
		GetInputValue(Inputs[1], ref _nodeFunction, canvas);
		_nodeFunction.Execute(ParentCanvas);
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
