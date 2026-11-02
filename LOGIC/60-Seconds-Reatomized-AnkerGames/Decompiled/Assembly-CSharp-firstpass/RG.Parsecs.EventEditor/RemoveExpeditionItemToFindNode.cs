using System;
using NodeEditorFramework;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Expedition Nodes/Remove Expedition Item To Find Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent)
})]
public class RemoveExpeditionItemToFindNode : ExpeditionNode
{
	public const string ID = "EE_RemoveExpeditionItemToFindNode";

	private const string INPUT_DESTINATION_NAME = "Destination";

	private const string INPUT_ITEM_NAME = "Item";

	private const string INPUT_IN_NAME = "In";

	private const string OUTPUT_OUT_NAME = "Out";

	private const int INPUT_IN_INDEX = 0;

	private const int INPUT_DESTINATION_INDEX = 1;

	private const int INPUT_ITEM_INDEX = 2;

	private const int OUTPUT_OUT_INDEX = 0;

	private const string NODE_NAME = "Remove Expedition Item To Find";

	[SerializeField]
	private ExpeditionDestination _destination;

	[SerializeField]
	private IItem _item;

	public override string GetID => "EE_RemoveExpeditionItemToFindNode";

	public override Node Create(Vector2 pos)
	{
		RemoveExpeditionItemToFindNode removeExpeditionItemToFindNode = ScriptableObject.CreateInstance<RemoveExpeditionItemToFindNode>();
		removeExpeditionItemToFindNode.rect = new Rect(pos.x, pos.y, 250f, 50f);
		removeExpeditionItemToFindNode.name = "Remove Expedition Item To Find";
		removeExpeditionItemToFindNode.CreateMutliInput("In", "Flow");
		removeExpeditionItemToFindNode.CreateInput("Destination", "ExpeditionDestination");
		removeExpeditionItemToFindNode.CreateInput("Item", "Item");
		removeExpeditionItemToFindNode.CreateOutput("Out", "Flow");
		return removeExpeditionItemToFindNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		RemoveExpeditionItemToFindNode removeExpeditionItemToFindNode = (RemoveExpeditionItemToFindNode)Create(rect.position + new Vector2(20f, 20f));
		removeExpeditionItemToFindNode._destination = _destination;
		removeExpeditionItemToFindNode._item = _item;
		return removeExpeditionItemToFindNode;
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
		ExpeditionManager.Instance.RemoveExpeditionItemToFind(_destination, _item);
		CheckAreAllFlowOutputsConnected();
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
