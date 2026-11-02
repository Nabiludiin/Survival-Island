using System;
using NodeEditorFramework;
using RG.Parsecs.EventEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

[Node(false, "Expedition Nodes/On Expedition Start Node", new Type[] { typeof(ExpeditionEvent) })]
public class OnExpeditionStartNode : FlowNode
{
	public const string ID = "EE_OnExpeditionStartNode";

	public const string NODE_NAME = "On Expedition Start";

	public override string GetID => "EE_OnExpeditionStartNode";

	protected override bool IsSingularNode => true;

	public override Node Create(Vector2 pos)
	{
		OnExpeditionStartNode onExpeditionStartNode = ScriptableObject.CreateInstance<OnExpeditionStartNode>();
		onExpeditionStartNode.rect = new Rect(pos.x, pos.y, 200f, 30f);
		onExpeditionStartNode.name = "On Expedition Start";
		onExpeditionStartNode.CreateOutput("Out", "Flow");
		return onExpeditionStartNode;
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
