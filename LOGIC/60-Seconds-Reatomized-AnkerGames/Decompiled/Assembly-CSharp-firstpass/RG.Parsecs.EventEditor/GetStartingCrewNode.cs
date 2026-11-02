using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Character Nodes/Get Starting Crew Node", new Type[]
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
public class GetStartingCrewNode : CharacterNode
{
	public const string Id = "getStartingCrewNode";

	private const int CREW_OUTPUT_INDEX = 0;

	private const string CREW_OUTPUT_NAME = "Crew";

	private const string NODE_NAME = "Get Starting Crew";

	public override string GetID => "getStartingCrewNode";

	public override Node Create(Vector2 pos)
	{
		GetStartingCrewNode getStartingCrewNode = ScriptableObject.CreateInstance<GetStartingCrewNode>();
		getStartingCrewNode.rect = new Rect(pos.x, pos.y, 200f, 40f);
		getStartingCrewNode.name = "Get Starting Crew";
		getStartingCrewNode.CreateOutput("Crew", "Int");
		return getStartingCrewNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return (GetStartingCrewNode)Create(rect.position + new Vector2(20f, 20f));
	}

	protected override void NodeGUI()
	{
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 0)
		{
			throw new NodeOutputNotFoundException(output, "getStartingCrewNode");
		}
		int startingCrew = CharacterManager.Instance.GetCharacterList().StartingCrew;
		return CastValue<T>(startingCrew);
	}
}
