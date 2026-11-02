using UnityEngine;

public class CookingStand_Electric : CookingStand
{
	[HideInInspector]
	public Block_CookingStand_Electric electricCookingStand;

	protected override void Start()
	{
		base.Start();
		electricCookingStand = blockCookingStand as Block_CookingStand_Electric;
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		Messages type = msg.Type;
		if ((uint)(type - 116) <= 2u)
		{
			Message_Battery message_Battery = msg as Message_Battery;
			if (msg != null && message_Battery.batteryIndex == electricCookingStand.battery.BatteryIndex)
			{
				return electricCookingStand.battery.OnBatteryMessage(msg);
			}
		}
		return base.Deserialize(msg, remoteID);
	}
}
