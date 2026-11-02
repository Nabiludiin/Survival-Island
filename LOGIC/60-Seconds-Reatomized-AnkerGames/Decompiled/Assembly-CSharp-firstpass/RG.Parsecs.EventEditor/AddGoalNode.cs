using System;
using NodeEditorFramework;
using RG.Parsecs.Common;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Goal Nodes/Add Goal", new Type[]
{
	typeof(SurvivalEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(ReportEvent),
	typeof(Goal)
})]
public class AddGoalNode : GoalNode
{
	private const string FLOW_INPUT_NAME = "In";

	private const string FLOW_OUTPUT_NAME = "Out";

	private const string GOAL_INTPUT_NAME = "Goal";

	private const int FLOW_INPUT_INDEX = 0;

	private const int GOAL_INPUT_INDEX = 1;

	private const int FLOW_OUTPUT_INDEX = 0;

	private const string NODE_NAME = "Add Goal";

	public const string Id = "EE_AddGoalNode";

	[SerializeField]
	private Goal _goal;

	public override string GetID => "EE_AddGoalNode";

	public override Node Create(Vector2 pos)
	{
		AddGoalNode addGoalNode = ScriptableObject.CreateInstance<AddGoalNode>();
		addGoalNode.rect = new Rect(pos.x, pos.y, 160f, 85f);
		addGoalNode.name = "Add Goal";
		addGoalNode.CreateMutliInput("In", "Flow");
		addGoalNode.CreateInput("Goal", "CNE_Goal");
		addGoalNode.CreateOutput("Out", "Flow");
		return addGoalNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		AddGoalNode addGoalNode = (AddGoalNode)Create(rect.position + new Vector2(20f, 20f));
		addGoalNode._goal = _goal;
		return addGoalNode;
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
		GoalManager.Instance.AddActiveGoal(_goal);
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
