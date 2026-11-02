using System;
using TMPro;
using UnityEngine;

public class BalboaRelayStationScreen : MonoBehaviour
{
	[SerializeField]
	private TextMeshPro frequencyText;

	[SerializeField]
	private TextMeshPro stationsActivatedText;

	[SerializeField]
	private GameObject edgeGlow;

	private void Awake()
	{
		QuestInteractable_Balboa_RelayStationButton.OnBalboaRelayStationButtonPressed = (Action)Delegate.Combine(QuestInteractable_Balboa_RelayStationButton.OnBalboaRelayStationButtonPressed, new Action(RefreshScreen));
		if (Raft_Network.IsHost)
		{
			SaveAndLoad.LoadComplete = (Action)Delegate.Combine(SaveAndLoad.LoadComplete, new Action(RefreshScreen));
		}
		else
		{
			Raft_Network.OnWorldReceivedLate = (Action)Delegate.Combine(Raft_Network.OnWorldReceivedLate, new Action(RefreshScreen));
		}
		RefreshScreen();
	}

	private void OnDestroy()
	{
		SaveAndLoad.LoadComplete = (Action)Delegate.Remove(SaveAndLoad.LoadComplete, new Action(RefreshScreen));
		Raft_Network.OnWorldReceivedLate = (Action)Delegate.Remove(Raft_Network.OnWorldReceivedLate, new Action(RefreshScreen));
	}

	private void RefreshScreen()
	{
		int activeStationCount = GetActiveStationCount();
		if (activeStationCount == 0)
		{
			frequencyText.gameObject.SetActiveSafe(value: false);
			stationsActivatedText.gameObject.SetActiveSafe(value: false);
		}
		else
		{
			bool flag = activeStationCount == 3;
			frequencyText.gameObject.SetActiveSafe(flag);
			stationsActivatedText.gameObject.SetActiveSafe(!flag);
			if (!flag)
			{
				LocalizationParameters.itemX = activeStationCount + "/3 ";
				stationsActivatedText.text = Helper.GetTerm("Game/Balboa/RelayStationsActive", applyParameters: true);
			}
			else
			{
				NoteBook value = ComponentManager<NoteBook>.Value;
				if (value != null)
				{
					value.UnlockSpecificNoteNetworked(902, showNotification: true);
				}
			}
		}
		edgeGlow.SetActiveSafe(activeStationCount > 0);
	}

	private int GetActiveStationCount()
	{
		int num = 0;
		if (QuestProgressTracker.HasFinishedQuest(QuestType.BalboaRelayStation1))
		{
			num++;
		}
		if (QuestProgressTracker.HasFinishedQuest(QuestType.BalboaRelayStation2))
		{
			num++;
		}
		if (QuestProgressTracker.HasFinishedQuest(QuestType.BalboaRelayStation3))
		{
			num++;
		}
		return num;
	}
}
