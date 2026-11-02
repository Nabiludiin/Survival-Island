using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class SpriteZOrder : MonoBehaviour
{
	public bool IsStatic;

	public float AnchorOffset;

	private SpriteRenderer Sprite;

	private void Start()
	{
		Sprite = GetComponent<SpriteRenderer>();
		AssignSortOrder();
	}

	private void Update()
	{
		if (!IsStatic)
		{
			AssignSortOrder();
		}
	}

	private void AssignSortOrder()
	{
		Sprite.sortingOrder = -Mathf.RoundToInt((transform.position.y + AnchorOffset) / 0.05f);
	}
}
