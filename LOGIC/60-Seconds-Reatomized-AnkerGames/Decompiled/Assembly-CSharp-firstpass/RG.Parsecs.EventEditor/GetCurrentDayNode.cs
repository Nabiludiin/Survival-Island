using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Utility Nodes/Get Current Day Node", new Type[]
{
	typeof(Goal),
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(ExpeditionEvent),
	typeof(SystemStatusEvent),
	typeof(EndGameCanvas),
	typeof(ConditionEvent)
})]
public class GetCurrentDayNode : FlowNode
{
	public const string ID = "GE_GetCurrentDayNode";

	private const string NODE_NAME = "Get Current Day";

	private const string OUTPUT_DAY_NAME = "Current Day";

	private const int OUTPUT_DAY_INDEX = 0;

	public override string GetID => "GE_GetCurrentDayNode";

	public override Node Create(Vector2 pos)
	{
		GetCurrentDayNode getCurrentDayNode = ScriptableObject.CreateInstance<GetCurrentDayNode>();
		getCurrentDayNode.rect = new Rect(pos.x, pos.y, 150f, 100f);
		getCurrentDayNode.name = "Get Current Day";
		getCurrentDayNode.CreateOutput("Current Day", "Int");
		return getCurrentDayNode;
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
		if (output != 0)
		{
			throw new NotExistingOutputException(GetID, output);
		}
		return CastValue<T>(EndOfDayManager.Instance.AcutalDay);
	}
}
