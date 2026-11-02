using System.Collections.Generic;
using UnityEngine;

public class OccupyingComponent : MonoBehaviour
{
	public Renderer[] renderers;

	private AdvancedCollision[] advancedCollisions;

	[HideInInspector]
	public List<BoxCollider> allAdvBoxColliders = new List<BoxCollider>();

	private MaterialRendConnection[] materialConnections;

	private void Awake()
	{
		FindRenderers();
		advancedCollisions = transform.GetComponentsInChildren<AdvancedCollision>();
		if (renderers != null && renderers.Length != 0)
		{
			materialConnections = new MaterialRendConnection[renderers.Length];
			for (int i = 0; i < renderers.Length; i++)
			{
				materialConnections[i].renderer = renderers[i];
				materialConnections[i].originalMaterials = materialConnections[i].renderer.sharedMaterials;
			}
		}
	}

	private void Start()
	{
		AdvancedCollision[] array = advancedCollisions;
		foreach (AdvancedCollision advancedCollision in array)
		{
			allAdvBoxColliders.AddRange(advancedCollision.colliders);
		}
	}

	private void Button_FindRenderers()
	{
		FindRenderers();
	}

	public void RestoreToDefaultMaterial()
	{
		for (int i = 0; i < renderers.Length; i++)
		{
			MaterialRendConnection[] array = materialConnections;
			for (int j = 0; j < array.Length; j++)
			{
				MaterialRendConnection materialRendConnection = array[j];
				if (materialRendConnection.renderer == renderers[i])
				{
					materialRendConnection.RestoreMaterials();
					break;
				}
			}
		}
	}

	public void SetNewMaterial(Material material)
	{
		for (int i = 0; i < renderers.Length; i++)
		{
			MaterialRendConnection[] array = materialConnections;
			for (int j = 0; j < array.Length; j++)
			{
				MaterialRendConnection materialRendConnection = array[j];
				if (materialRendConnection.renderer == renderers[i])
				{
					materialRendConnection.SetMaterial(material);
					break;
				}
			}
		}
	}

	public OverlappType IsCollidingWithOtherBlocks(SO_BlockCollisionMask collisionMask, Block parentBlock)
	{
		AdvancedCollision[] array = advancedCollisions;
		for (int i = 0; i < array.Length; i++)
		{
			OverlappType overlappType = array[i].IsOverlapping(allAdvBoxColliders.ToArray(), collisionMask, parentBlock);
			if (overlappType != OverlappType.None)
			{
				return overlappType;
			}
		}
		return OverlappType.None;
	}

	public void FindRenderers()
	{
		List<Renderer> list = new List<Renderer>();
		list.AddRange(transform.GetComponentsInChildren<MeshRenderer>(includeInactive: true));
		list.AddRange(transform.GetComponentsInChildren<SkinnedMeshRenderer>(includeInactive: true));
		for (int i = 0; i < list.Count; i++)
		{
			Renderer renderer = list[i];
			if (renderer == null || renderer.CompareTag("OccupyingComponentExclude"))
			{
				list.RemoveAt(i);
				i--;
			}
		}
		renderers = list.ToArray();
	}
}
