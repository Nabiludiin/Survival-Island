using System.Collections.Generic;
using NodeEditorFramework;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[NodeCanvasType("Event Editor/Survival Event", "Create new Survival Event", "New Survival Event", "Create new Survival Event")]
[CreateAssetMenu(menuName = "60 Parsecs!/Event Editor/New Survival Event")]
public class SurvivalEvent : ParsecsEvent
{
	[SerializeField]
	[Tooltip("Starting State for this event.")]
	private EEventState _startState = EEventState.ACTIVE;

	[SerializeField]
	[Tooltip("End State for this event that will be set when event was finished.")]
	private EEventState _endState = EEventState.JUNKED;

	[SerializeField]
	[Tooltip("Tags for which event will be restricted")]
	private List<EventTag> _tags = new List<EventTag>();

	[SerializeField]
	[Tooltip("Condition event which verify if event can be drawn for current day")]
	private List<ConditionEvent> _conditionEvents;

	[SerializeField]
	private bool _wasEventExecuted;

	[SerializeField]
	private bool _wasEventSuccessful = true;

	public bool WasEventExecuted => _wasEventExecuted;

	public EEventState StartState => _startState;

	public EEventState EndState => _endState;

	public EEventState CurrentState { get; private set; }

	public List<EventTag> Tags => _tags;

	public bool WasEventSuccessful
	{
		get
		{
			return _wasEventSuccessful;
		}
		set
		{
			_wasEventSuccessful = value;
		}
	}

	public bool CanBeDrawn(bool isScheduled = false)
	{
		if (_conditionEvents != null && _conditionEvents.Count > 0)
		{
			bool flag = true;
			for (int i = 0; i < _conditionEvents.Count; i++)
			{
				_conditionEvents[i].IsEventScheduled = isScheduled;
				if (!(_conditionEvents[i] == null))
				{
					_conditionEvents[i].Run();
					flag &= _conditionEvents[i].Result;
					if (!flag)
					{
						break;
					}
				}
			}
			return flag;
		}
		return true;
	}

	private new void OnEnable()
	{
		base.OnEnable();
	}

	protected override void SetInitialPhase()
	{
		CurrentState = _endState;
		EventManager.UpdateEventInDecks(this);
		CurrentPhase = EParsecsEventPhase.PREPARE;
	}

	protected override void UpdateCurrentPhase()
	{
		switch (CurrentPhase)
		{
		case EParsecsEventPhase.PREPARE:
			CurrentPhase = EParsecsEventPhase.ACTION;
			break;
		case EParsecsEventPhase.ACTION:
			CurrentPhase = EParsecsEventPhase.REPORT;
			break;
		case EParsecsEventPhase.REPORT:
			if (CurrentState == EEventState.DISCARDED || CurrentState == EEventState.ACTIVE)
			{
				CurrentPhase = EParsecsEventPhase.READY;
			}
			else
			{
				CurrentPhase = EParsecsEventPhase.FINISHED;
			}
			break;
		}
	}

	protected override void Execute()
	{
		switch (CurrentPhase)
		{
		case EParsecsEventPhase.PREPARE:
			ExecuteNode("EE_OnPrepareNode");
			_wasEventExecuted = true;
			break;
		case EParsecsEventPhase.ACTION:
			ExecuteNode("EE_OnActionNode");
			break;
		case EParsecsEventPhase.REPORT:
			ExecuteNode("EE_OnReportNode");
			break;
		}
	}

	protected override void EndEvent()
	{
	}

	protected override void InfoGUI()
	{
	}

	public override string Serialize()
	{
		return JsonUtility.ToJson(new SurvivalCurrentStateWrapper
		{
			CurrentPhase = CurrentPhase,
			CurrentState = CurrentState,
			WasEventExecute = _wasEventExecuted,
			WasEventSuccessful = _wasEventSuccessful
		});
	}

	public override void Deserialize(string jsonData)
	{
		SurvivalCurrentStateWrapper survivalCurrentStateWrapper = JsonUtility.FromJson<SurvivalCurrentStateWrapper>(jsonData);
		CurrentState = survivalCurrentStateWrapper.CurrentState;
		CurrentPhase = survivalCurrentStateWrapper.CurrentPhase;
		if (_saveEvent.SaveVersion > 3)
		{
			_wasEventExecuted = survivalCurrentStateWrapper.WasEventExecute;
			_wasEventSuccessful = survivalCurrentStateWrapper.WasEventSuccessful;
		}
		else
		{
			_wasEventSuccessful = true;
			_wasEventExecuted = false;
		}
	}

	public override void ResetData()
	{
		CurrentPhase = EParsecsEventPhase.READY;
		CurrentState = _startState;
		_wasEventExecuted = false;
		_wasEventSuccessful = true;
	}
}
