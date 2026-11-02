using System;
using System.Collections.Generic;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Calculation/Operations/Multiply Node", new Type[]
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
public class MultiplyNode : CalculationNode
{
	public const string ID = "GE_MultiplyNode";

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

	private const string NODE_NAME = "Multiply";

	public override string GetID => "GE_MultiplyNode";

	public override Node Create(Vector2 pos)
	{
		MultiplyNode multiplyNode = ScriptableObject.CreateInstance<MultiplyNode>();
		multiplyNode.rect = new Rect(pos.x, pos.y, 150f, 80f);
		multiplyNode.name = "Multiply";
		multiplyNode.CreateInput("Value " + 1, "Int");
		multiplyNode.CreateInput("Value " + 2, "Int");
		multiplyNode.CreateOutput("Result", "Int");
		multiplyNode._intValues = new List<int>();
		multiplyNode._intValues.Add(0);
		multiplyNode._intValues.Add(0);
		multiplyNode._floatValues = new List<float>();
		multiplyNode._floatValues.Add(0f);
		multiplyNode._floatValues.Add(0f);
		multiplyNode._type = EValueType.INT;
		multiplyNode._lastType = EValueType.INT;
		multiplyNode._isBeingDuplicated = false;
		return multiplyNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		MultiplyNode multiplyNode = ScriptableObject.CreateInstance<MultiplyNode>();
		multiplyNode.rect = new Rect(rect.position.x + 20f, rect.position.y + 20f, rect.width, rect.height);
		multiplyNode.name = "Multiply";
		multiplyNode._isBeingDuplicated = true;
		multiplyNode._intValues = new List<int>(_intValues);
		multiplyNode._floatValues = new List<float>(_floatValues);
		multiplyNode._type = _type;
		multiplyNode._lastType = _lastType;
		multiplyNode._created = _created;
		multiplyNode._typeConnection = _typeConnection;
		switch (_type)
		{
		case EValueType.INT:
		{
			for (int j = 0; j < _intValues.Count; j++)
			{
				multiplyNode.CreateInput("Value " + (j + 1), "Int");
			}
			multiplyNode.CreateOutput("Result", "Int");
			break;
		}
		case EValueType.FLOAT:
		{
			for (int i = 0; i < _floatValues.Count; i++)
			{
				multiplyNode.CreateInput("Value " + (i + 1), "Float");
			}
			multiplyNode.CreateOutput("Result", "Float");
			break;
		}
		default:
			throw new ArgumentOutOfRangeException();
		}
		multiplyNode.NodeEnable();
		return multiplyNode;
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
		float num = 1f;
		switch (_type)
		{
		case EValueType.INT:
		{
			for (int j = 0; j < _intValues.Count; j++)
			{
				int inputValue2 = GetInputValue(Inputs[j], _intValues[j], canvas);
				if (j != _intValues.Count - 1 || inputValue2 != 0)
				{
					num *= (float)inputValue2;
				}
			}
			break;
		}
		case EValueType.FLOAT:
		{
			for (int i = 0; i < _floatValues.Count; i++)
			{
				float inputValue = GetInputValue(Inputs[i], _floatValues[i], canvas);
				if (i != _floatValues.Count - 1 || inputValue != 0f)
				{
					num *= inputValue;
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
		return _type switch
		{
			EValueType.INT => CastValue<T>(Convert.ToInt32(Calculate(canvas))), 
			EValueType.FLOAT => CastValue<T>(Calculate(canvas)), 
			_ => throw new ArgumentOutOfRangeException(), 
		};
	}
}
