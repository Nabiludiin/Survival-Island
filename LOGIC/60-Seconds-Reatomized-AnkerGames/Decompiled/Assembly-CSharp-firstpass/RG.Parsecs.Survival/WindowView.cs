using System.Collections;
using RG.Parsecs.Common;
using UnityEngine;

namespace RG.Parsecs.Survival;

[RequireComponent(typeof(CanvasGroup))]
public abstract class WindowView : MonoBehaviour
{
	private CanvasGroup _windowCanvasGroup;

	private const float LERP_SPEED = 10f;

	private bool _isTransitioning;

	[SerializeField]
	private ComputerLampsListenerList _computerLampsListenerList;

	[SerializeField]
	private Animator _animator;

	public event WindowViewStatusChanged onWindowViewShown;

	public event WindowViewStatusChanged onWindowViewHidden;

	public void Show()
	{
		if (!_isTransitioning)
		{
			Singleton<GameManager>.Instance.RaycastCatcher.SetCatchingRaycasts(catchingRaycasts: true, this);
			OnShow();
			gameObject.SetActive(value: true);
			if ((bool)_animator)
			{
				_animator.SetBool("Visible", value: true);
			}
			if (onWindowViewShown != null)
			{
				onWindowViewShown();
			}
		}
	}

	public void Hide(bool disableRaycast = true)
	{
		if (!_isTransitioning)
		{
			if (onWindowViewHidden != null)
			{
				onWindowViewHidden();
			}
			if ((bool)_animator)
			{
				_animator.SetBool("Visible", value: false);
			}
			Singleton<GameManager>.Instance.RaycastCatcher.SetCatchingRaycasts(!disableRaycast, this);
			OnHide();
			if (_computerLampsListenerList != null)
			{
				_computerLampsListenerList.NotifyOnLampsUpdate();
			}
		}
	}

	public void InvokeOnClickAway()
	{
		OnClickAway();
	}

	protected virtual void OnClickAway()
	{
		Hide();
	}

	public virtual void OnShow()
	{
	}

	public virtual void OnHide()
	{
	}

	private IEnumerator ShowWindow()
	{
		if (_windowCanvasGroup == null)
		{
			_windowCanvasGroup = GetComponent<CanvasGroup>();
		}
		_windowCanvasGroup.alpha = 0f;
		while (_windowCanvasGroup.alpha < 0.99f)
		{
			_windowCanvasGroup.alpha = Mathf.Lerp(_windowCanvasGroup.alpha, 1f, 10f * Time.deltaTime);
			yield return new WaitForEndOfFrame();
		}
		_windowCanvasGroup.alpha = 1f;
		_isTransitioning = false;
	}

	private IEnumerator HideWindow()
	{
		if (_windowCanvasGroup == null)
		{
			_windowCanvasGroup = GetComponent<CanvasGroup>();
		}
		_windowCanvasGroup.alpha = 1f;
		while (_windowCanvasGroup.alpha > 0.01f)
		{
			_windowCanvasGroup.alpha = Mathf.Lerp(_windowCanvasGroup.alpha, 0f, 10f * Time.deltaTime);
			yield return new WaitForEndOfFrame();
		}
		_windowCanvasGroup.alpha = 0f;
		gameObject.SetActive(value: false);
		_isTransitioning = false;
	}
}
