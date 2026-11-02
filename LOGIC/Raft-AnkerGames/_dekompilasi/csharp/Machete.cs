using UnityEngine;

public class Machete : MeleeWeapon
{
	[SerializeField]
	private string macheteInteractTagName = "MacheteInteract";

	public override void OnMeleeStart()
	{
		base.OnMeleeStart();
	}

	public override void OnMeleeHit()
	{
		base.OnMeleeHit();
	}

	public override void OnMeleeWeaponSuccesfullRayHit(out RaycastHit hit)
	{
		base.OnMeleeWeaponSuccesfullRayHit(out hit);
		if (hit.collider != null)
		{
			CheckForMacheteInteractableViableTarget(hit);
		}
	}

	private void CheckForMacheteInteractableViableTarget(RaycastHit hit)
	{
		if (hit.transform.tag == macheteInteractTagName)
		{
			QuestEventBase componentInParent = hit.collider.transform.GetComponentInParent<QuestEventBase>();
			if (componentInParent != null)
			{
				MacheteInteractWithQuest(componentInParent);
			}
		}
	}

	private void MacheteInteractWithQuest(QuestEventBase qEntity)
	{
		playerNetwork.Inventory.RemoveDurabillityFromHotSlot();
		qEntity.SendOnInteractNetwork(playerNetwork);
	}
}
