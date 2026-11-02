using System;
using System.Collections.Generic;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Calculation/Operations/Divide Node", new Type[]
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
public class DivideNode : CalculationNode
{
	public const string ID = "GE_DivideNode";

	[SerializeField]
	private List<int> _intValues;

	[SerializeField]
	private List<float> _floatValues;

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

	private const string INPUT_VALUE_NAME = "Value ";

	private const string OUTPUT_RESULT_NAME = "Result";

	private const int OUTPUT_RESULT_INDEX = 0;

	private const string NODE_NAME = "Divide";

	public override string GetID => "GE_DivideNode";

	public override Node Create(Vector2 pos)
	{
		DivideNode divideNode = ScriptableObject.CreateInstance<DivideNode>();
		divideNode.rect = new Rect(pos.x, pos.y, 150f, 80f);
		divideNode.name = "Divide";
		divideNode.CreateInput("Value " + 1, "Int");
		divideNode.CreateInput("Value " + 2, "Int");
		divideNode.CreateOutput("Result", "Float");
		divideNode._intValues = new List<int>();
		divideNode._intValues.Add(0);
		divideNode._intValues.Add(0);
		divideNode._floatValues = new List<float>();
		divideNode._floatValues.Add(0f);
		divideNode._floatValues.Add(0f);
		divideNode._type = EValueType.INT;
		divideNode._lastType = EValueType.INT;
		divideNode._isBeingDuplicated = false;
		return divideNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		DivideNode divideNode = ScriptableObject.CreateInstance<DivideNode>();
		divideNode.rect = new Rect(rect.position.x + 20f, rect.position.y + 20f, rect.width, rect.height);
		divideNode.name = "Divide";
		divideNode._isBeingDuplicated = true;
		divideNode._intValues = new List<int>(_intValues);
		divideNode._floatValues = new List<float>(_floatValues);
		divideNode._type = _type;
		divideNode._lastType = _lastType;
		divideNode._created = _created;
		divideNode._typeConnection = _typeConnection;
		switch (_type)
		{
		case EValueType.INT:
		{
			for (int j = 0; j < _intValues.Count; j++)
			{
				divideNode.CreateInput("Value " + (j + 1), "Int");
			}
			break;
		}
		case EValueType.FLOAT:
		{
			for (int i = 0; i < _floatValues.Count; i++)
			{
				divideNode.CreateInput("Value " + (i + 1), "Float");
			}
			break;
		}
		default:
			throw new ArgumentOutOfRangeException();
		}
		divideNode.NodeEnable();
		divideNode.CreateOutput("Result", "Float");
		return divideNode;
	}

	private bool HasTypeChanged()
	{
		if (_lastType != _type)
		{
			_created = false;
			_lastType = _type;
			return true;
		}
		if (_isBeingDuplicated)
		{
			_created = false;
		}
		return false;
	}

	private void RemoveConnections()
	{
		for (int i = 0; i < Inputs.Count; i++)
		{
			Inputs[i].RemoveConnection();
		}
		for (int j = 0; j < Outputs.Count; j++)
		{
			for (int k = 0; k < Outputs[j].connections.Count; k++)
			{
				Outputs[j].connections[k].RemoveConnection();
			}
		}
		nodeKnobs.Clear();
		Outputs.Clear();
		Inputs.Clear();
	}

	private void UpdateInputsCount()
	{
	}

	protected override void NodeEnable()
	{
	}

	protected override void NodeGUI()
	{
	}

	private float Calculate(NodeCanvas canvas)
	{
		float num = 0f;
		switch (_type)
		{
		case EValueType.INT:
		{
			for (int j = 0; j < _intValues.Count; j++)
			{
				int inputValue2 = GetInputValue(Inputs[j], _intValues[j], canvas);
				if (j == 0)
				{
					num = inputValue2;
				}
				else if (j != _intValues.Count - 1 || inputValue2 != 0)
				{
					num /= (float)inputValue2;
				}
			}
			break;
		}
		case EValueType.FLOAT:
		{
			for (int i = 0; i < _floatValues.Count; i++)
			{
				float inputValue = GetInputValue(Inputs[i], _floatValues[i], canvas);
				if (i == 0)
				{
					num = inputValue;
				}
				else if (i != _floatValues.Count - 1 || inputValue != 0f)
				{
					num /= inputValue;
				}
			}
			break;
		}
		default:
			throw new ArgumentOutOfRangeException();
		}
		return num;
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 0)
		{
			throw new NotExistingOutputException(GetID, output);
		}
		return CastValue<T>(Calculate(canvas));
	}
}
