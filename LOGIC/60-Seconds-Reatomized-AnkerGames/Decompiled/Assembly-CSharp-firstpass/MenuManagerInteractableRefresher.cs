using RG.Parsecs.Menu;
using UnityEngine;
using UnityEngine.UI;

public class MenuManagerInteractableRefresher : MonoBehaviour, IRefreshInteractable
{
	[SerializeField]
	private MenuManager Menu;

	[SerializeField]
	private Selectable[] Selectables;

	public void RefreshInteractable()
	{
		for (int i = 0; i < Selectables.Length; i++)
		{
			Selectables[i].interactable = true;
		}
		Menu.RefreshInteractable();
	}
}
