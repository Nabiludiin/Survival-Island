using UnityEngine;

public class WaterValidTargetChecker : MonoBehaviour
{
	public float playerDistance = 30f;

	private void Start()
	{
	}

	private void Update()
	{
		Network_Player network_Player = Helper.ClosestPlayerInWaterToPoint(transform.position, playerDistance, includeDeadPlayers: false);
		if (network_Player != null && Helper.IsValidWaterTarget(network_Player))
		{
			transform.LookAt(network_Player.transform.position);
		}
	}
}
