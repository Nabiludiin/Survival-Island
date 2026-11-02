using RG.Parsecs.EventEditor;
using UnityEngine;
using UnityEngine.Events;

namespace RG.Parsecs.Menu;

public class IconManagerNewGameClick : IconManager
{
	[SerializeField]
	private GlobalBoolVariable tutorialCompleted;

	[SerializeField]
	protected UnityEvent OnClickTutorial;

	public override void Click()
	{
		if (isInteractable)
		{
			if (tutorialCompleted.Value)
			{
				OnClick.Invoke();
			}
			else
			{
				OnClickTutorial.Invoke();
			}
		}
	}
}
