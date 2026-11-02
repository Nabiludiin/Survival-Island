using Steamworks;
using UnityEngine;

public class TreasurePoint : MonoBehaviour_ID_Network
{
	public int uniqueTreasureIndex;

	public PickupItem_Networked pickupNetworked;

	public Animator treasureChestAnimator;

	public int excevationCount = 5;

	public float revelationRadiusBuried;

	public float innerRadiusBuried;

	public float revelationRadiusGround;

	public float innerRadiusGround;

	[SerializeField]
	private ParticleSystem digParticles;

	[HideInInspector]
	public int currentExcevationProgress;

	[HideInInspector]
	public bool isRevealedViaDetector;

	[HideInInspector]
	public float currentRevelationRadius;

	[HideInInspector]
	public float currentInnerRadius;

	[SerializeField]
	private TreasurePointDirt dirt;

	private Raft_Network network;

	public bool IsBuried => currentExcevationProgress < excevationCount;

	private void Start()
	{
		network = ComponentManager<Raft_Network>.Value;
	}

	private void Update()
	{
		UpdateExcevationAnimator();
	}

	protected override void OnDestroy()
	{
		NetworkIDManager.RemoveNetworkID(this, typeof(TreasurePoint));
		NetworkIDManager.RemoveNetworkID(pickupNetworked, typeof(PickupItem_Networked));
		if (dirt != null && !GameManager.IsLeavingGame && !GameManager.IsQuittingGame)
		{
			dirt.transform.SetParent(transform.parent);
			dirt.MarkAsRemoved();
		}
	}

	public virtual void Initialize(uint objectIndex, uint objectIndexPickup, Transform parent)
	{
		transform.SetParentSafe(parent);
		ObjectIndex = objectIndex;
		pickupNetworked.ObjectIndex = objectIndexPickup;
		NetworkIDManager.AddNetworkID(this, typeof(TreasurePoint));
		NetworkIDManager.AddNetworkID(pickupNetworked, typeof(PickupItem_Networked));
		SetIsDugUp(value: false);
		currentRevelationRadius = revelationRadiusBuried;
		currentInnerRadius = innerRadiusBuried;
	}

	public virtual int GetParentObjectIndex()
	{
		if (transform.parent == null)
		{
			return -1;
		}
		MonoBehaviour_ID_Network component = transform.parent.GetComponent<MonoBehaviour_ID_Network>();
		if (component != null)
		{
			return (int)component.ObjectIndex;
		}
		return -1;
	}

	public void ProgressExcevation(int amount, bool addAchievement = true)
	{
		SetExcevationProgress(currentExcevationProgress + amount, addAchievement);
	}

	public void MoveToPosition(Vector3 position, Vector3 upDirection)
	{
		if (upDirection.y < 0f)
		{
			upDirection.y *= -1f;
		}
		transform.position = position;
		transform.up = upDirection;
	}

	public void SetExcevationProgress(int progress, bool addAchievement = true)
	{
		currentExcevationProgress = progress;
		if (addAchievement && currentExcevationProgress == 1)
		{
			AchievementHandler.AddPlayerExcevationsTreasure();
		}
		if (currentExcevationProgress > excevationCount)
		{
			currentExcevationProgress = excevationCount;
		}
		pickupNetworked.PickupItem.canBePickedUp = !IsBuried;
		UpdateExcevationAnimator();
		PlayDigEffect();
	}

	public void ResetExcevationProgress()
	{
		SetExcevationProgress(0);
	}

	public void PlayDigEffect()
	{
		if (digParticles != null)
		{
			digParticles.Play();
		}
	}

	public void UpdateExcevationAnimator()
	{
		if (treasureChestAnimator != null)
		{
			treasureChestAnimator.SetInteger("Excevation", currentExcevationProgress);
		}
	}

	public void SetIsDugUp(bool value)
	{
		isRevealedViaDetector = value;
		if (value)
		{
			currentRevelationRadius = revelationRadiusGround;
			currentInnerRadius = innerRadiusGround;
		}
		else
		{
			currentRevelationRadius = revelationRadiusBuried;
			currentInnerRadius = innerRadiusBuried;
		}
	}

	public void RemoveSelf()
	{
		Object.Destroy(gameObject);
	}

	public virtual RGD_TreasurePoint_Networked GetSave()
	{
		return new RGD_TreasurePoint_Networked(this);
	}

	public override RGD Serialize_Save()
	{
		return new RGD_TreasurePoint(RGDType.TreasurePoint, RGDSortingOrder.TreasurePoints, this);
	}

	public void SendNetworkProgressExcevation(int amount)
	{
		Message_TreasurePoint_Excevate message_TreasurePoint_Excevate = new Message_TreasurePoint_Excevate(Messages.TreasurePoint_Excevate, network.NetworkIDManager, ObjectIndex, amount);
		if (Raft_Network.IsHost)
		{
			ProgressExcevation(amount);
			message_TreasurePoint_Excevate.exceAmount = currentExcevationProgress;
			network.RPC(message_TreasurePoint_Excevate, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
		else
		{
			network.SendP2P(network.HostID, message_TreasurePoint_Excevate, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	public void SendNetworkRevealTreasure()
	{
		Message_TreasurePoint_Reveal message = new Message_TreasurePoint_Reveal(Messages.TreasurePoint_Reveal, network.NetworkIDManager, ObjectIndex, toReveal: true);
		if (Raft_Network.IsHost)
		{
			isRevealedViaDetector = true;
			network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
		else
		{
			network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	public void SendNetworkMoveChest(Vector3 position, Vector3 upDirection)
	{
		Message_TreasurePoint_Move message = new Message_TreasurePoint_Move(Messages.TreasurePoint_Move, network.NetworkIDManager, ObjectIndex, position, upDirection);
		if (Raft_Network.IsHost)
		{
			MoveToPosition(position, upDirection);
			network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
		else
		{
			network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.TreasurePoint_Excevate:
		{
			Message_TreasurePoint_Excevate message_TreasurePoint_Excevate = msg as Message_TreasurePoint_Excevate;
			if (message_TreasurePoint_Excevate != null)
			{
				if (Raft_Network.IsHost)
				{
					ProgressExcevation(message_TreasurePoint_Excevate.exceAmount);
					message_TreasurePoint_Excevate.exceAmount = currentExcevationProgress;
					return true;
				}
				SetExcevationProgress(message_TreasurePoint_Excevate.exceAmount);
				return false;
			}
			return false;
		}
		case Messages.TreasurePoint_Reveal:
		{
			Message_TreasurePoint_Reveal message_TreasurePoint_Reveal = msg as Message_TreasurePoint_Reveal;
			if (message_TreasurePoint_Reveal != null)
			{
				isRevealedViaDetector = message_TreasurePoint_Reveal.reveal;
				return Raft_Network.IsHost;
			}
			break;
		}
		case Messages.TreasurePoint_Move:
		{
			Message_TreasurePoint_Move message_TreasurePoint_Move = msg as Message_TreasurePoint_Move;
			if (message_TreasurePoint_Move != null)
			{
				MoveToPosition(message_TreasurePoint_Move.Position, message_TreasurePoint_Move.UpDirection);
				return Raft_Network.IsHost;
			}
			break;
		}
		}
		return base.Deserialize(msg, remoteID);
	}
}
