using System;
using NodeEditorFramework;
using RG.Parsecs.Common;
using RG.Parsecs.EventEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.GoalEditor;

[Node(false, "Global Stats/Expedition Count", new Type[]
{
	typeof(Goal),
	typeof(ConditionEvent)
})]
public class ExpeditionCountNode : GoalNode
{
	private string _ID = "GE_ExpeditionCountNode";

	[SerializeField]
	private EExpeditionCountFilterType _expeditionFilter;

	private const int OUTPUT_INDEX = 0;

	private const string OUTPUT_NAME = "Output";

	private const string NODE_NAME = "Expedition Count";

	private int _returnedValue;

	public override string GetID => _ID;

	public override Node Create(Vector2 pos)
	{
		ExpeditionCountNode expeditionCountNode = ScriptableObject.CreateInstance<ExpeditionCountNode>();
		expeditionCountNode.rect = new Rect(pos.x, pos.y, 150f, 75f);
		expeditionCountNode.name = "Expedition Count";
		expeditionCountNode.CreateOutput("Output", "Int");
		return expeditionCountNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		ExpeditionCountNode expeditionCountNode = (ExpeditionCountNode)Create(rect.position + new Vector2(20f, 20f));
		expeditionCountNode._returnedValue = _returnedValue;
		expeditionCountNode._expeditionFilter = _expeditionFilter;
		return expeditionCountNode;
	}

	protected override void NodeEnable()
	{
	}

	protected override void NodeGUI()
	{
	}

	private void UpdateResults()
	{
		if (_expeditionFilter == EExpeditionCountFilterType.ALL)
		{
			_returnedValue = StatsManager.Instance.GetGlobalData("ExpeditionsMade");
		}
		else if (_expeditionFilter == EExpeditionCountFilterType.SUCCESSFUL)
		{
			_returnedValue = StatsManager.Instance.GetGlobalData("ExpeditionsSuccessful");
		}
		else
		{
			_returnedValue = StatsManager.Instance.GetGlobalData("ExpeditionsMade") - StatsManager.Instance.GetGlobalData("ExpeditionsSuccessful");
		}
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		UpdateResults();
		if (output != 0)
		{
			throw new NotExistingOutputException(GetID, output);
		}
		if ((T)(object)_returnedValue == null)
		{
			throw new MismatchedOutputTypeException(_returnedValue.GetType(), typeof(T));
		}
		return (T)(object)_returnedValue;
	}
}
