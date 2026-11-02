using System;
using UnityEngine;

public class CarryingComponent : MonoBehaviour
{
	public Action<Carry> OnStartCarry;

	public Action<Carry> OnStopCarry;

	[SerializeField]
	private Carry carriedObject;

	public bool CanJumpWithCarriedItem
	{
		get
		{
			if (IsCarrying)
			{
				return CarriedObject.canJumpWhenCarried;
			}
			return true;
		}
	}

	public bool IsCarrying => carriedObject != null;

	public Carry CarriedObject
	{
		get
		{
			return carriedObject;
		}
		set
		{
			if (carriedObject != null && value == null)
			{
				OnStopCarry?.Invoke(carriedObject);
				carriedObject = value;
			}
			else if (carriedObject == null && value != null)
			{
				carriedObject = value;
				OnStartCarry?.Invoke(carriedObject);
			}
		}
	}
}
