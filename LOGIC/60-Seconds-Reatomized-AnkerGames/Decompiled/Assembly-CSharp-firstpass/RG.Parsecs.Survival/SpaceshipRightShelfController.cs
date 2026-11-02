using System;
using UnityEngine;

namespace RG.Parsecs.Survival;

[ExecuteInEditMode]
public class SpaceshipRightShelfController : MonoBehaviour
{
	[SerializeField]
	private SpaceshipRightShelfStates _currentState;

	[SerializeField]
	private SpaceshipRightShelfStates _startingState;

	[SerializeField]
	private GameObject _door;

	[SerializeField]
	private SpaceshipItemController[] _containedItems;

	public void SetState(SpaceshipRightShelfStates state)
	{
		switch (state)
		{
		case SpaceshipRightShelfStates.CLOSED:
			_door.SetActive(value: true);
			break;
		case SpaceshipRightShelfStates.OPENED:
			_door.SetActive(value: false);
			break;
		default:
			throw new ArgumentOutOfRangeException("state", state, null);
		}
		_currentState = state;
	}

	public void UpdateState()
	{
		SpaceshipRightShelfStates spaceshipRightShelfStates = SpaceshipRightShelfStates.CLOSED;
		for (int i = 0; i < _containedItems.Length; i++)
		{
			if (_containedItems[i].CurrentState == SpaceshipItemState.VISIBLE)
			{
				spaceshipRightShelfStates = SpaceshipRightShelfStates.OPENED;
			}
		}
		switch (spaceshipRightShelfStates)
		{
		case SpaceshipRightShelfStates.CLOSED:
			_door.SetActive(value: true);
			break;
		case SpaceshipRightShelfStates.OPENED:
			_door.SetActive(value: false);
			break;
		default:
			throw new ArgumentOutOfRangeException("state", spaceshipRightShelfStates, null);
		}
		_currentState = spaceshipRightShelfStates;
	}
}
