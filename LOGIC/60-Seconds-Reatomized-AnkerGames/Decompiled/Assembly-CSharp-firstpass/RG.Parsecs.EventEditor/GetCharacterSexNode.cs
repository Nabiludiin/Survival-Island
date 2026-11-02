using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Character Nodes/Get Character Sex Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas)
})]
public class GetCharacterSexNode : CharacterNode
{
	private const string NODE_NAME = "Get Character Sex";

	private const string IS_FEMALE = "Is Female";

	private const string CHOSEN_CHARACTER = "Chosen Character";

	private const int OUTPUT_GET_CHARACTER_SEX_INDEX = 0;

	private const int INPUT_CHARACTER_INDEX = 0;

	public const string ID = "EE_getCharacterSexNode";

	[SerializeField]
	private Character _character;

	public override string GetID => "EE_getCharacterSexNode";

	public override Node Create(Vector2 pos)
	{
		GetCharacterSexNode getCharacterSexNode = ScriptableObject.CreateInstance<GetCharacterSexNode>();
		getCharacterSexNode.rect = new Rect(pos.x, pos.y, 200f, 105f);
		getCharacterSexNode.name = "Get Character Sex";
		getCharacterSexNode.CreateInput("Chosen Character", "Character");
		getCharacterSexNode.CreateOutput("Is Female", "Bool");
		return getCharacterSexNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		GetCharacterSexNode getCharacterSexNode = (GetCharacterSexNode)Create(rect.position + new Vector2(20f, 20f));
		getCharacterSexNode._character = _character;
		return getCharacterSexNode;
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
		GetInputValue(Inputs[0], ref _character, canvas);
		return CastValue<T>(_character.StaticData.Female);
	}
}
