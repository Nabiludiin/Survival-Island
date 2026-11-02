using System.Collections.Generic;
using UnityEngine;

public class ColliderPrefabEnabler : MonoBehaviour
{
	[SerializeField]
	private SphereCollider collider;

	[SerializeField]
	private float sphereFirstpersonRadius = 3.5f;

	[SerializeField]
	private float sphereThirdpersonRadius = 10f;

	private List<Block> attachedColliderBlocks = new List<Block>();

	private void Awake()
	{
		ComponentManager<ColliderPrefabEnabler>.Value = this;
	}

	public void SetColliderSizeFromThirdpersonState(bool isInThirdperson)
	{
		if (isInThirdperson)
		{
			collider.radius = sphereThirdpersonRadius;
		}
		else
		{
			collider.radius = sphereFirstpersonRadius;
		}
	}

	public void HideCollider()
	{
		if (collider != null)
		{
			collider.enabled = false;
		}
		DisableAllActiveColliderPrefabs();
	}

	public void ShowCollider()
	{
		if (!collider.enabled)
		{
			collider.enabled = true;
		}
	}

	public void AttachColliderPrefab(Block block)
	{
		if (!(block != null) || !(block.activeColliderPrefab == null) || !(block.colliderPrefab != null))
		{
			return;
		}
		GameObject gameObject = Object.Instantiate(block.colliderPrefab);
		gameObject.SetActiveSafe(value: false);
		if (gameObject != null)
		{
			block.activeColliderPrefab = gameObject.transform;
			block.activeColliderPrefab.SetParent(block.transform);
			Transform activeColliderPrefab = block.activeColliderPrefab;
			Vector3 localPosition = (block.activeColliderPrefab.localEulerAngles = Vector3.zero);
			activeColliderPrefab.localPosition = localPosition;
			block.activeColliderPrefab.localScale = Vector3.one;
			block.activeColliderPrefab.gameObject.SetActive(value: true);
			if (block.stableComponent == null)
			{
				block.stableComponent = block.GetComponentInChildren<StableComponent>();
			}
			if (block.hasBeenPlaced && block.activeColliderPrefab.transform.childCount > 0)
			{
				for (int i = 0; i < block.activeColliderPrefab.transform.childCount; i++)
				{
					Transform child = block.activeColliderPrefab.transform.GetChild(i);
					if (child != null)
					{
						child.gameObject.SetActiveSafe(value: true);
					}
				}
			}
		}
		attachedColliderBlocks.Add(block);
	}

	public void DetachColliderPrefab(Block block)
	{
		if (block != null && block.activeColliderPrefab != null)
		{
			Object.Destroy(block.activeColliderPrefab.gameObject);
			block.activeColliderPrefab = null;
		}
	}

	private void DisableAllActiveColliderPrefabs()
	{
		foreach (Block attachedColliderBlock in attachedColliderBlocks)
		{
			DetachColliderPrefab(attachedColliderBlock);
		}
		attachedColliderBlocks.Clear();
	}

	private void OnTriggerStay(Collider other)
	{
		AttachColliderPrefab(other.transform.GetComponentInParent<Block>());
	}

	private void OnTriggerExit(Collider other)
	{
		DetachColliderPrefab(other.transform.GetComponentInParent<Block>());
	}
}
