using FMODUnity;
using UnityEngine;

public class ApplyBuffComponent : MonoBehaviour
{
	public SO_Buff buffEffect;

	public EventReference er_applyBuff;

	private Network_Player playerNetwork;

	private void Start()
	{
		playerNetwork = GetComponentInParent<Network_Player>();
	}

	public void OnUse()
	{
		if (playerNetwork.IsLocalPlayer && buffEffect != null)
		{
			playerNetwork.Stats.buffManager.AddBuff(buffEffect);
		}
		if (!er_applyBuff.IsNull)
		{
			RuntimeManager.PlayOneShot(er_applyBuff, transform.position);
		}
	}
}
