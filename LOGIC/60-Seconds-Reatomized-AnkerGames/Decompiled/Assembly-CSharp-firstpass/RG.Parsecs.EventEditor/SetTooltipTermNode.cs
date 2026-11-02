using System;
using I2.Loc;
using NodeEditorFramework;
using RG.Parsecs.Common;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Utility Nodes/Set TooltipTerm Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent)
})]
public class SetTooltipTermNode : EventNode
{
	public const string ID = "EE_setTooltipTermNode";

	private const int FLOW_INPUT_INDEX = 0;

	private const int OUTPUT_OUT_INDEX = 0;

	private const int TOOLTIP_TERM_INPUT_INDEX = 1;

	private const int VALUE_INPUT_INDEX = 2;

	private const string NODE_NAME = "Set tooltip term";

	private const string OUTPUT_NAME = "Out";

	private const string FLOW_INPUT_NAME = "In";

	private const string TOOLTIP_TERM_INPUT_NAME = "Tooltip term";

	private const string VALUE_INPUT_NAME = "Value to set";

	[SerializeField]
	private LocalizedString _valueToSet;

	[SerializeField]
	private TooltipTerm _tooltipTerm;

	public override string GetID => "EE_setTooltipTermNode";

	public override Node Create(Vector2 pos)
	{
		SetTooltipTermNode setTooltipTermNode = ScriptableObject.CreateInstance<SetTooltipTermNode>();
		setTooltipTermNode.rect = new Rect(pos.x, pos.y, 250f, 40f);
		setTooltipTermNode.name = "Set tooltip term";
		setTooltipTermNode.CreateMutliInput("In", "Flow");
		setTooltipTermNode.CreateInput("Tooltip term", "TooltipTerm");
		setTooltipTermNode.CreateInput("Value to set", "LocalizedString");
		setTooltipTermNode.CreateOutput("Out", "Flow");
		return setTooltipTermNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		SetTooltipTermNode setTooltipTermNode = (SetTooltipTermNode)Create(rect.position + new Vector2(20f, 20f));
		setTooltipTermNode._valueToSet = _valueToSet;
		setTooltipTermNode._tooltipTerm = _tooltipTerm;
		return setTooltipTermNode;
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
		GetInputValue(Inputs[1], ref _tooltipTerm, canvas);
		GetInputValue(Inputs[2], ref _valueToSet, canvas);
		_tooltipTerm.Term = _valueToSet;
		CheckAreAllFlowOutputsConnected();
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
