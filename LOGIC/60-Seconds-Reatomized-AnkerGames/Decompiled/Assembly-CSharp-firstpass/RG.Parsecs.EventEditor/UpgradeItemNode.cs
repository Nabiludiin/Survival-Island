using System;
using NodeEditorFramework;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Supplies Nodes/Items/Upgrade Item Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent)
})]
public class UpgradeItemNode : ResourceNode
{
	public const string ID = "EE_UpgradeItemNode";

	[SerializeField]
	private IItem _item;

	[SerializeField]
	private bool _upgradeDamagedItem;

	private const string INPUT_IN_NAME = "In";

	private const string INPUT_ITEM_NAME = "Item";

	private const string OUTPUT_OUT_NAME = "Out";

	private const int INPUT_IN_INDEX = 0;

	private const int INPUT_ITEM_INDEX = 1;

	private const int OUTPUT_OUT_INDEX = 0;

	private const string OUTPUT_NOT_CONNECTED_MESSAGE = "Output is not connected";

	public override string GetID => "EE_UpgradeItemNode";

	public override Node Create(Vector2 pos)
	{
		UpgradeItemNode upgradeItemNode = ScriptableObject.CreateInstance<UpgradeItemNode>();
		upgradeItemNode.rect = new Rect(pos.x, pos.y, 180f, 130f);
		upgradeItemNode.name = "Upgrade Item";
		upgradeItemNode.CreateMutliInput("In", "Flow");
		upgradeItemNode.CreateInput("Item", "Item");
		upgradeItemNode.CreateOutput("Out", "Flow");
		return upgradeItemNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		UpgradeItemNode upgradeItemNode = (UpgradeItemNode)Create(rect.position + new Vector2(20f, 20f));
		upgradeItemNode._item = _item;
		return upgradeItemNode;
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
		if (_item is Remedium)
		{
			if (_item.IsUpgradable())
			{
				_item.Upgrade();
			}
		}
		else if (_upgradeDamagedItem)
		{
			if (!_item.IsMaxLevel())
			{
				((Item)_item).Repair();
				_item.Upgrade();
			}
		}
		else if (_item.IsUpgradable())
		{
			_item.Upgrade();
		}
		CheckAreAllFlowOutputsConnected();
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
