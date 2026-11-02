using System;
using UnityEngine;

public class DamageArea_OnOff : MonoBehaviour
{
	public bool useWaterTimer = true;

	public DamageArea_DamageOnce[] areas;

	public Intervaller intervaller;

	private void Awake()
	{
		if (this.intervaller == null)
		{
			Debug.Log("Intervaller Is Null, assign it", gameObject);
			return;
		}
		Intervaller intervaller = this.intervaller;
		intervaller.OnIntervalActivate = (Action)Delegate.Combine(intervaller.OnIntervalActivate, new Action(Activate));
		Intervaller intervaller2 = this.intervaller;
		intervaller2.OnIntervalDeactivate = (Action)Delegate.Combine(intervaller2.OnIntervalDeactivate, new Action(Disable));
	}

	private void Activate()
	{
		SetAreasActive(value: true);
	}

	private void Disable()
	{
		SetAreasActive(value: false);
	}

	private void SetAreasActive(bool value)
	{
		DamageArea_DamageOnce[] array = areas;
		foreach (DamageArea_DamageOnce obj in array)
		{
			obj.SetAreaActive(value);
			obj.ClearEntitiesFromArea();
			obj.ClearAffectedEnemiesFromArea();
		}
	}
}
