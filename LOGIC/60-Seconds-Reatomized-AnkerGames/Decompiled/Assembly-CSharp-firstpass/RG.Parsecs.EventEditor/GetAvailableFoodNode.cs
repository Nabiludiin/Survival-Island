using System;
using NodeEditorFramework;
using RG.Parsecs.Common;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(true, "Legacy/Food/Get Available Food Node", new Type[]
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
public class GetAvailableFoodNode : ResourceNode
{
	public const string ID = "EE_GetAvailableFoodNode";

	private const string FOOD_ID = "item_food";

	private const string OUTPUT_NAME = "Value";

	public override string GetID => "EE_GetAvailableFoodNode";

	public override Node Create(Vector2 pos)
	{
		GetAvailableFoodNode getAvailableFoodNode = ScriptableObject.CreateInstance<GetAvailableFoodNode>();
		getAvailableFoodNode.rect = new Rect(pos.x, pos.y, 150f, 60f);
		getAvailableFoodNode.name = "Get Available Food";
		getAvailableFoodNode.CreateOutput("Value", "Float");
		return getAvailableFoodNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return Create(rect.position + new Vector2(20f, 20f));
	}

	protected override void OnNodeValidate()
	{
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 0)
		{
			throw new NotExistingOutputException(GetID, output);
		}
		ConsumableRemedium consumableRemedium = (ConsumableRemedium)Singleton<ItemManager>.Instance.GetItem("item_food");
		return CastValue<T>(consumableRemedium.RuntimeData.Amount - consumableRemedium.RuntimeData.PlannedConsumption);
	}
}
