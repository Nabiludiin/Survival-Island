using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Calculation/Get Random Value Node", new Type[]
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
public class GetRandomValueNode : EventNode
{
	public const string ID = "GE_GetRandomValueNode";

	[SerializeField]
	private int _minInt;

	[SerializeField]
	private int _maxInt;

	[SerializeField]
	private float _minFloat;

	[SerializeField]
	private float _maxFloat;

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

	private const string INPUT_MIN_NAME = "Min";

	private const string INPUT_MAX_NAME = "Max";

	private const string OUTPUT_RANDOM_NAME = "Random";

	private const string NODE_NAME = "Get Random Value";

	private const int INPUT_MIN_INDEX = 0;

	private const int INPUT_MAX_INDEX = 1;

	private const int OUTPUT_RANDOM_INDEX = 0;

	public override string GetID => "GE_GetRandomValueNode";

	public override Node Create(Vector2 pos)
	{
		GetRandomValueNode getRandomValueNode = ScriptableObject.CreateInstance<GetRandomValueNode>();
		getRandomValueNode.rect = new Rect(pos.x, pos.y, 150f, 80f);
		getRandomValueNode.name = "Get Random Value";
		_type = EValueType.INT;
		_lastType = EValueType.INT;
		_isBeingDuplicated = false;
		getRandomValueNode.CreateInput("Min", "Int");
		getRandomValueNode.CreateInput("Max", "Int");
		getRandomValueNode.CreateOutput("Random", "Int");
		return getRandomValueNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		GetRandomValueNode getRandomValueNode = ScriptableObject.CreateInstance<GetRandomValueNode>();
		getRandomValueNode.rect = new Rect(pos.x, pos.y, 150f, 80f);
		getRandomValueNode.name = "Get Random Value";
		getRandomValueNode._minInt = _minInt;
		getRandomValueNode._maxInt = _maxInt;
		getRandomValueNode._minFloat = _minFloat;
		getRandomValueNode._maxFloat = _maxFloat;
		getRandomValueNode._type = _type;
		getRandomValueNode._lastType = _lastType;
		getRandomValueNode._created = _created;
		getRandomValueNode._isBeingDuplicated = true;
		getRandomValueNode._typeConnection = _typeConnection;
		getRandomValueNode.NodeEnable();
		return getRandomValueNode;
	}

	protected override void NodeEnable()
	{
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
			_isBeingDuplicated = false;
			_created = false;
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

	private void CreateInputs()
	{
		RemoveConnections();
		SetProperties();
		if (!_created)
		{
			CreateInput("Min", _typeConnection);
			CreateInput("Max", _typeConnection);
			CreateOutput("Random", _typeConnection);
			_created = true;
		}
	}

	private void SetProperties()
	{
	}

	protected override void NodeGUI()
	{
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		switch (_type)
		{
		case EValueType.INT:
		{
			GetInputValue(Inputs[0], ref _minInt, canvas);
			GetInputValue(Inputs[1], ref _maxInt, canvas);
			int num2 = UnityEngine.Random.Range(_minInt, _maxInt);
			return CastValue<T>(num2);
		}
		case EValueType.FLOAT:
		{
			GetInputValue(Inputs[0], ref _minFloat, canvas);
			GetInputValue(Inputs[1], ref _maxFloat, canvas);
			float num = UnityEngine.Random.Range(_minFloat, _maxFloat);
			return CastValue<T>(num);
		}
		default:
			throw new ArgumentOutOfRangeException();
		}
	}
}
