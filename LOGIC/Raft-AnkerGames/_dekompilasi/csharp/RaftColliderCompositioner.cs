using System.Collections.Generic;
using UnityEngine;

public class RaftColliderCompositioner : MonoBehaviour
{
	private RaftCollisionManager collMang;

	public GameObject external2DTransform;

	public GameObject external3DTransform;

	private CompositeCollider2D composite;

	private List<BoxCollider2D> latestShapeColliders = new List<BoxCollider2D>();

	public Vector2[] newPatherino;

	private void Start()
	{
		collMang = ComponentManager<RaftCollisionManager>.Value;
		composite = GetComponent<CompositeCollider2D>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.L))
		{
			Create2DCollider();
			UpdateColliderShape();
		}
	}

	private void UpdateColliderShape()
	{
		composite.GenerateGeometry();
	}

	private void RemoveOldColliders()
	{
		for (int i = 0; i < latestShapeColliders.Count; i++)
		{
			Object.Destroy(latestShapeColliders[i]);
		}
		latestShapeColliders.Clear();
	}

	private void Create2DCollider()
	{
		RemoveOldColliders();
		List<Vector3> blockPositions = collMang.GetBlockPositions();
		for (int i = 0; i < blockPositions.Count; i++)
		{
			Vector2 offset = new Vector2(blockPositions[i].x, blockPositions[i].z);
			BoxCollider2D boxCollider2D = external2DTransform.AddComponent<BoxCollider2D>();
			boxCollider2D.size = new Vector2(1.5f, 1.5f);
			boxCollider2D.offset = offset;
			boxCollider2D.usedByComposite = true;
			latestShapeColliders.Add(boxCollider2D);
		}
	}

	private void Create3DColliderFrom2D()
	{
		composite.GetPath(0, newPatherino);
	}
}
