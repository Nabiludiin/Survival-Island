using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Condition Nodes/Not Equal Node", new Type[]
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
public class NotEqualNode : ConditionNode
{
	public const string ID = "PE_NotEqualNode";

	[SerializeField]
	private bool _boolValue1;

	[SerializeField]
	private bool _boolValue2;

	[SerializeField]
	private int _intValue1;

	[SerializeField]
	private int _intValue2;

	[SerializeField]
	private string _stringValue1;

	[SerializeField]
	private string _stringValue2;

	[SerializeField]
	private float _floatValue1;

	[SerializeField]
	private float _floatValue2;

	[SerializeField]
	private EValueType _type;

	[SerializeField]
	private bool _created;

	[SerializeField]
	private EValueType _lastType;

	[NonSerialized]
	private bool _isBeingDuplicated;

	private const string FIRST_NODE_INPUT_NAME = "Value 1";

	private const string SECOND_NODE_INPUT_NAME = "Value 2";

	private const string OUTPUT_NAME = "Output";

	private const int OUTPUT_INDEX = 0;

	private const int INPUT_FIRST_INDEX = 0;

	private const int INPUT_SECOND_INDEX = 1;

	public override string GetID => "PE_NotEqualNode";

	public override Node Create(Vector2 pos)
	{
		NotEqualNode notEqualNode = ScriptableObject.CreateInstance<NotEqualNode>();
		notEqualNode.rect = new Rect(pos.x, pos.y, 150f, 130f);
		notEqualNode.name = "Not Equal";
		notEqualNode._type = EValueType.INT;
		notEqualNode._lastType = EValueType.INT;
		notEqualNode.CreateOutput("Output", "Bool");
		notEqualNode.CreateInput("Value 1", "Int");
		notEqualNode.CreateInput("Value 2", "Int");
		notEqualNode._isBeingDuplicated = false;
		return notEqualNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		NotEqualNode notEqualNode = (NotEqualNode)Create(rect.position + new Vector2(20f, 20f));
		notEqualNode._isBeingDuplicated = true;
		notEqualNode._boolValue1 = _boolValue1;
		notEqualNode._boolValue2 = _boolValue2;
		notEqualNode._floatValue1 = _floatValue1;
		notEqualNode._floatValue2 = _floatValue2;
		notEqualNode._stringValue1 = _stringValue1;
		notEqualNode._stringValue2 = _stringValue2;
		notEqualNode._intValue1 = _intValue1;
		notEqualNode._intValue2 = _intValue2;
		notEqualNode._type = _type;
		notEqualNode._created = _created;
		notEqualNode._lastType = _lastType;
		return notEqualNode;
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
			Inputs[i].HideLayout();
			if (Inputs[i] is NodeMultiInput)
			{
				((NodeMultiInput)Inputs[i]).RemoveConnection();
			}
			else
			{
				Inputs[i].RemoveConnection();
			}
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
		bool flag = false;
		switch (_type)
		{
		case EValueType.INT:
			GetInputValue(Inputs[0], ref _intValue1, canvas);
			GetInputValue(Inputs[1], ref _intValue2, canvas);
			flag = _intValue1 != _intValue2;
			break;
		case EValueType.FLOAT:
			GetInputValue(Inputs[0], ref _floatValue1, canvas);
			GetInputValue(Inputs[1], ref _floatValue2, canvas);
			flag = !_floatValue1.Equals(_floatValue2);
			break;
		case EValueType.STRING:
			GetInputValue(Inputs[0], ref _stringValue1, canvas);
			GetInputValue(Inputs[1], ref _stringValue2, canvas);
			flag = !_stringValue1.Equals(_stringValue2);
			break;
		case EValueType.BOOL:
			GetInputValue(Inputs[0], ref _boolValue1, canvas);
			GetInputValue(Inputs[1], ref _boolValue2, canvas);
			flag = _boolValue1 != _boolValue2;
			break;
		}
		return CastValue<T>(flag);
	}
}
