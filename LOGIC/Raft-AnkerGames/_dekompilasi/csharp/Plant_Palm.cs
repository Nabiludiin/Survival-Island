using UnityEngine;

public class Plant_Palm : Plant
{
	[Header("Palm settings")]
	[SerializeField]
	private HarvestableTree tree;

	public Collider[] treeColliders;

	public override void Start()
	{
		base.Start();
		tree.OnTreeHarvestEvent += OnTreeHarvest;
	}

	public override void Complete()
	{
		base.Complete();
		tree.PickupItem.canBePickedUp = true;
		Collider[] array = treeColliders;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].transform.tag = "Tree";
		}
	}

	private void OnTreeHarvest()
	{
		if (tree.Depleted)
		{
			PullRoots();
			PickupObjectManager.RemovePickupItem(pickupComponent.networkID);
		}
	}
}
