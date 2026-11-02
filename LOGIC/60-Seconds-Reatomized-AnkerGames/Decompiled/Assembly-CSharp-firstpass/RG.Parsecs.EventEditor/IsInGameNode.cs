using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Character Nodes/Is In Game Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas),
	typeof(ConditionEvent),
	typeof(Goal)
})]
public class IsInGameNode : CharacterNode
{
	public const string ID = "isInGameNode";

	private const string NODE_NAME = "Is In Game";

	private const string INPUT_NAME = "Character";

	private const string OUTPUT_NAME = "Is In Game";

	private const int CHARACTER_INPUT_INDEX = 0;

	private const int OUTPUT_INDEX = 0;

	[SerializeField]
	private Character _character;

	public override string GetID => "isInGameNode";

	public override Node Create(Vector2 pos)
	{
		IsInGameNode isInGameNode = ScriptableObject.CreateInstance<IsInGameNode>();
		isInGameNode.rect = new Rect(pos.x, pos.y, 200f, 130f);
		isInGameNode.name = "Is In Game";
		isInGameNode.CreateInput("Character", "Character");
		isInGameNode.CreateOutput("Is In Game", "Bool");
		return isInGameNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		IsInGameNode isInGameNode = (IsInGameNode)Create(rect.position + new Vector2(20f, 20f));
		isInGameNode._character = _character;
		return isInGameNode;
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
			throw new NodeOutputNotFoundException(output, "isInGameNode");
		}
		GetInputValue(Inputs[0], ref _character, canvas);
		return CastValue<T>(CharacterManager.Instance.GetCharacterList().CharactersInGame.Contains(_character));
	}
}
