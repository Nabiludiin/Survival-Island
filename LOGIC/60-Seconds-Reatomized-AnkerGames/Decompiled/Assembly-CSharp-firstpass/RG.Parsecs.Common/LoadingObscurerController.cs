using System.Collections;
using UnityEngine;

namespace RG.Parsecs.Common;

public class LoadingObscurerController : MonoBehaviour
{
	private Animator _animator;

	private bool _initialized;

	public string obscurerShowStateName = "LoadingObscurer_Show";

	public string obscurerVisibleStateName = "LoadingObscurer_Visible";

	public string obscurerHideStateName = "LoadingObscurer_Hide";

	public string obscurerHiddenStateName = "LoadingObscurer_Hidden";

	public bool IsObscurerVisible { get; private set; }

	private void Awake()
	{
		CheckAnimatorInitialization();
	}

	public IEnumerator ShowObscurer()
	{
		IsObscurerVisible = true;
		CheckAnimatorInitialization();
		yield return PlayAnimUntil(obscurerShowStateName, obscurerVisibleStateName);
	}

	public IEnumerator HideObscurer()
	{
		CheckAnimatorInitialization();
		yield return PlayAnimUntil(obscurerHideStateName, obscurerHiddenStateName);
		IsObscurerVisible = false;
	}

	public void InstantlyHideObscurer()
	{
		IsObscurerVisible = false;
		CheckAnimatorInitialization();
		_animator.Play(obscurerHiddenStateName);
	}

	public void InstantlyShowObscurer()
	{
		IsObscurerVisible = true;
		CheckAnimatorInitialization();
		_animator.Play(obscurerVisibleStateName);
	}

	private IEnumerator PlayAnimUntil(string startStateName, string finishStateName)
	{
		_animator.Play(startStateName);
		while (_animator.GetCurrentAnimatorClipInfo(0).Length != 0 && _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != finishStateName)
		{
			yield return null;
		}
	}

	private void CheckAnimatorInitialization()
	{
		if (!_initialized)
		{
			_animator = GetComponent<Animator>();
			_initialized = true;
		}
	}
}
