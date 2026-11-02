using RG.Parsecs.Common;
using UnityEngine;

namespace RG.Parsecs.Survival;

public abstract class DisplayView : MonoBehaviour
{
	[SerializeField]
	protected WindowView _windowView;

	public virtual void ShowWindowView()
	{
		if (!(_windowView == null) && !Singleton<HandCursor>.Instance.IsPointerOverGameObject() && !EndGameManager.Instance.GetEndGameData().RuntimeData.ShouldEndGame)
		{
			_windowView.Show();
		}
	}

	private void OnMouseUpAsButton()
	{
		ShowWindowView();
	}
}
