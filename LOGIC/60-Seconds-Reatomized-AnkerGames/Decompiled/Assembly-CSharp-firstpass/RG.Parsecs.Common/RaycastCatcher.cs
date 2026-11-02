using RG.Parsecs.Survival;
using RG.VirtualInput;
using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Common;

[RequireComponent(typeof(Image))]
public class RaycastCatcher : MonoBehaviour
{
	private Image _image;

	private WindowView _clickAwayListener;

	private bool _clickAwayBlocked;

	private Image Image
	{
		get
		{
			if (_image == null)
			{
				_image = GetComponent<Image>();
			}
			return _image;
		}
	}

	public bool IsBlockingRaycasts => Image.raycastTarget;

	public void SetCatchingRaycasts(bool catchingRaycasts, WindowView windowView)
	{
		Image.raycastTarget = catchingRaycasts;
		if (catchingRaycasts)
		{
			_clickAwayListener = windowView;
		}
		else
		{
			_clickAwayListener = null;
		}
	}

	public void SetClickawayBlocked(bool block)
	{
		_clickAwayBlocked = block;
		Singleton<VirtualInputManager>.Instance.IsGamepadInputBlocked = block;
	}

	public void SendOnClickAway()
	{
		if (!_clickAwayBlocked)
		{
			if (_clickAwayListener != null)
			{
				_clickAwayListener.InvokeOnClickAway();
			}
			else
			{
				Debug.LogWarning("Raycast Catcher issued ClickAway but there was no objects listening for it.");
			}
		}
	}
}
