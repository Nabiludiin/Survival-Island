using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Type Nodes/Get Item Node", new Type[]
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
public class GetItemNode : ResourceNode
{
	public const string ID = "EE_GetItemNode";

	public const string NODE_NAME = "Get Item";

	public const string OUTPUT_NAME = "Out";

	public const int OUTPUT_OUT_INDEX = 0;

	[SerializeField]
	private IItem _item;

	public override string GetID => "EE_GetItemNode";

	public override Node Create(Vector2 pos)
	{
		GetItemNode getItemNode = ScriptableObject.CreateInstance<GetItemNode>();
		getItemNode.rect = new Rect(pos.x, pos.y, 150f, 90f);
		getItemNode.name = "Get Item";
		getItemNode.CreateOutput("Out", "Item");
		return getItemNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		GetItemNode getItemNode = (GetItemNode)Create(rect.position + new Vector2(20f, 20f));
		getItemNode._item = _item;
		return getItemNode;
	}

	protected override void NodeEnable()
	{
	}

	protected override void NodeGUI()
	{
	}

	protected override void OnNodeValidate()
	{
		base.OnNodeValidate();
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 0)
		{
			throw new NotExistingOutputException(GetID, output);
		}
		if (_item == null)
		{
			throw new UnityException("No item provided for Get Item Node!");
		}
		return CastValue<T>(_item);
	}

	public override string ToString()
	{
		return _item.ToString();
	}
}
