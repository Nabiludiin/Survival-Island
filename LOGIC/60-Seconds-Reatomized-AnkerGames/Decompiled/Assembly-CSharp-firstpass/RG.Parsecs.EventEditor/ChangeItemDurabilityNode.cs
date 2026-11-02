using System;
using NodeEditorFramework;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Supplies Nodes/Items/Change item durability Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(ExpeditionEvent)
})]
public class ChangeItemDurabilityNode : ResourceNode
{
	public const string ID = "EE_changeItemDurabilityNode";

	private const string INPUT_IN_NAME = "In";

	private const string INPUT_ITEM_NAME = "Item";

	private const string INPUT_INT_VALUE = "Value";

	private const string OUTPUT_OUT_NAME = "Out";

	private const int INPUT_IN_INDEX = 0;

	private const int INPUT_ITEM_INDEX = 1;

	private const int INPUT_DURABAILITY_INDEX = 2;

	private const int OUTPUT_OUT_INDEX = 0;

	[SerializeField]
	private Item _item;

	[SerializeField]
	private int _durabilityDelta;

	public override string GetID => "EE_changeItemDurabilityNode";

	public override Node Create(Vector2 pos)
	{
		ChangeItemDurabilityNode changeItemDurabilityNode = ScriptableObject.CreateInstance<ChangeItemDurabilityNode>();
		changeItemDurabilityNode.rect = new Rect(pos.x, pos.y, 180f, 130f);
		changeItemDurabilityNode.name = "Change item durabaility";
		changeItemDurabilityNode.CreateMutliInput("In", "Flow");
		changeItemDurabilityNode.CreateInput("Item", "Item");
		changeItemDurabilityNode.CreateInput("Value", "Int");
		changeItemDurabilityNode.CreateOutput("Out", "Flow");
		return changeItemDurabilityNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		ChangeItemDurabilityNode changeItemDurabilityNode = (ChangeItemDurabilityNode)Create(rect.position + new Vector2(20f, 20f));
		changeItemDurabilityNode._item = _item;
		changeItemDurabilityNode._durabilityDelta = _durabilityDelta;
		return changeItemDurabilityNode;
	}

	protected override void NodeEnable()
	{
	}

	protected override void NodeGUI()
	{
	}

	public override void Execute(NodeCanvas canvas)
	{
		GetInputValue(Inputs[1], ref _item, canvas);
		GetInputValue(Inputs[2], ref _durabilityDelta, canvas);
		_item.RuntimeData.Durability += _durabilityDelta;
		CheckAreAllFlowOutputsConnected();
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
