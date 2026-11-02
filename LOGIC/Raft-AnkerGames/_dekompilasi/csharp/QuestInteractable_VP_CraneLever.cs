using FMODUnity;
using UnityEngine;

public class QuestInteractable_VP_CraneLever : QuestInteractable
{
	[SerializeField]
	private StudioEventEmitter emitter_craneMove;

	public EventReference er_sucess;

	public EventReference er_fail;

	public override bool CanPlayerInteract(Network_Player player, bool overwriteInventoryValidation = false)
	{
		if (base.CanPlayerInteract(player, overwriteInventoryValidation))
		{
			return QuestProgressTracker.HasFinishedQuest(QuestType.VarunaCraneKey);
		}
		return false;
	}

	public override void Interact(Network_Player player, bool successFull)
	{
		base.Interact(player, successFull);
		RuntimeManager.PlayOneShot(successFull ? er_sucess : er_fail, transform.position);
		if (successFull)
		{
			if (emitter_craneMove != null)
			{
				emitter_craneMove.PlaySafe();
			}
			else
			{
				Debug.Log("Emitter crane move is null");
			}
		}
	}
}
