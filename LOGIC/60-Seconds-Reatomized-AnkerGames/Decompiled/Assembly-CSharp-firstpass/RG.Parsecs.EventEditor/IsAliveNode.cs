using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Character Nodes/Is Alive Node", new Type[]
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
public class IsAliveNode : CharacterNode
{
	public const string ID = "EE_isAliveNode";

	private const string NODE_NAME = "Is Alive";

	private const string INPUT_NAME = "Character";

	private const string OUTPUT_NAME = "Is Alive";

	private const int CHARACTER_INPUT_INDEX = 0;

	private const int OUTPUT_INDEX = 0;

	[SerializeField]
	private Character _character;

	public override string GetID => "EE_isAliveNode";

	public override Node Create(Vector2 pos)
	{
		IsAliveNode isAliveNode = ScriptableObject.CreateInstance<IsAliveNode>();
		isAliveNode.rect = new Rect(pos.x, pos.y, 200f, 130f);
		isAliveNode.name = "Is Alive";
		isAliveNode.CreateInput("Character", "Character");
		isAliveNode.CreateOutput("Is Alive", "Bool");
		return isAliveNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		IsAliveNode isAliveNode = (IsAliveNode)Create(rect.position + new Vector2(20f, 20f));
		isAliveNode._character = _character;
		return isAliveNode;
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
			throw new NodeOutputNotFoundException(output, "EE_isAliveNode");
		}
		GetInputValue(Inputs[0], ref _character, canvas);
		return CastValue<T>(_character.RuntimeData.IsAlive());
	}
}
