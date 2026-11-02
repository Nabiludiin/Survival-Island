using System;
using FMODUnity;
using UnityEngine;

public class BrunosToolBoard : MonoBehaviour
{
	[SerializeField]
	private QuestInteractable[] toolslots;

	[SerializeField]
	private Animator animator;

	[SerializeField]
	private GameObject toolboardNoShimmer;

	[SerializeField]
	private GameObject toolboardShimmer;

	public EventReference er_toolboardFall_sound;

	private bool toolboardComplete;

	public Collider hiddenNoteCollider;

	private void Awake()
	{
		for (int i = 0; i < toolslots.Length; i++)
		{
			QuestInteractable obj = toolslots[i];
			obj.OnInteract = (Action<int, bool, Network_Player, bool>)Delegate.Combine(obj.OnInteract, new Action<int, bool, Network_Player, bool>(OnToolSlotInteract));
		}
		if (Raft_Network.IsHost)
		{
			SaveAndLoad.LoadComplete = (Action)Delegate.Combine(SaveAndLoad.LoadComplete, new Action(UpdateBoardState));
		}
		else
		{
			Raft_Network.OnWorldReceivedLate = (Action)Delegate.Combine(Raft_Network.OnWorldReceivedLate, new Action(UpdateBoardState));
		}
		UpdateBoardState();
	}

	private void OnDestroy()
	{
		SaveAndLoad.LoadComplete = (Action)Delegate.Remove(SaveAndLoad.LoadComplete, new Action(UpdateBoardState));
		Raft_Network.OnWorldReceivedLate = (Action)Delegate.Remove(Raft_Network.OnWorldReceivedLate, new Action(UpdateBoardState));
	}

	private void OnEnable()
	{
		UpdateBoardState();
	}

	private void OnToolSlotInteract(int objectStateIndex, bool atStart, Network_Player player, bool success)
	{
		UpdateBoardState();
		if (toolboardComplete && !atStart)
		{
			AchievementHandler.UnlockAchievement(AchievementType.ach_brunosToolboard);
			PlayToolBoardFall();
		}
	}

	public void PlayToolBoardFall()
	{
		RuntimeManager.PlayOneShot(er_toolboardFall_sound, transform.position);
	}

	private void UpdateBoardState()
	{
		if (AllToolsPlaced())
		{
			toolboardComplete = true;
			animator.SetBool("ToolsPlaced", toolboardComplete);
		}
		hiddenNoteCollider.enabled = toolboardComplete;
		toolboardShimmer.SetActiveSafe(!toolboardComplete);
		toolboardNoShimmer.SetActiveSafe(toolboardComplete);
	}

	private bool AllToolsPlaced()
	{
		QuestInteractable[] array = toolslots;
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i].IsAtMaxStateIndex)
			{
				return false;
			}
		}
		return true;
	}
}
