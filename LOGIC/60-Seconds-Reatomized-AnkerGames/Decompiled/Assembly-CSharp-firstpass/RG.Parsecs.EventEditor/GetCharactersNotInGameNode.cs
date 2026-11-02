using System;
using System.Collections.Generic;
using System.Linq;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Character Nodes/Get CharactersNotInGame Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas)
})]
public class GetCharactersNotInGameNode : CharacterNode
{
	public const string Id = "getCharactersNotInGameNode";

	private const int OUTPUT_OUT_INDEX = 0;

	private const string NODE_NAME = "Get Characters Not In Game";

	private const string OUTPUT_NAME = "Characters not in game";

	public override string GetID => "getCharactersNotInGameNode";

	public override Node Create(Vector2 pos)
	{
		GetCharactersNotInGameNode getCharactersNotInGameNode = ScriptableObject.CreateInstance<GetCharactersNotInGameNode>();
		getCharactersNotInGameNode.rect = new Rect(pos.x, pos.y, 175f, 40f);
		getCharactersNotInGameNode.name = "Get Characters Not In Game";
		getCharactersNotInGameNode.CreateOutput("Characters not in game", "CharacterList");
		return getCharactersNotInGameNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return (GetCharactersNotInGameNode)Create(rect.position + new Vector2(20f, 20f));
	}

	protected override void NodeGUI()
	{
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 0)
		{
			throw new NodeOutputNotFoundException(output, "getCharactersNotInGameNode");
		}
		List<Character> inGame = CharacterManager.Instance.GetCharacterList().CharactersInGame;
		List<Character> obj = (from x in CharacterManager.Instance.GetCharacterList().AllCharacters
			where !inGame.Contains(x)
			select (x)).ToList();
		return CastValue<T>(obj);
	}
}
