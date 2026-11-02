using FMODUnity;
using UnityEngine;

public class Intervaller_GameObjectToggler : Intervaller
{
	[SerializeField]
	private GameObject stateOneParent;

	[SerializeField]
	private GameObject stateTwoParent;

	[SerializeField]
	private Transform soundPositionParent;

	public EventReference er_stateOneOn;

	public EventReference er_stateTwoOn;

	private Network_Water water;

	private void Start()
	{
		water = ComponentManager<Network_Water>.Value;
	}

	private void Update()
	{
		if (water != null)
		{
			DoUpdate(water.WaterTime);
		}
	}

	protected override void ActivateInterval()
	{
		base.ActivateInterval();
		if (stateOneParent != null)
		{
			stateOneParent.SetActiveSafe(value: true);
			RuntimeManager.PlayOneShot(er_stateOneOn, soundPositionParent.position);
		}
		if (stateTwoParent != null)
		{
			stateTwoParent.SetActiveSafe(value: false);
		}
	}

	protected override void DeActivateInterval()
	{
		base.DeActivateInterval();
		if (stateOneParent != null)
		{
			stateOneParent.SetActiveSafe(value: false);
		}
		if (stateTwoParent != null)
		{
			stateTwoParent.SetActiveSafe(value: true);
			RuntimeManager.PlayOneShot(er_stateTwoOn, soundPositionParent.position);
		}
	}
}
