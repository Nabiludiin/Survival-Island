using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(true, "Legacy/Break Character Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas)
})]
public class BreakCharacterNodeLegacy : CharacterNode
{
	public const string ID = "EE_breakCharacterNode";

	private const string INPUT_NAME = "Character";

	private const int INPUT_INDEX = 0;

	private const int NAME_OUTPUT_INDEX = 0;

	private const string NAME_OUTPUT_NAME = "Name";

	private const int SURNAME_OUTPUT_INDEX = 1;

	private const string SURNAME_OUTPUT_NAME = "Surname";

	private const int IS_CAPTAIN_OUTPUT_INDEX = 2;

	private const string IS_CAPTAIN_OUTPUT_NAME = "Is Captain";

	private const string NODE_NAME = "Break Character";

	[SerializeField]
	private Character _character;

	public override string GetID => "EE_breakCharacterNode";

	public override Node Create(Vector2 pos)
	{
		BreakCharacterNodeLegacy breakCharacterNodeLegacy = ScriptableObject.CreateInstance<BreakCharacterNodeLegacy>();
		breakCharacterNodeLegacy.rect = new Rect(pos.x, pos.y, 150f, 225f);
		breakCharacterNodeLegacy.name = "Break Character";
		breakCharacterNodeLegacy.CreateInput("Character", "Character");
		breakCharacterNodeLegacy.CreateOutput("Name", "LocalizedString");
		breakCharacterNodeLegacy.CreateOutput("Surname", "LocalizedString");
		breakCharacterNodeLegacy.CreateOutput("Is Captain", "Bool");
		return breakCharacterNodeLegacy;
	}

	public override Node Duplicate(Vector2 pos)
	{
		BreakCharacterNodeLegacy breakCharacterNodeLegacy = (BreakCharacterNodeLegacy)Create(rect.position + new Vector2(20f, 20f));
		breakCharacterNodeLegacy._character = _character;
		return breakCharacterNodeLegacy;
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
		GetInputValue(Inputs[0], ref _character, canvas);
		return output switch
		{
			0 => CastValue<T>(_character.StaticData.Name), 
			1 => CastValue<T>(_character.StaticData.Surname), 
			2 => CastValue<T>(_character.RuntimeData.IsCaptain), 
			_ => throw new NotExistingOutputException(GetID, output), 
		};
	}
}
