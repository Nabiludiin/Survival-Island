using RG.Parsecs.Common;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class DisplayViewController : MonoBehaviour
{
	[SerializeField]
	private WindowView _windowView;

	private void OnMouseUpAsButton()
	{
		if (!(_windowView == null) && !Singleton<HandCursor>.Instance.IsPointerOverGameObject())
		{
			_windowView.Show();
		}
	}

	public void ShowWindowView()
	{
		_windowView.Show();
	}
}
