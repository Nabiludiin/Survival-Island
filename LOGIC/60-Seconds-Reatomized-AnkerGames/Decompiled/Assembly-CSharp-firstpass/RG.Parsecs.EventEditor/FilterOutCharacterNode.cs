using System;
using System.Collections.Generic;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Character Nodes/Filter Out Character Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas),
	typeof(ConditionEvent)
})]
public class FilterOutCharacterNode : CharacterNode
{
	public const string ID = "filterOutCharacterNode";

	private const int OUTPUT_OUT_INDEX = 0;

	private const int CHARACTERS_INPUT_INDEX = 0;

	private const int CHARACTER_INPUT_INDEX = 1;

	private const string NODE_NAME = "Filter Out Character";

	private const string OUTPUT_NAME = "Filtered";

	private const string CHARACTERS_INPUT_NAME = "Characters";

	private const string CHARACTER_INPUT_NAME = "Filtered out character";

	[SerializeField]
	private Character _character;

	public override string GetID => "filterOutCharacterNode";

	public override Node Create(Vector2 pos)
	{
		FilterOutCharacterNode filterOutCharacterNode = ScriptableObject.CreateInstance<FilterOutCharacterNode>();
		filterOutCharacterNode.rect = new Rect(pos.x, pos.y, 200f, 40f);
		filterOutCharacterNode.name = "Filter Out Character";
		filterOutCharacterNode.CreateInput("Characters", "CharacterList");
		filterOutCharacterNode.CreateInput("Filtered out character", "Character");
		filterOutCharacterNode.CreateOutput("Filtered", "CharacterList");
		return filterOutCharacterNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		FilterOutCharacterNode filterOutCharacterNode = (FilterOutCharacterNode)Create(rect.position + new Vector2(20f, 20f));
		filterOutCharacterNode._character = _character;
		return filterOutCharacterNode;
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
			throw new NodeOutputNotFoundException(output, "filterOutCharacterNode");
		}
		List<Character> list = new List<Character>();
		List<Character> inputValue = GetInputValue<List<Character>>(Inputs[0], canvas);
		GetInputValue(Inputs[1], ref _character, canvas);
		for (int i = 0; i < inputValue.Count; i++)
		{
			if (!_character.Equals(inputValue[i]))
			{
				list.Add(inputValue[i]);
			}
		}
		return CastValue<T>(list);
	}
}
