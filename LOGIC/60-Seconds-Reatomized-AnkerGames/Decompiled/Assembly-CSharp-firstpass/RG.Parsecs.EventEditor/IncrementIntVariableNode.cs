using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Variable Nodes/Increment Int Variable Node", new Type[]
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
public class IncrementIntVariableNode : EventNode
{
	public const string ID = "EE_IncrementIntVariableNode";

	private const int INPUT_INDEX_FLOW = 0;

	private const int OUTPUT_INDEX_FLOW = 0;

	private const int DATA_INPUT_INDEX = 1;

	private const int DATA_OUTPUT_INDEX = 1;

	private const string NODE_NAME = "Increment Int Variable Node";

	private const string IN_NAME = "In";

	private const string OUT_NAME = "Out";

	private const string OUTPUT_DATA_NAME = "Data output";

	[SerializeField]
	private Variable _variable;

	[SerializeField]
	private string _previousVarName;

	[SerializeField]
	private string _eventName = string.Empty;

	public override string GetID => "EE_IncrementIntVariableNode";

	public Variable Variable => _variable;

	public override Node Create(Vector2 pos)
	{
		IncrementIntVariableNode incrementIntVariableNode = ScriptableObject.CreateInstance<IncrementIntVariableNode>();
		incrementIntVariableNode.rect = new Rect(pos.x, pos.y, 300f, 200f);
		incrementIntVariableNode.name = "Increment Int Variable Node";
		incrementIntVariableNode.CreateMutliInput("In", "Flow");
		incrementIntVariableNode.CreateOutput("Out", "Flow");
		return incrementIntVariableNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		IncrementIntVariableNode incrementIntVariableNode = (IncrementIntVariableNode)Create(rect.position + new Vector2(20f, 20f));
		incrementIntVariableNode._variable = _variable;
		incrementIntVariableNode._previousVarName = _previousVarName;
		return incrementIntVariableNode;
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
		_variable.SetValue(++num);
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 1)
		{
			throw new NodeOutputNotFoundException(output, "EE_IncrementIntVariableNode");
		}
		return CastValue<T>(_variable.GetValue());
	}
}
