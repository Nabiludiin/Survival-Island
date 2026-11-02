using System;
using NodeEditorFramework;
using RG.Parsecs.Common;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.GoalEditor;

[Node(false, "Flow Nodes/Goal Evaluation Node", new Type[] { typeof(Goal) })]
public class GoalEvaluationNode : GoalNode
{
	public const string ID = "GE_GoalEvaluationNode";

	[SerializeField]
	private bool _conditionSuccess;

	[SerializeField]
	private bool _conditionFailure;

	private bool _isSingularNode;

	private const int INPUT_FLOW_INDEX = 0;

	private const int INPUT_GOAL_ACHIEVED_INDEX = 1;

	private const int INPUT_GOAL_FAILED_INDEX = 2;

	private const int OUTPUT_REWARDS_INDEX = 0;

	private const int OUTPUT_CONSEQUENCES_INDEX = 1;

	private const string INPUT_NAME = "In";

	private const string INPUT_GOAL_ACHIVED_TEXT = "Goal Achieved";

	private const string INPUT_GOAL_FAILED_TEXT = "Goal Failed";

	private const string OUTPUT_SUCCESS_TEXT = "Success rewards";

	private const string OUTPUT_FAILURE_TEXT = "Failure consequences";

	private const string NODE_NAME = "Goal Evaluation";

	public override string GetID => "GE_GoalEvaluationNode";

	protected override bool IsSingularNode => true;

	public override Node Create(Vector2 pos)
	{
		GoalEvaluationNode goalEvaluationNode = ScriptableObject.CreateInstance<GoalEvaluationNode>();
		goalEvaluationNode.rect = new Rect(pos.x, pos.y, 200f, 200f);
		goalEvaluationNode.name = "Goal Evaluation";
		goalEvaluationNode.CreateMutliInput("In", "Flow");
		goalEvaluationNode.CreateInput("Goal Achieved", "Bool");
		goalEvaluationNode.CreateInput("Goal Failed", "Bool");
		goalEvaluationNode.CreateOutput("Success rewards", "Flow");
		goalEvaluationNode.CreateOutput("Failure consequences", "Flow");
		_conditionSuccess = false;
		_conditionFailure = false;
		return goalEvaluationNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		GoalEvaluationNode goalEvaluationNode = (GoalEvaluationNode)Create(rect.position + new Vector2(20f, 20f));
		goalEvaluationNode._conditionSuccess = _conditionSuccess;
		goalEvaluationNode._conditionFailure = _conditionFailure;
		return goalEvaluationNode;
	}

	protected override void NodeGUI()
	{
	}

	public override void Execute(NodeCanvas canvas)
	{
		_conditionSuccess = GetInputValue<bool>(Inputs[1], canvas);
		_conditionFailure = GetInputValue<bool>(Inputs[2], canvas);
		if (_conditionSuccess)
		{
			GoalManager.Instance.MarkGoalAsAchieved((Goal)ParentCanvas);
			Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
		}
		else if (_conditionFailure)
		{
			GoalManager.Instance.MarkGoalAsFailed((Goal)ParentCanvas);
			Outputs[1].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
		}
	}
}
