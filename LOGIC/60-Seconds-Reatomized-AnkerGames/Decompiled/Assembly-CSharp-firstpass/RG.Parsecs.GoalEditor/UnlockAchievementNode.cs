using System;
using NodeEditorFramework;
using RG.Parsecs.Common;
using RG.Parsecs.EventEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.GoalEditor;

[Node(false, "Goal Nodes/Unlock Achievement", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(Goal)
})]
public class UnlockAchievementNode : ParsecsNode
{
	public const string ID = "GE_UnlockAchievementNode";

	[SerializeField]
	private Achievement _achievement;

	private const int INPUT_FLOW_INDEX = 0;

	private const int OUTPUT_FLOW_INDEX = 0;

	private const string INPUT_FLOW_NAME = "In";

	private const string OUTPUT_FLOW_NAME = "Out";

	private const string NODE_NAME = "Unlock achievement";

	public override string GetID => "GE_UnlockAchievementNode";

	public override Node Create(Vector2 pos)
	{
		UnlockAchievementNode unlockAchievementNode = ScriptableObject.CreateInstance<UnlockAchievementNode>();
		unlockAchievementNode.rect = new Rect(pos.x, pos.y, 200f, 100f);
		unlockAchievementNode.name = "Unlock achievement";
		unlockAchievementNode.CreateMutliInput("In", "Flow");
		unlockAchievementNode.CreateOutput("Out", "Flow");
		return unlockAchievementNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		UnlockAchievementNode unlockAchievementNode = (UnlockAchievementNode)Create(rect.position + new Vector2(20f, 20f));
		unlockAchievementNode._achievement = _achievement;
		return unlockAchievementNode;
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
		if (!_achievement.IsAchieved)
		{
			AchievementsSystem.UnlockAchievement(_achievement);
		}
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
