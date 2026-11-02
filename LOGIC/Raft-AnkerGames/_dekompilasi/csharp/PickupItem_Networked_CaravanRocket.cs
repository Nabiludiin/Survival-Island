using UnityEngine;

public class PickupItem_Networked_CaravanRocket : PickupItem_Networked_SeparateModel
{
	[SerializeField]
	private GameObject ropeModel;

	public override void HandleOnPickedUp()
	{
		base.HandleOnPickedUp();
		bool value = gameObject != null && gameObject.activeSelf;
		if (spawnType == ObjectSpawnType.GameObject)
		{
			ropeModel.SetActiveSafe(value);
		}
	}

	public override void OnRestored()
	{
		base.OnRestored();
		if (spawnType == ObjectSpawnType.GameObject && !gameObject.activeSelf)
		{
			ropeModel.SetActiveSafe(value: false);
		}
	}
}
