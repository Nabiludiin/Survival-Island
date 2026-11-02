using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.EventEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.NodeEditor;

[Node(false, "Utility Nodes/Get Interrupted Crafting Operation Node", new Type[]
{
	typeof(Goal),
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas)
})]
public class GetInterruptedCraftingOperationNode : EventNode
{
	public const string ID = "PE_GetInterruptedCraftingOperationNode";

	private const int OUTPUT_CHARACTER_INDEX = 0;

	private const string OUTPUT_STRING_NAME = "String";

	private const string NODE_NAME = "Get Interrupted Crafting Operation";

	public override string GetID => "PE_GetInterruptedCraftingOperationNode";

	public override Node Create(Vector2 pos)
	{
		GetInterruptedCraftingOperationNode getInterruptedCraftingOperationNode = ScriptableObject.CreateInstance<GetInterruptedCraftingOperationNode>();
		getInterruptedCraftingOperationNode.rect = new Rect(pos.x, pos.y, 300f, 150f);
		getInterruptedCraftingOperationNode.name = "Get Interrupted Crafting Operation";
		getInterruptedCraftingOperationNode.CreateOutput("String", "String");
		return getInterruptedCraftingOperationNode;
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
		string empty = string.Empty;
		return CastValue<T>(CraftingManager.GetInterruptedOperation() switch
		{
			EPlannedCraftingAction.NONE => (object)"none", 
			EPlannedCraftingAction.CRAFT => "craft", 
			EPlannedCraftingAction.RECYCLE => "recycle", 
			EPlannedCraftingAction.REPAIR => "repair", 
			EPlannedCraftingAction.UPGRADE => "upgrade", 
			_ => throw new ArgumentOutOfRangeException(), 
		});
	}
}
