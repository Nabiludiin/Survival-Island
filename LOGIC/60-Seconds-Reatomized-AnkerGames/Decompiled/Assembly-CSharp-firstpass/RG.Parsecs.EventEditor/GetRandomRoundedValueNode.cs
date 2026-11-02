using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Calculation/Get Random Rounded Value Node", new Type[]
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
public class GetRandomRoundedValueNode : EventNode
{
	public const string ID = "GE_GetRandomRoundedValueNode";

	[SerializeField]
	private float _minFloat;

	[SerializeField]
	private float _maxFloat;

	[SerializeField]
	private bool _floor;

	[SerializeField]
	private int _precision;

	[SerializeField]
	private bool _created;

	[NonSerialized]
	private bool _isBeingDuplicated;

	[SerializeField]
	private string _typeConnection = string.Empty;

	private const string INPUT_MIN_NAME = "Min";

	private const string INPUT_MAX_NAME = "Max";

	private const string INPUT_FLOOR_NAME = "Floor";

	private const string INPUT_PRECISION_NAME = "Precision";

	private const string OUTPUT_RANDOM_NAME = "Random";

	private const string NODE_NAME = "Get Random Rounded Value";

	private const int INPUT_MIN_INDEX = 0;

	private const int INPUT_MAX_INDEX = 1;

	private const int INPUT_FLOOR_INDEX = 2;

	private const int INPUT_PRECISION_INDEX = 3;

	private const int OUTPUT_RANDOM_INDEX = 0;

	public override string GetID => "GE_GetRandomRoundedValueNode";

	public override Node Create(Vector2 pos)
	{
		GetRandomRoundedValueNode getRandomRoundedValueNode = ScriptableObject.CreateInstance<GetRandomRoundedValueNode>();
		getRandomRoundedValueNode.rect = new Rect(pos.x, pos.y, 250f, 200f);
		getRandomRoundedValueNode.name = "Get Random Rounded Value";
		_isBeingDuplicated = false;
		getRandomRoundedValueNode.CreateInput("Min", "Float");
		getRandomRoundedValueNode.CreateInput("Max", "Float");
		getRandomRoundedValueNode.CreateInput("Floor", "Bool");
		getRandomRoundedValueNode.CreateOutput("Random", "Float");
		return getRandomRoundedValueNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		GetRandomRoundedValueNode getRandomRoundedValueNode = ScriptableObject.CreateInstance<GetRandomRoundedValueNode>();
		getRandomRoundedValueNode.rect = new Rect(pos.x, pos.y, 250f, 200f);
		getRandomRoundedValueNode.name = "Get Random Rounded Value";
		getRandomRoundedValueNode._minFloat = _minFloat;
		getRandomRoundedValueNode._maxFloat = _maxFloat;
		getRandomRoundedValueNode._floor = _floor;
		getRandomRoundedValueNode._precision = _precision;
		getRandomRoundedValueNode._created = _created;
		getRandomRoundedValueNode._isBeingDuplicated = true;
		getRandomRoundedValueNode._typeConnection = _typeConnection;
		getRandomRoundedValueNode.NodeEnable();
		return getRandomRoundedValueNode;
	}

	protected override void NodeEnable()
	{
	}

	private bool ShouldCreateInputs()
	{
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
			CreateInput("Floor", _typeConnection);
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
		GetInputValue(Inputs[0], ref _minFloat, canvas);
		GetInputValue(Inputs[1], ref _maxFloat, canvas);
		GetInputValue(Inputs[2], ref _floor, canvas);
		float num = UnityEngine.Random.Range(_minFloat, _maxFloat);
		num = ((!_floor) ? (Mathf.Ceil(num * (float)_precision) / (float)_precision) : (Mathf.Floor(num * (float)_precision) / (float)_precision));
		return CastValue<T>(num);
	}
}
