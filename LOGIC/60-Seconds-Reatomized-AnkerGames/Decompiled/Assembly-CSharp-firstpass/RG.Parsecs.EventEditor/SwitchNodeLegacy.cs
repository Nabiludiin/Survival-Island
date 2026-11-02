using System;
using System.Collections.Generic;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Serializable]
[Node(true, "Legacy/Switch Node Legacy", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas)
})]
public class SwitchNodeLegacy : FlowNode
{
	public const string ID = "PE_SwitchNodeLegacy";

	private const string NODE_NAME = "Switch";

	private const string INPUT_VALUE_NAME = "Value";

	private const string OUTPUT_DEFAULT_NAME = "Default";

	private const string OUTPUT_CHOICE_NAME = "Choice";

	private const string OUTPUT_RESULT_NAME = "Result";

	private const int INPUT_VALUE_INDEX = 1;

	private const int OUTPUT_DEFAULT_INDEX = 0;

	[SerializeField]
	private string _inputStringValue;

	[SerializeField]
	private int _inputIntValue;

	[SerializeField]
	private List<string> _stringValues;

	[SerializeField]
	private List<int> _intValues;

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

	public override string GetID => "PE_SwitchNodeLegacy";

	public override Node Create(Vector2 pos)
	{
		SwitchNodeLegacy switchNodeLegacy = ScriptableObject.CreateInstance<SwitchNodeLegacy>();
		switchNodeLegacy.rect = new Rect(pos.x, pos.y, 200f, 150f);
		switchNodeLegacy.name = "Switch";
		switchNodeLegacy._stringValues = new List<string> { "Empty" };
		switchNodeLegacy._intValues = new List<int> { 0 };
		switchNodeLegacy._type = EValueType.INT;
		switchNodeLegacy._lastType = EValueType.INT;
		switchNodeLegacy._created = false;
		switchNodeLegacy._isBeingDuplicated = false;
		switchNodeLegacy._lastOutputsCount = 1;
		switchNodeLegacy.CreateMutliInput("In", "Flow");
		switchNodeLegacy.CreateInput("Value", "Int");
		switchNodeLegacy.CreateOutput("Element 1", "Flow");
		switchNodeLegacy.CreateOutput("Default", "Flow");
		switchNodeLegacy.CreateOutput("Result", "Int");
		return switchNodeLegacy;
	}

	public override Node Duplicate(Vector2 pos)
	{
		SwitchNodeLegacy switchNodeLegacy = ScriptableObject.CreateInstance<SwitchNodeLegacy>();
		switchNodeLegacy.rect = new Rect(pos.x, pos.y, 200f, 150f);
		switchNodeLegacy.name = "Switch";
		switchNodeLegacy._stringValues = new List<string>(_stringValues);
		switchNodeLegacy._intValues = new List<int>(_intValues);
		switchNodeLegacy._type = _type;
		switchNodeLegacy._lastType = _lastType;
		switchNodeLegacy._created = _created;
		switchNodeLegacy._isBeingDuplicated = true;
		switchNodeLegacy._typeConnection = _typeConnection;
		switchNodeLegacy._lastOutputsCount = _lastOutputsCount;
		switchNodeLegacy.CreateInput("In", "Flow");
		switchNodeLegacy.NodeEnable();
		return switchNodeLegacy;
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

	protected override void OnNodeValidate()
	{
		base.OnNodeValidate();
		LogMessage("This node is deprecated. Use new version of this node.", EMessageType.INFO);
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != Outputs.Count - 1)
		{
			throw new NotExistingOutputException(GetID, Outputs.Count - 1);
		}
		switch (_type)
		{
		case EValueType.INT:
			GetInputValue(Inputs[1], ref _inputIntValue, canvas);
			return CastValue<T>(_inputIntValue);
		case EValueType.STRING:
			GetInputValue(Inputs[1], ref _inputStringValue, canvas);
			return CastValue<T>(_inputStringValue);
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	public override void Execute(NodeCanvas canvas)
	{
		switch (_type)
		{
		case EValueType.INT:
		{
			GetInputValue(Inputs[1], ref _inputIntValue, canvas);
			for (int j = 0; j < _intValues.Count; j++)
			{
				if (_inputIntValue == _intValues[j])
				{
					if (Outputs[j].isConnected)
					{
						Outputs[j].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
						return;
					}
					throw new NotConnectedOutputException(GetID, j);
				}
			}
			break;
		}
		case EValueType.STRING:
		{
			GetInputValue(Inputs[1], ref _inputStringValue, canvas);
			for (int i = 0; i < _stringValues.Count; i++)
			{
				if (_inputStringValue == _stringValues[i])
				{
					if (Outputs[i].isConnected)
					{
						Outputs[i].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
						return;
					}
					throw new NotConnectedOutputException(GetID, i);
				}
			}
			break;
		}
		default:
			throw new ArgumentOutOfRangeException();
		}
		if (Outputs[Outputs.Count - 2].isConnected)
		{
			Outputs[Outputs.Count - 2].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
			return;
		}
		throw new NotConnectedOutputException(GetID, Outputs.Count - 2);
	}
}
