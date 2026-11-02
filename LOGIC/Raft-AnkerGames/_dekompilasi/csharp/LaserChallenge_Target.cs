using UnityEngine;

public class LaserChallenge_Target : MonoBehaviour
{
	public int hp = 100;

	public ParticleSystem particlesHit;

	public QuestInteractable interactable;

	private bool isDestroyed;

	public void HitWithLaser(int damage = 1)
	{
		if (isDestroyed)
		{
			return;
		}
		hp -= damage;
		particlesHit.PlaySafe();
		if (hp <= 0)
		{
			if (Raft_Network.IsHost)
			{
				interactable.SendOnInteractNetwork(null, forceSuccess: true);
			}
			isDestroyed = true;
			particlesHit.StopSafe();
		}
	}
}
