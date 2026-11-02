using UnityEngine;

public class TreasurePointDirt : MonoBehaviour
{
	private bool isRemoved;

	public void MarkAsRemoved()
	{
		isRemoved = true;
	}

	private void OnDisable()
	{
		if (isRemoved && !GameManager.IsLeavingGame)
		{
			Object.Destroy(gameObject);
		}
	}

	private void OnEnable()
	{
		if (isRemoved && !GameManager.IsLeavingGame)
		{
			Object.Destroy(gameObject);
		}
	}
}
