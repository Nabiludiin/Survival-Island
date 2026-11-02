using UnityEngine;

public class Block_Windturbine : Block
{
	[SerializeField]
	private WindTurbine windturbine;

	public override RGD Serialize_Save()
	{
		return new RGD_Block_WindTurbine(RGDType.Block_WindTurbine, this, windturbine);
	}

	public override RGD_Block GetBlockCreationData()
	{
		return new RGD_Block_WindTurbine(RGDType.Block_WindTurbine, this, windturbine);
	}
}
