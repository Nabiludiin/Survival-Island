using System;

[Serializable]
public class QuestInteractable_ComponentData_RagdollComponent : QuestInteractable_ComponentData
{
	public RagdollManager ragDollManager;

	public float ragdollForce;

	public override void OnFinishedInteraction(Network_Player player, bool atStart, bool successfull)
	{
		base.OnFinishedInteraction(player, atStart, successfull);
		if (ragDollManager != null)
		{
			ragDollManager.SetRagdollState(state: true);
			if (ragdollForce > 0f)
			{
				ragDollManager.AddForceToAllParts(ragdollForce);
			}
		}
	}
}
