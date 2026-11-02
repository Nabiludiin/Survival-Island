using UnityEngine;

public class Pickup_Specific : MonoBehaviour
{
	public virtual void PickupSpecific(PlayerInventory inventory)
	{
		_ = inventory == null;
	}
}
