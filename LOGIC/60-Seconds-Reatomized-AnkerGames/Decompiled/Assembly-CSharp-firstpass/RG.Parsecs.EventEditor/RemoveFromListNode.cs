using System;
using System.Collections;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Calculation/Remove From List Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas)
})]
public class RemoveFromListNode : ParsecsNode
{
	private const int FLOW_INPUT_INDEX = 0;

	private const string FLOW_INPUT_NAME = "Remove from list";

	private const int FLOW_OUTPUT_INDEX = 0;

	private const string FLOW_OUTPUT_NAME = "Out";

	private const string LIST_OUTPUT_NAME = "List with removal";

	private const string LIST_INPUT_NAME = "List";

	private const string ITEM_INPUT_NAME = "Item to remove";

	private const int LIST_INPUT_INDEX = 1;

	private const int LIST_OUTPUT_INDEX = 1;

	private const int ITEM_INPUT_INDEX = 2;

	private const string NODE_NAME = "Remove from list";

	public const string ID = "PE_RemoveFromListListNode";

	private IList _list;

	public override string GetID => "PE_RemoveFromListListNode";

	public override Node Create(Vector2 pos)
	{
		RemoveFromListNode removeFromListNode = ScriptableObject.CreateInstance<RemoveFromListNode>();
		removeFromListNode.rect = new Rect(pos.x, pos.y, 220f, 50f);
		removeFromListNode.name = "Remove from list";
		removeFromListNode.CreateMutliInput("Remove from list", "Flow");
		removeFromListNode.CreateInput("List", ListConnection.ID);
		removeFromListNode.CreateInput("Item to remove", ObjectConnection.ID);
		removeFromListNode.CreateOutput("Out", "Flow");
		removeFromListNode.CreateOutput("List with removal", ListConnection.ID);
		return removeFromListNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return (RemoveFromListNode)Create(rect.position + new Vector2(20f, 20f));
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
			throw new NodeOutputNotFoundException(output, "PE_RemoveFromListListNode");
		}
		return CastValue<T>(_list);
	}

	public override void Execute(NodeCanvas canvas)
	{
		CheckAreAllFlowOutputsConnected();
		_list = GetInputValue<IList>(Inputs[1], canvas);
		object inputValue = GetInputValue<object>(Inputs[2], canvas);
		_list.Remove(inputValue);
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}

	public override string ToString()
	{
		return _list.Count.ToString();
	}
}
