using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.EventEditor;
using RG.Parsecs.GoalEditor;
using UnityEngine;

namespace RG.Parsecs.NodeEditor;

[Node(false, "Utility Nodes/Cast To Status Node", new Type[]
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
public class CastToStatusNode : EventNode
{
	public const string ID = "PE_CastToStatus";

	private const int INPUT_OBJECT_INDEX = 0;

	private const int OUTPUT_CHARACTER_INDEX = 0;

	private const string INPUT_OBJECT_NAME = "Object";

	private const string OUTPUT_STATUS_NAME = "Status";

	private const string NODE_NAME = "Cast To Status";

	public override string GetID => "PE_CastToStatus";

	public override Node Create(Vector2 pos)
	{
		CastToStatusNode castToStatusNode = ScriptableObject.CreateInstance<CastToStatusNode>();
		castToStatusNode.rect = new Rect(pos.x, pos.y, 150f, 75f);
		castToStatusNode.name = "Cast To Status";
		castToStatusNode.CreateInput("Object", ObjectConnection.ID);
		castToStatusNode.CreateOutput("Status", "CharacterStatusConnection");
		return castToStatusNode;
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
