using System;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class SpaceshipLeftShelfItemController : SpaceshipItemController
{
	[SerializeField]
	private SpaceshipLeftShelfController _spaceshipLeftShelfController;

	public override void SetState(SpaceshipItemState state)
	{
		base.SetState(state);
		switch (state)
		{
		case SpaceshipItemState.VISIBLE:
			_spaceshipLeftShelfController.SetShelf(3, closed: false);
			break;
		case SpaceshipItemState.HIDDEN:
			_spaceshipLeftShelfController.SetShelf(3, closed: true);
			break;
		default:
			throw new ArgumentOutOfRangeException("state", state, null);
		}
	}
}
