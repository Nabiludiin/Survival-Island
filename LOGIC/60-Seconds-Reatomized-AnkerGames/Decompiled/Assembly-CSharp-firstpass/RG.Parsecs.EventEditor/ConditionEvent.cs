using NodeEditorFramework;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[NodeCanvasType("Event Editor/Condition Event", "Create new Condition Event", "New Condition Event", "Create new Condition Event")]
[CreateAssetMenu(menuName = "60 Parsecs!/Event Editor/New Condition Event")]
public class ConditionEvent : ParsecsEvent
{
	private bool _result = true;

	private bool _isEventScheduled;

	public bool IsEventScheduled
	{
		get
		{
			return _isEventScheduled;
		}
		set
		{
			_isEventScheduled = value;
		}
	}

	public bool Result
	{
		get
		{
			return _result;
		}
		set
		{
			_result = value;
		}
	}

	protected override void SetInitialPhase()
	{
		CurrentPhase = EParsecsEventPhase.READY;
	}

	protected override void UpdateCurrentPhase()
	{
		CurrentPhase = EParsecsEventPhase.READY;
	}

	protected override void Execute()
	{
		ExecuteNode("EE_ConditionEvaluationNode");
	}

	public override void ResetData()
	{
		CurrentPhase = EParsecsEventPhase.READY;
	}
}
