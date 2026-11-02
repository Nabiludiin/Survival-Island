using System;
using NodeEditorFramework;
using RG.Parsecs.Common;
using RG.Parsecs.EventEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.GoalEditor;

[Node(false, "Goal Nodes/Is Goal Failed", new Type[]
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
public class IsGoalFailedNode : GoalNode
{
	public const string ID = "GE_IsGoalFailedNode";

	private const string GOAL_INTPUT_NAME = "Goal";

	private const string IS_FAILED_OUTPUT_NAME = "Is Failed";

	private const int GOAL_INPUT_INDEX = 0;

	private const int IS_FAILED_OUTPUT_INDEX = 0;

	private const string NODE_NAME = "Is Goal Failed";

	[SerializeField]
	private Goal _goal;

	public override string GetID => "GE_IsGoalFailedNode";

	public override Node Create(Vector2 pos)
	{
		IsGoalFailedNode isGoalFailedNode = ScriptableObject.CreateInstance<IsGoalFailedNode>();
		isGoalFailedNode.name = "Is Goal Failed";
		isGoalFailedNode.rect = new Rect(pos.x, pos.y, 160f, 100f);
		isGoalFailedNode.CreateMutliInput("Goal", "CNE_Goal");
		isGoalFailedNode.CreateOutput("Is Failed", "Bool");
		return isGoalFailedNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		IsGoalFailedNode isGoalFailedNode = (IsGoalFailedNode)Create(rect.position + new Vector2(20f, 20f));
		isGoalFailedNode._goal = _goal;
		return isGoalFailedNode;
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
		return CastValue<T>(GoalManager.Instance.IsGoalFailed(_goal));
	}
}
