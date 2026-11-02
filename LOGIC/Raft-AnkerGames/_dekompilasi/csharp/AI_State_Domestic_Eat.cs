using Steamworks;
using UnityEngine;

public class AI_State_Domestic_Eat : AI_State_Wait
{
	public Item_Base itemToEat;

	public Plant plantToEat;

	private PlantManager plantManager;

	private AI_NetworkBehaviour_Domestic domesticBehaviour;

	protected override void Start()
	{
		base.Start();
		domesticBehaviour = GetComponentInParent<AI_NetworkBehaviour_Domestic>();
	}

	public override void UpdateState()
	{
		base.UpdateState();
		if (Raft_Network.IsHost && plantToEat != null)
		{
			Vector3 position = plantToEat.transform.position;
			position.y = stateMachine.transform.position.y;
			Vector3 direction = position - stateMachine.transform.position;
			stateMachine.movement.LookDirection(direction, stateMachine.movement.RotationSpeed);
		}
	}

	public override void ExitState()
	{
		base.ExitState();
		if (Raft_Network.IsHost)
		{
			if (plantManager == null)
			{
				plantManager = ComponentManager<Network_Player>.Value.PlantManager;
			}
			if (plantToEat != null)
			{
				domesticBehaviour.HungerTimer += GameModeValueManager.GetCurrentGameModeValue().domesticVariables.hungerGain;
				domesticBehaviour.Network.RPC(new Message_HarvestPlant(Messages.PlantManager_HarvestPlant, plantManager, plantToEat, giveYield: false), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				plantManager.Harvest(plantToEat, giveYield: false);
			}
		}
	}
}
