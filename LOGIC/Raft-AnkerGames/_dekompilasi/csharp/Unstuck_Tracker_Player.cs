using UnityEngine;

public class Unstuck_Tracker_Player : Unstuck_Tracker
{
	[SerializeField]
	private Network_Player trackedPlayer;

	public override bool IsUpdateValid
	{
		get
		{
			if (base.IsUpdateValid)
			{
				return trackedPlayer.IsLocalPlayer;
			}
			return false;
		}
	}

	protected override void Start()
	{
		base.Start();
		if (trackedPlayer == null)
		{
			_ = (bool)((tracked != null) ? (trackedPlayer = tracked.GetComponent<Network_Player>()) : (trackedPlayer = GetComponent<Network_Player>()));
		}
	}

	protected override void Teleport(Vector3 point)
	{
		if (trackedPlayer.PersonController.controllerType == ControllerType.Water)
		{
			trackedPlayer.PersonController.SwitchControllerType(ControllerType.Ground);
		}
		base.Teleport(point);
	}

	protected override void StuckFixTPRaft()
	{
		base.StuckFixTPRaft();
		trackedPlayer.SetToWalkableBlockPosition();
	}

	protected override bool ShouldTPToRaft(Vector3 originPoint, Vector3 destinationPoint)
	{
		if (trackedPlayer.PersonController.controllerType == ControllerType.Water)
		{
			return true;
		}
		return base.ShouldTPToRaft(originPoint, destinationPoint);
	}
}
