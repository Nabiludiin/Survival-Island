using UnityEngine;

namespace RG.Parsecs.Survival;

public class StarlogVisualController : VisualsController
{
	[SerializeField]
	private StarlogComputerController _starlogComputerController;

	private void OnMouseUpAsButton()
	{
		_starlogComputerController.ShowWindowView();
	}
}
