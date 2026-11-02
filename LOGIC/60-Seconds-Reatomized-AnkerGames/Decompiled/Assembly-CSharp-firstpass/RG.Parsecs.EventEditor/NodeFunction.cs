using System.Collections.Generic;
using NodeEditorFramework;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[NodeCanvasType("Event Editor/Node Function", "Create new Node Function", "New Node Function", "Create new Node Function")]
[CreateAssetMenu(menuName = "60 Parsecs!/Event Editor/New Node Function")]
public class NodeFunction : NodeCanvas
{
	[SerializeField]
	[Tooltip("Name of the function that will be displayed as a name of the node that invokes it.")]
	private string _functionName;

	[SerializeField]
	private NodeCanvas _invokingCanvas;

	[SerializeField]
	private NodeFunctionConnectionDefinition[] _inputs;

	[SerializeField]
	private NodeFunctionConnectionDefinition[] _outputs;

	private List<SetOutputValueNode> _outputNodes;

	private List<string> _names = new List<string>();

	public string FunctionName => _functionName;

	public NodeFunctionConnectionDefinition[] Inputs => _inputs;

	public NodeFunctionConnectionDefinition[] Outputs => _outputs;

	public void SetInputValue(string inputName, object value)
	{
		for (int i = 0; i < _inputs.Length; i++)
		{
			if (_inputs[i].ConnectionName.Equals(inputName))
			{
				_inputs[i].RuntimeValue = value;
			}
		}
	}

	public object GetInputValue(string inputName)
	{
		for (int i = 0; i < _inputs.Length; i++)
		{
			if (_inputs[i].ConnectionName.Equals(inputName))
			{
				return _inputs[i].RuntimeValue;
			}
		}
		throw new UnityException($"Function input {inputName} was not found in function {_functionName}");
	}

	public void SetOutputValue(string outputName, object value)
	{
		for (int i = 0; i < _outputs.Length; i++)
		{
			if (_outputs[i].ConnectionName.Equals(outputName))
			{
				_outputs[i].RuntimeValue = value;
			}
		}
	}

	public object GetOutputValue(string outputName)
	{
		for (int i = 0; i < _outputs.Length; i++)
		{
			if (_outputs[i].ConnectionName.Equals(outputName))
			{
				return _outputs[i].RuntimeValue;
			}
		}
		throw new UnityException($"Function output {outputName} was not found in function {_functionName}");
	}

	public NodeFunctionConnectionDefinition GetInputConnectionDefinition(string inputName)
	{
		for (int i = 0; i < _inputs.Length; i++)
		{
			if (_inputs[i].ConnectionName.Equals(inputName))
			{
				return _inputs[i];
			}
		}
		return null;
	}

	public NodeFunctionConnectionDefinition GetOutputConnectionDefinition(string outputName)
	{
		for (int i = 0; i < _outputs.Length; i++)
		{
			if (_outputs[i].ConnectionName.Equals(outputName))
			{
				return _outputs[i];
			}
		}
		return null;
	}

	public NodeFunctionConnectionDefinition.EConnectionDefinitionMatchState CheckIsInputDefinitionMatched(NodeFunctionConnectionDefinition inputDef, out string originalDefType)
	{
		NodeFunctionConnectionDefinition inputConnectionDefinition = GetInputConnectionDefinition(inputDef.ConnectionName);
		return CheckIsDefinitionMatched(inputConnectionDefinition, inputDef, out originalDefType);
	}

	public NodeFunctionConnectionDefinition.EConnectionDefinitionMatchState CheckIsOutputDefinitionMatched(NodeFunctionConnectionDefinition outputDef, out string originalDefType)
	{
		NodeFunctionConnectionDefinition outputConnectionDefinition = GetOutputConnectionDefinition(outputDef.ConnectionName);
		return CheckIsDefinitionMatched(outputConnectionDefinition, outputDef, out originalDefType);
	}

	private NodeFunctionConnectionDefinition.EConnectionDefinitionMatchState CheckIsDefinitionMatched(NodeFunctionConnectionDefinition first, NodeFunctionConnectionDefinition other, out string firstDefType)
	{
		if (first == null)
		{
			firstDefType = string.Empty;
			return NodeFunctionConnectionDefinition.EConnectionDefinitionMatchState.NOT_FOUND;
		}
		firstDefType = first.ConnectionTypeId;
		if (!other.ConnectionTypeId.Equals(first.ConnectionTypeId))
		{
			return NodeFunctionConnectionDefinition.EConnectionDefinitionMatchState.DIFFERENT_TYPE;
		}
		return NodeFunctionConnectionDefinition.EConnectionDefinitionMatchState.MATCHING;
	}

	private void CanvasGUI()
	{
	}

	public void DrawInfoGUI()
	{
	}

	public virtual void Execute(NodeCanvas invokingCanvas)
	{
		_invokingCanvas = invokingCanvas;
		ExecuteNode("GE_StartNode");
		_invokingCanvas = null;
	}

	protected void ExecuteNode(string nodeId)
	{
		EventNode eventNode = FindNodeOfId<EventNode>(nodeId);
		if (eventNode == null)
		{
			throw new NodeNotFoundException(nodeId, name);
		}
		eventNode.ExecuteWithErrorHandling(this);
	}

	public T GetOriginalCanvasAs<T>() where T : NodeCanvas
	{
		if (_invokingCanvas == null || _invokingCanvas == this)
		{
			return EventManager.Instance.RuntimeEvent as T;
		}
		NodeFunction nodeFunction = _invokingCanvas as NodeFunction;
		if (nodeFunction != null)
		{
			return nodeFunction.GetOriginalCanvasAs<T>();
		}
		return _invokingCanvas as T;
	}
}
