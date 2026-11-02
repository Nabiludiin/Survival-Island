using System;
using System.Collections.Generic;
using NodeEditorFramework;
using RG.Parsecs.Common;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Stats Nodes/Get Yes No Stats Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(Goal),
	typeof(ConditionEvent),
	typeof(ReportEvent)
})]
public class GetYesNoStatsNode : GoalNode
{
	public const string ID = "GE_GetYesNoStatsNode";

	private const string NODE_NAME = "Get Yes/No Stats";

	private const string OUTPUT_RESULT_NAME = "Result";

	private const string INPUT_YES_NO_DECISION_NAME = "Yes/No";

	private const int OUTPUT_RESULT_INDEX = 0;

	private const int INPUT_DECISION_INDEX = 0;

	[SerializeField]
	private bool _decision = true;

	public override string GetID => "GE_GetYesNoStatsNode";

	public override Node Create(Vector2 pos)
	{
		GetYesNoStatsNode getYesNoStatsNode = ScriptableObject.CreateInstance<GetYesNoStatsNode>();
		getYesNoStatsNode.name = "Get Yes/No Stats";
		getYesNoStatsNode.rect = new Rect(pos.x, pos.y, 200f, 100f);
		getYesNoStatsNode.CreateInput("Yes/No", "Bool");
		getYesNoStatsNode.CreateOutput("Result", "Int");
		return getYesNoStatsNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		GetYesNoStatsNode getYesNoStatsNode = (GetYesNoStatsNode)Create(rect.position + new Vector2(20f, 20f));
		getYesNoStatsNode._decision = _decision;
		return getYesNoStatsNode;
	}

	protected override void NodeEnable()
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
		GetInputValue(Inputs[0], ref _decision, canvas);
		List<YesNoDecisionStatsEntry> yesNoDecisionStatsEntries = StatsManager.Instance.YesNoDecisionStatsEntries;
		int num = 0;
		for (int i = 0; i < yesNoDecisionStatsEntries.Count; i++)
		{
			if (yesNoDecisionStatsEntries[i].YesDecision)
			{
				num++;
			}
		}
		if (_decision)
		{
			return CastValue<T>(num);
		}
		return CastValue<T>(yesNoDecisionStatsEntries.Count - num);
	}
}
