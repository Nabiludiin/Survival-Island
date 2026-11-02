using UnityEngine;

public class Stat_WellBeing : Stat
{
	public static WellBeing Factor = WellBeing.Normal;

	public static float groundSpeedMultiplier = 0.6f;

	public static float swimSpeedMultiplier = 0.7f;

	public static float oxygenLostMultiplier = 2f;

	public static float WellBeingLimit = 0.15f;

	[SerializeField]
	private Stat_Consumable stat_hunger;

	[SerializeField]
	private Stat_Consumable stat_thirst;

	private Network_Player playerNetwork;

	private void Start()
	{
		playerNetwork = GetComponentInParent<Network_Player>();
	}

	private void Update()
	{
		if (GameModeValueManager.IsPlayerInvurnerable)
		{
			Factor = WellBeing.Good;
			return;
		}
		if (playerNetwork == null)
		{
			playerNetwork = GetComponentInParent<Network_Player>();
		}
		if (playerNetwork == null || !playerNetwork.IsLocalPlayer)
		{
			enabled = false;
		}
		else if (!(stat_hunger == null) && !(stat_thirst == null))
		{
			float num = ((stat_thirst.NormalValue < stat_hunger.NormalValue) ? stat_thirst.NormalValue : stat_hunger.NormalValue);
			if (num < WellBeingLimit)
			{
				Factor = WellBeing.Bad;
			}
			else if (num < 0.5f)
			{
				Factor = WellBeing.Normal;
			}
			else
			{
				Factor = WellBeing.Good;
			}
		}
	}
}
