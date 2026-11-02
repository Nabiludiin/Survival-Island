using System;
using NodeEditorFramework;
using RG.Parsecs.EventEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.GoalEditor;

[Node(false, "Utility Nodes/Is Game Finished Node", new Type[]
{
	typeof(Goal),
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(ConditionEvent)
})]
public class IsGameFinishedNode : GoalNode
{
	public const string ID = "GE_IsGameFinishedNode";

	private const string NODE_NAME = "Is Game Finished";

	private const string IS_FINISHED_OUTPUT_NAME = "Is Finished";

	private const string IS_WON_OUTPUT_NAME = "Is Won";

	private const int IS_FINISHED_OUTPUT_INDEX = 0;

	private const int IS_WON_OUTPUT_INDEX = 1;

	public override string GetID => "GE_IsGameFinishedNode";

	public override Node Create(Vector2 pos)
	{
		IsGameFinishedNode isGameFinishedNode = ScriptableObject.CreateInstance<IsGameFinishedNode>();
		isGameFinishedNode.rect = new Rect(pos.x, pos.y, 150f, 150f);
		isGameFinishedNode.name = "Is Game Finished";
		isGameFinishedNode.CreateOutput("Is Finished", "Bool");
		isGameFinishedNode.CreateOutput("Is Won", "Bool");
		return isGameFinishedNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return Create(rect.position + new Vector2(20f, 20f));
	}

	protected override void NodeGUI()
	{
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		EndGameData endGameData = EndGameManager.Instance.GetEndGameData();
		return output switch
		{
			0 => CastValue<T>(endGameData.RuntimeData.ShouldEndGame), 
			1 => CastValue<T>(endGameData.RuntimeData.IsGameWon), 
			_ => throw new NotExistingOutputException(GetID, output), 
		};
	}
}
