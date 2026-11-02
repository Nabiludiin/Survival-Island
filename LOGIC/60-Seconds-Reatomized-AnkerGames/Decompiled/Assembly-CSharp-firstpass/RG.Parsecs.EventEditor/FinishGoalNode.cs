using System;
using NodeEditorFramework;
using RG.Parsecs.Common;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Goal Nodes/Finish Goal", new Type[]
{
	typeof(SurvivalEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(ReportEvent),
	typeof(Goal)
})]
public class FinishGoalNode : GoalNode
{
	public const string ID = "EE_FinishGoalNode";

	private const string FLOW_INPUT_NAME = "In";

	private const string FLOW_OUTPUT_NAME = "Out";

	private const string GOAL_INPUT_NAME = "Goal";

	private const string GOAL_STATUS_NAME = "Achieved";

	private const int FLOW_INPUT_INDEX = 0;

	private const int GOAL_INPUT_INDEX = 1;

	private const int STATUS_GOAL_INPUT_INDEX = 2;

	private const int FLOW_OUTPUT_INDEX = 0;

	private const string NODE_NAME = "Finish Goal";

	[SerializeField]
	private Goal _goal;

	[SerializeField]
	private bool _isAchieved = true;

	public override string GetID => "EE_FinishGoalNode";

	public override Node Create(Vector2 pos)
	{
		FinishGoalNode finishGoalNode = ScriptableObject.CreateInstance<FinishGoalNode>();
		finishGoalNode.rect = new Rect(pos.x, pos.y, 160f, 85f);
		finishGoalNode.name = "Finish Goal";
		finishGoalNode.CreateMutliInput("In", "Flow");
		finishGoalNode.CreateInput("Goal", "CNE_Goal");
		finishGoalNode.CreateInput("Achieved", "Bool");
		finishGoalNode.CreateOutput("Out", "Flow");
		return finishGoalNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		FinishGoalNode finishGoalNode = (FinishGoalNode)Create(rect.position + new Vector2(20f, 20f));
		finishGoalNode._goal = _goal;
		return finishGoalNode;
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

	public override void Execute(NodeCanvas canvas)
	{
		CheckAreAllFlowOutputsConnected();
		GetInputValue(Inputs[1], ref _goal, canvas);
		GetInputValue(Inputs[2], ref _isAchieved, canvas);
		if (_isAchieved)
		{
			GoalManager.Instance.MarkGoalAsAchieved(_goal);
		}
		else
		{
			GoalManager.Instance.MarkGoalAsFailed(_goal);
		}
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
