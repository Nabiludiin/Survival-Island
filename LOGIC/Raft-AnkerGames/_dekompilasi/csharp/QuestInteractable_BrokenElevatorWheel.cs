using UnityEngine;

public class QuestInteractable_BrokenElevatorWheel : QuestInteractable_ObjectOfInterestSlot
{
	[SerializeField]
	private Elevator elevator;

	protected override void Update()
	{
		if (Raft_Network.IsHost)
		{
			base.Update();
		}
		if (elevator != null && IsAtMaxStateIndex)
		{
			elevator.isFunctioning = true;
		}
	}
}
