using System.Collections.Generic;
using UnityEngine;

public class FishingBaitHandler : MonoBehaviour
{
	public List<BaitConnection> baitConnections;

	[SerializeField]
	private Network_Player playerNetwork;

	[SerializeField]
	private RandomDropper defaultDropper;

	[SerializeField]
	private RandomDropper metalRodDropper;

	[SerializeField]
	private Mesh defaultBobberMesh;

	[SerializeField]
	private Material defaultBobberMaterial;

	private Item_Base currentBait;

	public Item_Base CurrentBait => currentBait;

	public void Start()
	{
		playerNetwork = GetComponentInParent<Network_Player>();
	}

	public void EquipBait(Item_Base baseItem)
	{
		currentBait = baseItem;
	}

	public void ConsumeBait()
	{
		if (currentBait != null)
		{
			playerNetwork.Inventory.RemoveItem(currentBait.name, 1);
		}
	}

	public void SetBaitModelFromCurrentBait(MeshRenderer renderer, MeshFilter filter)
	{
		Material bobberMaterial = defaultBobberMaterial;
		Mesh bobberMesh = defaultBobberMesh;
		if (currentBait != null)
		{
			foreach (BaitConnection baitConnection in baitConnections)
			{
				if (baitConnection.bait.UniqueIndex == currentBait.UniqueIndex && HasBaitOfCurrentType())
				{
					bobberMesh = baitConnection.bobberMesh;
					bobberMaterial = baitConnection.bobberMaterial;
					break;
				}
			}
		}
		renderer.sharedMaterial = bobberMaterial;
		filter.sharedMesh = bobberMesh;
	}

	public Item_Base GetRandomItemFromCurrentBaitPool(bool advancedRod = false)
	{
		if (currentBait == null)
		{
			if (advancedRod)
			{
				return metalRodDropper.GetRandomItem();
			}
			return defaultDropper.GetRandomItem();
		}
		foreach (BaitConnection baitConnection in baitConnections)
		{
			if (baitConnection.bait.UniqueIndex == currentBait.UniqueIndex)
			{
				return baitConnection.dropper.GetRandomItem();
			}
		}
		return defaultDropper.GetRandomItem();
	}

	public bool HasBaitOfCurrentType()
	{
		if (playerNetwork.Inventory.GetItemCount(currentBait) > 0)
		{
			return true;
		}
		return false;
	}
}
