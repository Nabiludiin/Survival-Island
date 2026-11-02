using System;
using NodeEditorFramework;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Event Nodes/Get Character Parameter Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ExpeditionEvent),
	typeof(SystemEvent),
	typeof(ReportEvent),
	typeof(SystemStatusEvent),
	typeof(ConditionEvent)
})]
public class GetCharacterParameterNode : FunctionNode
{
	public const string ID = "EE_GetCharacterParameterNode";

	[SerializeField]
	private string _name;

	private const string NAME_INPUT_NAME = "Parameter Name";

	private const int NAME_INPUT_ID = 0;

	private const string VALUE_OUTPUT_NAME = "Character Value";

	private const int VALUE_OUTPUT_ID = 0;

	private const string NAME_INPUT_EMPTY_ERROR_MESSAGE = "Parameter Name property is not connected or it's value is an empty string.";

	private const string NAME_INPUT_WRONG_ERROR_MESSAGE = "Specified parameter name doesn't exist in this ParsecEvent.";

	public override string GetID => "EE_GetCharacterParameterNode";

	public override Node Create(Vector2 pos)
	{
		GetCharacterParameterNode getCharacterParameterNode = ScriptableObject.CreateInstance<GetCharacterParameterNode>();
		getCharacterParameterNode.rect = new Rect(pos.x, pos.y, 200f, 40f);
		getCharacterParameterNode.name = "Get Character Parameter";
		getCharacterParameterNode.CreateInput("Parameter Name", "String");
		getCharacterParameterNode.CreateOutput("Character Value", "Character");
		return getCharacterParameterNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		GetCharacterParameterNode getCharacterParameterNode = (GetCharacterParameterNode)Create(rect.position + new Vector2(20f, 20f));
		getCharacterParameterNode._name = _name;
		return getCharacterParameterNode;
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

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		GetInputValue(Inputs[0], ref _name, canvas);
		Character characterParameterByName = ParentEvent.GetCharacterParameterByName(_name);
		return CastValue<T>(characterParameterByName);
	}
}
