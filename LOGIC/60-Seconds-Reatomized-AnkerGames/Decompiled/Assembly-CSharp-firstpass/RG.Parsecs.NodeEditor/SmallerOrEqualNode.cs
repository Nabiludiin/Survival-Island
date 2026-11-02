using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.EventEditor;
using RG.Parsecs.GoalEditor;
using UnityEngine;

namespace RG.Parsecs.NodeEditor;

[Node(false, "Condition Nodes/Smaller Or Equal Node", new Type[]
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
public class SmallerOrEqualNode : ConditionNode
{
	public const string ID = "PE_SmallerOrEqualNode";

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

	private const string NODE_NAME = "Smaller Or Equal";

	public override string GetID => "PE_SmallerOrEqualNode";

	public override Node Create(Vector2 pos)
	{
		SmallerOrEqualNode smallerOrEqualNode = ScriptableObject.CreateInstance<SmallerOrEqualNode>();
		smallerOrEqualNode.rect = new Rect(pos.x, pos.y, 150f, 85f);
		smallerOrEqualNode.name = "Smaller Or Equal";
		smallerOrEqualNode.CreateOutput("Result", "Bool");
		smallerOrEqualNode.CreateInput("A", "Int");
		smallerOrEqualNode.CreateInput("B", "Int");
		smallerOrEqualNode._type = EValueType.INT;
		smallerOrEqualNode._lastType = EValueType.INT;
		smallerOrEqualNode._isBeingDuplicated = false;
		return smallerOrEqualNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		SmallerOrEqualNode smallerOrEqualNode = ScriptableObject.CreateInstance<SmallerOrEqualNode>();
		smallerOrEqualNode.rect = new Rect(pos.x, pos.y, 150f, 85f);
		smallerOrEqualNode.name = "Smaller Or Equal";
		smallerOrEqualNode._firstValueInt = _firstValueInt;
		smallerOrEqualNode._secondValueInt = _secondValueInt;
		smallerOrEqualNode._firstValueFloat = _firstValueFloat;
		smallerOrEqualNode._secondValueFloat = _secondValueFloat;
		smallerOrEqualNode._type = _type;
		smallerOrEqualNode._lastType = _lastType;
		smallerOrEqualNode._created = _created;
		smallerOrEqualNode._isBeingDuplicated = true;
		smallerOrEqualNode.CreateOutput("Result", "Bool");
		smallerOrEqualNode.NodeEnable();
		return smallerOrEqualNode;
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

	private void RemoveConnections()
	{
		for (int i = 0; i < Inputs.Count; i++)
		{
			Inputs[i].RemoveConnection();
		}
		nodeKnobs.RemoveRange(1, nodeKnobs.Count - 1);
		Inputs.Clear();
	}

	private void CreateInputs()
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
			result = _firstValueInt <= _secondValueInt;
			break;
		case EValueType.FLOAT:
			GetInputValue(Inputs[0], ref _firstValueFloat, canvas);
			GetInputValue(Inputs[1], ref _secondValueFloat, canvas);
			result = _firstValueFloat <= _secondValueFloat;
			break;
		}
		return result;
	}
}
