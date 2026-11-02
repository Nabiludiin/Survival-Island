using UnityEngine;

public class QuestInteractableComponent_Animation : QuestInteractableComponentBase<QuestInteractable_ComponentData_Animation>
{
	[SerializeField]
	public Animator animatorController;

	public bool alwaysSendObjectStateIndexToAnimator = true;

	public QuestInteractable_TriggeredAt alwaysSendObjectStateTriggeredAt;

	protected override void Awake()
	{
		base.Awake();
		if (animatorController == null)
		{
			animatorController = GetComponentInParent<Animator>();
		}
	}

	private void OnEnable()
	{
		OnInteract(interactable.CurrentObjectStateIndex, atStart: true, null, successfull: true);
	}

	protected override bool OnInteract(int objectStateIndex, bool atStart, Network_Player player, bool successfull)
	{
		if (base.OnInteract(objectStateIndex, atStart, player, successfull) && alwaysSendObjectStateIndexToAnimator && (alwaysSendObjectStateTriggeredAt == QuestInteractable_TriggeredAt.TRIGGER_BOTH || ((alwaysSendObjectStateTriggeredAt == QuestInteractable_TriggeredAt.TRIGGER_SUCCESS) & successfull) || (alwaysSendObjectStateTriggeredAt == QuestInteractable_TriggeredAt.TRIGGER_FAIL && !successfull)))
		{
			ApplyInteractIndexToAnimator(objectStateIndex);
			if (atStart)
			{
				SetAnimationToEnd();
			}
		}
		return true;
	}

	protected override void FinishInteractionWithData(QuestInteractable_ComponentData_Animation data, Network_Player player, bool atStart, bool successfull)
	{
		base.FinishInteractionWithData(data, player, atStart, successfull);
		AnimatorPropertySetting_Legit animationSettings = data.animationSettings;
		switch (animationSettings.propertyType)
		{
		case AnimatorPropertyType.Float:
			if (animationSettings.lerp)
			{
				if (data.lerpFloatRoutine != null)
				{
					SingletonGeneric<GameManager>.Singleton.StopCoroutine(data.lerpFloatRoutine);
					data.lerpFloatRoutine = null;
				}
				data.lerpFloatRoutine = SingletonGeneric<GameManager>.Singleton.StartCoroutine(animationSettings.LerpFloat(animatorController, animationSettings.parameterName, animationSettings.floatValue, animationSettings.lerpSpeed));
			}
			else
			{
				animatorController.SetFloat(animationSettings.parameterName, animationSettings.floatValue);
			}
			break;
		case AnimatorPropertyType.Bool:
			animatorController.SetBool(animationSettings.parameterName, animationSettings.boolValue);
			break;
		case AnimatorPropertyType.Trigger:
			animatorController.SetTrigger(animationSettings.parameterName);
			break;
		case AnimatorPropertyType.Int:
			animatorController.SetInteger(animationSettings.parameterName, animationSettings.intValue);
			break;
		}
		if (atStart)
		{
			SetAnimationToEnd();
		}
	}

	protected override void ListenToEventsWithTriggerSettings()
	{
		base.ListenToEventsWithTriggerSettings();
		if (alwaysSendObjectStateIndexToAnimator)
		{
			if ((alwaysSendObjectStateTriggeredAt == QuestInteractable_TriggeredAt.TRIGGER_SUCCESS || alwaysSendObjectStateTriggeredAt == QuestInteractable_TriggeredAt.TRIGGER_BOTH) && !hasSubscribedToSucess)
			{
				SubscribeToSuccess();
			}
			if ((alwaysSendObjectStateTriggeredAt == QuestInteractable_TriggeredAt.TRIGGER_FAIL || alwaysSendObjectStateTriggeredAt == QuestInteractable_TriggeredAt.TRIGGER_BOTH) && !hasSubscribedToFail)
			{
				SubscribeToFail();
			}
		}
	}

	private void ApplyInteractIndexToAnimator(int objectStateIndex)
	{
		if (animatorController != null)
		{
			animatorController.SetInteger("StateIndex", objectStateIndex);
		}
	}

	private void SetAnimationToEnd()
	{
		CancelInvoke("SetAnimationToEndDelayed");
		Invoke("SetAnimationToEndDelayed", 0.1f);
	}

	private void SetAnimationToEndDelayed()
	{
		animatorController.Play(0, 0, 1f);
	}
}
