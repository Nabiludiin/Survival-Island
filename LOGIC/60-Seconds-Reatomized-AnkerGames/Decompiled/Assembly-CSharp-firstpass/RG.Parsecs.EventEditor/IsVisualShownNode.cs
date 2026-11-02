using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Utility Nodes/Is Visual Shown Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas),
	typeof(Goal),
	typeof(ConditionEvent)
})]
public class IsVisualShownNode : EventNode
{
	public const string ID = "EE_isVisualShownNode";

	private const int VISUALDATA_INPUT_INDEX = 0;

	private const int VISUAL_ID_INPUT_INDEX = 1;

	private const int IS_SHOWN_OUTPUT_INDEX = 0;

	private const string NODE_NAME = "Is visual shown";

	private const string VISUALDATA_INPUT_NAME = "Visual Data";

	private const string VISUALID_INPUT_NAME = "Visual Id";

	private const string IS_SHOWN_OUTPUT_NAME = "Is Shown";

	[SerializeField]
	private VisualsData _visualsData;

	[SerializeField]
	private VisualId _visualId;

	public override string GetID => "EE_isVisualShownNode";

	public override Node Create(Vector2 pos)
	{
		IsVisualShownNode isVisualShownNode = ScriptableObject.CreateInstance<IsVisualShownNode>();
		isVisualShownNode.rect = new Rect(pos.x, pos.y, 250f, 40f);
		isVisualShownNode.name = "Is visual shown";
		isVisualShownNode.CreateInput("Visual Data", "VisualsData");
		isVisualShownNode.CreateInput("Visual Id", "VisualId");
		isVisualShownNode.CreateOutput("Is Shown", "Bool");
		return isVisualShownNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		IsVisualShownNode isVisualShownNode = (IsVisualShownNode)Create(rect.position + new Vector2(20f, 20f));
		isVisualShownNode._visualsData = _visualsData;
		isVisualShownNode._visualId = _visualId;
		return isVisualShownNode;
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
		if (_visualId == null)
		{
			return CastValue<T>(_visualsData.RuntimeData.VisualToDisplay != null);
		}
		return CastValue<T>(_visualsData.RuntimeData.VisualToDisplay == _visualId);
	}
}
