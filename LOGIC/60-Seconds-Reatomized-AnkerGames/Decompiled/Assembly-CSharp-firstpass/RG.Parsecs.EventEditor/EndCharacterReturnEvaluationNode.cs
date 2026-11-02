using System;
using NodeEditorFramework;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Expedition Nodes/End Character Return Evaluation Node", new Type[] { typeof(ExpeditionEvent) })]
public class EndCharacterReturnEvaluationNode : FlowNode
{
	public const string ID = "EE_EndCharacterReturnEvaluationNode";

	public override string GetID => "EE_EndCharacterReturnEvaluationNode";

	public override Node Create(Vector2 pos)
	{
		EndCharacterReturnEvaluationNode endCharacterReturnEvaluationNode = ScriptableObject.CreateInstance<EndCharacterReturnEvaluationNode>();
		endCharacterReturnEvaluationNode.rect = new Rect(pos.x, pos.y, 250f, 50f);
		endCharacterReturnEvaluationNode.name = "End Character Return Evaluation";
		endCharacterReturnEvaluationNode.CreateMutliInput("In", "Flow");
		return endCharacterReturnEvaluationNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return Create(rect.position + new Vector2(20f, 20f));
	}

	public override void Execute(NodeCanvas canvas)
	{
		ExpeditionEvent expeditionEvent = ParentEvent as ExpeditionEvent;
		if (expeditionEvent != null && !expeditionEvent.ReturnEvaluated)
		{
			Character expeditionCharacter = ExpeditionManager.Instance.GetExpeditionCharacter();
			expeditionEvent.CharacterReturned = !expeditionCharacter.RuntimeData.HasStatusPreventingReturnFromExpedition();
			expeditionEvent.ReturnEvaluated = true;
		}
	}
}
