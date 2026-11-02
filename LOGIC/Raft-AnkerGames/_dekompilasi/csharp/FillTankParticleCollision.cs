using UnityEngine;

public class FillTankParticleCollision : ParticleCollisionReciever
{
	public FillCounter fillCounter;

	public float fillAmount = 1f;

	public override void OnParticleCollision(GameObject other)
	{
		if (Raft_Network.IsHost)
		{
			fillCounter.IncrementNetworked(fillAmount);
		}
	}
}
