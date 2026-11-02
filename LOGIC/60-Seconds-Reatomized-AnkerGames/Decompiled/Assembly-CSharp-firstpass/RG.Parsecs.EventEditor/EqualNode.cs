using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Condition Nodes/Equal Node", new Type[]
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
public class EqualNode : ConditionNode
{
	public const string ID = "PE_EqualNode";

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
	private CharacterStatus _statusValue1;

	[SerializeField]
	private CharacterStatus _statusValue2;

	[SerializeField]
	private Character _characterValue1;

	[SerializeField]
	private Character _characterValue2;

	[SerializeField]
	private IItem _itemValue1;

	[SerializeField]
	private IItem _itemValue2;

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

	public override string GetID => "PE_EqualNode";

	public override Node Create(Vector2 pos)
	{
		EqualNode equalNode = ScriptableObject.CreateInstance<EqualNode>();
		equalNode.rect = new Rect(pos.x, pos.y, 150f, 130f);
		equalNode.name = "Equal";
		equalNode._type = EValueType.INT;
		equalNode._lastType = EValueType.INT;
		equalNode.CreateOutput("Output", "Bool");
		equalNode.CreateInput("Value 1", "Int");
		equalNode.CreateInput("Value 2", "Int");
		equalNode._isBeingDuplicated = false;
		return equalNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		EqualNode equalNode = (EqualNode)Create(rect.position + new Vector2(20f, 20f));
		equalNode._isBeingDuplicated = true;
		equalNode._boolValue1 = _boolValue1;
		equalNode._boolValue2 = _boolValue2;
		equalNode._floatValue1 = _floatValue1;
		equalNode._floatValue2 = _floatValue2;
		equalNode._statusValue1 = _statusValue1;
		equalNode._statusValue2 = _statusValue2;
		equalNode._stringValue1 = _stringValue1;
		equalNode._stringValue2 = _stringValue2;
		equalNode._intValue1 = _intValue1;
		equalNode._intValue2 = _intValue2;
		equalNode._type = _type;
		equalNode._characterValue1 = _characterValue1;
		equalNode._characterValue2 = _characterValue2;
		equalNode._itemValue1 = _itemValue1;
		equalNode._itemValue2 = _itemValue2;
		equalNode._created = _created;
		equalNode._lastType = _lastType;
		return equalNode;
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
			flag = _intValue1 == _intValue2;
			break;
		case EValueType.FLOAT:
			GetInputValue(Inputs[0], ref _floatValue1, canvas);
			GetInputValue(Inputs[1], ref _floatValue2, canvas);
			flag = _floatValue1.Equals(_floatValue2);
			break;
		case EValueType.STRING:
			GetInputValue(Inputs[0], ref _stringValue1, canvas);
			GetInputValue(Inputs[1], ref _stringValue2, canvas);
			flag = _stringValue1.Equals(_stringValue2);
			break;
		case EValueType.BOOL:
			GetInputValue(Inputs[0], ref _boolValue1, canvas);
			GetInputValue(Inputs[1], ref _boolValue2, canvas);
			flag = _boolValue1 == _boolValue2;
			break;
		case EValueType.CHARACTER:
			GetInputValue(Inputs[0], ref _characterValue1, canvas);
			GetInputValue(Inputs[1], ref _characterValue2, canvas);
			flag = _characterValue1.ID == _characterValue2.ID;
			break;
		case EValueType.ITEM:
			GetInputValue(Inputs[0], ref _itemValue1, canvas);
			GetInputValue(Inputs[1], ref _itemValue2, canvas);
			flag = _itemValue1.BaseStaticData.ItemId == _itemValue2.BaseStaticData.ItemId;
			break;
		case EValueType.STATUS:
			GetInputValue(Inputs[0], ref _statusValue1, canvas);
			GetInputValue(Inputs[1], ref _statusValue2, canvas);
			flag = _statusValue1.Id == _statusValue2.Id;
			break;
		}
		return CastValue<T>(flag);
	}
}
