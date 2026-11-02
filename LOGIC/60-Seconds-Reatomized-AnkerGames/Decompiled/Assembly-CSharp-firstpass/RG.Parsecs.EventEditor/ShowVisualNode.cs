using System;
using FMODUnity;
using I2.Loc;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Utility Nodes/Show Visual Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas),
	typeof(Goal)
})]
public class ShowVisualNode : EventNode
{
	public const string ID = "EE_showVisualNode";

	private const int FLOW_INPUT_INDEX = 0;

	private const int FLOW_OUTPUT_INDEX = 0;

	private const int VISUALDATA_INPUT_INDEX = 1;

	private const int VISUAL_ID_INPUT_INDEX = 2;

	private const int SOUND_EVENT_INPUT_INDEX = 3;

	private const int TERM_INPUT_INDEX = 4;

	private const string NODE_NAME = "Show visual";

	private const string FLOW_OUTPUT_NAME = "Out";

	private const string FLOW_INPUT_NAME = "In";

	private const string VISUALDATA_INPUT_NAME = "Visual Data";

	private const string VISUALID_INPUT_NAME = "Visual Id to display";

	private const string SOUND_EVENT_INPUT_NAME = "Sound event to play on click";

	private const string TERM_INPUT_NAME = "Term to display in tooltip";

	[SerializeField]
	private VisualsData _visualsData;

	[SerializeField]
	private VisualId _visualId;

	[EventRef]
	[SerializeField]
	private string _soundEvent;

	[SerializeField]
	private LocalizedString _term;

	public override string GetID => "EE_showVisualNode";

	public override Node Create(Vector2 pos)
	{
		ShowVisualNode showVisualNode = ScriptableObject.CreateInstance<ShowVisualNode>();
		showVisualNode.rect = new Rect(pos.x, pos.y, 250f, 40f);
		showVisualNode.name = "Show visual";
		showVisualNode.CreateMutliInput("In", "Flow");
		showVisualNode.CreateInput("Visual Data", "VisualsData");
		showVisualNode.CreateInput("Visual Id to display", "VisualId");
		showVisualNode.CreateInput("Sound event to play on click", "String");
		showVisualNode.CreateInput("Term to display in tooltip", "LocalizedString");
		showVisualNode.CreateOutput("Out", "Flow");
		return showVisualNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		ShowVisualNode showVisualNode = (ShowVisualNode)Create(rect.position + new Vector2(20f, 20f));
		showVisualNode._visualsData = _visualsData;
		showVisualNode._visualId = _visualId;
		showVisualNode._soundEvent = _soundEvent;
		showVisualNode._term = _term;
		return showVisualNode;
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
		GetInputValue(Inputs[1], ref _visualsData, canvas);
		GetInputValue(Inputs[2], ref _visualId, canvas);
		GetInputValue(Inputs[3], ref _soundEvent, canvas);
		GetInputValue(Inputs[4], ref _term, canvas);
		_visualsData.RuntimeData.VisualToDisplay = _visualId;
		if (_visualsData.StaticData.SoundSlot != null && !string.IsNullOrEmpty(_soundEvent))
		{
			_visualsData.StaticData.SoundSlot.SoundEventName = _soundEvent;
		}
		if (_visualsData.StaticData.TooltipTerm != null && (string)_term != null && !_term.IsNullOrEmpty())
		{
			_visualsData.StaticData.TooltipTerm.Term = _term;
		}
		CheckAreAllFlowOutputsConnected();
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
