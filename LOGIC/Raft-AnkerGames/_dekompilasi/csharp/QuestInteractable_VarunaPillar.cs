using UnityEngine;

public class QuestInteractable_VarunaPillar : QuestInteractable
{
	public int placeBarrelIndex = 1;

	public bool killsBoss;

	[SerializeField]
	private Transform midPointOfPillar;

	[SerializeField]
	private float knockbackRadius;

	[SerializeField]
	private float knockbackForce;

	protected override bool IsPlayerConsideredBusy => false;

	private void OnDrawGizmos()
	{
		if (midPointOfPillar != null)
		{
			Gizmos.DrawWireSphere(midPointOfPillar.position, knockbackRadius);
		}
	}

	public override bool HasInteractRequirement(Network_Player player, bool hasInventoryValidation = false)
	{
		if (CurrentObjectStateIndex != placeBarrelIndex || !IsPlayerHoldingBarrel(player))
		{
			return false;
		}
		return base.HasInteractRequirement(player, hasInventoryValidation);
	}

	public override void DisplayRequirementText(Network_Player player)
	{
		if (CurrentObjectStateIndex == placeBarrelIndex)
		{
			base.DisplayRequirementText(player);
		}
	}

	public override bool CanPlayerInteract(Network_Player player, bool overwriteInventoryValidation = false)
	{
		if (CurrentObjectStateIndex == placeBarrelIndex && IsPlayerHoldingBarrel(player))
		{
			return base.CanPlayerInteract(player, overwriteInventoryValidation);
		}
		return false;
	}

	public override void Interact(Network_Player player, bool successFull)
	{
		base.Interact(player, successFull);
		if (successFull && IsPlayerHoldingBarrel(player))
		{
			CarryNetworked component = player.CarryingComponent.CarriedObject.GetComponent<CarryNetworked>();
			component.OnPreDestroy();
			Object.Destroy(component.gameObject);
		}
		if (IsAtMaxStateIndex && Helper.LocalPlayerIsWithinDistance(midPointOfPillar.position, knockbackRadius))
		{
			Network_Player localPlayer = Network.GetLocalPlayer();
			if (localPlayer != null)
			{
				Vector3 vector = localPlayer.transform.position - midPointOfPillar.position;
				float num = 1f - Mathf.Clamp01(vector.magnitude / knockbackRadius);
				localPlayer.PersonController.SetExternalVelocity(vector.normalized * (knockbackForce * num));
			}
		}
	}

	public override string GetRequirementText()
	{
		return " " + Helper.GetTerm("Game/ExplosiveBarrel");
	}

	private bool IsPlayerHoldingBarrel(Network_Player player)
	{
		if (player != null && player.CarryingComponent.IsCarrying && player.CarryingComponent.CarriedObject.carryObjectType == CarryObjectType.VarunaBossBarrel)
		{
			return true;
		}
		return false;
	}
}
