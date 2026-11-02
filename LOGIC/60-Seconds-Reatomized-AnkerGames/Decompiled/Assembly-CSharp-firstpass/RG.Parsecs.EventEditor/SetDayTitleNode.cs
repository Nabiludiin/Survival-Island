using System;
using I2.Loc;
using NodeEditorFramework;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Utility Nodes/Set Day Title Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent)
})]
public class SetDayTitleNode : EventNode
{
	public const string ID = "EE_setDayTitleNode";

	private const int FLOW_INPUT_INDEX = 0;

	private const int OUTPUT_OUT_INDEX = 0;

	private const int VALUE_INPUT_INDEX = 1;

	private const string NODE_NAME = "Set day title";

	private const string OUTPUT_NAME = "Out";

	private const string FLOW_INPUT_NAME = "In";

	private const string VALUE_INPUT_NAME = "Title";

	[SerializeField]
	private LocalizedString _valueToSet;

	public override string GetID => "EE_setDayTitleNode";

	public override Node Create(Vector2 pos)
	{
		SetDayTitleNode setDayTitleNode = ScriptableObject.CreateInstance<SetDayTitleNode>();
		setDayTitleNode.rect = new Rect(pos.x, pos.y, 250f, 40f);
		setDayTitleNode.name = "Set day title";
		setDayTitleNode.CreateMutliInput("In", "Flow");
		setDayTitleNode.CreateInput("Title", "LocalizedString");
		setDayTitleNode.CreateOutput("Out", "Flow");
		return setDayTitleNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		SetDayTitleNode setDayTitleNode = (SetDayTitleNode)Create(rect.position + new Vector2(20f, 20f));
		setDayTitleNode._valueToSet = _valueToSet;
		return setDayTitleNode;
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
		GetInputValue(Inputs[1], ref _valueToSet, canvas);
		EndOfDayManager.Instance.DayTitle = _valueToSet;
		CheckAreAllFlowOutputsConnected();
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
