using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Supplies Nodes/Items/Is Item Max Level Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas),
	typeof(ConditionEvent)
})]
public class IsItemMaxLevelNode : ResourceNode
{
	public const string ID = "EE_IsItemMaxLevelNode";

	[SerializeField]
	private IItem _item;

	private const string INPUT_ITEM_NAME = "Item";

	private const string OUTPUT_IS_UPGRAGED_NAME = "Result";

	private const int INPUT_ITEM_INDEX = 0;

	private const int OUTPUT_IS_UPGRADED_INDEX = 0;

	public override string GetID => "EE_IsItemMaxLevelNode";

	public override Node Create(Vector2 pos)
	{
		IsItemMaxLevelNode isItemMaxLevelNode = ScriptableObject.CreateInstance<IsItemMaxLevelNode>();
		isItemMaxLevelNode.rect = new Rect(pos.x, pos.y, 180f, 130f);
		isItemMaxLevelNode.name = "Is Max Level";
		isItemMaxLevelNode.CreateInput("Item", "Item");
		isItemMaxLevelNode.CreateOutput("Result", "Bool");
		return isItemMaxLevelNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		IsItemMaxLevelNode isItemMaxLevelNode = (IsItemMaxLevelNode)Create(rect.position + new Vector2(20f, 20f));
		isItemMaxLevelNode._item = _item;
		return isItemMaxLevelNode;
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
		return CastValue<T>(_item.IsMaxLevel());
	}
}
