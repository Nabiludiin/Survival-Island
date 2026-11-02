using System;
using NodeEditorFramework;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Character Nodes/Set Displays Slide In Ending Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(Goal),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent)
})]
public class SetDisplaysSlideInEndingNode : CharacterNode
{
	public const string ID = "setDisplaysSlideInEndingNode";

	public const string FLOW_INPUT_NAME = "In";

	public const string CHARACTER_INPUT_NAME = "Character";

	public const string VALUE_INPUT_NAME = "Value to set";

	public const string NODE_NAME = "Set Displays Slide";

	public const string FLOW_OUTPUT_NAME = "Out";

	public const string SET_FOR_ALL_INPUT_NAME = "Set for all characters";

	public const int FLOW_INPUT_INDEX = 0;

	public const int VALUE_INPUT_INDEX = 2;

	public const int SET_FOR_ALL_INPUT_INDEX = 3;

	public const int CHARACTER_INPUT_INDEX = 1;

	public const int FLOW_OUTPUT_INDEX = 0;

	[SerializeField]
	private bool _valueToSet;

	[SerializeField]
	private bool _setForAll;

	[SerializeField]
	private Character _character;

	public override string GetID => "setDisplaysSlideInEndingNode";

	public override Node Create(Vector2 pos)
	{
		SetDisplaysSlideInEndingNode setDisplaysSlideInEndingNode = ScriptableObject.CreateInstance<SetDisplaysSlideInEndingNode>();
		setDisplaysSlideInEndingNode.rect = new Rect(pos.x, pos.y, 200f, 105f);
		setDisplaysSlideInEndingNode.name = "Set Displays Slide";
		setDisplaysSlideInEndingNode.CreateMutliInput("In", "Flow");
		setDisplaysSlideInEndingNode.CreateInput("Character", "Character");
		setDisplaysSlideInEndingNode.CreateInput("Value to set", "Bool");
		setDisplaysSlideInEndingNode.CreateInput("Set for all characters", "Bool");
		setDisplaysSlideInEndingNode.CreateOutput("Out", "Flow");
		return setDisplaysSlideInEndingNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		SetDisplaysSlideInEndingNode setDisplaysSlideInEndingNode = (SetDisplaysSlideInEndingNode)Create(rect.position + new Vector2(20f, 20f));
		setDisplaysSlideInEndingNode._character = _character;
		setDisplaysSlideInEndingNode._valueToSet = _valueToSet;
		setDisplaysSlideInEndingNode._setForAll = _setForAll;
		return setDisplaysSlideInEndingNode;
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
		GetInputValue(Inputs[1], ref _character, canvas);
		GetInputValue(Inputs[2], ref _valueToSet, canvas);
		GetInputValue(Inputs[2], ref _setForAll, canvas);
		if (_setForAll)
		{
			for (int i = 0; i < CharacterManager.Instance.GetCharacterList().CharactersInGame.Count; i++)
			{
				CharacterManager.Instance.GetCharacterList().CharactersInGame[i].RuntimeData.ShowsEndOfGameSlide = _valueToSet;
			}
		}
		else
		{
			_character.RuntimeData.ShowsEndOfGameSlide = _valueToSet;
		}
		CheckAreAllFlowOutputsConnected();
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
