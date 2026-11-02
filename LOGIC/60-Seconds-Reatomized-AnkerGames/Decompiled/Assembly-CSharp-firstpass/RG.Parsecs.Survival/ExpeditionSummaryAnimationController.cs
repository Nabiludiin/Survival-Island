using System.Collections;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class ExpeditionSummaryAnimationController : MonoBehaviour
{
	public enum ESummaryState
	{
		HIDDEN,
		DESTINATION,
		CHARACTER,
		EQUIPMENT
	}

	[SerializeField]
	private ExpeditionSummaryController _expeditionSummaryController;

	[SerializeField]
	private CanvasGroup _expeditionCanvasGroup;

	[SerializeField]
	private GameObject _expeditionView;

	[SerializeField]
	private Animator _animator;

	private ESummaryState _currentState;

	private bool _isInTransition;

	private bool _queuedAnimation;

	private void OnEnable()
	{
		SetStateWithoutAnim();
	}

	public void SetSummaryState(ESummaryState state)
	{
		_expeditionCanvasGroup.interactable = false;
		_currentState = state;
		if (_expeditionView.activeSelf)
		{
			StartCoroutine(SetState(animate: true));
		}
		else
		{
			SetStateWithoutAnim();
		}
	}

	public void ResetSummary()
	{
		_currentState = ESummaryState.HIDDEN;
		SetStateWithoutAnim();
	}

	private void SetStateWithoutAnim()
	{
		switch (_currentState)
		{
		case ESummaryState.HIDDEN:
			_animator.Play("Summary_Hidden");
			_animator.SetInteger("Visible", (int)_currentState);
			break;
		case ESummaryState.DESTINATION:
			_animator.Play("Summary_Visible_One");
			_animator.SetInteger("Visible", (int)_currentState);
			_expeditionSummaryController.SetExpeditionDestination(temporaryCard: true);
			break;
		case ESummaryState.CHARACTER:
			_animator.Play("Summary_Visible_Two");
			_animator.SetInteger("Visible", (int)_currentState);
			_expeditionSummaryController.SetExpeditionCharacter(temporaryCard: true);
			break;
		case ESummaryState.EQUIPMENT:
			_animator.Play("Summary_Visible_Three");
			_animator.SetInteger("Visible", (int)_currentState);
			_expeditionSummaryController.SetExpeditionEquipment(temporaryCard: true);
			break;
		}
		_expeditionCanvasGroup.interactable = true;
	}

	private IEnumerator SetState(bool animate)
	{
		if (_isInTransition)
		{
			_queuedAnimation = true;
			yield return new WaitWhile(() => _isInTransition);
			_queuedAnimation = false;
		}
		else
		{
			_isInTransition = true;
		}
		switch (_currentState)
		{
		case ESummaryState.HIDDEN:
			if (!animate)
			{
				_animator.Play("Summary_Hidden");
			}
			_animator.SetInteger("Visible", (int)_currentState);
			yield return new WaitForSeconds(0.1f);
			yield return new WaitWhile(() => _animator.IsInTransition(0));
			break;
		case ESummaryState.DESTINATION:
			_expeditionSummaryController.SetExpeditionDestination(temporaryCard: true);
			if (!animate)
			{
				_animator.Play("Summary_Visible_One");
			}
			_animator.SetInteger("Visible", (int)_currentState);
			yield return new WaitForSeconds(0.1f);
			yield return new WaitWhile(() => _animator.IsInTransition(0));
			break;
		case ESummaryState.CHARACTER:
			_expeditionSummaryController.SetExpeditionCharacter(temporaryCard: true);
			if (!animate)
			{
				_animator.Play("Summary_Visible_Two");
			}
			_animator.SetInteger("Visible", (int)_currentState);
			yield return new WaitForSeconds(0.1f);
			yield return new WaitWhile(() => _animator.IsInTransition(0));
			break;
		case ESummaryState.EQUIPMENT:
			_expeditionSummaryController.SetExpeditionEquipment(temporaryCard: true);
			if (!animate)
			{
				_animator.Play("Summary_Visible_Three");
			}
			_animator.SetInteger("Visible", (int)_currentState);
			yield return new WaitForSeconds(0.1f);
			yield return new WaitWhile(() => _animator.IsInTransition(0));
			break;
		}
		if (_isInTransition)
		{
			_isInTransition = false;
		}
		if (!_queuedAnimation)
		{
			_expeditionCanvasGroup.interactable = true;
		}
	}

	private bool IsAnimatorInState(string state)
	{
		return _animator.GetCurrentAnimatorStateInfo(0).IsName(state);
	}
}
