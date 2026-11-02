using UnityEngine;

public class PickupItem_Networked_ChangeMesh : PickupItem_Networked
{
	[SerializeField]
	private MeshFilter meshFilter;

	[SerializeField]
	private Mesh defaultMesh;

	[SerializeField]
	private Mesh pickedUpMesh;

	[SerializeField]
	private Collider pickupCollider;

	private void OnValidate()
	{
		if (defaultMesh == null && meshFilter != null)
		{
			GrabDefaultMeshFromFilter();
		}
	}

	private void GrabDefaultMeshFromFilter()
	{
		if (meshFilter != null)
		{
			defaultMesh = meshFilter.sharedMesh;
		}
	}

	public override void HandleOnPickedUp()
	{
		base.HandleOnPickedUp();
		RefreshMesh();
	}

	public override void OnRestored()
	{
		base.OnRestored();
		RefreshMesh();
	}

	private void RefreshMesh()
	{
		if (!(gameObject == null))
		{
			if (meshFilter != null && pickedUpMesh != null)
			{
				Mesh sharedMesh = (gameObject.activeSelf ? defaultMesh : pickedUpMesh);
				meshFilter.sharedMesh = sharedMesh;
			}
			if (pickupCollider != null)
			{
				pickupCollider.enabled = gameObject.activeSelf;
			}
		}
	}
}
