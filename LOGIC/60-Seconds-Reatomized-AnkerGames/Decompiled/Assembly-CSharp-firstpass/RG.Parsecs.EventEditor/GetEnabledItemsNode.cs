using System;
using System.Collections.Generic;
using NodeEditorFramework;
using RG.Parsecs.Common;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Supplies Nodes/Items/Get Enabled Items Node", new Type[]
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
public class GetEnabledItemsNode : ResourceNode
{
	public const string ID = "GE_GetEnabledItemsNode";

	private const string NODE_NAME = "Get Enabled Items";

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

	public override string GetID => "GE_GetEnabledItemsNode";

	public override Node Create(Vector2 pos)
	{
		GetEnabledItemsNode getEnabledItemsNode = ScriptableObject.CreateInstance<GetEnabledItemsNode>();
		getEnabledItemsNode.name = "Get Enabled Items";
		getEnabledItemsNode.rect = new Rect(pos.x, pos.y, 150f, 100f);
		getEnabledItemsNode.CreateInput("Available", "Bool");
		getEnabledItemsNode.CreateInput("Items", "Bool");
		getEnabledItemsNode.CreateInput("Remediums", "Bool");
		getEnabledItemsNode.CreateInput("Consumables", "Bool");
		getEnabledItemsNode.CreateOutput("List", ListConnection.ID);
		return getEnabledItemsNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		GetEnabledItemsNode getEnabledItemsNode = (GetEnabledItemsNode)Create(rect.position + new Vector2(20f, 20f));
		getEnabledItemsNode._available = _available;
		getEnabledItemsNode._includeItems = _includeItems;
		getEnabledItemsNode._includeRemedium = _includeRemedium;
		getEnabledItemsNode._includeConsumable = _includeConsumable;
		return getEnabledItemsNode;
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
			if (item.BaseRuntimeData.IsAvailable == _available && item.BaseRuntimeData.IsEnabled)
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
