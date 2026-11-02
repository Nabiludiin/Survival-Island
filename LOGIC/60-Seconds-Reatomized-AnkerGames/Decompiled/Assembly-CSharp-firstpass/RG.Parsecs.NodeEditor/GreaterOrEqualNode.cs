using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.EventEditor;
using RG.Parsecs.GoalEditor;
using UnityEngine;

namespace RG.Parsecs.NodeEditor;

[Node(false, "Condition Nodes/Greater Or Equal Node", new Type[]
{
	typeof(Goal),
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas),
	typeof(ConditionEvent)
})]
public class GreaterOrEqualNode : ConditionNode
{
	public const string ID = "PE_GreaterOrEqualNode";

	[SerializeField]
	private int _firstValueInt;

	[SerializeField]
	private int _secondValueInt;

	[SerializeField]
	private float _firstValueFloat;

	[SerializeField]
	private float _secondValueFloat;

	[SerializeField]
	private EValueType _type;

	[SerializeField]
	private EValueType _lastType;

	[SerializeField]
	private bool _created;

	[NonSerialized]
	private bool _isBeingDuplicated;

	private string _typeConnection = string.Empty;

	private const int INPUT_A_INDEX = 0;

	private const int INPUT_B_INDEX = 1;

	private const int OUTPUT_RESULT_INDEX = 0;

	private const string INPUT_A_NAME = "A";

	private const string INPUT_B_NAME = "B";

	private const string RESULT_OUTPUT_NAME = "Result";

	private const string NODE_NAME = "Greater or Equal";

	public override string GetID => "PE_GreaterOrEqualNode";

	public override Node Create(Vector2 pos)
	{
		GreaterOrEqualNode greaterOrEqualNode = ScriptableObject.CreateInstance<GreaterOrEqualNode>();
		greaterOrEqualNode.rect = new Rect(pos.x, pos.y, 150f, 85f);
		greaterOrEqualNode.name = "Greater or Equal";
		greaterOrEqualNode.CreateOutput("Result", "Bool");
		greaterOrEqualNode.CreateInput("A", "Int");
		greaterOrEqualNode.CreateInput("B", "Int");
		greaterOrEqualNode._type = EValueType.INT;
		greaterOrEqualNode._lastType = EValueType.INT;
		greaterOrEqualNode._isBeingDuplicated = false;
		return greaterOrEqualNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		GreaterOrEqualNode greaterOrEqualNode = ScriptableObject.CreateInstance<GreaterOrEqualNode>();
		greaterOrEqualNode.rect = new Rect(pos.x, pos.y, 150f, 85f);
		greaterOrEqualNode.name = "Greater or Equal";
		greaterOrEqualNode._firstValueInt = _firstValueInt;
		greaterOrEqualNode._secondValueInt = _secondValueInt;
		greaterOrEqualNode._firstValueFloat = _firstValueFloat;
		greaterOrEqualNode._secondValueFloat = _secondValueFloat;
		greaterOrEqualNode._type = _type;
		greaterOrEqualNode._lastType = _lastType;
		greaterOrEqualNode._created = _created;
		greaterOrEqualNode._isBeingDuplicated = true;
		greaterOrEqualNode.CreateOutput("Result", "Bool");
		greaterOrEqualNode.NodeEnable();
		return greaterOrEqualNode;
	}

	private bool ShouldCreateInputs()
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
			_isBeingDuplicated = false;
			return true;
		}
		return false;
	}

	private void CreateInputs()
	{
	}

	private void RemoveConnections()
	{
		for (int i = 0; i < Inputs.Count; i++)
		{
			Inputs[i].RemoveConnection();
		}
		nodeKnobs.RemoveRange(1, nodeKnobs.Count - 1);
		Inputs.Clear();
	}

	protected override void NodeEnable()
	{
	}

	protected override void NodeGUI()
	{
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 0)
		{
			throw new NotExistingOutputException(GetID, output);
		}
		bool flag = Calculate(canvas);
		return CastValue<T>(flag);
	}

	private bool Calculate(NodeCanvas canvas)
	{
		bool result = false;
		switch (_type)
		{
		case EValueType.INT:
			GetInputValue(Inputs[0], ref _firstValueInt, canvas);
			GetInputValue(Inputs[1], ref _secondValueInt, canvas);
			result = _firstValueInt >= _secondValueInt;
			break;
		case EValueType.FLOAT:
			GetInputValue(Inputs[0], ref _firstValueFloat, canvas);
			GetInputValue(Inputs[1], ref _secondValueFloat, canvas);
			result = _firstValueFloat >= _secondValueFloat;
			break;
		}
		return result;
	}
}
