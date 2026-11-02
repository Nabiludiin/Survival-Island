using UnityEngine;

public class PickupItem_Networked_SeparateModel : PickupItem_Networked
{
	[SerializeField]
	private GameObject separateModel;

	public override void HandleOnPickedUp()
	{
		base.HandleOnPickedUp();
		bool value = gameObject != null && gameObject.activeSelf;
		switch (spawnType)
		{
		case ObjectSpawnType.GameObject:
			separateModel.SetActiveSafe(value);
			break;
		case ObjectSpawnType.Prefab:
			Object.Destroy(separateModel);
			break;
		}
	}

	public override void OnRestored()
	{
		base.OnRestored();
		if (spawnType == ObjectSpawnType.GameObject)
		{
			separateModel.SetActiveSafe(gameObject.activeSelf);
		}
	}
}
