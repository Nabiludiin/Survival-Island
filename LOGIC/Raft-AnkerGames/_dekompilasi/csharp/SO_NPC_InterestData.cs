using FMODUnity;
using UnityEngine;

[CreateAssetMenu(fileName = "NPC_InterestData", menuName = "Raft/NPC/NPC_InterestData")]
public class SO_NPC_InterestData : ScriptableObject
{
	[SerializeField]
	public bool allowTalkingAnimator = true;

	[SerializeField]
	public float interactDuration = 5f;

	[SerializeField]
	public float endAnimationDelay;

	[SerializeField]
	public string handItemName = string.Empty;

	[SerializeField]
	public float handItemStartDelay;

	[SerializeField]
	public AnimatorPropertySetting[] onReachAnimatorPropertySettings;

	public EventReference er_startInteract;

	public void ApplyAnimationSettings(AnimatorController animatorController, AIStateEvent eventType)
	{
		if (animatorController == null || onReachAnimatorPropertySettings == null || onReachAnimatorPropertySettings.Length == 0)
		{
			return;
		}
		AnimatorPropertySetting[] array = onReachAnimatorPropertySettings;
		foreach (AnimatorPropertySetting animatorPropertySetting in array)
		{
			if (animatorPropertySetting.eventType == eventType)
			{
				switch (animatorPropertySetting.propertyType)
				{
				case AnimatorPropertyType.Int:
					animatorController.SetInt(animatorPropertySetting.parameterName, animatorPropertySetting.intValue);
					break;
				case AnimatorPropertyType.Float:
					animatorController.SetFloat(animatorPropertySetting.parameterName, animatorPropertySetting.floatValue);
					break;
				case AnimatorPropertyType.Bool:
					animatorController.SetBool(animatorPropertySetting.parameterName, animatorPropertySetting.boolValue);
					break;
				case AnimatorPropertyType.Trigger:
					animatorController.SetTrigger(animatorPropertySetting.parameterName);
					break;
				}
			}
		}
	}
}
