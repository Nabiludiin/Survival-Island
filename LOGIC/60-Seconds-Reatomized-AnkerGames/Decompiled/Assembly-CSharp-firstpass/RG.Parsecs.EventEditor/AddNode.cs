using System;
using System.Collections.Generic;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Calculation/Operations/Add Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas),
	typeof(Goal),
	typeof(ConditionEvent)
})]
public class AddNode : CalculationNode
{
	public const string ID = "GE_AddNode";

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

	private const string NODE_NAME = "Add";

	public override string GetID => "GE_AddNode";

	public override Node Create(Vector2 pos)
	{
		AddNode addNode = ScriptableObject.CreateInstance<AddNode>();
		addNode.rect = new Rect(pos.x, pos.y, 150f, 80f);
		addNode.name = "Add";
		addNode.CreateInput("Value " + 1, "Int");
		addNode.CreateInput("Value " + 2, "Int");
		addNode.CreateOutput("Result", "Int");
		addNode._intValues = new List<int>();
		addNode._intValues.Add(0);
		addNode._intValues.Add(0);
		addNode._floatValues = new List<float>();
		addNode._floatValues.Add(0f);
		addNode._floatValues.Add(0f);
		addNode._type = EValueType.INT;
		addNode._lastType = EValueType.INT;
		addNode._isBeingDuplicated = false;
		return addNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		AddNode addNode = ScriptableObject.CreateInstance<AddNode>();
		addNode.rect = new Rect(rect.position.x + 20f, rect.position.y + 20f, rect.width, rect.height);
		addNode.name = "Add";
		addNode._isBeingDuplicated = true;
		addNode._intValues = new List<int>(_intValues);
		addNode._floatValues = new List<float>(_floatValues);
		addNode._type = _type;
		addNode._lastType = _lastType;
		addNode._created = _created;
		addNode._typeConnection = _typeConnection;
		switch (_type)
		{
		case EValueType.INT:
		{
			for (int j = 0; j < _intValues.Count; j++)
			{
				addNode.CreateInput("Value " + (j + 1), "Int");
			}
			addNode.CreateOutput("Result", "Int");
			break;
		}
		case EValueType.FLOAT:
		{
			for (int i = 0; i < _floatValues.Count; i++)
			{
				addNode.CreateInput("Value " + (i + 1), "Float");
			}
			addNode.CreateOutput("Result", "Float");
			break;
		}
		default:
			throw new ArgumentOutOfRangeException();
		}
		addNode.NodeEnable();
		return addNode;
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
				num += (float)inputValue2;
			}
			break;
		}
		case EValueType.FLOAT:
		{
			for (int i = 0; i < _floatValues.Count; i++)
			{
				float inputValue = GetInputValue(Inputs[i], _floatValues[i], canvas);
				num += inputValue;
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
			EValueType.INT => CastValue<T>((int)Calculate(canvas)), 
			EValueType.FLOAT => CastValue<T>(Calculate(canvas)), 
			_ => throw new ArgumentOutOfRangeException(), 
		};
	}
}
