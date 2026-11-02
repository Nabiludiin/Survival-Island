using System;
using NodeEditorFramework;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Supplies Nodes/Items/Downgrade Item Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent)
})]
public class DowngradeItemNode : ResourceNode
{
	public const string ID = "EE_DowngradeItemNode";

	[SerializeField]
	private IItem _item;

	private const string INPUT_IN_NAME = "In";

	private const string INPUT_ITEM_NAME = "Item";

	private const string OUTPUT_OUT_NAME = "Out";

	private const int INPUT_IN_INDEX = 0;

	private const int INPUT_ITEM_INDEX = 1;

	private const int OUTPUT_OUT_INDEX = 0;

	private const string OUTPUT_NOT_CONNECTED_MESSAGE = "Output is not connected";

	public override string GetID => "EE_DowngradeItemNode";

	public override Node Create(Vector2 pos)
	{
		DowngradeItemNode downgradeItemNode = ScriptableObject.CreateInstance<DowngradeItemNode>();
		downgradeItemNode.rect = new Rect(pos.x, pos.y, 180f, 130f);
		downgradeItemNode.name = "Downgrade Item";
		downgradeItemNode.CreateMutliInput("In", "Flow");
		downgradeItemNode.CreateInput("Item", "Item");
		downgradeItemNode.CreateOutput("Out", "Flow");
		return downgradeItemNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		DowngradeItemNode downgradeItemNode = (DowngradeItemNode)Create(rect.position + new Vector2(20f, 20f));
		downgradeItemNode._item = _item;
		return downgradeItemNode;
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
		_item.Downgrade();
		CheckAreAllFlowOutputsConnected();
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
