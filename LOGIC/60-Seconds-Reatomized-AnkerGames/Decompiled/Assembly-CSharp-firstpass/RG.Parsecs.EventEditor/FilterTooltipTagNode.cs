using System;
using NodeEditorFramework;
using RG.Parsecs.Common;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Utility Nodes/Filter TooltipTag Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent)
})]
public class FilterTooltipTagNode : EventNode
{
	public const string ID = "EE_filterTooltipTagNode";

	private const int FLOW_INPUT_INDEX = 0;

	private const int OUTPUT_OUT_INDEX = 0;

	private const int TOOLTIP_TERM_INPUT_INDEX = 1;

	private const int VALUE_INPUT_INDEX = 2;

	private const string NODE_NAME = "Filter tooltip tag";

	private const string OUTPUT_NAME = "Out";

	private const string FLOW_INPUT_NAME = "In";

	private const string TOOLTIP_TERM_INPUT_NAME = "Tooltip tag";

	private const string VALUE_INPUT_NAME = "Filter out tooltips with tag";

	[SerializeField]
	private bool _valueToSet;

	[SerializeField]
	private TooltipTag _tooltipTag;

	public override string GetID => "EE_filterTooltipTagNode";

	public override Node Create(Vector2 pos)
	{
		FilterTooltipTagNode filterTooltipTagNode = ScriptableObject.CreateInstance<FilterTooltipTagNode>();
		filterTooltipTagNode.rect = new Rect(pos.x, pos.y, 250f, 40f);
		filterTooltipTagNode.name = "Filter tooltip tag";
		filterTooltipTagNode.CreateMutliInput("In", "Flow");
		filterTooltipTagNode.CreateInput("Tooltip tag", "TooltipTag");
		filterTooltipTagNode.CreateInput("Filter out tooltips with tag", "Bool");
		filterTooltipTagNode.CreateOutput("Out", "Flow");
		return filterTooltipTagNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		FilterTooltipTagNode filterTooltipTagNode = (FilterTooltipTagNode)Create(rect.position + new Vector2(20f, 20f));
		filterTooltipTagNode._valueToSet = _valueToSet;
		filterTooltipTagNode._tooltipTag = _tooltipTag;
		return filterTooltipTagNode;
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

	public override void Execute(NodeCanvas canvas)
	{
		GetInputValue(Inputs[1], ref _tooltipTag, canvas);
		GetInputValue(Inputs[2], ref _valueToSet, canvas);
		if (_valueToSet)
		{
			if (TooltipTagManager.Instance.TagData != null && !TooltipTagManager.Instance.TagData.CurrentFilteredTags.Contains(_tooltipTag))
			{
				TooltipTagManager.Instance.TagData.AddTag(_tooltipTag);
			}
		}
		else if (TooltipTagManager.Instance.TagData != null && TooltipTagManager.Instance.TagData.CurrentFilteredTags.Contains(_tooltipTag))
		{
			TooltipTagManager.Instance.TagData.RemoveTag(_tooltipTag);
		}
		CheckAreAllFlowOutputsConnected();
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
