using System;
using UnityEngine;

namespace RG.Parsecs.Survival;

[ExecuteInEditMode]
public class SpaceshipItemController : MonoBehaviour
{
	[SerializeField]
	private SpaceshipItemState _currentState;

	[SerializeField]
	protected SpriteRenderer SpriteRenderer;

	protected bool PopupEnabled;

	public SpaceshipItemState CurrentState => _currentState;

	private void Start()
	{
	}

	public virtual void SetState(SpaceshipItemState state)
	{
		_currentState = state;
		switch (state)
		{
		case SpaceshipItemState.VISIBLE:
			SpriteRenderer.gameObject.SetActive(value: true);
			break;
		case SpaceshipItemState.HIDDEN:
			SpriteRenderer.gameObject.SetActive(value: false);
			break;
		default:
			throw new ArgumentOutOfRangeException("state", state, null);
		}
	}

	private void OnMouseOver()
	{
	}

	private void OnMouseExit()
	{
	}
}
