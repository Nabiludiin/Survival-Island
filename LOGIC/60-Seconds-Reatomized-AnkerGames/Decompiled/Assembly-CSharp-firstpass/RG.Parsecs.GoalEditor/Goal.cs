using I2.Loc;
using NodeEditorFramework;
using RG.Core.SaveSystem;
using RG.Parsecs.Common;
using RG.Parsecs.EventEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.GoalEditor;

[NodeCanvasType("Goal Editor/New Goal", "Create new Goal", "New Goal", "Create new Goal")]
[CreateAssetMenu(menuName = "60 Parsecs!/Goal Editor/New Goal")]
public class Goal : NodeCanvas, ISaveable
{
	[SerializeField]
	private SaveEvent _saveEvent;

	private const string GOAL_PREFIX = "goal";

	[SerializeField]
	private bool _isVisible = true;

	[SerializeField]
	private bool _isGlobal;

	[SerializeField]
	private LocalizedString _goalName;

	[SerializeField]
	private LocalizedString _goalDescription;

	[SerializeField]
	private bool _isAchieved;

	[SerializeField]
	private EndGameStat _stat;

	[Tooltip("It allows to modify the goal name during the game. Set None if the goal name will be modify during the game")]
	[SerializeField]
	private TooltipTerm _tooltipTerm;

	[Tooltip("Set the priority of the goal. Goals are sorted descending and then evaluated from the highest priority to the lowest.")]
	[SerializeField]
	private int _priority;

	public int Order { get; set; }

	public int Priority => _priority;

	public bool IsVisible => _isVisible;

	public bool IsGlobalGoal => _isGlobal;

	public LocalizedString Name
	{
		get
		{
			if (_tooltipTerm != null && (string)_tooltipTerm.Term != null && !string.IsNullOrEmpty(_tooltipTerm.Term))
			{
				return _tooltipTerm.Term;
			}
			return _goalName;
		}
	}

	public LocalizedString Description => _goalDescription;

	public bool IsAchieved
	{
		get
		{
			return _isAchieved;
		}
		set
		{
			_isAchieved = value;
		}
	}

	public EndGameStat Stat => _stat;

	public string ID => Guid;

	public override string GuidPrefix => "goal";

	public void ExecuteStart()
	{
		ParsecsNode parsecsNode = FindNodeOfId("GE_StartNode");
		if (!(parsecsNode == null))
		{
			parsecsNode.ExecuteWithErrorHandling(this);
		}
	}

	public ParsecsNode FindNodeOfId(string id)
	{
		for (int i = 0; i < nodes.Count; i++)
		{
			if (nodes[i].GetID.Equals(id))
			{
				return (ParsecsNode)nodes[i];
			}
		}
		return null;
	}

	private void OnEnable()
	{
		Register();
	}

	private void OnDestroy()
	{
		Unregister();
	}

	public string Serialize()
	{
		return JsonUtility.ToJson(new BoolWrapper
		{
			Value = _isAchieved
		});
	}

	public void Deserialize(string jsonData)
	{
		_isAchieved = JsonUtility.FromJson<BoolWrapper>(jsonData).Value;
	}

	public void Register()
	{
		if (_saveEvent != null)
		{
			_saveEvent.RegisterListener(this);
			return;
		}
		Debug.LogWarningFormat("SaveEvent not set in Goal {0} with id {1}", name, ID);
	}

	public void Unregister()
	{
		if (_saveEvent != null)
		{
			_saveEvent.UnregisterListener(this);
			return;
		}
		Debug.LogWarningFormat("SaveEvent not set in Goal {0} with id {1}", name, ID);
	}

	public void ResetData()
	{
		_isAchieved = false;
	}
}
