using NodeEditorFramework;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[NodeCanvasType("Event Editor/Expedition Event", "Create new Expedition Event", "New Expedition Event", "Create new Expedition Event")]
[CreateAssetMenu(menuName = "60 Parsecs!/Event Editor/New Expedition Event")]
public class ExpeditionEvent : ParsecsEvent
{
	[SerializeField]
	[Header("Expedition")]
	[Tooltip("Minimum amount of days character will spend on this expedition.")]
	private GlobalIntVariable _minimumDaysAway;

	[SerializeField]
	[Tooltip("Maximum amount of days character will spend on this expedition.")]
	private GlobalIntVariable _maximumDaysAway;

	private EExpeditionStatus _expeditionStatus = EExpeditionStatus.BEGIN;

	private int _expeditionLength;

	private bool _returnEvaluated;

	private bool _characterReturned;

	[SerializeField]
	private bool _wasEventExecuted;

	[SerializeField]
	private bool _wasEventSuccessful = true;

	public int MinimumDaysAway
	{
		get
		{
			if (_minimumDaysAway == null)
			{
				Debug.LogWarningFormat("Variable Minimum Days Away not set in event {0}. Falling back to {1}", name, 1);
				return 1;
			}
			if (_minimumDaysAway.Value < 0)
			{
				return 0;
			}
			return _minimumDaysAway.Value;
		}
	}

	public int MaximumDaysAway
	{
		get
		{
			if (_maximumDaysAway == null)
			{
				Debug.LogWarningFormat("Variable Maximum Days Away not set in event {0}. Falling back to {1}", name, 3);
				return 3;
			}
			if (_minimumDaysAway.Value < 0)
			{
				return 0;
			}
			return _maximumDaysAway.Value;
		}
	}

	public int ExpeditionLength
	{
		get
		{
			return _expeditionLength;
		}
		set
		{
			if (value < 0)
			{
				_expeditionLength = 0;
			}
			else
			{
				_expeditionLength = value;
			}
		}
	}

	public bool CharacterReturned
	{
		get
		{
			return _characterReturned;
		}
		set
		{
			_characterReturned = value;
		}
	}

	public bool ReturnEvaluated
	{
		get
		{
			return _returnEvaluated;
		}
		set
		{
			_returnEvaluated = value;
		}
	}

	public bool WasEventExecuted => _wasEventExecuted;

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

	protected override void SetInitialPhase()
	{
		CurrentPhase = EParsecsEventPhase.REPORT;
	}

	protected override void UpdateCurrentPhase()
	{
		if (_expeditionStatus == EExpeditionStatus.END || _expeditionLength == 0)
		{
			CurrentPhase = EParsecsEventPhase.READY;
		}
		else if (_expeditionStatus == EExpeditionStatus.BEGIN)
		{
			_expeditionStatus = EExpeditionStatus.END;
		}
	}

	protected override void BeginEvent()
	{
		base.BeginEvent();
		_expeditionStatus = EExpeditionStatus.BEGIN;
		_returnEvaluated = false;
	}

	protected override void Execute()
	{
		_wasEventExecuted = true;
		switch (_expeditionStatus)
		{
		case EExpeditionStatus.BEGIN:
			ExecuteNode("EE_OnExpeditionStartNode");
			if (_expeditionLength == 0)
			{
				EndExpedition();
			}
			break;
		case EExpeditionStatus.END:
			EndExpedition();
			break;
		}
	}

	private void EndExpedition()
	{
		CurrentPhase = EParsecsEventPhase.PREPARE;
		ExecuteNode("EE_OnEvaluateCharacterReturnNode");
		_wasEventSuccessful = _returnEvaluated;
		if (_returnEvaluated)
		{
			Debug.Log("Evaluated Return From Expedition");
		}
		else
		{
			Debug.Log("Not Evaluated Return From Expedition");
		}
		CurrentPhase = EParsecsEventPhase.REPORT;
		ExecuteNode("EE_OnExpeditionEndNode");
	}

	protected override void InfoGUI()
	{
	}

	public override string Serialize()
	{
		return JsonUtility.ToJson(new ExpeditionCurrentStateWrapper
		{
			CurrentState = _expeditionStatus,
			ExpeditionLength = _expeditionLength,
			CurrentPhase = CurrentPhase,
			WasEventExecuted = _wasEventExecuted,
			WasEventSuccessful = _wasEventSuccessful
		});
	}

	public override void Deserialize(string jsonData)
	{
		ExpeditionCurrentStateWrapper expeditionCurrentStateWrapper = JsonUtility.FromJson<ExpeditionCurrentStateWrapper>(jsonData);
		_expeditionStatus = expeditionCurrentStateWrapper.CurrentState;
		_expeditionLength = expeditionCurrentStateWrapper.ExpeditionLength;
		CurrentPhase = expeditionCurrentStateWrapper.CurrentPhase;
		_wasEventExecuted = expeditionCurrentStateWrapper.WasEventExecuted;
		_wasEventSuccessful = expeditionCurrentStateWrapper.WasEventSuccessful;
	}

	public override void ResetData()
	{
		_expeditionStatus = EExpeditionStatus.BEGIN;
		CurrentPhase = EParsecsEventPhase.READY;
		_wasEventExecuted = false;
		_wasEventSuccessful = false;
	}
}
