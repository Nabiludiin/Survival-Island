using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.EventEditor;
using RG.Parsecs.GoalEditor;
using UnityEngine;

namespace RG.Parsecs.NodeEditor;

[Node(false, "Utility Nodes/Cast To Character Node", new Type[]
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
public class CastToCharacterNode : EventNode
{
	public const string ID = "PE_CastToCharacter";

	private const int INPUT_OBJECT_INDEX = 0;

	private const int OUTPUT_CHARACTER_INDEX = 0;

	private const string INPUT_OBJECT_NAME = "Object";

	private const string OUTPUT_CHARACTER_NAME = "Character";

	private const string NODE_NAME = "Cast To Character";

	public override string GetID => "PE_CastToCharacter";

	public override Node Create(Vector2 pos)
	{
		CastToCharacterNode castToCharacterNode = ScriptableObject.CreateInstance<CastToCharacterNode>();
		castToCharacterNode.rect = new Rect(pos.x, pos.y, 150f, 75f);
		castToCharacterNode.name = "Cast To Character";
		castToCharacterNode.CreateInput("Object", ObjectConnection.ID);
		castToCharacterNode.CreateOutput("Character", "Character");
		return castToCharacterNode;
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
