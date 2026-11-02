using System;
using I2.Loc;
using NodeEditorFramework;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Character Nodes/Add Character History Text", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(Goal)
})]
public class AddCharacterHistoryTextNode : CharacterNode
{
	private const string ID = "EE_AddCharacterHistoryText";

	private const string NODE_NAME = "Character History Text";

	private const string INPUT_IN_NAME = "In";

	private const string INPUT_CHARACTER_NAME = "Character";

	private const string INPUT_TEXT_NAME = "Text";

	private const string OUTPUT_OUT_NAME = "Out";

	private const int INPUT_IN_INDEX = 0;

	private const int INPUT_CHARACTER_INDEX = 1;

	private const int INPUT_TEXT_INDEX = 2;

	private const int OUTPUT_OUT_INDEX = 0;

	private const string TERM_NOT_SET_MESSAGE = "Term was not set!";

	[SerializeField]
	private LocalizedString _text;

	[SerializeField]
	private Character _character;

	public override string GetID => "EE_AddCharacterHistoryText";

	public override Node Create(Vector2 pos)
	{
		AddCharacterHistoryTextNode addCharacterHistoryTextNode = ScriptableObject.CreateInstance<AddCharacterHistoryTextNode>();
		addCharacterHistoryTextNode.rect = new Rect(pos.x, pos.y, 200f, 150f);
		addCharacterHistoryTextNode.name = "Character History Text";
		addCharacterHistoryTextNode.CreateMutliInput("In", "Flow");
		addCharacterHistoryTextNode.CreateInput("Character", "Character");
		addCharacterHistoryTextNode.CreateInput("Text", "LocalizedString");
		addCharacterHistoryTextNode.CreateOutput("Out", "Flow");
		return addCharacterHistoryTextNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		AddCharacterHistoryTextNode addCharacterHistoryTextNode = (AddCharacterHistoryTextNode)Create(rect.position + new Vector2(20f, 20f));
		addCharacterHistoryTextNode._text = _text;
		addCharacterHistoryTextNode._character = _character;
		return addCharacterHistoryTextNode;
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

	public override void Execute(NodeCanvas canvas)
	{
		GetInputValue(Inputs[2], ref _text, canvas);
		GetInputValue(Inputs[1], ref _character, canvas);
		CheckAreAllFlowOutputsConnected();
		if (_character != null)
		{
			_character.RuntimeData.AddTextUpdate(_text);
		}
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
