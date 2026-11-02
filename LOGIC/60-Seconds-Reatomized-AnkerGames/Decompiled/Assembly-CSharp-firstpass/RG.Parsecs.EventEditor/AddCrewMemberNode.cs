using System;
using NodeEditorFramework;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Character Nodes/Add Crew Member Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent)
})]
public class AddCrewMemberNode : CharacterNode
{
	public const string Id = "EE_AddCrewMemberNode";

	private const int FLOW_INPUT_INDEX = 0;

	private const int OUTPUT_OUT_INDEX = 0;

	private const int CHARACTER_INPUT_INDEX = 1;

	private const string NODE_NAME = "Add Crew Member";

	private const string OUTPUT_NAME = "Out";

	private const string FLOW_INPUT_NAME = "In";

	private const string CHARACTERS_INPUT_NAME = "Character";

	private const int MAX_CREW_SIZE = 4;

	[SerializeField]
	private Character _character;

	public override string GetID => "EE_AddCrewMemberNode";

	public override Node Create(Vector2 pos)
	{
		AddCrewMemberNode addCrewMemberNode = ScriptableObject.CreateInstance<AddCrewMemberNode>();
		addCrewMemberNode.rect = new Rect(pos.x, pos.y, 220f, 40f);
		addCrewMemberNode.name = "Add Crew Member";
		addCrewMemberNode.CreateMutliInput("In", "Flow");
		addCrewMemberNode.CreateInput("Character", "Character");
		addCrewMemberNode.CreateOutput("Out", "Flow");
		return addCrewMemberNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		AddCrewMemberNode addCrewMemberNode = (AddCrewMemberNode)Create(rect.position + new Vector2(20f, 20f));
		addCrewMemberNode._character = _character;
		return addCrewMemberNode;
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

	public override void Execute(NodeCanvas canvas)
	{
		CheckAreAllFlowOutputsConnected();
		GetInputValue(Inputs[1], ref _character, canvas);
		CharacterList characterList = CharacterManager.Instance.GetCharacterList();
		if (characterList.GetCharacterCount() < 4 && _character != null && !characterList.CharactersInGame.Contains(_character))
		{
			characterList.AddCharToList(_character);
		}
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
