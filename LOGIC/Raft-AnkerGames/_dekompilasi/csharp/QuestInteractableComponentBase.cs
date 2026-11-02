using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestInteractableComponentBase<T> : MonoBehaviour where T : QuestInteractable_ComponentData
{
	public List<T> dataComponents = new List<T>();

	[SerializeField]
	protected bool triggerInteractAtLoad = true;

	[SerializeField]
	protected QuestEventBase interactable;

	protected bool hasSubscribedToSucess;

	protected bool hasSubscribedToFail;

	protected virtual void Awake()
	{
		if (interactable == null)
		{
			interactable = GetComponent<QuestEventBase>();
		}
		if (interactable != null)
		{
			ListenToEventsWithTriggerSettings();
		}
		else
		{
			Debug.LogError("Interactable is null :(", gameObject);
		}
		foreach (T dataComponent in dataComponents)
		{
			if (dataComponent != null && dataComponent.voiceData != null)
			{
				dataComponent.delay = dataComponent.voiceData.totalVoiceLineDuration + 1f;
			}
		}
	}

	protected virtual void Start()
	{
	}

	private void OnDestroy()
	{
		if (interactable != null)
		{
			UnlistenToEventsWithTriggerSettings();
		}
		else
		{
			Debug.LogError("Interactable is null :(", gameObject);
		}
	}

	private void OnInteractEvent(int objectStateIndex, bool atStart, Network_Player player, bool successfull)
	{
		OnInteract(objectStateIndex, atStart, player, successfull);
	}

	protected virtual bool OnInteract(int objectStateIndex, bool atStart, Network_Player player, bool successfull)
	{
		bool flag = true;
		if (atStart)
		{
			flag = triggerInteractAtLoad;
		}
		if (flag && dataComponents.ContainsItems())
		{
			foreach (T dataComponent in dataComponents)
			{
				if (dataComponent != null && (objectStateIndex == dataComponent.interactionIndex || dataComponent.interactionIndex == -1) && IsComponentOfCorrectTriggerType(dataComponent, successfull))
				{
					StartInteractionWithData(dataComponent, atStart, player, successfull);
				}
			}
		}
		return flag;
	}

	protected virtual void StartInteractionWithData(T data, bool atStart, Network_Player player, bool successfull)
	{
		if (data != null)
		{
			if ((data.delay <= 0f) | atStart)
			{
				FinishInteractionWithData(data, player, atStart, successfull);
			}
			else
			{
				SingletonGeneric<GameManager>.Singleton.StartCoroutine(HandleDelayedDataInteraction(data, player, atStart, successfull));
			}
		}
	}

	private IEnumerator HandleDelayedDataInteraction(T data, Network_Player player, bool atStart, bool successfull)
	{
		if (data != null)
		{
			yield return new WaitForSeconds(data.delay);
			FinishInteractionWithData(data, player, atStart, successfull);
		}
	}

	protected virtual void FinishInteractionWithData(T data, Network_Player player, bool atStart, bool successfull)
	{
		data?.OnFinishedInteraction(player, atStart, successfull);
	}

	protected virtual void ListenToEventsWithTriggerSettings()
	{
		hasSubscribedToSucess = false;
		hasSubscribedToFail = false;
		for (int i = 0; i < dataComponents.Count; i++)
		{
			if (!hasSubscribedToSucess && dataComponents[i].triggeredAt == QuestInteractable_TriggeredAt.TRIGGER_SUCCESS)
			{
				SubscribeToSuccess();
			}
			if (!hasSubscribedToFail && dataComponents[i].triggeredAt == QuestInteractable_TriggeredAt.TRIGGER_FAIL)
			{
				SubscribeToFail();
			}
			if (hasSubscribedToSucess && hasSubscribedToFail)
			{
				break;
			}
		}
	}

	protected virtual bool IsComponentOfCorrectTriggerType(QuestInteractable_ComponentData data, bool interactSuccess)
	{
		switch (data.triggeredAt)
		{
		case QuestInteractable_TriggeredAt.TRIGGER_BOTH:
			return true;
		case QuestInteractable_TriggeredAt.TRIGGER_SUCCESS:
			if (interactSuccess)
			{
				return true;
			}
			break;
		case QuestInteractable_TriggeredAt.TRIGGER_FAIL:
			if (!interactSuccess)
			{
				return true;
			}
			break;
		}
		return false;
	}

	protected void SubscribeToSuccess()
	{
		QuestEventBase questEventBase = interactable;
		questEventBase.OnInteract = (Action<int, bool, Network_Player, bool>)Delegate.Combine(questEventBase.OnInteract, new Action<int, bool, Network_Player, bool>(OnInteractEvent));
		hasSubscribedToSucess = true;
	}

	protected void SubscribeToFail()
	{
		QuestEventBase questEventBase = interactable;
		questEventBase.OnInteractFail = (Action<int, bool, Network_Player, bool>)Delegate.Combine(questEventBase.OnInteractFail, new Action<int, bool, Network_Player, bool>(OnInteractEvent));
		hasSubscribedToFail = true;
	}

	protected void UnlistenToEventsWithTriggerSettings()
	{
		QuestEventBase questEventBase = interactable;
		questEventBase.OnInteract = (Action<int, bool, Network_Player, bool>)Delegate.Remove(questEventBase.OnInteract, new Action<int, bool, Network_Player, bool>(OnInteractEvent));
		QuestEventBase questEventBase2 = interactable;
		questEventBase2.OnInteractFail = (Action<int, bool, Network_Player, bool>)Delegate.Remove(questEventBase2.OnInteractFail, new Action<int, bool, Network_Player, bool>(OnInteractEvent));
	}
}
