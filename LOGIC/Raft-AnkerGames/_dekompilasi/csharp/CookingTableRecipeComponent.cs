using UnityEngine;

public class CookingTableRecipeComponent : MonoBehaviour
{
	[SerializeField]
	private CookingTable_Recipe_UI recipeUI;

	private Network_Player playerNetwork;

	public void OnSelect()
	{
		if (playerNetwork == null)
		{
			playerNetwork = GetComponentInParent<Network_Player>();
		}
		Item_Base currentItemInHand = playerNetwork.PlayerItemManager.useItemController.GetCurrentItemInHand();
		if (currentItemInHand == null)
		{
			return;
		}
		Block blockPrefab = currentItemInHand.settings_buildable.GetBlockPrefab(0);
		if (!(blockPrefab == null))
		{
			CookingTable_Recipe_UI component = blockPrefab.GetComponent<CookingTable_Recipe_UI>();
			if (!(component == null))
			{
				recipeUI.ShowRecipe(component.Recipe);
			}
		}
	}
}
