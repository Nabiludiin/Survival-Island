using System;
using NodeEditorFramework;
using RG.Parsecs.EventEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

[Node(false, "Flow Nodes/On Prepare Node", new Type[] { typeof(SurvivalEvent) })]
public class OnPrepareNode : FlowNode
{
	public const string ID = "EE_OnPrepareNode";

	public const string NODE_NAME = "On Prepare";

	public override string GetID => "EE_OnPrepareNode";

	protected override bool IsSingularNode => true;

	public override Node Create(Vector2 pos)
	{
		OnPrepareNode onPrepareNode = ScriptableObject.CreateInstance<OnPrepareNode>();
		onPrepareNode.rect = new Rect(pos.x, pos.y, 150f, 30f);
		onPrepareNode.name = "On Prepare";
		onPrepareNode.CreateOutput("Out", "Flow");
		return onPrepareNode;
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
