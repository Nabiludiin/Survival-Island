using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Mission/Unlock Medal Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(NodeFunction),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas),
	typeof(Goal),
	typeof(SystemEvent)
})]
public class UnlockMedalNode : EventNode
{
	public const string ID = "SE_UnlockMedalNode";

	private const string NODE_NAME = "Unlock medal";

	private const string INPUT_FLOW_NAME = "In";

	private const string INPUT_GOAL_NAME = "Goal";

	private const string OUTPUT_FLOW_NAME = "Out";

	private const int INPUT_FLOW_INDEX = 0;

	private const int INPUT_GOAL_INDEX = 1;

	private const int OUTPUT_FLOW_INDEX = 0;

	[SerializeField]
	private Goal _goal;

	public override string GetID => "SE_UnlockMedalNode";

	public override Node Create(Vector2 pos)
	{
		UnlockMedalNode unlockMedalNode = ScriptableObject.CreateInstance<UnlockMedalNode>();
		unlockMedalNode.rect = new Rect(pos.x, pos.y, 300f, 200f);
		unlockMedalNode.name = "Unlock medal";
		unlockMedalNode.CreateMutliInput("In", "Flow");
		unlockMedalNode.CreateInput("Goal", "CNE_Goal");
		unlockMedalNode.CreateOutput("Out", "Flow");
		return unlockMedalNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		UnlockMedalNode unlockMedalNode = (UnlockMedalNode)Create(rect.position + new Vector2(20f, 20f));
		unlockMedalNode._goal = _goal;
		return unlockMedalNode;
	}

	protected override void NodeEnable()
	{
	}

	protected override void NodeGUI()
	{
	}

	public override void Execute(NodeCanvas canvas)
	{
		GetInputValue(Inputs[1], ref _goal, canvas);
		MissionManager.Instance.UnlockMedal(_goal);
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
