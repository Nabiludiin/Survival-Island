using System;
using System.Collections.Generic;
using NodeEditorFramework;
using RG.Parsecs.Common;
using RG.Parsecs.EventEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Node(true, "Legacy Nodes/Items/Get Random Item Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent)
})]
public class GetRandomItemNode : ResourceNode
{
	public const string ID = "EE_GetRandomItemNode";

	[SerializeField]
	private bool _itemsOnShip;

	private const string NODE_NAME = "Get Random Item";

	private const int OUTPUT_INDEX = 0;

	private const string ITEM_OUTPUT_NAME = "Item";

	private const int ITEM_FOUND_OUTPUT_INDEX = 1;

	private const string ITEM_FOUND_OUTPUT_NAME = "Was Item Found";

	public override string GetID => "EE_GetRandomItemNode";

	public override Node Create(Vector2 pos)
	{
		GetRandomItemNode getRandomItemNode = ScriptableObject.CreateInstance<GetRandomItemNode>();
		getRandomItemNode.rect = new Rect(pos.x, pos.y, 180f, 120f);
		getRandomItemNode.name = "Get Random Item";
		getRandomItemNode.CreateOutput("Item", "Item");
		getRandomItemNode.CreateOutput("Was Item Found", "Bool");
		return getRandomItemNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		GetRandomItemNode getRandomItemNode = (GetRandomItemNode)Create(rect.position + new Vector2(20f, 20f));
		getRandomItemNode._itemsOnShip = _itemsOnShip;
		return getRandomItemNode;
	}

	protected override int GetCurrentNodeVersion()
	{
		return 2;
	}

	protected override void OnRecreate(Vector2 pos)
	{
		RecreateOutput("Item", "Item");
		RecreateOutput("Was Item Found", "Bool");
	}

	protected override void NodeEnable()
	{
	}

	protected override void NodeGUI()
	{
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 0 && output != 1)
		{
			throw new NotExistingOutputException(GetID, output);
		}
		List<Item> items = Singleton<ItemManager>.Instance.GetItemList().GetItems();
		List<Item> list = new List<Item>();
		if (_itemsOnShip)
		{
			for (int i = 0; i < items.Count; i++)
			{
				if (items[i].RuntimeData.IsAvailable && !items[i].RuntimeData.IsOnExpedition)
				{
					list.Add(items[i]);
				}
			}
		}
		else
		{
			for (int j = 0; j < items.Count; j++)
			{
				if (!items[j].RuntimeData.IsAvailable)
				{
					list.Add(items[j]);
				}
			}
		}
		if (output == 0)
		{
			int num = UnityEngine.Random.Range(0, list.Count);
			return CastValue<T>(list[num]);
		}
		return CastValue<T>(list.Count > 0);
	}
}
