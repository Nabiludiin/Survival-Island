using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Type Nodes/Get Character Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas),
	typeof(Goal),
	typeof(ConditionEvent)
})]
public class GetCharacterNode : CharacterNode
{
	public const string ID = "getCharacterNode";

	public const string NODE_NAME = "Get Character";

	public const string OUTPUT_NAME = "Out";

	public const int OUTPUT_OUT_INDEX = 0;

	[SerializeField]
	private Character _character;

	public override string GetID => "getCharacterNode";

	public override Node Create(Vector2 pos)
	{
		GetCharacterNode getCharacterNode = ScriptableObject.CreateInstance<GetCharacterNode>();
		getCharacterNode.rect = new Rect(pos.x, pos.y, 150f, 90f);
		getCharacterNode.name = "Get Character";
		getCharacterNode.CreateOutput("Out", "Character");
		return getCharacterNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		GetCharacterNode getCharacterNode = (GetCharacterNode)Create(rect.position + new Vector2(20f, 20f));
		getCharacterNode._character = _character;
		return getCharacterNode;
	}

	protected override void NodeEnable()
	{
	}

	protected override void NodeGUI()
	{
	}

	protected override void OnNodeValidate()
	{
		base.OnNodeValidate();
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 0)
		{
			throw new NotExistingOutputException(GetID, output);
		}
		if (_character == null)
		{
			throw new UnityException("No character provided for Get Character Node!");
		}
		return CastValue<T>(_character);
	}

	public override string ToString()
	{
		return _character.ToString();
	}
}
