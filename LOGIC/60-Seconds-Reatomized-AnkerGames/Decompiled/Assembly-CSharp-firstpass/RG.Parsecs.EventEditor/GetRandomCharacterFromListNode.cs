using System;
using System.Collections.Generic;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Character Nodes/Get Random Character From List Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas)
})]
public class GetRandomCharacterFromListNode : CharacterNode
{
	private const int FLOW_INPUT_INDEX = 0;

	private const string FLOW_INPUT_NAME = "Randomize selection";

	private const int FLOW_OUTPUT_INDEX = 0;

	private const string FLOW_OUTPUT_NAME = "Out";

	private const string OUTPUT_NAME = "Character";

	private const string INPUT_NAME = "Character list";

	private const int CHARACTER_INPUT_INDEX = 1;

	private const int OUTPUT_INDEX = 1;

	private const string NODE_NAME = "Get Random Character From List";

	public const string ID = "getRandomCharacterFromListNode";

	private Character _chosenCharacter;

	public override string GetID => "getRandomCharacterFromListNode";

	public override Node Create(Vector2 pos)
	{
		GetRandomCharacterFromListNode getRandomCharacterFromListNode = ScriptableObject.CreateInstance<GetRandomCharacterFromListNode>();
		getRandomCharacterFromListNode.rect = new Rect(pos.x, pos.y, 220f, 50f);
		getRandomCharacterFromListNode.name = "Get Random Character From List";
		getRandomCharacterFromListNode.CreateMutliInput("Randomize selection", "Flow");
		getRandomCharacterFromListNode.CreateInput("Character list", "CharacterList");
		getRandomCharacterFromListNode.CreateOutput("Out", "Flow");
		getRandomCharacterFromListNode.CreateOutput("Character", "Character");
		return getRandomCharacterFromListNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return (GetRandomCharacterFromListNode)Create(rect.position + new Vector2(20f, 20f));
	}

	protected override void NodeGUI()
	{
	}

	protected override void OnNodeValidate()
	{
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 1)
		{
			throw new NodeOutputNotFoundException(output, "getRandomCharacterFromListNode");
		}
		if (_chosenCharacter == null)
		{
			ChooseCharacter(canvas);
		}
		return CastValue<T>(_chosenCharacter);
	}

	public override void Execute(NodeCanvas canvas)
	{
		CheckAreAllFlowOutputsConnected();
		ChooseCharacter(canvas);
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}

	private void ChooseCharacter(NodeCanvas canvas)
	{
		List<Character> inputValue = GetInputValue<List<Character>>(Inputs[1], canvas);
		if (inputValue != null && inputValue.Count > 0)
		{
			_chosenCharacter = inputValue[UnityEngine.Random.Range(0, inputValue.Count)];
		}
	}
}
