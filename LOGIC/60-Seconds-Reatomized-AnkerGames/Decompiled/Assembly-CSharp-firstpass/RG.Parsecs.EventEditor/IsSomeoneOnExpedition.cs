using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Character Nodes/Is Someone On Expedition Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas),
	typeof(ConditionEvent)
})]
public class IsSomeoneOnExpedition : CharacterNode
{
	private const string OUTPUT_NAME = "Is On Expedition";

	private const int OUTPUT_INDEX = 0;

	private const string NODE_NAME = "Is Someone On Expedition";

	public const string ID = "EE_isSomeoneOnExpeditionNode";

	public override string GetID => "EE_isSomeoneOnExpeditionNode";

	public override Node Create(Vector2 pos)
	{
		IsSomeoneOnExpedition isSomeoneOnExpedition = ScriptableObject.CreateInstance<IsSomeoneOnExpedition>();
		isSomeoneOnExpedition.rect = new Rect(pos.x, pos.y, 150f, 50f);
		isSomeoneOnExpedition.name = "Is Someone On Expedition";
		isSomeoneOnExpedition.CreateOutput("Is On Expedition", "Bool");
		return isSomeoneOnExpedition;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return Create(rect.position + new Vector2(20f, 20f));
	}

	protected override void NodeGUI()
	{
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 0)
		{
			throw new NodeOutputNotFoundException(output, "EE_isSomeoneOnExpeditionNode");
		}
		bool flag = CharacterManager.Instance.GetCharacterList().GetCharactersWithStatus("status_onexpedition").Count > 0;
		return CastValue<T>(flag);
	}
}
