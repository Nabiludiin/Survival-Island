using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class ResourceStation : MonoBehaviour
{
	public PlayerResources playerResFile;

	public ResourceStationResource[] entries;

	public void Refresh()
	{
		for (int i = 0; i < playerResFile.RuntimeResources.Count; i++)
		{
			GameResource gameResource = playerResFile.RuntimeResources[i];
			entries[i].amount.text = gameResource.Amount.ToString();
			entries[i].icon.sprite = gameResource.Resource.Icon;
		}
	}
}
