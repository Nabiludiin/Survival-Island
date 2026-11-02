using System;
using NodeEditorFramework;
using RG.Parsecs.EventEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

[Node(false, "Flow Nodes/On Report Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent)
})]
public class OnReportNode : FlowNode
{
	public const string ID = "EE_OnReportNode";

	public const string NODE_NAME = "On Report";

	public override string GetID => "EE_OnReportNode";

	protected override bool IsSingularNode => true;

	public override Node Create(Vector2 pos)
	{
		OnReportNode onReportNode = ScriptableObject.CreateInstance<OnReportNode>();
		onReportNode.rect = new Rect(pos.x, pos.y, 150f, 30f);
		onReportNode.name = "On Report";
		onReportNode.CreateOutput("Out", "Flow");
		return onReportNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return Create(rect.position + new Vector2(20f, 20f));
	}

	public override void Execute(NodeCanvas canvas)
	{
		CheckAreAllFlowOutputsConnected();
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
