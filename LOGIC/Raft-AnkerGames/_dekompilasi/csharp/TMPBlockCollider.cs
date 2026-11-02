using UnityEngine;

public class TMPBlockCollider : MonoBehaviour, IBlockPlaced
{
	[SerializeField]
	public Collider[] collidersToRemove;

	private void RenameColliders()
	{
		if (collidersToRemove == null || collidersToRemove.Length == 0)
		{
			return;
		}
		int num = 0;
		for (int i = 0; i < collidersToRemove.Length; i++)
		{
			Collider collider = collidersToRemove[i];
			if (!(collider == null))
			{
				collider.name = "TMP BlockCollider " + num;
				num++;
			}
		}
	}

	void IBlockPlaced.OnBlockPlacedInterface()
	{
		if (collidersToRemove == null || collidersToRemove.Length == 0)
		{
			return;
		}
		Collider[] array = collidersToRemove;
		foreach (Collider collider in array)
		{
			if (collider != null)
			{
				Object.Destroy(collider.gameObject);
			}
		}
	}
}
