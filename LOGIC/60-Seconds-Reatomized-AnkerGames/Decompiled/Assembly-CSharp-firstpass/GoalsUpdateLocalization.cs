using I2.Loc;
using UnityEngine;

[CreateAssetMenu(menuName = "60 Parsecs!/New Goals Update Localization")]
public class GoalsUpdateLocalization : ScriptableObject
{
	private const string GOAL_NAME_LOCALIZATION_VARIABLE = "GOAL_NAME";

	[SerializeField]
	private LocalizedString _goalAchievedTerm;

	[SerializeField]
	private LocalizedString _goalFailedTerm;

	[SerializeField]
	private LocalizedString _goalUpdateTerm;

	public LocalizedString GoalAchievedTerm => _goalAchievedTerm;

	public LocalizedString GoalFailedTerm => _goalFailedTerm;

	public LocalizedString GoalUpdateTerm => _goalUpdateTerm;
}
