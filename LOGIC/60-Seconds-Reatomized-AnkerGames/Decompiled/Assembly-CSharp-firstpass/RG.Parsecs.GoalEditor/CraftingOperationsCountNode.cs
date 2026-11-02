using System;
using NodeEditorFramework;
using RG.Parsecs.Common;
using RG.Parsecs.EventEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.GoalEditor;

[Node(false, "Global Stats/Crafting Operations Count Node", new Type[]
{
	typeof(Goal),
	typeof(ConditionEvent)
})]
public class CraftingOperationsCountNode : GoalNode
{
	private enum ECraftingAction
	{
		CRAFT,
		RECYCLE,
		REPAIR
	}

	private const string ID = "GE_CraftingOperationsCountNode";

	private const string NODE_NAME = "Global Crafting Operations Count";

	private const string OUTPUT_COUNT_NAME = "Count";

	private const int OUTPUT_COUNT_INDEX = 0;

	[SerializeField]
	private ECraftingAction _action;

	public override string GetID => "GE_CraftingOperationsCountNode";

	public override Node Create(Vector2 pos)
	{
		CraftingOperationsCountNode craftingOperationsCountNode = ScriptableObject.CreateInstance<CraftingOperationsCountNode>();
		craftingOperationsCountNode.rect = new Rect(pos.x, pos.y, 200f, 150f);
		craftingOperationsCountNode.name = "Global Crafting Operations Count";
		craftingOperationsCountNode.CreateOutput("Count", "Int");
		return craftingOperationsCountNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		CraftingOperationsCountNode craftingOperationsCountNode = (CraftingOperationsCountNode)Create(rect.position + new Vector2(20f, 20f));
		craftingOperationsCountNode._action = _action;
		return craftingOperationsCountNode;
	}

	protected override void NodeEnable()
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
		int num = 0;
		if (_action == ECraftingAction.CRAFT)
		{
			num = StatsManager.Instance.GetGlobalData("CraftedItems");
		}
		else if (_action == ECraftingAction.RECYCLE)
		{
			num = StatsManager.Instance.GetGlobalData("RecycledItems");
		}
		else if (_action == ECraftingAction.REPAIR)
		{
			num = StatsManager.Instance.GetGlobalData("RepairedItems");
		}
		return CastValue<T>(num);
	}
}
