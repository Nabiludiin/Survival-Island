using UnityEngine;

public class QuestInteractable_ObjectOfInterestSlot : QuestInteractable
{
	[SerializeField]
	private Transform objectOfInterestTracker;

	protected virtual void Update()
	{
		if (Raft_Network.IsHost && CurrentObjectStateIndex == 0 && objectOfInterestTracker.childCount > 0)
		{
			SendOnInteractNetwork(null, forceSuccess: true);
		}
	}
}
