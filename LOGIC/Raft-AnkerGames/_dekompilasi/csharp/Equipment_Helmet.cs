using UnityEngine;

public class Equipment_Helmet : Equipment_ArmorPiece
{
	[SerializeField]
	private HairStyle hairStyle;

	public override void SetModelState(bool state)
	{
		base.SetModelState(state);
		if (!playerNetwork.HazmatSuit.Equipped)
		{
			if (!state)
			{
				playerNetwork.currentModel.ActivateHairStyle(HairStyle.Full, useAsCurrentHairStyle: true);
			}
			else
			{
				playerNetwork.currentModel.ActivateHairStyle(hairStyle, useAsCurrentHairStyle: true);
			}
		}
	}
}
