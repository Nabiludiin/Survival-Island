using UnityEngine;

public class AI_NetworkBehaviour_Boss_Varuna : AI_NetworkBehaviour_Animal
{
	[SerializeField]
	private DamageBox_Networked networkedDamageBox;

	private AI_StateMachine_Boss_Varuna bossSM;

	protected override void Start()
	{
		base.Start();
		bossSM = stateMachineAnimal as AI_StateMachine_Boss_Varuna;
	}

	public void ForceKill()
	{
		networkEntity.IsInvurnerable = false;
		ComponentManager<Network_Host>.Value.DamageEntity(bossSM.networkBehaviour.networkEntity, bossSM.networkBehaviour.transform, 999999f, Vector3.zero, Vector3.up, EntityType.Player);
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.DamageBox_Hit:
			if (Raft_Network.IsHost && networkedDamageBox != null)
			{
				Message_NetworkBehaviour_ID_SteamID message_NetworkBehaviour_ID_SteamID = msg as Message_NetworkBehaviour_ID_SteamID;
				Network_Player playerFromID = network.GetPlayerFromID(message_NetworkBehaviour_ID_SteamID.SteamID);
				if (playerFromID != null && !playerFromID.IsLocalPlayer)
				{
					networkedDamageBox.OnDamage?.Invoke(playerFromID);
				}
			}
			return false;
		case Messages.Charge_Hit:
			bossSM.HitSolidWithChargeFeedback();
			break;
		}
		return base.Deserialize(msg, remoteID);
	}

	public override RGD_AI_NetworkBehaviour Create_SaveData()
	{
		return new RGD_AI_NetworkBehaviour(this);
	}
}
