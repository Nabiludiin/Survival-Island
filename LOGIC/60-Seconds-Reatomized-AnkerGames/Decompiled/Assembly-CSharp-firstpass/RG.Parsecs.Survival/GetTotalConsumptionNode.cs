using System;
using System.Collections.Generic;
using NodeEditorFramework;
using RG.Parsecs.Common;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.EventEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Node(false, "Supplies Nodes/Consumables/Get Total Consumption Node", new Type[]
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
public class GetTotalConsumptionNode : ResourceNode
{
	public const string ID = "EE_GetTotalConsumptionNode";

	[SerializeField]
	private ConsumableRemedium _consumableRemedium;

	private const string NODE_NAME = "Get Total Consumption";

	private const string CONSUMABLE_INPUT_NAME = "Consumable";

	private const string CONSUMED_OUTPUT_NAME = "Total Consumed";

	private const string RATIONED_OUTPUT_NAME = "Total Rationioned";

	private const int CONSUMABLE_INPUT_INDEX = 0;

	private const int RATIONED_OUTPUT_INDEX = 0;

	private const int CONSUMED_OUTPUT_INDEX = 1;

	public override string GetID => "EE_GetTotalConsumptionNode";

	public override Node Create(Vector2 pos)
	{
		GetTotalConsumptionNode getTotalConsumptionNode = ScriptableObject.CreateInstance<GetTotalConsumptionNode>();
		getTotalConsumptionNode.name = "Get Total Consumption";
		getTotalConsumptionNode.rect = new Rect(pos.x, pos.y, 200f, 60f);
		getTotalConsumptionNode.CreateInput("Consumable", "ConsumableRemedium");
		getTotalConsumptionNode.CreateOutput("Total Rationioned", "Int");
		getTotalConsumptionNode.CreateOutput("Total Consumed", "Float");
		return getTotalConsumptionNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		GetTotalConsumptionNode getTotalConsumptionNode = (GetTotalConsumptionNode)Create(rect.position + new Vector2(20f, 20f));
		getTotalConsumptionNode._consumableRemedium = _consumableRemedium;
		return getTotalConsumptionNode;
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
		GetInputValue(Inputs[0], ref _consumableRemedium, canvas);
		List<RationingStatsEntry> rationingStatsEntries = StatsManager.Instance.RationingStatsEntries;
		int num = 0;
		for (int i = 0; i < rationingStatsEntries.Count; i++)
		{
			if (rationingStatsEntries[i].ItemId.Equals(_consumableRemedium.BaseStaticData.ItemId))
			{
				num++;
			}
		}
		return output switch
		{
			0 => CastValue<T>(num), 
			1 => CastValue<T>((float)num * _consumableRemedium.StaticData.ConsumeRatio), 
			_ => throw new NotExistingOutputException(GetID, output), 
		};
	}
}
