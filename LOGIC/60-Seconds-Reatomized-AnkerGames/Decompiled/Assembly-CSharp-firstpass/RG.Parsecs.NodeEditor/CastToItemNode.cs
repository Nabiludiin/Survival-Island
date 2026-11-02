using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.EventEditor;
using RG.Parsecs.GoalEditor;
using UnityEngine;

namespace RG.Parsecs.NodeEditor;

[Node(false, "Utility Nodes/Cast To Item Node", new Type[]
{
	typeof(Goal),
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas),
	typeof(ConditionEvent)
})]
public class CastToItemNode : EventNode
{
	public const string ID = "PE_CastToItem";

	private const int INPUT_OBJECT_INDEX = 0;

	private const int OUTPUT_CHARACTER_INDEX = 0;

	private const string INPUT_OBJECT_NAME = "Object";

	private const string OUTPUT_CHARACTER_NAME = "Item";

	private const string NODE_NAME = "Cast To Item";

	public override string GetID => "PE_CastToItem";

	public override Node Create(Vector2 pos)
	{
		CastToItemNode castToItemNode = ScriptableObject.CreateInstance<CastToItemNode>();
		castToItemNode.rect = new Rect(pos.x, pos.y, 150f, 75f);
		castToItemNode.name = "Cast To Item";
		castToItemNode.CreateInput("Object", ObjectConnection.ID);
		castToItemNode.CreateOutput("Item", "Item");
		return castToItemNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return Create(rect.position + new Vector2(20f, 20f));
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 0)
		{
			throw new NotExistingOutputException(GetID, output);
		}
		return CastValue<T>(GetInputValue<object>(Inputs[0], canvas));
	}
}
