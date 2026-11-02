using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Mission/Is Mission Ongoing Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(NodeFunction),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas),
	typeof(Goal),
	typeof(SystemEvent),
	typeof(ConditionEvent)
})]
public class IsMissionOngoingNode : ParsecsNode
{
	public const string ID = "PE_IsMissionOngoingNode";

	public const string NODE_NAME = "Is Mission Ongoing";

	private const int OUTPUT_IS_ONGOING = 0;

	private const string OUTPUT_IS_ONGOING_NAME = "Is Ongoing";

	public override string GetID => "PE_IsMissionOngoingNode";

	public override Node Create(Vector2 pos)
	{
		IsMissionOngoingNode isMissionOngoingNode = ScriptableObject.CreateInstance<IsMissionOngoingNode>();
		isMissionOngoingNode.rect = new Rect(pos.x, pos.y, 150f, 100f);
		isMissionOngoingNode.name = "Is Mission Ongoing";
		isMissionOngoingNode.CreateOutput("Is Ongoing", "Bool");
		return isMissionOngoingNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return (IsMissionOngoingNode)Create(pos + new Vector2(20f, 20f));
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 0)
		{
			throw new NotExistingOutputException(GetID, output);
		}
		return CastValue<T>(MissionManager.Instance.IsMissionOngoing());
	}
}
