using System;
using NodeEditorFramework;
using RG.Parsecs.EventEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

[Node(false, "Event Nodes/Skip days", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(ExpeditionEvent)
})]
public class SkipDaysNode : EventNode
{
	public const string ID = "EE_SkipDaysNode";

	private const string NODE_NAME = "Skip days";

	private const string INPUT_FLOW_NAME = "In";

	private const string INPUT_DAYS_NAME = "Days To Skip";

	private const string OUTPUT_FLOW_NAME = "Out";

	private const int INPUT_FLOW_INDEX = 0;

	private const int INPUT_DAYS_INDEX = 1;

	private const int OUTPUT_FLOW_INDEX = 0;

	[SerializeField]
	private int _daysToSkip = 2;

	private const int MINIMAL_DAYS_TO_SKIP = 2;

	public override string GetID => "EE_SkipDaysNode";

	public override Node Create(Vector2 pos)
	{
		SkipDaysNode skipDaysNode = ScriptableObject.CreateInstance<SkipDaysNode>();
		skipDaysNode.rect = new Rect(pos.x, pos.y, 100f, 100f);
		skipDaysNode.name = "Skip days";
		skipDaysNode.CreateMutliInput("In", "Flow");
		skipDaysNode.CreateInput("Days To Skip", "Int");
		skipDaysNode.CreateOutput("Out", "Flow");
		return skipDaysNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		SkipDaysNode skipDaysNode = (SkipDaysNode)Create(rect.position + new Vector2(20f, 20f));
		skipDaysNode._daysToSkip = _daysToSkip;
		return skipDaysNode;
	}

	protected override void NodeEnable()
	{
	}

	protected override void NodeGUI()
	{
	}

	public override void Execute(NodeCanvas canvas)
	{
		GetInputValue(Inputs[1], ref _daysToSkip, canvas);
		SchedulerManager.Instance.SkipDays(_daysToSkip);
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
