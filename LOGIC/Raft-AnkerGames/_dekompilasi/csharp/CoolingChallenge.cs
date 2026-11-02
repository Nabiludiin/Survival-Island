using System;
using FMODUnity;
using TMPro;
using UnityEngine;

public class CoolingChallenge : MonoBehaviour
{
	[SerializeField]
	private QuestType questToCompleteOnDone = QuestType.None;

	[SerializeField]
	private QuestInteractable[] childSlots;

	[SerializeField]
	private GameObject[] objectsToDisableAfterChallenge;

	[SerializeField]
	private GameObject[] objectsToEnableAfterChallenge;

	[SerializeField]
	private Renderer leverRenderer;

	[SerializeField]
	private TextMeshPro screenTextMesh;

	[SerializeField]
	private string term_insertRods;

	[SerializeField]
	private string term_error;

	[SerializeField]
	private string term_valvesShut;

	[SerializeField]
	private string term_success;

	[SerializeField]
	private Color32 safeColor;

	[SerializeField]
	private Color32 errorColor;

	[SerializeField]
	private StudioEventEmitter emitter_alarm;

	[SerializeField]
	private TimerEventer alarmTimer;

	[SerializeField]
	private QuestInteractable coolingStationLever;

	[SerializeField]
	private StudioEventEmitter emitter_error;

	private bool challengeComplete;

	private Quest quest_rodsInserted;

	private Quest quest_leverPulled;

	private Quest quest_reactorComplete;

	private MaterialPropertyBlock matPropBlock;

	private void Awake()
	{
		for (int i = 0; i < childSlots.Length; i++)
		{
			QuestInteractable obj = childSlots[i];
			obj.OnInteract = (Action<int, bool, Network_Player, bool>)Delegate.Combine(obj.OnInteract, new Action<int, bool, Network_Player, bool>(OnChildInteract));
		}
		QuestInteractable questInteractable = coolingStationLever;
		questInteractable.OnInteract = (Action<int, bool, Network_Player, bool>)Delegate.Combine(questInteractable.OnInteract, new Action<int, bool, Network_Player, bool>(OnLeverInteract));
		quest_rodsInserted = QuestProgressTracker.GetQuestByType(QuestType.TemperanceCoolingRodsInserted);
		Quest quest = quest_rodsInserted;
		quest.OnQuestComplete = (Action)Delegate.Combine(quest.OnQuestComplete, new Action(RefreshScreenTextDependingOnQuests));
		quest_leverPulled = QuestProgressTracker.GetQuestByType(QuestType.TemperanceCoolingLeverPulled);
		Quest quest2 = quest_leverPulled;
		quest2.OnQuestComplete = (Action)Delegate.Combine(quest2.OnQuestComplete, new Action(RefreshScreenTextDependingOnQuests));
		quest_reactorComplete = QuestProgressTracker.GetQuestByType(QuestType.TemperanceReactorComplete);
		Quest quest3 = quest_reactorComplete;
		quest3.OnQuestComplete = (Action)Delegate.Combine(quest3.OnQuestComplete, new Action(RefreshScreenTextDependingOnQuests));
		if (Raft_Network.IsHost)
		{
			SaveAndLoad.LoadComplete = (Action)Delegate.Combine(SaveAndLoad.LoadComplete, new Action(UpdateChallengeState));
		}
		else
		{
			Raft_Network.OnWorldReceivedLate = (Action)Delegate.Combine(Raft_Network.OnWorldReceivedLate, new Action(UpdateChallengeState));
		}
		UpdateChallengeState();
	}

	private void OnDestroy()
	{
		SaveAndLoad.LoadComplete = (Action)Delegate.Remove(SaveAndLoad.LoadComplete, new Action(UpdateChallengeState));
		Raft_Network.OnWorldReceivedLate = (Action)Delegate.Remove(Raft_Network.OnWorldReceivedLate, new Action(UpdateChallengeState));
	}

	private void OnEnable()
	{
		UpdateChallengeState();
	}

	private void Update()
	{
		if (emitter_alarm != null && quest_reactorComplete != null && !quest_reactorComplete.isQuestComplete && alarmTimer.DoUpdate(Time.deltaTime, autoResetTimer: true))
		{
			emitter_alarm.Play();
		}
	}

	private void OnChildInteract(int objectStateIndex, bool atStart, Network_Player player, bool success)
	{
		UpdateChallengeState();
		if (challengeComplete && !atStart)
		{
			QuestProgressTracker.FinishQuestRPC(questToCompleteOnDone);
		}
	}

	public RGD_LandmarkItem GetRGDSave()
	{
		return null;
	}

	private void SetScreenText(string term)
	{
		if (screenTextMesh != null)
		{
			screenTextMesh.text = Helper.GetTerm(term);
		}
	}

	private void SetScreenTextColor(Color32 color)
	{
		if (screenTextMesh != null)
		{
			screenTextMesh.color = color;
		}
	}

	private void SetLeverShimmer(bool shimmer)
	{
		if (matPropBlock == null)
		{
			matPropBlock = new MaterialPropertyBlock();
		}
		float value = (shimmer ? 1f : 0f);
		leverRenderer.GetPropertyBlock(matPropBlock);
		matPropBlock.SetFloat("_Shimmer", value);
		leverRenderer.SetPropertyBlock(matPropBlock);
	}

	private void UpdateChallengeState()
	{
		if (AllChildrenAtMaxStateIndex())
		{
			challengeComplete = true;
			SetLeverShimmer(!QuestProgressTracker.HasFinishedQuest(QuestType.TemperanceCoolingLeverPulled));
		}
		RefreshScreenTextDependingOnQuests();
	}

	private void RefreshScreenTextDependingOnQuests()
	{
		if (quest_reactorComplete.isQuestComplete)
		{
			SetScreenText(term_success);
			SetScreenTextColor(safeColor);
			DisableRadiationZones();
		}
		else if (quest_leverPulled.isQuestComplete)
		{
			SetScreenText(term_error);
			SetScreenTextColor(errorColor);
			SetLeverShimmer(shimmer: false);
		}
		else if (quest_rodsInserted.isQuestComplete)
		{
			SetScreenText(term_valvesShut);
			SetScreenTextColor(safeColor);
		}
		else
		{
			SetScreenText(term_insertRods);
			SetScreenTextColor(errorColor);
		}
	}

	private bool AllChildrenAtMaxStateIndex()
	{
		QuestInteractable[] array = childSlots;
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i].IsAtMaxStateIndex)
			{
				return false;
			}
		}
		return true;
	}

	private void DisableRadiationZones()
	{
		Invoke("DisableRadiation", 3.5f);
	}

	private void DisableRadiation()
	{
		GameObject[] array = objectsToDisableAfterChallenge;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActiveSafe(value: false);
		}
		array = objectsToEnableAfterChallenge;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActiveSafe(value: true);
		}
	}

	private void OnLeverInteract(int id, bool atStart, Network_Player player, bool success)
	{
		if (success && id == 1 && atStart && emitter_error.gameObject.activeInHierarchy)
		{
			emitter_error.PlaySafe();
		}
	}
}
