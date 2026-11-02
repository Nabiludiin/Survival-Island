using System;
using UnityEngine;

[Serializable]
public class Object_Chicken
{
	[SerializeField]
	private PickupItem_Networked chickenEgg;

	public PickupItem_Networked LayEgg(Vector3 localRaftPosition, uint eggObjectIndex)
	{
		PickupItem_Networked pickupItem_Networked = UnityEngine.Object.Instantiate(chickenEgg);
		pickupItem_Networked.transform.SetParentSafe(SingletonGeneric<GameManager>.Singleton.lockedPivot);
		pickupItem_Networked.transform.localPosition = localRaftPosition;
		PickupItem_Networked component = pickupItem_Networked.GetComponent<PickupItem_Networked>();
		component.ObjectIndex = eggObjectIndex;
		NetworkIDManager.AddNetworkID(component, NetworkIDTag.ChickenEgg);
		return component;
	}
}
