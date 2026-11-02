using UnityEngine;

namespace RG.Parsecs.Survival;

public class SpaceshipRightShelfItemController : SpaceshipItemController
{
	[SerializeField]
	private SpaceshipRightShelfController _rightShelfController;

	public override void SetState(SpaceshipItemState state)
	{
		base.SetState(state);
		_rightShelfController.UpdateState();
	}
}
