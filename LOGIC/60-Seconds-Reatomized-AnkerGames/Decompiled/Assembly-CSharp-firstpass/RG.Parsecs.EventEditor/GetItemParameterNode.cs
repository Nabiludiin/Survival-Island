using System;
using NodeEditorFramework;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Event Nodes/Get Item Parameter Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ExpeditionEvent),
	typeof(SystemEvent),
	typeof(SystemStatusEvent),
	typeof(ReportEvent),
	typeof(ConditionEvent)
})]
public class GetItemParameterNode : FunctionNode
{
	public const string ID = "EE_GetItemParameterNode";

	[SerializeField]
	private string _name;

	private const string NAME_INPUT_NAME = "Parameter Name";

	private const int NAME_INPUT_ID = 0;

	private const string VALUE_OUTPUT_NAME = "Item Value";

	private const int VALUE_OUTPUT_ID = 0;

	private const string NAME_INPUT_EMPTY_ERROR_MESSAGE = "Parameter Name property is not connected or it's value is an empty string.";

	private const string NAME_INPUT_WRONG_ERROR_MESSAGE = "Specified parameter name doesn't exist in this ParsecEvent.";

	public override string GetID => "EE_GetItemParameterNode";

	public override Node Create(Vector2 pos)
	{
		GetItemParameterNode getItemParameterNode = ScriptableObject.CreateInstance<GetItemParameterNode>();
		getItemParameterNode.rect = new Rect(pos.x, pos.y, 200f, 40f);
		getItemParameterNode.name = "Get Item Parameter";
		getItemParameterNode.CreateInput("Parameter Name", "String");
		getItemParameterNode.CreateOutput("Item Value", "Item");
		return getItemParameterNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		GetItemParameterNode getItemParameterNode = (GetItemParameterNode)Create(rect.position + new Vector2(20f, 20f));
		getItemParameterNode._name = _name;
		return getItemParameterNode;
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
		IItem itemParameterByName = ParentEvent.GetItemParameterByName(_name);
		return CastValue<T>(itemParameterByName);
	}
}
