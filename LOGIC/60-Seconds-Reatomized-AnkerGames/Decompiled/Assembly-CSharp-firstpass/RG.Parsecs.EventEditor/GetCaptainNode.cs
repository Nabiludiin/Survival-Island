using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Character Nodes/Get Captain Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas),
	typeof(Goal),
	typeof(SystemStatusEvent)
})]
public class GetCaptainNode : CharacterNode
{
	private const string CURRENT_CAPTAIN_NAME = "Captain";

	private const int OUTPUT_GETCAPTAIN_INDEX = 0;

	public const string ID = "EE_getCaptainNode";

	public override string GetID => "EE_getCaptainNode";

	public override Node Create(Vector2 pos)
	{
		GetCaptainNode getCaptainNode = ScriptableObject.CreateInstance<GetCaptainNode>();
		getCaptainNode.rect = new Rect(pos.x, pos.y, 150f, 50f);
		getCaptainNode.name = "Get Captain";
		getCaptainNode.CreateOutput("Captain", "Character");
		return getCaptainNode;
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
			throw new NotExistingOutputException(GetID, output);
		}
		Character captain = CharacterManager.Instance.GetCharacterList().GetCaptain();
		return CastValue<T>(captain);
	}
}
