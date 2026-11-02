using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Variable Nodes/Decrement Int Variable Node", new Type[]
{
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(ExpeditionEvent),
	typeof(SystemStatusEvent),
	typeof(EndGameCanvas),
	typeof(Goal)
})]
public class DecrementIntVariableNode : EventNode
{
	public const string ID = "EE_DecrementIntVariableNode";

	private const int INPUT_INDEX_FLOW = 0;

	private const int OUTPUT_INDEX_FLOW = 0;

	private const int DATA_INPUT_INDEX = 1;

	private const int DATA_OUTPUT_INDEX = 1;

	private const string NODE_NAME = "Decrement Int Variable Node";

	private const string IN_NAME = "In";

	private const string OUT_NAME = "Out";

	private const string OUTPUT_DATA_NAME = "Data output";

	[SerializeField]
	private Variable _variable;

	[SerializeField]
	private string _previousVarName;

	[SerializeField]
	private string _eventName = string.Empty;

	public override string GetID => "EE_DecrementIntVariableNode";

	public Variable Variable => _variable;

	public override Node Create(Vector2 pos)
	{
		DecrementIntVariableNode decrementIntVariableNode = ScriptableObject.CreateInstance<DecrementIntVariableNode>();
		decrementIntVariableNode.rect = new Rect(pos.x, pos.y, 300f, 200f);
		decrementIntVariableNode.name = "Decrement Int Variable Node";
		decrementIntVariableNode.CreateMutliInput("In", "Flow");
		decrementIntVariableNode.CreateOutput("Out", "Flow");
		return decrementIntVariableNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		DecrementIntVariableNode decrementIntVariableNode = (DecrementIntVariableNode)Create(rect.position + new Vector2(20f, 20f));
		decrementIntVariableNode._variable = _variable;
		decrementIntVariableNode._previousVarName = _previousVarName;
		return decrementIntVariableNode;
	}

	protected override void NodeEnable()
	{
	}

	private void DrawVariablesKnobes()
	{
	}

	protected override void NodeGUI()
	{
	}

	private void CheckLocalVariables()
	{
	}

	protected override void OnNodeValidate()
	{
	}

	public override void Execute(NodeCanvas canvas)
	{
		int num = (int)_variable.GetValue();
		_variable.SetValue(--num);
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 1)
		{
			throw new NodeOutputNotFoundException(output, "EE_DecrementIntVariableNode");
		}
		return CastValue<T>(_variable.GetValue());
	}
}
