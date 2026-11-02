using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Supplies Nodes/Items/Get Item Level Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas),
	typeof(ConditionEvent),
	typeof(Goal)
})]
public class GetItemLevelNode : ResourceNode
{
	public const string ID = "EE_GetItemLevelNode";

	[SerializeField]
	private IItem _item;

	private const int INPUT_ITEM_INDEX = 0;

	private const int OUTPUT_GET_LEVEL_INDEX = 0;

	private const string INPUT_ITEM_NAME = "Item";

	private const string OUTPUT_GET_LEVEL_NAME = "Get Level";

	public override string GetID => "EE_GetItemLevelNode";

	public override Node Create(Vector2 pos)
	{
		GetItemLevelNode getItemLevelNode = ScriptableObject.CreateInstance<GetItemLevelNode>();
		getItemLevelNode.rect = new Rect(pos.x, pos.y, 180f, 120f);
		getItemLevelNode.name = "Get Item Level";
		getItemLevelNode.CreateInput("Item", "Item");
		getItemLevelNode.CreateOutput("Get Level", "Int");
		return getItemLevelNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		GetItemLevelNode getItemLevelNode = (GetItemLevelNode)Create(rect.position + new Vector2(20f, 20f));
		getItemLevelNode._item = _item;
		return getItemLevelNode;
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
		if (output != 0)
		{
			throw new NotExistingOutputException(GetID, output);
		}
		GetInputValue(Inputs[0], ref _item, canvas);
		return CastValue<T>(_item.BaseRuntimeData.Level);
	}
}
