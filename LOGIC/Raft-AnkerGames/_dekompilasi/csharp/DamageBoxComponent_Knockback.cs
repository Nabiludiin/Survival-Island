using UnityEngine;

public class DamageBoxComponent_Knockback : DamageBoxComponent
{
	[SerializeField]
	private float knockbackMultiplier = 1f;

	[SerializeField]
	private float resetPlayerGroundedTimer = 0.5f;

	[SerializeField]
	private bool knockbackIsAdditive = true;

	[SerializeField]
	private bool directionIsRelativeToPlayer;

	[SerializeField]
	private Vector3 localKnockbackDirection = Vector3.zero;

	protected override void OnDamageBoxDamagePlayer(Network_Player player)
	{
		base.OnDamageBoxDamagePlayer(player);
		if (player.IsLocalPlayer)
		{
			Vector3 zero = Vector3.zero;
			Vector3 velocity = ((!directionIsRelativeToPlayer) ? transform.TransformDirection(localKnockbackDirection) : (player.transform.position - transform.position)).normalized * knockbackMultiplier;
			if (knockbackIsAdditive)
			{
				player.PersonController.AddExternalVelocity(velocity, resetPlayerGroundedTimer);
			}
			else
			{
				player.PersonController.SetExternalVelocity(velocity, resetPlayerGroundedTimer);
			}
		}
	}
}
