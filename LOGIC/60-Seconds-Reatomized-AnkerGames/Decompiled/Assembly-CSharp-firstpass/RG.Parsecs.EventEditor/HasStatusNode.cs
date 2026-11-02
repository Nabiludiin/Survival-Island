using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Character Nodes/Has Status Node", new Type[]
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
public class HasStatusNode : CharacterNode
{
	public const string ID = "HasStatusNode";

	private const string NODE_NAME = "Has Status";

	private const string CHAR_INPUT_NAME = "Character";

	private const string STATUS_INPUT_NAME = "Status";

	private const string OUTPUT_NAME = "Has Status";

	private const int CHAR_INPUT_INDEX = 0;

	private const int STATUS_INPUT_INDEX = 1;

	private const int OUTPUT_INDEX = 0;

	[SerializeField]
	private CharacterStatus _status;

	[SerializeField]
	private Character _character;

	public override string GetID => "HasStatusNode";

	public override Node Create(Vector2 pos)
	{
		HasStatusNode hasStatusNode = ScriptableObject.CreateInstance<HasStatusNode>();
		hasStatusNode.rect = new Rect(pos.x, pos.y, 200f, 130f);
		hasStatusNode.name = "Has Status";
		hasStatusNode.CreateInput("Character", "Character");
		hasStatusNode.CreateInput("Status", "CharacterStatusConnection");
		hasStatusNode.CreateOutput("Has Status", "Bool");
		return hasStatusNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		HasStatusNode hasStatusNode = (HasStatusNode)Create(rect.position + new Vector2(20f, 20f));
		hasStatusNode._status = _status;
		hasStatusNode._character = _character;
		return hasStatusNode;
	}

	protected override void NodeEnable()
	{
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
			throw new NodeOutputNotFoundException(output, "HasStatusNode");
		}
		GetInputValue(Inputs[1], ref _status, canvas);
		GetInputValue(Inputs[0], ref _character, canvas);
		return CastValue<T>(_character.RuntimeData.HasStatus(_status.Id));
	}
}
