using System;
using System.Collections.Generic;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Serializable]
[Node(false, "Flow Nodes/Switch Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas),
	typeof(ConditionEvent)
})]
public class SwitchNode : FlowNode
{
	public const string ID = "PE_SwitchNode";

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
	private Character _inputCharacterValue;

	[SerializeField]
	private IItem _inputItemValue;

	[SerializeField]
	private CharacterStatus _inputStatusValue;

	[SerializeField]
	private List<string> _stringValues;

	[SerializeField]
	private List<int> _intValues;

	[SerializeField]
	private List<Character> _characterValues;

	[SerializeField]
	private List<IItem> _itemValues;

	[SerializeField]
	private List<CharacterStatus> _statusValues;

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

	public override string GetID => "PE_SwitchNode";

	public override Node Create(Vector2 pos)
	{
		SwitchNode switchNode = ScriptableObject.CreateInstance<SwitchNode>();
		switchNode.rect = new Rect(pos.x, pos.y, 200f, 150f);
		switchNode.name = "Switch";
		switchNode._stringValues = new List<string> { "Empty" };
		switchNode._intValues = new List<int> { 0 };
		switchNode._type = EValueType.INT;
		switchNode._lastType = EValueType.INT;
		switchNode._created = false;
		switchNode._isBeingDuplicated = false;
		switchNode._lastOutputsCount = 1;
		switchNode.CreateMutliInput("In", "Flow");
		switchNode.CreateInput("Value", "Int");
		switchNode.CreateOutput("Element 1", "Flow");
		switchNode.CreateOutput("Default", "Flow");
		switchNode.CreateOutput("Result", "Int");
		return switchNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		SwitchNode switchNode = ScriptableObject.CreateInstance<SwitchNode>();
		switchNode.rect = new Rect(pos.x, pos.y, 200f, 150f);
		switchNode.name = "Switch";
		switchNode._stringValues = new List<string>(_stringValues);
		switchNode._intValues = new List<int>(_intValues);
		switchNode._characterValues = new List<Character>(_characterValues);
		switchNode._itemValues = new List<IItem>(_itemValues);
		switchNode._statusValues = new List<CharacterStatus>(_statusValues);
		switchNode._type = _type;
		switchNode._lastType = _lastType;
		switchNode._created = _created;
		switchNode._isBeingDuplicated = true;
		switchNode._typeConnection = _typeConnection;
		switchNode._lastOutputsCount = _lastOutputsCount;
		switchNode.CreateInput("In", "Flow");
		switchNode.NodeEnable();
		return switchNode;
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
		switch (_type)
		{
		case EValueType.INT:
			GetInputValue(Inputs[1], ref _inputIntValue, canvas);
			return CastValue<T>(_inputIntValue);
		case EValueType.STRING:
			GetInputValue(Inputs[1], ref _inputStringValue, canvas);
			return CastValue<T>(_inputStringValue);
		case EValueType.CHARACTER:
			GetInputValue(Inputs[1], ref _inputCharacterValue, canvas);
			return CastValue<T>(_inputCharacterValue);
		case EValueType.ITEM:
			GetInputValue(Inputs[1], ref _inputItemValue, canvas);
			return CastValue<T>(_inputItemValue);
		case EValueType.STATUS:
			GetInputValue(Inputs[1], ref _inputStatusValue, canvas);
			return CastValue<T>(_inputStatusValue);
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
			for (int l = 0; l < _stringValues.Count; l++)
			{
				if (_inputStringValue == _stringValues[l])
				{
					if (Outputs[l].isConnected)
					{
						Outputs[l].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
						return;
					}
					throw new NotConnectedOutputException(GetID, l);
				}
			}
			break;
		}
		case EValueType.CHARACTER:
		{
			GetInputValue(Inputs[1], ref _inputCharacterValue, canvas);
			for (int m = 0; m < _characterValues.Count; m++)
			{
				if (_inputCharacterValue == _characterValues[m])
				{
					if (Outputs[m].isConnected)
					{
						Outputs[m].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
						return;
					}
					throw new NotConnectedOutputException(GetID, m);
				}
			}
			break;
		}
		case EValueType.ITEM:
		{
			GetInputValue(Inputs[1], ref _inputItemValue, canvas);
			for (int k = 0; k < _itemValues.Count; k++)
			{
				if (_inputItemValue == _itemValues[k])
				{
					if (Outputs[k].isConnected)
					{
						Outputs[k].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
						return;
					}
					throw new NotConnectedOutputException(GetID, k);
				}
			}
			break;
		}
		case EValueType.STATUS:
		{
			GetInputValue(Inputs[1], ref _inputStatusValue, canvas);
			for (int i = 0; i < _statusValues.Count; i++)
			{
				if (_inputStatusValue == _statusValues[i])
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
