using System;
using UnityEngine;

public class DamageBoxComponent : MonoBehaviour
{
	[SerializeField]
	private DamageBox damageBox;

	private void OnEnable()
	{
		if (this.damageBox != null)
		{
			DamageBox damageBox = this.damageBox;
			damageBox.OnDamage = (Action<Network_Player>)Delegate.Combine(damageBox.OnDamage, new Action<Network_Player>(OnDamageBoxDamagePlayer));
		}
	}

	private void OnDisable()
	{
		if (this.damageBox != null)
		{
			DamageBox damageBox = this.damageBox;
			damageBox.OnDamage = (Action<Network_Player>)Delegate.Remove(damageBox.OnDamage, new Action<Network_Player>(OnDamageBoxDamagePlayer));
		}
	}

	protected virtual void OnDamageBoxDamagePlayer(Network_Player player)
	{
	}
}
