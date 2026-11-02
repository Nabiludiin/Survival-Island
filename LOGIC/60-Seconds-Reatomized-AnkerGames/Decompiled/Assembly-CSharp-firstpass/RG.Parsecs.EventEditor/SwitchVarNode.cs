using System;
using System.Collections.Generic;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Serializable]
[Node(false, "Flow Nodes/Switch Var Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas)
})]
public class SwitchVarNode : FlowNode
{
	public const string ID = "PE_SwitchVarNode";

	private const string NODE_NAME = "Switch Var";

	private const string INPUT_VALUE_NAME = "Value";

	private const string OUTPUT_DEFAULT_NAME = "Default";

	private const string OUTPUT_CHOICE_NAME = "Choice";

	private const string OUTPUT_RESULT_NAME = "Result";

	private const int INPUT_VALUE_INDEX = 1;

	private const int OUTPUT_DEFAULT_INDEX = 0;

	[SerializeField]
	private int _inputIntValue;

	[SerializeField]
	private List<Variable> _values;

	[SerializeField]
	private EValueType _type;

	[SerializeField]
	private EValueType _lastType;

	[SerializeField]
	private bool _created;

	[NonSerialized]
	private bool _isBeingDuplicated;

	[SerializeField]
	private string _typeConnection = string.Empty;

	[SerializeField]
	private int _lastOutputsCount;

	public override string GetID => "PE_SwitchVarNode";

	public override Node Create(Vector2 pos)
	{
		SwitchVarNode switchVarNode = ScriptableObject.CreateInstance<SwitchVarNode>();
		switchVarNode.rect = new Rect(pos.x, pos.y, 200f, 150f);
		switchVarNode.name = "Switch Var";
		switchVarNode._values = new List<Variable> { null };
		switchVarNode._type = EValueType.INT;
		switchVarNode._lastType = EValueType.INT;
		switchVarNode._created = false;
		switchVarNode._isBeingDuplicated = false;
		switchVarNode._lastOutputsCount = 1;
		switchVarNode.CreateMutliInput("In", "Flow");
		switchVarNode.CreateInput("Value", "Int");
		switchVarNode.CreateOutput("Element 1", "Flow");
		switchVarNode.CreateOutput("Default", "Flow");
		switchVarNode.CreateOutput("Result", "Int");
		return switchVarNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		SwitchVarNode switchVarNode = ScriptableObject.CreateInstance<SwitchVarNode>();
		switchVarNode.rect = new Rect(pos.x, pos.y, 200f, 150f);
		switchVarNode.name = "Switch Var";
		switchVarNode._values = new List<Variable>(_values);
		switchVarNode._type = _type;
		switchVarNode._lastType = _lastType;
		switchVarNode._created = _created;
		switchVarNode._isBeingDuplicated = true;
		switchVarNode._typeConnection = _typeConnection;
		switchVarNode._lastOutputsCount = _lastOutputsCount;
		switchVarNode.CreateInput("In", "Flow");
		switchVarNode.NodeEnable();
		return switchVarNode;
	}

	private void SetProperties()
	{
	}

	private bool ShouldCreateKnobs()
	{
		return false;
	}

	private void RemoveConnections()
	{
	}

	private void CreateKnobs()
	{
	}

	protected override void NodeEnable()
	{
	}

	protected override void NodeGUI()
	{
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != Outputs.Count - 1)
		{
			throw new NotExistingOutputException(GetID, Outputs.Count - 1);
		}
		if (_type == EValueType.INT)
		{
			GetInputValue(Inputs[1], ref _inputIntValue, canvas);
			return CastValue<T>(_inputIntValue);
		}
		throw new ArgumentOutOfRangeException();
	}

	public override void Execute(NodeCanvas canvas)
	{
		if (_type == EValueType.INT)
		{
			GetInputValue(Inputs[1], ref _inputIntValue, canvas);
			for (int i = 0; i < _values.Count; i++)
			{
				if (_inputIntValue == (int)_values[i].GetValue())
				{
					if (Outputs[i].isConnected)
					{
						Outputs[i].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
						return;
					}
					throw new NotConnectedOutputException(GetID, i);
				}
			}
			if (Outputs[Outputs.Count - 2].isConnected)
			{
				Outputs[Outputs.Count - 2].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
				return;
			}
			throw new NotConnectedOutputException(GetID, Outputs.Count - 2);
		}
		throw new ArgumentOutOfRangeException();
	}
}
