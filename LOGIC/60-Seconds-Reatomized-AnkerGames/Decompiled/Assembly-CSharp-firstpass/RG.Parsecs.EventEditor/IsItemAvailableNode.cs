using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Supplies Nodes/Items/Is Item Available Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas),
	typeof(Goal),
	typeof(ConditionEvent)
})]
public class IsItemAvailableNode : ResourceNode
{
	public const string ID = "EE_IsAvailableNode";

	[SerializeField]
	private IItem _item;

	private const int INPUT_ITEM_INDEX = 0;

	private const int OUTPUT_IS_AVAILABLE_INDEX = 0;

	private const string INPUT_ITEM_NAME = "Item";

	private const string OUTPUT_IS_AVAILABLE_NAME = "Is Available";

	public override string GetID => "EE_IsAvailableNode";

	public override Node Create(Vector2 pos)
	{
		IsItemAvailableNode isItemAvailableNode = ScriptableObject.CreateInstance<IsItemAvailableNode>();
		isItemAvailableNode.rect = new Rect(pos.x, pos.y, 180f, 120f);
		isItemAvailableNode.name = "Is Item Available";
		isItemAvailableNode.CreateInput("Item", "Item");
		isItemAvailableNode.CreateOutput("Is Available", "Bool");
		return isItemAvailableNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		IsItemAvailableNode isItemAvailableNode = (IsItemAvailableNode)Create(rect.position + new Vector2(20f, 20f));
		isItemAvailableNode._item = _item;
		return isItemAvailableNode;
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
		bool flag = _item.BaseRuntimeData.IsAvailable || _item.BaseRuntimeData.IsOnExpedition;
		return CastValue<T>(flag);
	}
}
