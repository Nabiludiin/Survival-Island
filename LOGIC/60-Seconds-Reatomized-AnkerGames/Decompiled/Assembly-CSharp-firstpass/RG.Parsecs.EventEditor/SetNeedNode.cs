using System;
using NodeEditorFramework;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Character Nodes/Set Need Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent)
})]
public class SetNeedNode : CharacterNode
{
	public const string ID = "EE_setNeedNode";

	private const int FLOW_INPUT_INDEX = 0;

	private const int OUTPUT_OUT_INDEX = 0;

	private const int CHARACTERS_INPUT_INDEX = 1;

	private const int VALUE_INPUT_INDEX = 2;

	private const string NODE_NAME = "Set need";

	private const string OUTPUT_NAME = "Out";

	private const string FLOW_INPUT_NAME = "In";

	private const string CHARACTERS_INPUT_NAME = "Character";

	private const string VALUE_INPUT_NAME = "Value to set";

	[SerializeField]
	private CharacterNeed _need;

	[SerializeField]
	private float _valueToSet;

	[SerializeField]
	private Character _character;

	public override string GetID => "EE_setNeedNode";

	public override Node Create(Vector2 pos)
	{
		SetNeedNode setNeedNode = ScriptableObject.CreateInstance<SetNeedNode>();
		setNeedNode.rect = new Rect(pos.x, pos.y, 220f, 40f);
		setNeedNode.name = "Set need";
		setNeedNode.CreateMutliInput("In", "Flow");
		setNeedNode.CreateInput("Character", "Character");
		setNeedNode.CreateInput("Value to set", "Float");
		setNeedNode.CreateOutput("Out", "Flow");
		return setNeedNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		SetNeedNode setNeedNode = (SetNeedNode)Create(rect.position + new Vector2(20f, 20f));
		setNeedNode._need = _need;
		setNeedNode._valueToSet = _valueToSet;
		setNeedNode._character = _character;
		return setNeedNode;
	}

	protected override void NodeEnable()
	{
	}

	protected override void OnNodeValidate()
	{
	}

	protected override void NodeGUI()
	{
	}

	public override void Execute(NodeCanvas canvas)
	{
		GetInputValue(Inputs[1], ref _character, canvas);
		GetInputValue(Inputs[2], ref _valueToSet, canvas);
		_character.RuntimeData.SetNeedValue(_need.Id, _valueToSet);
		CheckAreAllFlowOutputsConnected();
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
