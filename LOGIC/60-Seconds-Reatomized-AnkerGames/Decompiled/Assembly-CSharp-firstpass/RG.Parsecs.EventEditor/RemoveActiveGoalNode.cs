using System;
using NodeEditorFramework;
using RG.Parsecs.Common;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Goal Nodes/Remove Active Goal", new Type[]
{
	typeof(SurvivalEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(ReportEvent),
	typeof(Goal)
})]
public class RemoveActiveGoalNode : GoalNode
{
	public const string Id = "EE_RemoveActiveGoalNode";

	private const string FLOW_INPUT_NAME = "In";

	private const string FLOW_OUTPUT_NAME = "Out";

	private const string GOAL_INTPUT_NAME = "Goal";

	private const int FLOW_INPUT_INDEX = 0;

	private const int GOAL_INPUT_INDEX = 1;

	private const int FLOW_OUTPUT_INDEX = 0;

	private const string NODE_NAME = "Remove Game Goal";

	[SerializeField]
	private Goal _goal;

	public override string GetID => "EE_RemoveActiveGoalNode";

	public override Node Create(Vector2 pos)
	{
		RemoveActiveGoalNode removeActiveGoalNode = ScriptableObject.CreateInstance<RemoveActiveGoalNode>();
		removeActiveGoalNode.rect = new Rect(pos.x, pos.y, 150f, 85f);
		removeActiveGoalNode.name = "Remove Game Goal";
		removeActiveGoalNode.CreateMutliInput("In", "Flow");
		removeActiveGoalNode.CreateInput("Goal", "CNE_Goal");
		removeActiveGoalNode.CreateOutput("Out", "Flow");
		return removeActiveGoalNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		RemoveActiveGoalNode removeActiveGoalNode = (RemoveActiveGoalNode)Create(rect.position + new Vector2(20f, 20f));
		removeActiveGoalNode._goal = _goal;
		return removeActiveGoalNode;
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
		GoalManager.Instance.RemoveActiveGoal(_goal);
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
