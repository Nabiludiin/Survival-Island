using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.EventEditor;
using RG.Parsecs.GoalEditor;
using UnityEngine;

namespace RG.Parsecs.NodeEditor;

[Node(false, "Utility Nodes/Cast To Resource Node", new Type[]
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
public class CastToResourceNode : EventNode
{
	public const string ID = "PE_CastToResourceNode";

	private const int INPUT_OBJECT_INDEX = 0;

	private const int OUTPUT_RESOURCE_INDEX = 0;

	private const string INPUT_OBJECT_NAME = "Object";

	private const string OUTPUT_RESOURCE_NAME = "Resource";

	private const string NODE_NAME = "Cast To Resource";

	public override string GetID => "PE_CastToResourceNode";

	public override Node Create(Vector2 pos)
	{
		CastToResourceNode castToResourceNode = ScriptableObject.CreateInstance<CastToResourceNode>();
		castToResourceNode.rect = new Rect(pos.x, pos.y, 150f, 75f);
		castToResourceNode.name = "Cast To Resource";
		castToResourceNode.CreateInput("Object", ObjectConnection.ID);
		castToResourceNode.CreateOutput("Resource", "Resources");
		return castToResourceNode;
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
