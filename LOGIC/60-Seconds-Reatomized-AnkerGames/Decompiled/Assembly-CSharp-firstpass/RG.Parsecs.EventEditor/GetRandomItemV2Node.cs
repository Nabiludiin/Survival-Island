using System;
using System.Collections.Generic;
using NodeEditorFramework;
using RG.Parsecs.Common;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Supplies Nodes/Items/Get Random Item Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent)
})]
public class GetRandomItemV2Node : ResourceNode
{
	public const string ID = "EE_GetRandomItemV2Node";

	[SerializeField]
	private bool _availableItems;

	[SerializeField]
	private bool _items = true;

	[SerializeField]
	private bool _remedium = true;

	[SerializeField]
	private bool _consumables = true;

	private const string NODE_NAME = "Get Random Item";

	private const string AVAILABLE_ITEMS_INPUT_NAME = "Availables Items";

	private const string ITEM_INPUT_NAME = "Item";

	private const string REMEDIUM_INPUT_NAME = "Remedium";

	private const string CONSUMABLES_INPUT_NAME = "Consumable";

	private const string ITEM_FOUND_OUTPUT_NAME = "Was Item Found";

	private const string CHOOSEN_ITEM_OUTPUT_NAME = "Chosen Item";

	private const int CHOOSEN_ITEM_INDEX = 0;

	private const int OBJECT_FOUND_OUTPUT_INDEX = 1;

	private const int AVAIABLE_ITEMS_INPUT_INDEX = 0;

	private const int ITEM_INPUT_INDEX = 1;

	private const int REMEDIUM_INPUT_INDEX = 2;

	private const int CONSUMABLE_INPUT_INDEX = 3;

	public override string GetID => "EE_GetRandomItemV2Node";

	public override Node Create(Vector2 pos)
	{
		GetRandomItemV2Node getRandomItemV2Node = ScriptableObject.CreateInstance<GetRandomItemV2Node>();
		getRandomItemV2Node.rect = new Rect(pos.x, pos.y, 180f, 120f);
		getRandomItemV2Node.name = "Get Random Item";
		getRandomItemV2Node.CreateInput("Availables Items", "Bool");
		getRandomItemV2Node.CreateInput("Item", "Bool");
		getRandomItemV2Node.CreateInput("Remedium", "Bool");
		getRandomItemV2Node.CreateInput("Consumable", "Bool");
		getRandomItemV2Node.CreateOutput("Chosen Item", "Item");
		getRandomItemV2Node.CreateOutput("Was Item Found", "Bool");
		return getRandomItemV2Node;
	}

	public override Node Duplicate(Vector2 pos)
	{
		GetRandomItemV2Node getRandomItemV2Node = (GetRandomItemV2Node)Create(rect.position + new Vector2(20f, 20f));
		getRandomItemV2Node._availableItems = _availableItems;
		getRandomItemV2Node._items = _items;
		getRandomItemV2Node._remedium = _remedium;
		getRandomItemV2Node._consumables = _consumables;
		return getRandomItemV2Node;
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

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		GetInputValue(Inputs[0], ref _availableItems, canvas);
		GetInputValue(Inputs[1], ref _items, canvas);
		GetInputValue(Inputs[2], ref _remedium, canvas);
		GetInputValue(Inputs[3], ref _consumables, canvas);
		List<IItem> list = new List<IItem>();
		ItemList itemList = Singleton<ItemManager>.Instance.GetItemList();
		if (_items)
		{
			List<Item> items = itemList.GetItems();
			for (int i = 0; i < items.Count; i++)
			{
				if (items[i].BaseRuntimeData.IsAvailable == _availableItems && !items[i].RuntimeData.IsOnExpedition)
				{
					list.Add(items[i]);
				}
			}
		}
		if (_remedium)
		{
			List<Remedium> remedies = itemList.GetRemedies();
			for (int j = 0; j < remedies.Count; j++)
			{
				if (remedies[j].BaseRuntimeData.IsAvailable == _availableItems && !remedies[j].RuntimeData.IsOnExpedition)
				{
					list.Add(remedies[j]);
				}
			}
		}
		if (_consumables)
		{
			List<ConsumableRemedium> consumableRemedies = itemList.GetConsumableRemedies();
			for (int k = 0; k < consumableRemedies.Count; k++)
			{
				if (consumableRemedies[k].BaseRuntimeData.IsAvailable == _availableItems && !consumableRemedies[k].RuntimeData.IsOnExpedition)
				{
					list.Add(consumableRemedies[k]);
				}
			}
		}
		switch (output)
		{
		case 0:
		{
			int num = UnityEngine.Random.Range(0, list.Count);
			return CastValue<T>(list[num]);
		}
		case 1:
			return CastValue<T>(list.Count > 0);
		default:
			throw new NotExistingOutputException(GetID, output);
		}
	}
}
