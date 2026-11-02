using System.Collections.Generic;
using RG.Parsecs.Common;
using RG.Parsecs.GoalEditor;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class GoalsPanelController : StarlogPanelController
{
	[SerializeField]
	[Tooltip("Transform that will be used as parent for current goals.")]
	private Transform _currentGoals;

	[SerializeField]
	[Tooltip("Transform that will be used as parent for personal goals.")]
	private Transform _personalGoal;

	[SerializeField]
	[Tooltip("Transform that will be used as parent for all goals.")]
	private GameObject _goalPrefab;

	private GoalManager _goalManager;

	private List<Goal> _goalsOnList;

	private List<GameObject> _gameObjectsGoalList;

	private void Awake()
	{
		if (_gameObjectsGoalList == null)
		{
			_gameObjectsGoalList = new List<GameObject>();
		}
		_goalManager = GoalManager.Instance;
		UpdateGoalsPanel();
	}

	public void UpdateGoalsPanel()
	{
		if (_goalManager == null)
		{
			_goalManager = GoalManager.Instance;
		}
		ClearContent();
		AddGoalList(_goalManager.ActiveGoals, SetGoalView.EGoalStatus.IN_PROGRESS);
		AddGoalList(_goalManager.AchievedGoals, SetGoalView.EGoalStatus.FINISHED);
		AddGoalList(_goalManager.FailedGoals, SetGoalView.EGoalStatus.FAILED);
	}

	private void ClearContent()
	{
		if (_gameObjectsGoalList == null)
		{
			_gameObjectsGoalList = new List<GameObject>();
		}
		for (int num = _gameObjectsGoalList.Count - 1; num >= 0; num--)
		{
			Object.Destroy(_gameObjectsGoalList[num]);
		}
	}

	private void AddGoalList(List<Goal> goals, SetGoalView.EGoalStatus status)
	{
		for (int i = 0; i < goals.Count; i++)
		{
			Goal goal = goals[i];
			if (goal.IsVisible)
			{
				Transform parent = ((!(goal == CharacterManager.Instance.GetCharacterList().GetCaptain().StaticData.PersonalGoal)) ? _currentGoals : _personalGoal);
				GameObject gameObject = Object.Instantiate(_goalPrefab);
				_gameObjectsGoalList.Add(gameObject);
				gameObject.transform.SetParent(parent, worldPositionStays: false);
				gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
				SetGoalView component = gameObject.GetComponent<SetGoalView>();
				if ((bool)goal)
				{
					string text = goal.Name;
					text += ((goal.Stat != null) ? $" ({goal.Stat.StatValue})" : "");
					component.SetGoalViewData(status, text);
				}
			}
		}
	}
}
