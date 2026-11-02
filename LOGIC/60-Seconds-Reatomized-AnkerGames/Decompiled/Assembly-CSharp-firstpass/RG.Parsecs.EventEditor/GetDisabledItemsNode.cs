using System;
using System.Collections.Generic;
using NodeEditorFramework;
using RG.Parsecs.Common;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Supplies Nodes/Items/Get Disabled Items Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(ConditionEvent),
	typeof(Goal)
})]
public class GetDisabledItemsNode : ResourceNode
{
	public const string ID = "GE_GetDisabledItemsNode";

	private const string NODE_NAME = "Get Disabled Items";

	private const string OUTPUT_NAME = "List";

	private const string AVAILABLE_INPUT_NAME = "Available";

	private const string INPUT_INCLUDE_ITEMS_NAME = "Items";

	private const string INPUT_INCLUDE_REMEDIUM_NAME = "Remediums";

	private const string INPUT_INCLUDE_CONSUMEABLE_NAME = "Consumables";

	private const int OUTPUT_LIST_INDEX = 0;

	private const int INPUT_AVAILABLE_INDEX = 0;

	private const int INPUT_INCLUDE_ITEMS_INDEX = 1;

	private const int INPUT_INCLUDE_REMEDIUM_INDEX = 2;

	private const int INPUT_INCLUDE_CONSUMEABLE_INDEX = 3;

	[SerializeField]
	private bool _available = true;

	[SerializeField]
	private bool _includeItems = true;

	[SerializeField]
	private bool _includeRemedium = true;

	[SerializeField]
	private bool _includeConsumable;

	public override string GetID => "GE_GetDisabledItemsNode";

	public override Node Create(Vector2 pos)
	{
		GetDisabledItemsNode getDisabledItemsNode = ScriptableObject.CreateInstance<GetDisabledItemsNode>();
		getDisabledItemsNode.name = "Get Disabled Items";
		getDisabledItemsNode.rect = new Rect(pos.x, pos.y, 150f, 100f);
		getDisabledItemsNode.CreateInput("Available", "Bool");
		getDisabledItemsNode.CreateInput("Items", "Bool");
		getDisabledItemsNode.CreateInput("Remediums", "Bool");
		getDisabledItemsNode.CreateInput("Consumables", "Bool");
		getDisabledItemsNode.CreateOutput("List", ListConnection.ID);
		return getDisabledItemsNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		GetDisabledItemsNode getDisabledItemsNode = (GetDisabledItemsNode)Create(rect.position + new Vector2(20f, 20f));
		getDisabledItemsNode._available = _available;
		getDisabledItemsNode._includeItems = _includeItems;
		getDisabledItemsNode._includeRemedium = _includeRemedium;
		getDisabledItemsNode._includeConsumable = _includeConsumable;
		return getDisabledItemsNode;
	}

	protected override void NodeEnable()
	{
	}

	protected override void OnNodeValidate()
	{
	}

	protected override void NodeGUI()
	{
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 0)
		{
			throw new NotExistingOutputException(GetID, output);
		}
		GetInputValue(Inputs[0], ref _available, canvas);
		GetInputValue(Inputs[1], ref _includeItems, canvas);
		GetInputValue(Inputs[2], ref _includeRemedium, canvas);
		GetInputValue(Inputs[3], ref _includeConsumable, canvas);
		List<IItem> list = new List<IItem>();
		List<IItem> items = Singleton<ItemManager>.Instance.GetItemList().Items;
		for (int i = 0; i < items.Capacity; i++)
		{
			IItem item = items[i];
			if (item.BaseRuntimeData.IsAvailable == _available && !item.BaseRuntimeData.IsEnabled)
			{
				if (_includeItems && item is Item)
				{
					list.Add(item);
				}
				else if (_includeRemedium && item is Remedium)
				{
					list.Add(item);
				}
				else if (_includeConsumable && item is ConsumableRemedium)
				{
					list.Add(item);
				}
			}
		}
		return CastValue<T>(list);
	}
}
