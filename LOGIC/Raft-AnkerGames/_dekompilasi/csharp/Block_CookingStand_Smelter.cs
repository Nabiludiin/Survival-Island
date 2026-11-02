using UnityEngine;

public class Block_CookingStand_Smelter : Block_CookingStand
{
	public Battery battery;

	[SerializeField]
	private int batteryUsesPerMin = 1;

	private float timer;

	private bool isUsingEnergy;

	protected override void Update()
	{
		base.Update();
	}

	protected override void StartEnergySource()
	{
		battery.On = true;
	}

	protected override void StopEnergySource()
	{
		battery.On = false;
	}

	protected override bool IsUsingEnergySource()
	{
		return battery.On;
	}
}
