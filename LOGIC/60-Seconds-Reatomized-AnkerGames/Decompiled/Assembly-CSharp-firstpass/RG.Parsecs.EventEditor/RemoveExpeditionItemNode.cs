using System;
using NodeEditorFramework;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Expedition Nodes/Remove Expedition Item Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent)
})]
public class RemoveExpeditionItemNode : ExpeditionNode
{
	public const string ID = "EE_RemoveExpeditionItemNode";

	private const string INPUT_ITEM_NAME = "Item";

	private const string INPUT_IN_NAME = "In";

	private const string OUTPUT_OUT_NAME = "Out";

	private const int INPUT_ITEM_INDEX = 1;

	private const int INPUT_IN_INDEX = 0;

	private const int OUTPUT_OUT_INDEX = 0;

	private const string NODE_NAME = "Remove Expedition Item";

	[SerializeField]
	private IItem _item;

	public override string GetID => "EE_RemoveExpeditionItemNode";

	public override Node Create(Vector2 pos)
	{
		RemoveExpeditionItemNode removeExpeditionItemNode = ScriptableObject.CreateInstance<RemoveExpeditionItemNode>();
		removeExpeditionItemNode.rect = new Rect(pos.x, pos.y, 170f, 50f);
		removeExpeditionItemNode.name = "Remove Expedition Item";
		removeExpeditionItemNode.CreateMutliInput("In", "Flow");
		removeExpeditionItemNode.CreateInput("Item", "Item");
		removeExpeditionItemNode.CreateOutput("Out", "Flow");
		return removeExpeditionItemNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		RemoveExpeditionItemNode removeExpeditionItemNode = (RemoveExpeditionItemNode)Create(rect.position + new Vector2(20f, 20f));
		removeExpeditionItemNode._item = _item;
		return removeExpeditionItemNode;
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
		GetInputValue(Inputs[1], ref _item, canvas);
		ExpeditionManager.Instance.RemoveExpeditionItem(_item);
		CheckAreAllFlowOutputsConnected();
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
