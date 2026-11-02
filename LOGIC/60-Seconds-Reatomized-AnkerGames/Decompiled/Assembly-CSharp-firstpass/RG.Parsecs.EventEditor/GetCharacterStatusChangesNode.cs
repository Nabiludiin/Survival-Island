using System;
using System.Collections.Generic;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Character Nodes/Get Status Changes Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas),
	typeof(Goal)
})]
public class GetCharacterStatusChangesNode : CharacterNode
{
	public const string Id = "PE_getCharacterStatusChangesNode";

	private const int CHARACTER_INPUT_INDEX = 0;

	private const string CHARACTER_INPUT_NAME = "Character";

	private const int GAINED_INPUT_INDEX = 1;

	private const string GAINED_INPUT_NAME = "Gained";

	private const int LOST_INPUT_INDEX = 2;

	private const string LOST_INPUT_NAME = "Lost";

	private const int STATUSES_OUTPUT_INDEX = 0;

	private const string STATUSES_OUTPUT_NAME = "Statuses";

	private const string NODE_NAME = "Get Status Change";

	[SerializeField]
	private Character _character;

	[SerializeField]
	private bool _gained = true;

	[SerializeField]
	private bool _lost;

	public override string GetID => "PE_getCharacterStatusChangesNode";

	public override Node Create(Vector2 pos)
	{
		GetCharacterStatusChangesNode getCharacterStatusChangesNode = ScriptableObject.CreateInstance<GetCharacterStatusChangesNode>();
		getCharacterStatusChangesNode.rect = new Rect(pos.x, pos.y, 200f, 40f);
		getCharacterStatusChangesNode.name = "Get Status Change";
		getCharacterStatusChangesNode.CreateInput("Character", "Character");
		getCharacterStatusChangesNode.CreateInput("Gained", "Bool");
		getCharacterStatusChangesNode.CreateInput("Lost", "Bool");
		getCharacterStatusChangesNode.CreateOutput("Statuses", ListConnection.ID);
		return getCharacterStatusChangesNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		GetCharacterStatusChangesNode getCharacterStatusChangesNode = (GetCharacterStatusChangesNode)Create(rect.position + new Vector2(20f, 20f));
		getCharacterStatusChangesNode._character = _character;
		getCharacterStatusChangesNode._gained = _gained;
		getCharacterStatusChangesNode._lost = _lost;
		return getCharacterStatusChangesNode;
	}

	protected override void OnNodeValidate()
	{
	}

	protected override void NodeEnable()
	{
	}

	protected override void NodeGUI()
	{
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 0)
		{
			throw new NodeOutputNotFoundException(output, "PE_getCharacterStatusChangesNode");
		}
		GetInputValue(Inputs[0], ref _character, canvas);
		GetInputValue(Inputs[1], ref _gained, canvas);
		GetInputValue(Inputs[2], ref _lost, canvas);
		List<CharacterStatus> obj = ((!_gained && _lost) ? _character.RuntimeData.GetStatusesLost() : _character.RuntimeData.GetStatusesGained());
		return CastValue<T>(obj);
	}
}
