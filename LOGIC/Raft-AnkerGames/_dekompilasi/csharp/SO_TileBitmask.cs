using UnityEngine;

[CreateAssetMenu(fileName = "TileBitmask", menuName = "Raft/Miscellaneous/TileBitmask")]
public class SO_TileBitmask : ScriptableObject
{
	public TileBitmaskType bitmaskType;

	public TileBitmaskConnection[] bitmaskMeshes;

	private void OnValidate()
	{
		for (int i = 0; i < bitmaskMeshes.Length; i++)
		{
			bitmaskMeshes[i].bitmaskIndex = i;
		}
	}

	public TileBitmaskConnection GetBitmaskConnectionFromBitmaskValue(int bitmaskValue)
	{
		if (bitmaskValue >= 0 && bitmaskValue < bitmaskMeshes.Length)
		{
			return bitmaskMeshes[bitmaskValue];
		}
		return null;
	}
}
