using System;
using NodeEditorFramework;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Supplies Nodes/Items/Is Item Enabled Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(ConditionEvent)
})]
public class IsItemEnabledNode : ResourceNode
{
	private const string OUTPUT_NOT_CONNECTED_MESSAGE = "Output is not connected";

	private const string ITEM_NOT_CONNECTED_MESSAGE = "No item set in Is Item Enabled node";

	public const string ID = "GE_IsItemEnabledNode";

	private const string NODE_NAME = "Is Item Enabled";

	private const string OUTPUT_NAME = "Is Enabled";

	private const string ITEM_INPUT_NAME = "Item";

	private const int OUTPUT_INDEX = 0;

	private const int ITEM_INPUT_INDEX = 0;

	[SerializeField]
	private IItem _item;

	public override string GetID => "GE_IsItemEnabledNode";

	public override Node Create(Vector2 pos)
	{
		IsItemEnabledNode isItemEnabledNode = ScriptableObject.CreateInstance<IsItemEnabledNode>();
		isItemEnabledNode.name = "Is Item Enabled";
		isItemEnabledNode.rect = new Rect(pos.x, pos.y, 150f, 100f);
		isItemEnabledNode.CreateInput("Item", "Item");
		isItemEnabledNode.CreateOutput("Is Enabled", "Bool");
		return isItemEnabledNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		IsItemEnabledNode isItemEnabledNode = (IsItemEnabledNode)Create(rect.position + new Vector2(20f, 20f));
		isItemEnabledNode._item = _item;
		return isItemEnabledNode;
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
		GetInputValue(Inputs[0], ref _item, canvas);
		return CastValue<T>(_item.BaseRuntimeData.IsEnabled);
	}
}
