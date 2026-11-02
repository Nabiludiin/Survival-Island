using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Variable Nodes/Set Variable Node", new Type[]
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
public class SetVariableNode : EventNode
{
	public const string ID = "EE_SetVariableNode";

	private const int INPUT_INDEX_FLOW = 0;

	private const int OUTPUT_INDEX_FLOW = 0;

	private const int DATA_INPUT_INDEX = 1;

	private const int DATA_OUTPUT_INDEX = 1;

	private const string NODE_NAME = "Set Variable Node";

	private const string IN_NAME = "In";

	private const string OUT_NAME = "Out";

	private const string INPUT_DATA_NAME = "Data";

	private const string OUTPUT_DATA_NAME = "Data output";

	[SerializeField]
	private Variable _variable;

	[SerializeField]
	private object _data;

	[SerializeField]
	private string _previousVarName;

	[SerializeField]
	private string _eventName = string.Empty;

	public override string GetID => "EE_SetVariableNode";

	public Variable Variable => _variable;

	public override Node Create(Vector2 pos)
	{
		SetVariableNode setVariableNode = ScriptableObject.CreateInstance<SetVariableNode>();
		setVariableNode.rect = new Rect(pos.x, pos.y, 300f, 200f);
		setVariableNode.name = "Set Variable Node";
		setVariableNode.CreateMutliInput("In", "Flow");
		setVariableNode.CreateOutput("Out", "Flow");
		return setVariableNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		SetVariableNode setVariableNode = (SetVariableNode)Create(rect.position + new Vector2(20f, 20f));
		setVariableNode._variable = _variable;
		setVariableNode._data = _data;
		setVariableNode._previousVarName = _previousVarName;
		return setVariableNode;
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
		object currentValue = _data;
		GetInputValue(Inputs[1], ref currentValue, canvas);
		_variable.SetValue(currentValue);
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 1)
		{
			throw new NodeOutputNotFoundException(output, "EE_SetVariableNode");
		}
		return CastValue<T>(_variable.GetValue());
	}
}
