using System;
using NodeEditorFramework;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Character Nodes/Set Attribute Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent)
})]
public class SetAttributeNode : CharacterNode
{
	public const string ID = "setAttributeNode";

	private const int FLOW_INPUT_INDEX = 0;

	private const int FLOW_OUTPUT_INDEX = 0;

	private const int CHARACTER_INPUT_INDEX = 1;

	private const int VALUE_INPUT_INDEX = 2;

	private const string NODE_NAME = "Set attribute";

	private const string OUTPUT_NAME = "Out";

	private const string FLOW_INPUT_NAME = "In";

	private const string CHARACTERS_INPUT_NAME = "Character";

	private const string VALUE_INPUT_NAME = "Value to set";

	[SerializeField]
	private CharacterAttribute _attribute;

	[SerializeField]
	private float _valueToSet;

	[SerializeField]
	private Character _character;

	public override string GetID => "setAttributeNode";

	public override Node Create(Vector2 pos)
	{
		SetAttributeNode setAttributeNode = ScriptableObject.CreateInstance<SetAttributeNode>();
		setAttributeNode.rect = new Rect(pos.x, pos.y, 220f, 40f);
		setAttributeNode.name = "Set attribute";
		setAttributeNode.CreateMutliInput("In", "Flow");
		setAttributeNode.CreateInput("Character", "Character");
		setAttributeNode.CreateInput("Value to set", "Float");
		setAttributeNode.CreateOutput("Out", "Flow");
		return setAttributeNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		SetAttributeNode setAttributeNode = (SetAttributeNode)Create(rect.position + new Vector2(20f, 20f));
		setAttributeNode._attribute = _attribute;
		setAttributeNode._valueToSet = _valueToSet;
		setAttributeNode._character = _character;
		return setAttributeNode;
	}

	protected override void NodeEnable()
	{
	}

	protected override void NodeGUI()
	{
	}

	protected override void OnNodeValidate()
	{
	}

	public override void Execute(NodeCanvas canvas)
	{
		GetInputValue(Inputs[1], ref _character, canvas);
		GetInputValue(Inputs[2], ref _valueToSet, canvas);
		_character.RuntimeData.SetAttributeValue(_attribute.Id, _valueToSet);
		CheckAreAllFlowOutputsConnected();
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
