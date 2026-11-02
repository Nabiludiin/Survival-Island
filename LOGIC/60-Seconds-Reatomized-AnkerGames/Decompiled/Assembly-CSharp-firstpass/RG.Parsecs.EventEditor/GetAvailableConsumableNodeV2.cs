using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Supplies Nodes/Consumables/Get Available Consumable Node", new Type[]
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
public class GetAvailableConsumableNodeV2 : ResourceNode
{
	public const string ID = "EE_GetAvailableConsumableNodeV2";

	private const string AVAILABLE_OUTPUT_NAME = "Value";

	private const string PLANNED_CONSUMPTION_OUTPUT_NAME = "Planned consumption";

	private const string NODE_NAME = "Get Available Consumable";

	private const string INPUT_CONSUMABLE_NAME = "Consumable object";

	private const int INPUT_CONSUMABLE_INDEX = 0;

	private const int OUTPUT_VALUE_INDEX = 0;

	private const int OUTPUT_PLANNED_INDEX = 1;

	[SerializeField]
	private ConsumableRemedium _consumableRemedium;

	public override string GetID => "EE_GetAvailableConsumableNodeV2";

	public override Node Create(Vector2 pos)
	{
		GetAvailableConsumableNodeV2 getAvailableConsumableNodeV = ScriptableObject.CreateInstance<GetAvailableConsumableNodeV2>();
		getAvailableConsumableNodeV.rect = new Rect(pos.x, pos.y, 200f, 60f);
		getAvailableConsumableNodeV.name = "Get Available Consumable";
		getAvailableConsumableNodeV.CreateInput("Consumable object", "ConsumableRemedium");
		getAvailableConsumableNodeV.CreateOutput("Value", "Float");
		getAvailableConsumableNodeV.CreateOutput("Planned consumption", "Float");
		return getAvailableConsumableNodeV;
	}

	public override Node Duplicate(Vector2 pos)
	{
		GetAvailableConsumableNodeV2 getAvailableConsumableNodeV = (GetAvailableConsumableNodeV2)Create(rect.position + new Vector2(20f, 20f));
		getAvailableConsumableNodeV._consumableRemedium = _consumableRemedium;
		return getAvailableConsumableNodeV;
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
		return output switch
		{
			0 => CastValue<T>(_consumableRemedium.RuntimeData.Amount - _consumableRemedium.RuntimeData.PlannedConsumption), 
			1 => CastValue<T>(_consumableRemedium.RuntimeData.PlannedConsumption), 
			_ => throw new NotExistingOutputException(GetID, output), 
		};
	}
}
