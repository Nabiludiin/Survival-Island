using System;
using System.Collections.Generic;
using NodeEditorFramework;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Expedition Nodes/Break Destination Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent)
})]
public class BreakExpeditionDestinationNode : ExpeditionNode
{
	public const string ID = "EE_BreakExpeditionDestinationNode";

	public const string NODE_NAME = "Break Destination";

	public const string INPUT_DESTINATION_NAME = "Destination";

	public const int INPUT_DESTINATION_INDEX = 0;

	public const string OUTPUT_NAME_NAME = "Name";

	public const string OUTPUT_DESCRIPTION_NAME = "Description";

	public const string OUTPUT_REQUIRED_LEVEL_NAME = "Required System Level";

	public const string OUTPUT_ENABLED_NAME = "Enabled";

	public const string OUTPUT_EXPEDITION_EVENT_NAME = "Expedition Event";

	public const string OUTPUT_CHANCE_ITEMS_NAME = "Chance For Items";

	public const string OUTPUT_CHANCE_RESOURCES_NAME = "Chance For Resources";

	public const string OUTPUT_THREATS_NAME = "Threats";

	public const string OUTPUT_ICON_NAME = "Icon";

	public const int OUTPUT_NAME_INDEX = 0;

	public const int OUTPUT_DESCRIPTION_INDEX = 1;

	public const int OUTPUT_REQUIRED_LEVEL_INDEX = 2;

	public const int OUTPUT_ENABLED_INDEX = 3;

	public const int OUTPUT_EXPEDITION_EVENT_INDEX = 4;

	public const int OUTPUT_CHANCE_ITEMS_INDEX = 5;

	public const int OUTPUT_CHANCE_RESOURCES_INDEX = 6;

	public const int OUTPUT_THREATS_INDEX = 7;

	public const int OUTPUT_ICON_INDEX = 8;

	[SerializeField]
	private ExpeditionDestination _destination;

	public override string GetID => "EE_BreakExpeditionDestinationNode";

	public override Node Create(Vector2 pos)
	{
		BreakExpeditionDestinationNode breakExpeditionDestinationNode = ScriptableObject.CreateInstance<BreakExpeditionDestinationNode>();
		breakExpeditionDestinationNode.rect = new Rect(pos.x, pos.y, 180f, 90f);
		breakExpeditionDestinationNode.name = "Break Destination";
		breakExpeditionDestinationNode.CreateInput("Destination", "ExpeditionDestination");
		breakExpeditionDestinationNode.CreateOutput("Name", "LocalizedString");
		breakExpeditionDestinationNode.CreateOutput("Description", "LocalizedString");
		breakExpeditionDestinationNode.CreateOutput("Required System Level", "Int");
		breakExpeditionDestinationNode.CreateOutput("Enabled", "Bool");
		breakExpeditionDestinationNode.CreateOutput("Expedition Event", "ParsecsEvent");
		breakExpeditionDestinationNode.CreateOutput("Chance For Items", "ItemList");
		breakExpeditionDestinationNode.CreateOutput("Chance For Resources", "ResourcesList");
		breakExpeditionDestinationNode.CreateOutput("Threats", "ExpeditionThreatList");
		breakExpeditionDestinationNode.CreateOutput("Icon", "Sprite");
		return breakExpeditionDestinationNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		BreakExpeditionDestinationNode breakExpeditionDestinationNode = (BreakExpeditionDestinationNode)Create(rect.position + new Vector2(20f, 20f));
		breakExpeditionDestinationNode._destination = _destination;
		return breakExpeditionDestinationNode;
	}

	protected override void NodeEnable()
	{
	}

	protected override void NodeGUI()
	{
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		return output switch
		{
			0 => CastValue<T>(_destination.Name), 
			1 => CastValue<T>(_destination.Description), 
			2 => CastValue<T>(_destination.RequiredSystemLevel), 
			3 => CastValue<T>(_destination.Enabled), 
			4 => CastValue<T>(_destination.Event), 
			5 => CastValue<T>(new List<IItem>(_destination.DynamicData.Items)), 
			6 => CastValue<T>(new List<Resource>(_destination.ResourcesToFind)), 
			7 => CastValue<T>(_destination.Threats), 
			8 => CastValue<T>(_destination.DestinationSprite), 
			_ => throw new NotExistingOutputException(GetID, output), 
		};
	}
}
