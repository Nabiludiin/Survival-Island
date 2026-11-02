using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.EventEditor;
using RG.Parsecs.GoalEditor;
using UnityEngine;

namespace RG.Parsecs.NodeEditor;

[Node(false, "Utility Nodes/To String Node", new Type[]
{
	typeof(Goal),
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas)
})]
public class ToStringNode : EventNode
{
	public const string ID = "PE_ToStringNode";

	private const int INPUT_OBJECT_INDEX = 0;

	private const int OUTPUT_CHARACTER_INDEX = 0;

	private const string INPUT_OBJECT_NAME = "Object";

	private const string OUTPUT_STRING_NAME = "String";

	private const string NODE_NAME = "To String";

	public override string GetID => "PE_ToStringNode";

	public override Node Create(Vector2 pos)
	{
		ToStringNode toStringNode = ScriptableObject.CreateInstance<ToStringNode>();
		toStringNode.rect = new Rect(pos.x, pos.y, 250f, 150f);
		toStringNode.name = "To String";
		toStringNode.CreateInput("Object", ObjectConnection.ID);
		toStringNode.CreateOutput("String", "String");
		return toStringNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return Create(rect.position + new Vector2(20f, 20f));
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
		object inputValue = GetInputValue<object>(Inputs[0], canvas);
		return CastValue<T>(inputValue.ToString());
	}
}
