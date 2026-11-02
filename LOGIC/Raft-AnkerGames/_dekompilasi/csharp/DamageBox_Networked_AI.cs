using UnityEngine;

public class DamageBox_Networked_AI : DamageBox_Networked
{
	[SerializeField]
	private AI_NetworkBehaviourType aiType = AI_NetworkBehaviourType.None;

	protected override void Start()
	{
		base.Start();
		if (aiType != AI_NetworkBehaviourType.None)
		{
			EntityVariables difficultyEntityVariableFromAINetworkBehaviourType = GameModeValueManager.GetDifficultyEntityVariableFromAINetworkBehaviourType(aiType);
			if (difficultyEntityVariableFromAINetworkBehaviourType != null)
			{
				SetDamageAmount(Mathf.CeilToInt((float)damage * difficultyEntityVariableFromAINetworkBehaviourType.damageMultiplier));
			}
		}
	}
}
