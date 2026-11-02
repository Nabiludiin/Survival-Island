using UnityEngine;

public class AddBonusHealthComponent : MonoBehaviour
{
	[SerializeField]
	private float amount;

	private Network_Player playerNetwork;

	private void Start()
	{
		playerNetwork = GetComponentInParent<Network_Player>();
	}

	public void OnUse()
	{
		if (playerNetwork.IsLocalPlayer)
		{
			playerNetwork.Stats.stat_BonusHealth.Value += amount;
		}
	}
}
