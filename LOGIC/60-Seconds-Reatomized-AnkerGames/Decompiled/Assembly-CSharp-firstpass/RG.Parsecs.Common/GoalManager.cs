using System.Collections.Generic;
using RG.Core.SaveSystem;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.Common;

public class GoalManager : MonoBehaviour, ISaveable
{
	[SerializeField]
	private SaveEvent _saveEvent;

	private static GoalManager _instance;

	[SerializeField]
	private MissionManagerData _missionManagerData;

	[SerializeField]
	private GoalsGlowController _goalsGlowController;

	[SerializeField]
	private SurvivalData _survivalData;

	[SerializeField]
	private bool _shouldShowUpdateIntial = true;

	[SerializeField]
	private bool _checkIsCaptainAlive = true;

	private bool _shouldShowUpdate = true;

	private bool _isLoadedFromSave;

	protected bool _shouldGlow;

	private Goal _personalGoal;

	private List<Goal> _goalQueue;

	private int _counter;

	[SerializeField]
	private EndOfDayListenerList _endOfDay;

	[SerializeField]
	private List<Goal> _activeGoals;

	[SerializeField]
	private List<Goal> _achievedGoals;

	[SerializeField]
	private List<Goal> _failedGoals;

	[SerializeField]
	private GoalsUpdateLocalization _goalsUpdateLocalization;

	private const string GOAL_MANAGER_SAVE_ID = "MB_GoalManager";

	public static GoalManager Instance
	{
		get
		{
			if (_instance == null)
			{
				FindInstanceInScene();
			}
			return _instance;
		}
	}

	public List<Goal> ActiveGoals => _activeGoals;

	public List<Goal> AchievedGoals => _achievedGoals;

	public List<Goal> FailedGoals => _failedGoals;

	public string ID => "MB_GoalManager";

	private static void FindInstanceInScene()
	{
		_instance = Object.FindObjectOfType<GoalManager>();
		if (_instance == null)
		{
			Debug.LogError("No GoalManager in scene!");
		}
	}

