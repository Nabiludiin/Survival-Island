using System;
using NodeEditorFramework;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Supplies Nodes/Items/Set item durability Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(ExpeditionEvent)
})]
public class SetItemDurabilityNode : ResourceNode
{
	public const string ID = "EE_setItemDurabilityNode";

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
	private int _durability;

	public override string GetID => "EE_setItemDurabilityNode";

	public override Node Create(Vector2 pos)
	{
		SetItemDurabilityNode setItemDurabilityNode = ScriptableObject.CreateInstance<SetItemDurabilityNode>();
		setItemDurabilityNode.rect = new Rect(pos.x, pos.y, 180f, 130f);
		setItemDurabilityNode.name = "Set Item durabaility";
		setItemDurabilityNode.CreateMutliInput("In", "Flow");
		setItemDurabilityNode.CreateInput("Item", "Item");
		setItemDurabilityNode.CreateInput("Value", "Int");
		setItemDurabilityNode.CreateOutput("Out", "Flow");
		return setItemDurabilityNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		SetItemDurabilityNode setItemDurabilityNode = (SetItemDurabilityNode)Create(rect.position + new Vector2(20f, 20f));
		setItemDurabilityNode._item = _item;
		setItemDurabilityNode._durability = _durability;
		return setItemDurabilityNode;
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
		GetInputValue(Inputs[2], ref _durability, canvas);
		_item.RuntimeData.Durability = _durability;
		CheckAreAllFlowOutputsConnected();
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
