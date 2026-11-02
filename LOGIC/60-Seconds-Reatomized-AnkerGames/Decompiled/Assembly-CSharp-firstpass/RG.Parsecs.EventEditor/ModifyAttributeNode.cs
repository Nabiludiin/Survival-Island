using System;
using NodeEditorFramework;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Character Nodes/Modify Attribute Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent)
})]
public class ModifyAttributeNode : CharacterNode
{
	public const string Id = "modifyAttributeNode";

	private const int FLOW_INPUT_INDEX = 0;

	private const int OUTPUT_OUT_INDEX = 0;

	private const int CHARACTER_INPUT_INDEX = 1;

	private const int CHANGE_INPUT_INDEX = 2;

	private const string NODE_NAME = "Modify attribute";

	private const string OUTPUT_NAME = "Out";

	private const string FLOW_INPUT_NAME = "In";

	private const string CHARACTERS_INPUT_NAME = "Character";

	private const string CHANGE_INPUT_NAME = "Change amount";

	[SerializeField]
	private CharacterAttribute _attribute;

	[SerializeField]
	private float _changeAmount;

	[SerializeField]
	private Character _character;

	public override string GetID => "modifyAttributeNode";

	public override Node Create(Vector2 pos)
	{
		ModifyAttributeNode modifyAttributeNode = ScriptableObject.CreateInstance<ModifyAttributeNode>();
		modifyAttributeNode.rect = new Rect(pos.x, pos.y, 220f, 40f);
		modifyAttributeNode.name = "Modify attribute";
		modifyAttributeNode.CreateMutliInput("In", "Flow");
		modifyAttributeNode.CreateInput("Character", "Character");
		modifyAttributeNode.CreateInput("Change amount", "Float");
		modifyAttributeNode.CreateOutput("Out", "Flow");
		return modifyAttributeNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		ModifyAttributeNode modifyAttributeNode = (ModifyAttributeNode)Create(rect.position + new Vector2(20f, 20f));
		modifyAttributeNode._attribute = _attribute;
		modifyAttributeNode._changeAmount = _changeAmount;
		modifyAttributeNode._character = _character;
		return modifyAttributeNode;
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
		CheckAreAllFlowOutputsConnected();
		GetInputValue(Inputs[1], ref _character, canvas);
		GetInputValue(Inputs[2], ref _changeAmount, canvas);
		_character.RuntimeData.ChangeAttributeValue(_attribute.Id, _changeAmount);
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
