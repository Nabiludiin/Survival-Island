public class MotorwheelFuelTank : Tank
{
	public Tank_Gauge tankGauge;

	private void OnValidate()
	{
		if (tankGauge == null)
		{
			tankGauge = GetComponentInChildren<Tank_Gauge>();
		}
	}

	protected override void LocalTankAmountChange()
	{
		base.LocalTankAmountChange();
	}
}
