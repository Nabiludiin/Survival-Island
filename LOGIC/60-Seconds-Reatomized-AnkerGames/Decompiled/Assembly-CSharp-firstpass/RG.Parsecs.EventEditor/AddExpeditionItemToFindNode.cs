using System;
using NodeEditorFramework;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Expedition Nodes/Add Expedition Item To Find Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent)
})]
public class AddExpeditionItemToFindNode : ExpeditionNode
{
	public const string ID = "EE_AddExpeditionItemToFindNode";

	private const string INPUT_DESTINATION_NAME = "Destination";

	private const string INPUT_ITEM_NAME = "Item";

	private const string INPUT_IN_NAME = "In";

	private const string OUTPUT_OUT_NAME = "Out";

	private const int INPUT_IN_INDEX = 0;

	private const int INPUT_DESTINATION_INDEX = 1;

	private const int INPUT_ITEM_INDEX = 2;

	private const int OUTPUT_OUT_INDEX = 0;

	private const string NODE_NAME = "Add Expedition Item To Find";

	[SerializeField]
	private ExpeditionDestination _destination;

	[SerializeField]
	private IItem _item;

	public override string GetID => "EE_AddExpeditionItemToFindNode";

	public override Node Create(Vector2 pos)
	{
		AddExpeditionItemToFindNode addExpeditionItemToFindNode = ScriptableObject.CreateInstance<AddExpeditionItemToFindNode>();
		addExpeditionItemToFindNode.rect = new Rect(pos.x, pos.y, 250f, 50f);
		addExpeditionItemToFindNode.name = "Add Expedition Item To Find";
		addExpeditionItemToFindNode.CreateMutliInput("In", "Flow");
		addExpeditionItemToFindNode.CreateInput("Destination", "ExpeditionDestination");
		addExpeditionItemToFindNode.CreateInput("Item", "Item");
		addExpeditionItemToFindNode.CreateOutput("Out", "Flow");
		return addExpeditionItemToFindNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		AddExpeditionItemToFindNode addExpeditionItemToFindNode = (AddExpeditionItemToFindNode)Create(rect.position + new Vector2(20f, 20f));
		addExpeditionItemToFindNode._destination = _destination;
		addExpeditionItemToFindNode._item = _item;
		return addExpeditionItemToFindNode;
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
		GetInputValue(Inputs[1], ref _destination, canvas);
		GetInputValue(Inputs[2], ref _item, canvas);
		ExpeditionManager.Instance.AddExpeditionItemToFind(_destination, _item);
		CheckAreAllFlowOutputsConnected();
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
