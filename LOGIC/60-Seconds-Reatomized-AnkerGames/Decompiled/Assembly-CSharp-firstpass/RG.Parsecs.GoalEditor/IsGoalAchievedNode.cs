using System;
using NodeEditorFramework;
using RG.Parsecs.Common;
using RG.Parsecs.EventEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.GoalEditor;

[Node(false, "Goal Nodes/Is Goal Achieved", new Type[]
{
	typeof(SurvivalEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(ReportEvent),
	typeof(Goal),
	typeof(ConditionEvent)
})]
public class IsGoalAchievedNode : GoalNode
{
	public const string ID = "GE_IsGoalAchievedNode";

	private const string GOAL_INTPUT_NAME = "Goal";

	private const string IS_ACHIEVED_OUTPUT_NAME = "Is Achieved";

	private const int GOAL_INPUT_INDEX = 0;

	private const int IS_ACHIEVED_OUTPUT_INDEX = 0;

	private const string NODE_NAME = "Is Goal Achieved";

	[SerializeField]
	private Goal _goal;

	public override string GetID => "GE_IsGoalAchievedNode";

	public override Node Create(Vector2 pos)
	{
		IsGoalAchievedNode isGoalAchievedNode = ScriptableObject.CreateInstance<IsGoalAchievedNode>();
		isGoalAchievedNode.rect = new Rect(pos.x, pos.y, 160f, 85f);
		isGoalAchievedNode.name = "Is Goal Achieved";
		isGoalAchievedNode.CreateMutliInput("Goal", "CNE_Goal");
		isGoalAchievedNode.CreateOutput("Is Achieved", "Bool");
		return isGoalAchievedNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		IsGoalAchievedNode isGoalAchievedNode = (IsGoalAchievedNode)Create(rect.position + new Vector2(20f, 20f));
		isGoalAchievedNode._goal = _goal;
		return isGoalAchievedNode;
	}

	protected override void NodeEnable()
	{
	}

	protected override void OnNodeValidate()
	{
	}

	protected override void NodeGUI()
	{
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 0)
		{
			throw new NotExistingOutputException(GetID, output);
		}
		GetInputValue(Inputs[0], ref _goal, canvas);
		return CastValue<T>(GoalManager.Instance.IsGoalAchieved(_goal));
	}
}
