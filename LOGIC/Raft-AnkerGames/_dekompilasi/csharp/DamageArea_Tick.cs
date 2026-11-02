using UnityEngine;

public class DamageArea_Tick : DamageArea
{
	[SerializeField]
	private bool damageOnEnter = true;

	[SerializeField]
	private float tickInterval = 1f;

	[SerializeField]
	private float timer;

	protected override void Update()
	{
		base.Update();
		if (IsConsideredActive && Raft_Network.IsHost && playersInArea.ContainsItems())
		{
			timer += Time.deltaTime;
			if (timer >= tickInterval)
			{
				timer = 0f;
				Tick();
			}
		}
	}

	protected override void OnAddNewPlayer(Network_Player player)
	{
		if (damageOnEnter)
		{
			base.OnAddNewPlayer(player);
		}
	}

	protected override void OnRemovePlayer(Network_Player player)
	{
		base.OnRemovePlayer(player);
		if (playersInArea.Count == 0 && damageOnEnter)
		{
			timer = 0f;
		}
	}

	private void Tick()
	{
		DamageEntitiesInArea();
	}
}
