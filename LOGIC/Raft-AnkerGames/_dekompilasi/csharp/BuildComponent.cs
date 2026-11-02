using UnityEngine;

public class BuildComponent : MonoBehaviour
{
	private BlockCreator blockCreator;

	private Network_Player playerNetwork;

	private void Start()
	{
		playerNetwork = GetComponentInParent<Network_Player>();
	}

	public void OnSelect()
	{
		if (playerNetwork == null)
		{
			playerNetwork = GetComponentInParent<Network_Player>();
		}
		if (blockCreator == null)
		{
			blockCreator = playerNetwork.BlockCreator;
		}
		if (playerNetwork.IsLocalPlayer)
		{
			if (!blockCreator.gameObject.activeInHierarchy)
			{
				blockCreator.gameObject.SetActive(value: true);
			}
			Item_Base currentItemInHand = playerNetwork.PlayerItemManager.useItemController.GetCurrentItemInHand();
			blockCreator.SetBlockTypeToBuild(currentItemInHand.UniqueName);
		}
	}

	public void OnDeSelect()
	{
		if (playerNetwork.IsLocalPlayer && blockCreator != null)
		{
			blockCreator.SetGhostBlockVisibility(visible: false);
			blockCreator.gameObject.SetActive(value: false);
		}
	}
}
