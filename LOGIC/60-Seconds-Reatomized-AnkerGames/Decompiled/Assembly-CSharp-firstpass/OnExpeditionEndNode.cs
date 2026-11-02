using System;
using NodeEditorFramework;
using RG.Parsecs.EventEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

[Node(false, "Expedition Nodes/On Expedition End Node", new Type[] { typeof(ExpeditionEvent) })]
public class OnExpeditionEndNode : FlowNode
{
	public const string ID = "EE_OnExpeditionEndNode";

	public const string NODE_NAME = "On Expedition End";

	public const string OUTPUT_RETURNED_NAME = "Character Returned";

	public const string OUTPUT_LOST_NAME = "Character Lost";

	public const int OUTPUT_RETURNED_INDEX = 0;

	public const int OUTPUT_LOST_INDEX = 1;

	public override string GetID => "EE_OnExpeditionEndNode";

	protected override bool IsSingularNode => true;

	public override Node Create(Vector2 pos)
	{
		OnExpeditionEndNode onExpeditionEndNode = ScriptableObject.CreateInstance<OnExpeditionEndNode>();
		onExpeditionEndNode.rect = new Rect(pos.x, pos.y, 200f, 30f);
		onExpeditionEndNode.name = "On Expedition End";
		onExpeditionEndNode.CreateOutput("Character Returned", "Flow");
		onExpeditionEndNode.CreateOutput("Character Lost", "Flow");
		return onExpeditionEndNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return Create(rect.position + new Vector2(20f, 20f));
	}

	public override void Execute(NodeCanvas canvas)
	{
		CheckAreAllFlowOutputsConnected();
		ExpeditionEvent expeditionEvent = ParentEvent as ExpeditionEvent;
		if (expeditionEvent != null)
		{
			if (!expeditionEvent.ReturnEvaluated)
			{
				throw new UnityException("Character Return not evaluated! Evaluate return first using Evaluate Character Return and End Character Return Evaluation nodes.");
			}
			if (expeditionEvent.CharacterReturned)
			{
				Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
			}
			else
			{
				Outputs[1].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
			}
		}
	}
}
