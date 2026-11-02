using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class CharacterAnimationController : MonoBehaviour
{
	private Animator _animator;

	private SpriteRenderer _spriteRenderer;

	private CharacterState _currentState;

	private CharacterStateSide _currentCharacterStateSide;

	private float _horizontal;

	private float _vertical;

	public bool Catch;

	private bool _doingCatch;

	private float _speed;

	private bool _doingJump;

	private float _facing;

	public bool DoingCatch => _doingCatch;

	private void Awake()
	{
		_animator = GetComponent<Animator>();
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_currentState = CharacterState.IDLE;
		_currentCharacterStateSide = CharacterStateSide.Front;
	}

	private void Update()
	{
		if (!_doingJump)
		{
			if (_doingCatch)
			{
				DoCatch();
				return;
			}
			UpdateState();
			UpdateAnimator();
		}
	}

	private void UpdateState()
	{
		float num = 0.01f;
		float num2 = Mathf.Abs(_horizontal);
		float num3 = Mathf.Abs(_vertical);
		if (num2 > 0f || num3 > 0f)
		{
			_speed = ((num2 > num3) ? num2 : num3);
		}
		else
		{
			_speed = 0f;
		}
		_currentState = ((_speed > 0f) ? CharacterState.RUN : CharacterState.IDLE);
		if (Catch)
		{
			_currentState = CharacterState.CATCH;
		}
		switch (_currentCharacterStateSide)
		{
		case CharacterStateSide.Side:
			if (_facing == 0f)
			{
				if (_horizontal > 0f && num2 > num)
				{
					_facing = 1f;
					_spriteRenderer.flipX = true;
				}
				else if (_horizontal < 0f && num2 > num)
				{
					_facing = -1f;
					_spriteRenderer.flipX = false;
				}
			}
			else if (_facing > 0f)
			{
				if (!(_horizontal >= 0f) && num2 > num)
				{
					_facing = -1f;
					_spriteRenderer.flipX = false;
				}
			}
			else if (_horizontal > 0f && num2 > num)
			{
				_facing = 1f;
				_spriteRenderer.flipX = true;
			}
			break;
		case CharacterStateSide.Front:
		case CharacterStateSide.Back:
			break;
		}
	}

	internal void Jump()
	{
		_doingJump = true;
		_animator.SetTrigger("Jump");
	}

	private void UpdateAnimator()
	{
		float num = Mathf.Abs(_horizontal);
		float num2 = Mathf.Abs(_vertical);
		if (!Mathf.Approximately(num, 0f) || !Mathf.Approximately(num2, 0f))
		{
			if (num > num2)
			{
				_animator.SetBool("Front", value: false);
				_animator.SetBool("Side", value: true);
				_animator.SetBool("Back", value: false);
				_currentCharacterStateSide = CharacterStateSide.Side;
			}
			else if (_vertical > 0f)
			{
				_animator.SetBool("Front", value: false);
				_animator.SetBool("Side", value: false);
				_animator.SetBool("Back", value: true);
				_currentCharacterStateSide = CharacterStateSide.Back;
			}
			else if ((double)_vertical <= 0.0)
			{
				_animator.SetBool("Front", value: true);
				_animator.SetBool("Side", value: false);
				_animator.SetBool("Back", value: false);
				_currentCharacterStateSide = CharacterStateSide.Front;
			}
		}
		if (_currentState == CharacterState.CATCH)
		{
			Catch = false;
			_doingCatch = true;
			_animator.SetTrigger("Catch");
		}
		_animator.SetFloat("Speed", _speed);
	}

	internal void SetInput(float vertical, float horizontal)
	{
		_vertical = vertical;
		_horizontal = horizontal;
	}

	private void DoCatch()
	{
		if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("emmet_catch_front") && !_animator.GetCurrentAnimatorStateInfo(0).IsName("emmet_catch_side") && !_animator.GetCurrentAnimatorStateInfo(0).IsName("emmet_catch_back"))
		{
			_doingCatch = false;
		}
	}
}