	private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
		}
	}

	private void OnEnable()
	{
		if (_instance == this)
		{
			_endOfDay.RegisterOnEndOfDay(EvaluteGoals, "Goals", 20, this);
			Register();
		}
	}

	private void Start()
	{
		if (_isLoadedFromSave)
		{
			return;
		}
		_goalQueue = new List<Goal>();
		Goal personalGoal = CharacterManager.Instance.GetCharacterList().GetCaptain().StaticData.PersonalGoal;
		personalGoal.Order = _counter++;
		_instance.AddActiveGoal(personalGoal);
		_personalGoal = personalGoal;
		for (int i = 0; i < _missionManagerData.CurrentMission.Goals.Count; i++)
		{
			Goal goal = _missionManagerData.CurrentMission.Goals[i].Goal;
			if (!goal.IsAchieved)
			{
				_instance.AddActiveGoal(goal);
			}
		}
		for (int j = 0; j < _missionManagerData.CurrentMission.AdditionalGoals.Count; j++)
		{
			Goal goal2 = _missionManagerData.CurrentMission.AdditionalGoals[j];
			if (!goal2.IsAchieved)
			{
				_instance.AddActiveGoal(goal2);
			}
		}
	}

	private void OnDestroy()
	{
		Unregister();
		_endOfDay.UnregisterOnEndOfDay(EvaluteGoals, "Goals");
	}

	public void AddActiveGoal(Goal goal)
	{
		if (_activeGoals.Contains(goal))
		{
			return;
		}
		goal.Order = _counter++;
		_activeGoals.Add(goal);
		AddGoalToQueue(goal);
		if (goal.IsVisible && _shouldShowUpdate && (!_checkIsCaptainAlive || CharacterManager.Instance.GetCharacterList().GetCaptain().RuntimeData.IsAlive()))
		{
			_shouldShowUpdate = false;
			StartGlowing();
			if (_survivalData.CurrentDay > 1)
			{
				EventManager.DisplayGoalsUpdate(_goalsUpdateLocalization.GoalUpdateTerm, null);
			}
		}
	}

	private void AddGoalToQueue(Goal goal)
	{
		_goalQueue.Add(goal);
		_goalQueue.Sort((Goal g1, Goal g2) => g2.Priority - g1.Priority);
	}

	public void CheckPersonalGoal()
	{
		if (_personalGoal != null)
		{
			_personalGoal.ExecuteStart();
		}
	}

	public bool IsGoalAchieved(Goal goal)
	{
		return _achievedGoals.Contains(goal);
	}

	public bool IsGoalFailed(Goal goal)
	{
		return _failedGoals.Contains(goal);
	}

	private void EvaluteGoals()
	{
		_shouldShowUpdate = _shouldShowUpdateIntial;
		for (int num = _activeGoals.Count - 1; num >= 0; num--)
		{
			_goalQueue[num].ExecuteStart();
		}
		if (_goalsGlowController != null)
		{
			_shouldGlow = _goalsGlowController.Visible;
		}
	}

	public void RemoveActiveGoal(Goal goal)
	{
		if (goal.IsVisible && _shouldShowUpdate)
		{
			_shouldShowUpdate = false;
			StartGlowing();
			if (_survivalData.CurrentDay > 1)
			{
				EventManager.DisplayGoalsUpdate(_goalsUpdateLocalization.GoalUpdateTerm, null);
			}
		}
		_activeGoals.Remove(goal);
		_goalQueue.Remove(goal);
	}

	public virtual void MarkGoalAsAchieved(Goal goal)
	{
		goal.IsAchieved = true;
		_activeGoals.Remove(goal);
		_goalQueue.Remove(goal);
		if (_achievedGoals.Contains(goal))
		{
			return;
		}
		goal.Order = _achievedGoals.Count + 1;
		_achievedGoals.Add(goal);
		_achievedGoals.Sort((Goal g1, Goal g2) => g2.Order - g1.Order);
		if (goal.IsVisible && (!_checkIsCaptainAlive || CharacterManager.Instance.GetCharacterList().GetCaptain().RuntimeData.IsAlive()))
		{
			StartGlowing();
			if (_survivalData.CurrentDay > 1)
			{
				EventManager.DisplayGoalsUpdate(_goalsUpdateLocalization.GoalAchievedTerm, goal.Name);
			}
		}
	}

	public virtual void MarkGoalAsFailed(Goal goal)
	{
		_activeGoals.Remove(goal);
		_goalQueue.Remove(goal);
		if (_failedGoals.Contains(goal))
		{
			return;
		}
		goal.Order = _failedGoals.Count + 1;
		_failedGoals.Add(goal);
		_failedGoals.Sort((Goal g1, Goal g2) => g2.Order - g1.Order);
		if (goal.IsVisible && (!_checkIsCaptainAlive || CharacterManager.Instance.GetCharacterList().GetCaptain().RuntimeData.IsAlive()))
		{
			StartGlowing();
			if (_survivalData.CurrentDay > 1)
			{
				EventManager.DisplayGoalsUpdate(_goalsUpdateLocalization.GoalFailedTerm, goal.Name);
			}
		}
	}

	protected virtual void StartGlowing()
	{
		if (!(_goalsGlowController == null))
		{
			_goalsGlowController.Show();
			_shouldGlow = true;
		}
	}

	public string Serialize()
	{
		GoalManagerWrapper goalManagerWrapper = new GoalManagerWrapper
		{
			AchievedGoals = new List<GoalWrapper>(),
			ActiveGoals = new List<GoalWrapper>(),
			FailedGoals = new List<GoalWrapper>(),
			Counter = _counter
		};
		for (int i = 0; i < _achievedGoals.Count; i++)
		{
			GoalWrapper item = new GoalWrapper
			{
				Goal = _achievedGoals[i].ID,
				Order = _achievedGoals[i].Order
			};
			goalManagerWrapper.AchievedGoals.Add(item);
		}
		for (int j = 0; j < _activeGoals.Count; j++)
		{
			GoalWrapper item2 = new GoalWrapper
			{
				Goal = _activeGoals[j].ID,
				Order = _activeGoals[j].Order
			};
			goalManagerWrapper.ActiveGoals.Add(item2);
		}
		for (int k = 0; k < _failedGoals.Count; k++)
		{
			GoalWrapper item3 = new GoalWrapper
			{
				Goal = _failedGoals[k].ID,
				Order = _failedGoals[k].Order
			};
			goalManagerWrapper.FailedGoals.Add(item3);
		}
		goalManagerWrapper.ShouldGlow = _shouldGlow;
		return JsonUtility.ToJson(goalManagerWrapper);
	}

	public void Deserialize(string jsonData)
	{
		_isLoadedFromSave = true;
		GoalManagerWrapper goalManagerWrapper = JsonUtility.FromJson<GoalManagerWrapper>(jsonData);
		_achievedGoals.Clear();
		_activeGoals.Clear();
		_failedGoals.Clear();
		_goalQueue = new List<Goal>();
		_counter = goalManagerWrapper.Counter;
		for (int i = 0; i < goalManagerWrapper.AchievedGoals.Count; i++)
		{
			GoalWrapper goalWrapper = goalManagerWrapper.AchievedGoals[i];
			Goal goal = (Goal)_saveEvent.GetReferenceObjectByID(goalWrapper.Goal);
			goal.Order = goalWrapper.Order;
			_achievedGoals.Add(goal);
		}
		for (int j = 0; j < goalManagerWrapper.ActiveGoals.Count; j++)
		{
			GoalWrapper goalWrapper2 = goalManagerWrapper.ActiveGoals[j];
			Goal goal2 = (Goal)_saveEvent.GetReferenceObjectByID(goalWrapper2.Goal);
			goal2.Order = goalWrapper2.Order;
			_activeGoals.Add(goal2);
			AddGoalToQueue(goal2);
		}
		for (int k = 0; k < goalManagerWrapper.FailedGoals.Count; k++)
		{
			GoalWrapper goalWrapper3 = goalManagerWrapper.FailedGoals[k];
			Goal goal3 = (Goal)_saveEvent.GetReferenceObjectByID(goalWrapper3.Goal);
			goal3.Order = goalWrapper3.Order;
			_failedGoals.Add(goal3);
		}
		if (_saveEvent.SaveVersion > 1)
		{
			_shouldGlow = goalManagerWrapper.ShouldGlow;
			if (_shouldGlow)
			{
				StartGlowing();
			}
		}
	}

	public void Register()
	{
		_saveEvent.RegisterListener(this);
	}

	public void Unregister()
	{
		_saveEvent.UnregisterListener(this);
	}

	public void ResetData()
	{
		_achievedGoals.Clear();
		_activeGoals.Clear();
		_failedGoals.Clear();
	}
}
