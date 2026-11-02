using System;
using NodeEditorFramework;
using RG.Parsecs.Common;
using RG.Parsecs.EventEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.GoalEditor;

[Node(false, "Goal Nodes/Progress Achievement", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(Goal)
})]
public class ProgressAchievementNode : ParsecsNode
{
	public const string ID = "GE_ProgressAchievementNode";

	[SerializeField]
	private Achievement _achievement;

	[SerializeField]
	private int _maxValue;

	private const int INPUT_FLOW_INDEX = 0;

	private const int PROGRESS_INPUT = 1;

	private const int OUTPUT_FLOW_INDEX = 0;

	private const string INPUT_FLOW_NAME = "In";

	private const string OUTPUT_FLOW_NAME = "Out";

	private const string INPUT_PROGRESS_NAME = "Progress";

	private const string NODE_NAME = "Progress achievement";

	public override string GetID => "GE_ProgressAchievementNode";

	public override Node Create(Vector2 pos)
	{
		ProgressAchievementNode progressAchievementNode = ScriptableObject.CreateInstance<ProgressAchievementNode>();
		progressAchievementNode.rect = new Rect(pos.x, pos.y, 200f, 100f);
		progressAchievementNode.name = "Progress achievement";
		progressAchievementNode.CreateMutliInput("In", "Flow");
		progressAchievementNode.CreateInput("Progress", "Int");
		progressAchievementNode.CreateOutput("Out", "Flow");
		return progressAchievementNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return Create(rect.position + new Vector2(20f, 20f));
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
		int inputValue = GetInputValue<int>(Inputs[1], canvas);
		if (!_achievement.IsAchieved)
		{
			AchievementsSystem.ProgressAchievement(_achievement, inputValue, _maxValue);
		}
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
