using System;
using System.Collections;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Calculation/Add To List Node", new Type[]
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
public class AddToListNode : ParsecsNode
{
	private const int FLOW_INPUT_INDEX = 0;

	private const string FLOW_INPUT_NAME = "Add to list";

	private const int FLOW_OUTPUT_INDEX = 0;

	private const string FLOW_OUTPUT_NAME = "Out";

	private const string LIST_OUTPUT_NAME = "List with addition";

	private const string LIST_INPUT_NAME = "List";

	private const string ITEM_INPUT_NAME = "Item to add";

	private const int LIST_INPUT_INDEX = 1;

	private const int LIST_OUTPUT_INDEX = 1;

	private const int ITEM_INPUT_INDEX = 2;

	private const string NODE_NAME = "Add to list";

	public const string ID = "PE_AddToListListNode";

	private IList _list;

	public override string GetID => "PE_AddToListListNode";

	public override Node Create(Vector2 pos)
	{
		AddToListNode addToListNode = ScriptableObject.CreateInstance<AddToListNode>();
		addToListNode.rect = new Rect(pos.x, pos.y, 220f, 50f);
		addToListNode.name = "Add to list";
		addToListNode.CreateMutliInput("Add to list", "Flow");
		addToListNode.CreateInput("List", ListConnection.ID);
		addToListNode.CreateInput("Item to add", ObjectConnection.ID);
		addToListNode.CreateOutput("Out", "Flow");
		addToListNode.CreateOutput("List with addition", ListConnection.ID);
		return addToListNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return (AddToListNode)Create(rect.position + new Vector2(20f, 20f));
	}

	protected override void NodeGUI()
	{
	}

	protected override void OnNodeValidate()
	{
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 1)
		{
			throw new NodeOutputNotFoundException(output, "PE_AddToListListNode");
		}
		return CastValue<T>(_list);
	}

	public override void Execute(NodeCanvas canvas)
	{
		CheckAreAllFlowOutputsConnected();
		_list = GetInputValue<IList>(Inputs[1], canvas);
		object inputValue = GetInputValue<object>(Inputs[2], canvas);
		_list.Add(inputValue);
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}

	public override string ToString()
	{
		return _list.Count.ToString();
	}
}
