using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Supplies Nodes/Consumables/Consumable Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(Goal),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas),
	typeof(ConditionEvent)
})]
public class ConsumableNode : CalculationNode
{
	public const string ID = "EE_ConsumablelNode";

	[SerializeField]
	private ConsumableRemedium _consumable;

	private const string NODE_NAME = "Consumable Node";

	private const string OUTPUT_NAME = "Value";

	private const int OUTPUT_INDEX = 0;

	public override string GetID => "EE_ConsumablelNode";

	public override Node Create(Vector2 pos)
	{
		ConsumableNode consumableNode = ScriptableObject.CreateInstance<ConsumableNode>();
		consumableNode.rect = new Rect(pos.x, pos.y, 180f, 100f);
		consumableNode.name = "Consumable Node";
		consumableNode.CreateOutput("Value", "ConsumableRemedium");
		return consumableNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return Create(rect.position + new Vector2(20f, 20f));
	}

	protected override void NodeEnable()
	{
	}

	protected override void NodeGUI()
	{
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 0)
		{
			throw new NodeOutputNotFoundException(output, "EE_ConsumablelNode");
		}
		return CastValue<T>(_consumable);
	}
}
