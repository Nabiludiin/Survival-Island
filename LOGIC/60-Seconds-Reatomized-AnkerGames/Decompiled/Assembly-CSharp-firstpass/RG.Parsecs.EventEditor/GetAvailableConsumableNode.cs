using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(true, "Legacy/Supplies Nodes/Consumables/Get Available Consumable Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas),
	typeof(ConditionEvent)
})]
public class GetAvailableConsumableNode : ResourceNode
{
	public const string ID = "EE_GetAvailableConsumableNode";

	private const string OUTPUT_NAME = "Value";

	private const string NODE_NAME = "Get Available Consumable";

	private const string INPUT_CONSUMABLE_NAME = "Consumable object";

	private const int INPUT_CONSUMABLE_INDEX = 0;

	private const int OUTPUT_VALUE_INDEX = 0;

	[SerializeField]
	private ConsumableRemedium _consumableRemedium;

	public override string GetID => "EE_GetAvailableConsumableNode";

	public override Node Create(Vector2 pos)
	{
		GetAvailableConsumableNode getAvailableConsumableNode = ScriptableObject.CreateInstance<GetAvailableConsumableNode>();
		getAvailableConsumableNode.rect = new Rect(pos.x, pos.y, 200f, 60f);
		getAvailableConsumableNode.name = "Get Available Consumable";
		getAvailableConsumableNode.CreateInput("Consumable object", "ConsumableRemedium");
		getAvailableConsumableNode.CreateOutput("Value", "Float");
		return getAvailableConsumableNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		GetAvailableConsumableNode getAvailableConsumableNode = (GetAvailableConsumableNode)Create(rect.position + new Vector2(20f, 20f));
		getAvailableConsumableNode._consumableRemedium = _consumableRemedium;
		return getAvailableConsumableNode;
	}

	protected override void NodeEnable()
	{
	}

	protected override void OnNodeValidate()
	{
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
		return CastValue<T>(_consumableRemedium.RuntimeData.Amount - _consumableRemedium.RuntimeData.PlannedConsumption);
	}
}
