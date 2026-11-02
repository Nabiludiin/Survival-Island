using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Character Nodes/Break Character Node", new Type[]
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
public class BreakCharacterNodeV2 : CharacterNode
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

	private const int SEX_OUTPUT_INDEX = 3;

	private const string IS_FEMALE_NAME = "Is Female";

	private const string NODE_NAME = "Break Character";

	private const string ICON_TERM_NAME = "Icon Term";

	private const int ICON_TERM_OUTPUT_INDEX = 4;

	[SerializeField]
	private Character _character;

	public override string GetID => "EE_breakCharacterNode";

	public override Node Create(Vector2 pos)
	{
		BreakCharacterNodeV2 breakCharacterNodeV = ScriptableObject.CreateInstance<BreakCharacterNodeV2>();
		breakCharacterNodeV.rect = new Rect(pos.x, pos.y, 150f, 225f);
		breakCharacterNodeV.name = "Break Character";
		breakCharacterNodeV.CreateInput("Character", "Character");
		breakCharacterNodeV.CreateOutput("Name", "LocalizedString");
		breakCharacterNodeV.CreateOutput("Surname", "LocalizedString");
		breakCharacterNodeV.CreateOutput("Is Captain", "Bool");
		breakCharacterNodeV.CreateOutput("Is Female", "Bool");
		breakCharacterNodeV.CreateOutput("Icon Term", "LocalizedString");
		return breakCharacterNodeV;
	}

	public override Node Duplicate(Vector2 pos)
	{
		BreakCharacterNodeV2 breakCharacterNodeV = (BreakCharacterNodeV2)Create(rect.position + new Vector2(20f, 20f));
		breakCharacterNodeV._character = _character;
		return breakCharacterNodeV;
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
			3 => CastValue<T>(_character.StaticData.Female), 
			4 => CastValue<T>(_character.StaticData.IconTerm), 
			_ => throw new NotExistingOutputException(GetID, output), 
		};
	}
}
