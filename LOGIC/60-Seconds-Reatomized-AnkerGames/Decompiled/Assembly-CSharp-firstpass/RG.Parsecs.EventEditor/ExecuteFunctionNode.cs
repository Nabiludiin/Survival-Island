using System;
using System.Collections.Generic;
using NodeEditorFramework;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Function Nodes/Execute Function Node", new Type[]
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
public class ExecuteFunctionNode : EventNode
{
	public const string ID = "EE_ExecuteFunctionNode";

	private const string PARTIAL_NODE_NAME = "Execute: ";

	private const string NODE_NAME = "Execute Function Node";

	private const string INPUT_IN_NAME = "In";

	private const string OUTPUT_OUT_NAME = "Out";

	private const string NODE_FUNCTION_NOT_SET_MESSAGE = "Node function is not set.";

	[SerializeField]
	private NodeFunction _nodeFunction;

	[SerializeField]
	private NodeFunction _lastNodeFunction;

	[SerializeField]
	private bool _functionParsed;

	private Dictionary<string, object> _outputValues;

	private bool _refreshKnobs;

	public override string GetID => "EE_ExecuteFunctionNode";

	public NodeFunction Function => _nodeFunction;

	public override Node Create(Vector2 pos)
	{
		ExecuteFunctionNode executeFunctionNode = ScriptableObject.CreateInstance<ExecuteFunctionNode>();
		executeFunctionNode.rect = new Rect(pos.x, pos.y, 300f, 200f);
		executeFunctionNode.name = "Execute Function Node";
		executeFunctionNode.CreateMutliInput("In", "Flow");
		executeFunctionNode.CreateOutput("Out", "Flow");
		return executeFunctionNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		ExecuteFunctionNode executeFunctionNode = (ExecuteFunctionNode)Create(rect.position + new Vector2(20f, 20f));
		executeFunctionNode._nodeFunction = _nodeFunction;
		executeFunctionNode._lastNodeFunction = null;
		executeFunctionNode._functionParsed = false;
		return executeFunctionNode;
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

	private void ParseFunctionIfNecessary()
	{
	}

	private void ClearFunction()
	{
	}

	private void ParseFunction()
	{
	}

	public override void Execute(NodeCanvas canvas)
	{
		SetInputValues(canvas);
		_nodeFunction.Execute(ParentCanvas);
		SetOutputValues();
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}

	private void SetInputValues(NodeCanvas canvas)
	{
		for (int i = 1; i < Inputs.Count; i++)
		{
			_nodeFunction.SetInputValue(Inputs[i].name, GetInputValue<object>(Inputs[i], canvas));
		}
	}

	private void SetOutputValues()
	{
		if (_outputValues == null)
		{
			_outputValues = new Dictionary<string, object>();
		}
		else
		{
			_outputValues.Clear();
		}
		for (int i = 1; i < Outputs.Count; i++)
		{
			_outputValues.Add(Outputs[i].name, _nodeFunction.GetOutputValue(Outputs[i].name));
		}
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output == 0 || output > Outputs.Count - 1)
		{
			throw new NotExistingOutputException(GetID, output);
		}
		string key = Outputs[output].name;
		return CastValue<T>(_outputValues[key]);
	}
}
