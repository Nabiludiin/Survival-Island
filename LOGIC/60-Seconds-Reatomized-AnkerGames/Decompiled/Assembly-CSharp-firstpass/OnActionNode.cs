using System;
using NodeEditorFramework;
using RG.Parsecs.EventEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

[Node(false, "Flow Nodes/On Action Node", new Type[] { typeof(SurvivalEvent) })]
public class OnActionNode : FlowNode
{
	public const string ID = "EE_OnActionNode";

	public const string NODE_NAME = "On Action";

	public override string GetID => "EE_OnActionNode";

	protected override bool IsSingularNode => true;

	public override Node Create(Vector2 pos)
	{
		OnActionNode onActionNode = ScriptableObject.CreateInstance<OnActionNode>();
		onActionNode.rect = new Rect(pos.x, pos.y, 150f, 30f);
		onActionNode.name = "On Action";
		onActionNode.CreateOutput("Out", "Flow");
		return onActionNode;
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
