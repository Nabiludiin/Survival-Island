using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.EventEditor;
using RG.Parsecs.GoalEditor;
using UnityEngine;

namespace RG.Parsecs.NodeEditor;

[Node(false, "Condition Nodes/Is Null Node", new Type[]
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
public class IsNullNode : ConditionNode
{
	public const string ID = "PE_IsNullNode";

	private const int OUTPUT_RESULT_INDEX = 0;

	private const int INPUT_OBJECT_INDEX = 0;

	private const string NODE_NAME = "Is Null Node";

	private const string INPUT_OBJECT_NAME = "Object";

	private const string INPUT_RESULT_NAME = "Result";

	public override string GetID => "PE_IsNullNode";

	public override Node Create(Vector2 pos)
	{
		IsNullNode isNullNode = ScriptableObject.CreateInstance<IsNullNode>();
		isNullNode.rect = new Rect(pos.x, pos.y, 100f, 85f);
		isNullNode.name = "Is Null Node";
		isNullNode.CreateInput("Object", ObjectConnection.ID);
		isNullNode.CreateOutput("Result", "Bool");
		return isNullNode;
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
		UnityEngine.Object currentValue = null;
		GetInputValue(Inputs[0], ref currentValue, canvas);
		return CastValue<T>(currentValue == null);
	}
}
