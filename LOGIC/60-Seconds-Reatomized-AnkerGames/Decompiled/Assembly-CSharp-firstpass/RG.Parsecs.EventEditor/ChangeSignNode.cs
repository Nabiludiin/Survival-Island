using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Calculation/Operations/Change Sign Node", new Type[]
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
public class ChangeSignNode : CalculationNode
{
	public const string ID = "GE_ChangeSignNode";

	[SerializeField]
	private int _intValue;

	[SerializeField]
	private float _floatValue;

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

	private const int INPUT_VALUE_INDEX = 0;

	private const string NODE_NAME = "Change Sign";

	public override string GetID => "GE_ChangeSignNode";

	public override Node Create(Vector2 pos)
	{
		ChangeSignNode changeSignNode = ScriptableObject.CreateInstance<ChangeSignNode>();
		changeSignNode.rect = new Rect(pos.x, pos.y, 150f, 80f);
		changeSignNode.name = "Change Sign";
		changeSignNode.CreateInput("Value ", "Int");
		changeSignNode.CreateOutput("Result", "Int");
		changeSignNode._intValue = _intValue;
		changeSignNode._floatValue = _floatValue;
		changeSignNode._type = EValueType.INT;
		changeSignNode._lastType = EValueType.INT;
		changeSignNode._isBeingDuplicated = false;
		return changeSignNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		ChangeSignNode changeSignNode = ScriptableObject.CreateInstance<ChangeSignNode>();
		changeSignNode.rect = new Rect(rect.position.x + 20f, rect.position.y + 20f, rect.width, rect.height);
		changeSignNode.name = "Change Sign";
		changeSignNode._isBeingDuplicated = true;
		changeSignNode._intValue = _intValue;
		changeSignNode._floatValue = _floatValue;
		changeSignNode._type = _type;
		changeSignNode._lastType = _lastType;
		changeSignNode._created = _created;
		changeSignNode._typeConnection = _typeConnection;
		switch (_type)
		{
		case EValueType.INT:
			changeSignNode.CreateInput("Value ", "Int");
			changeSignNode.CreateOutput("Result", "Int");
			break;
		case EValueType.FLOAT:
			changeSignNode.CreateInput("Value ", "Float");
			changeSignNode.CreateOutput("Result", "Float");
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		changeSignNode.NodeEnable();
		return changeSignNode;
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
		Inputs[0].RemoveConnection();
		for (int i = 0; i < Outputs[0].connections.Count; i++)
		{
			Outputs[0].connections[i].RemoveConnection();
		}
		nodeKnobs.Clear();
		Outputs.Clear();
		Inputs.Clear();
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
		return _type switch
		{
			EValueType.INT => GetInputValue(Inputs[0], _intValue, canvas) * -1, 
			EValueType.FLOAT => GetInputValue(Inputs[0], _floatValue, canvas) * -1f, 
			_ => throw new ArgumentOutOfRangeException(), 
		};
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
