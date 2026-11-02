using System;
using NodeEditorFramework;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Expedition Nodes/Get Expedition Character Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(ConditionEvent)
})]
public class GetExpeditionCharacterNode : ExpeditionNode
{
	public const string ID = "EE_GetCharacterOnExpeditionNode";

	private const string OUTPUT_CHARACTER_NAME = "Character";

	private const int OUTPUT_CHARACTER_INDEX = 0;

	private const string NODE_NAME = "Get Expedition Character";

	public override string GetID => "EE_GetCharacterOnExpeditionNode";

	public override Node Create(Vector2 pos)
	{
		GetExpeditionCharacterNode getExpeditionCharacterNode = ScriptableObject.CreateInstance<GetExpeditionCharacterNode>();
		getExpeditionCharacterNode.rect = new Rect(pos.x, pos.y, 170f, 50f);
		getExpeditionCharacterNode.name = "Get Expedition Character";
		getExpeditionCharacterNode.CreateOutput("Character", "Character");
		return getExpeditionCharacterNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return Create(rect.position + new Vector2(20f, 20f));
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output == 0)
		{
			return CastValue<T>(ExpeditionManager.Instance.GetExpeditionCharacter());
		}
		throw new NotExistingOutputException(GetID, output);
	}
}
