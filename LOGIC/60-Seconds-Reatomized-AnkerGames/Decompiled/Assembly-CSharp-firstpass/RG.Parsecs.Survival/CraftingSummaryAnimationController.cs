using System.Collections;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class CraftingSummaryAnimationController : MonoBehaviour
{
	[SerializeField]
	private CraftingSummaryController _craftingSummary;

	[SerializeField]
	private CanvasGroup _itemSelectionPanel;

	[SerializeField]
	private Animator _animator;

	private bool _enabled;

	private bool _transition;

	private bool _queuedTransition;

	private bool _forceTransition;

	private const string VAR_VISIBLE = "Visible";

	private const string STATE_VISIBLE = "CraftingSummary_Visible";

	private const string STATE_HIDDEN = "CraftingSummary_Hidden";

	public bool Ready
	{
		get
		{
			if (_transition)
			{
				return !_queuedTransition;
			}
			return true;
		}
	}

	private void Update()
	{
		if (_forceTransition)
		{
			_forceTransition = false;
			SetVisible(!_enabled, force: true);
		}
	}

	public void Reset()
	{
		_enabled = false;
		_transition = false;
		_queuedTransition = false;
	}

	public void SetForceTransition()
	{
		_forceTransition = true;
	}

	public void SetVisible(bool visible, bool force = false)
	{
		_enabled = visible;
		if (visible)
		{
			_craftingSummary.SetContent(temporaryCard: true);
		}
		if (force)
		{
			if (visible)
			{
				_animator.Play("CraftingSummary_Visible");
			}
			else
			{
				_animator.Play("CraftingSummary_Hidden");
			}
		}
		_animator.SetBool("Visible", visible);
	}

	public void Show(bool visible)
	{
		if (_transition)
		{
			_queuedTransition = true;
		}
		else
		{
			StartCoroutine(Transition(visible));
		}
	}

	private bool IsAnimatorInState(string state)
	{
		return _animator.GetCurrentAnimatorStateInfo(0).IsName(state);
	}

	private IEnumerator Transition(bool visible)
	{
		_transition = true;
		_itemSelectionPanel.interactable = false;
		if (visible && !_enabled)
		{
			SetVisible(visible: true);
			while (!IsAnimatorInState("CraftingSummary_Visible"))
			{
				yield return null;
			}
		}
		else
		{
			SetVisible(visible: false);
			while (!IsAnimatorInState("CraftingSummary_Hidden"))
			{
				yield return null;
			}
		}
		if (_queuedTransition)
		{
			StartCoroutine(Transition(!visible));
			_queuedTransition = false;
		}
		else
		{
			_transition = false;
			_itemSelectionPanel.interactable = true;
		}
	}
}
