using UnityEngine;

public class TerrainDataCopySettings : MonoBehaviour
{
	public TerrainData sourceData;

	public TerrainData[] targetData;

	public void CopyDetails()
	{
		if (!(sourceData == null) && targetData != null)
		{
			DetailPrototype[] detailPrototypes = sourceData.detailPrototypes;
			for (int i = 0; i < targetData.Length; i++)
			{
				targetData[i].detailPrototypes = detailPrototypes;
			}
		}
	}

	public bool CanCopy()
	{
		if (sourceData == null || targetData == null || targetData.Length == 0)
		{
			return false;
		}
		return true;
	}

	private bool DoesDetailExists(DetailPrototype detail, DetailPrototype[] details)
	{
		for (int i = 0; i < details.Length; i++)
		{
			if (details[i] != null && details[i].Equals(detail))
			{
				return true;
			}
		}
		return false;
	}

	private DetailPrototype GetMatchingDetail(DetailPrototype detail, DetailPrototype[] details)
	{
		for (int i = 0; i < details.Length; i++)
		{
			if (details[i] != null && details[i].prototypeTexture == detail.prototypeTexture)
			{
				return details[i];
			}
		}
		return null;
	}
}
