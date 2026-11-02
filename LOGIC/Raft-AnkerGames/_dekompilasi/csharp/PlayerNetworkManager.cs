using System.Collections.Generic;
using UnityEngine;

public class PlayerNetworkManager : MonoBehaviour_Network, SceneEventInterface
{
	public bool IsAttached;

	public static List<Brick_Wet> allBricks = new List<Brick_Wet>();

	private Network_Player playerNetwork;

	private void Start()
	{
		playerNetwork = GetComponentInParent<Network_Player>();
	}

	void SceneEventInterface.OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent == SceneEvent.LeaveGame)
		{
			allBricks.Clear();
		}
	}

	public void PickupWetBrick(Brick_Wet brick)
	{
		if (!(brick == null))
		{
			if (playerNetwork == null)
			{
				playerNetwork = GetComponentInParent<Network_Player>();
			}
			playerNetwork.Animator.SetAnimation(PlayerAnimation.Trigger_GrabItem);
			if (playerNetwork.IsLocalPlayer)
			{
				playerNetwork.Inventory.AddItem(brick.pickup.item.UniqueName, brick.pickup.amount);
			}
			if (allBricks != null)
			{
				allBricks.Remove(brick);
			}
			Object.Destroy(brick.gameObject);
		}
	}

	public Brick_Wet GetBrickByObjectIndex(uint objectIndex)
	{
		return allBricks.Find((Brick_Wet b) => b.ObjectIndex == objectIndex);
	}

	public override void Serialize_Create()
	{
		AddMessage(new Message_PlayerNetworkManager_Create(Messages.PlayerNetworkManager_Create, this));
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.PlayerNetworkManager_Create:
		{
			Message_PlayerNetworkManager_Create message_PlayerNetworkManager_Create = msg as Message_PlayerNetworkManager_Create;
			message_PlayerNetworkManager_Create.Restore(this, message_PlayerNetworkManager_Create);
			return false;
		}
		case Messages.Brick_Pickup:
		{
			Message_Brick message_Brick2 = msg as Message_Brick;
			Brick_Wet brickByObjectIndex2 = GetBrickByObjectIndex(message_Brick2.brickObjectIndex);
			if (brickByObjectIndex2 != null)
			{
				PickupWetBrick(brickByObjectIndex2);
				return true;
			}
			return false;
		}
		case Messages.Brick_Complete:
		{
			Message_Brick message_Brick = msg as Message_Brick;
			Brick_Wet brickByObjectIndex = GetBrickByObjectIndex(message_Brick.brickObjectIndex);
			if (brickByObjectIndex != null)
			{
				brickByObjectIndex.OnFinishedDrying();
				return true;
			}
			return false;
		}
		default:
			return false;
		}
	}
}
