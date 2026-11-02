using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Flow Nodes/End Flow Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas),
	typeof(Goal),
	typeof(ConditionEvent)
})]
public class EndFlowNode : FlowNode
{
	public const string ID = "EE_EndFlowNode";

	public override string GetID => "EE_EndFlowNode";

	public override Node Create(Vector2 pos)
	{
		EndFlowNode endFlowNode = ScriptableObject.CreateInstance<EndFlowNode>();
		endFlowNode.rect = new Rect(pos.x, pos.y, 150f, 50f);
		endFlowNode.name = "End Flow";
		endFlowNode.CreateMutliInput("In", "Flow");
		return endFlowNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return Create(rect.position + new Vector2(20f, 20f));
	}

	public override void Execute(NodeCanvas canvas)
	{
	}
}
