using System.Collections;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class SummaryPanelAnimationController : MonoBehaviour
{
	[SerializeField]
	private string _showAnimationName = "Show";

	[SerializeField]
	private string _visibleStateName = "Visible";

	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private bool _showOnEnable;

	public void OnEnable()
	{
		if (_showOnEnable)
		{
			StartCoroutine(RunShowAnimation());
		}
	}

	public IEnumerator RunShowAnimation()
	{
		if (!(_animator == null))
		{
			_animator.Play(_showAnimationName);
			while (!_animator.GetCurrentAnimatorStateInfo(0).IsName(_visibleStateName))
			{
				yield return null;
			}
		}
	}
}
