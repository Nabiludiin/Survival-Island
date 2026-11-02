using System;
using NodeEditorFramework;
using RG.Parsecs.Common;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Supplies Nodes/Items/Decraft Item Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent)
})]
public class DecraftItemNode : ResourceNode
{
	public const string ID = "EE_DecraftItemNode";

	[SerializeField]
	private IItem _item;

	private const string INPUT_IN_NAME = "In";

	private const string INPUT_ITEM_NAME = "Item";

	private const string OUTPUT_OUT_NAME = "Out";

	private const int INPUT_IN_INDEX = 0;

	private const int INPUT_ITEM_INDEX = 1;

	private const int OUTPUT_OUT_INDEX = 0;

	private const string OUTPUT_NOT_CONNECTED_MESSAGE = "Output is not connected";

	public override string GetID => "EE_DecraftItemNode";

	public override Node Create(Vector2 pos)
	{
		DecraftItemNode decraftItemNode = ScriptableObject.CreateInstance<DecraftItemNode>();
		decraftItemNode.rect = new Rect(pos.x, pos.y, 180f, 130f);
		decraftItemNode.name = "Decraft Item";
		decraftItemNode.CreateMutliInput("In", "Flow");
		decraftItemNode.CreateInput("Item", "Item");
		decraftItemNode.CreateOutput("Out", "Flow");
		return decraftItemNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		DecraftItemNode decraftItemNode = (DecraftItemNode)Create(rect.position + new Vector2(20f, 20f));
		decraftItemNode._item = _item;
		return decraftItemNode;
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
		_item.Remove();
		Singleton<ItemManager>.Instance.GetPlayerResources().AddResources(_item.GetRecyclingCostForCurrentLevel().Resources);
		SchedulerManager.Instance.Schedule(_item.BaseStaticData.RecyclingEvent, 0);
		CheckAreAllFlowOutputsConnected();
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
